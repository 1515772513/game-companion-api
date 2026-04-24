using GameCompanion.Api.DTOs.Companion;
using GameCompanion.Api.Models.Entities;
using GameCompanion.Api.Models;
using Microsoft.EntityFrameworkCore;
using GameCompanion.Api.Data;
using GameCompanion.Api.Utils;
using GameCompanion.Api.Services.DictTranslate;

namespace GameCompanion.Api.Services;

/// <summary>
/// 陪玩师服务实现
/// </summary>
public class CompanionService : ICompanionService
{
    private readonly GameCompanionContext _context;
    private readonly ILogger<CompanionService> _logger;
    private readonly IDictTranslateService _dictTranslateService;

    public CompanionService(GameCompanionContext context, ILogger<CompanionService> logger, IDictTranslateService dictTranslateService)
    {
        _context = context;
        _logger = logger;
        _dictTranslateService = dictTranslateService;
    }

    /// <summary>
    /// 申请成为陪玩师（支持：多游戏+多服务+多背景轮播图）
    /// </summary>
    public async Task<ApiResponse<ApplicationStatusResponse>> ApplyCompanionAsync(ApplyCompanionRequest request, int userId)
    {
        try
        {
            if (userId == 0)
            {
                return ApiResponse<ApplicationStatusResponse>.ErrorResponse(401, "未授权，请先登录");
            }

            // 检查重复申请
            var existing = await _context.Companions.FirstOrDefaultAsync(c => c.UserId == userId);
            if (existing != null)
            {
                if (existing.Status == 0)
                    return ApiResponse<ApplicationStatusResponse>.ErrorResponse(400, "已有待审核申请，请勿重复提交");
                if (existing.Status == 1)
                    return ApiResponse<ApplicationStatusResponse>.ErrorResponse(400, "您已是认证陪玩师");
            }

            // 1. 创建陪玩师基础信息
            var companion = new Companion
            {
                UserId = userId,
                RealName = request.RealName,
                IdCard = request.IdCard,
                IdCardFront = request.IdCardFrontUrl,
                IdCardBack = request.IdCardBackUrl,
                Phone = request.Phone,
                Nickname = request.Nickname,
                Bio = request.Bio,
                Tags = request.Tags != null ? string.Join(",", request.Tags) : null,
                Status = 0, // 待审核
                OnlineStatus = "offline",
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
            };
            _context.Companions.Add(companion);
            await _context.SaveChangesAsync();

            // 2. 批量添加游戏技能（不变）
            foreach (var item in request.GameSkills)
            {
                var companionGame = new CompanionGame
                {
                    CompanionId = companion.Id,
                    GameId = item.GameId,
                    GameLevel = item.GameRank,
                    ServiceType = item.ServiceType,
                    PricePerGame = item.Price,
                    CreatedAt = DateTime.Now
                };
                _context.CompanionGames.Add(companionGame);
            }

            // 3. ====================== 批量插入【多张背景轮播图】 ======================
            if (request.BackgroundImages != null && request.BackgroundImages.Any())
            {
                foreach (var img in request.BackgroundImages)
                {
                    var backgroundImage = new CompanionBackgroundImage
                    {
                        Id = Guid.NewGuid(),
                        CompanionId = companion.Id,
                        FileId = img.FileId,
                        Sort = img.Sort,
                        CreateTime = DateTime.Now,
                        UpdateTime = DateTime.Now,
                        IsDeleted = 0
                    };
                    _context.CompanionBackgroundImages.Add(backgroundImage);
                }
            }
            
            // 完善用户信息
            
            // 先查询用户信息
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return ApiResponse<ApplicationStatusResponse>.ErrorResponse(404, "未找到用户信息");
            }
            // 更新信息
            user.RealName = request.RealName;
            user.IdCard = request.IdCard;
            user.Phone = request.Phone;
            user.Name = request.RealName;
            
            _context.Users.Update(user);

            // 统一提交
            await _context.SaveChangesAsync();

            // 返回结果
            var resp = new ApplicationStatusResponse
            {
                ApplicationId = companion.Id,
                CertificationStatus = 0,
                CertificationStatusText = "待审核",
                CertificationApplyTime = companion.CreatedAt.ToDateTimeString()
            };

            return ApiResponse<ApplicationStatusResponse>.SuccessResponse(resp, "申请提交成功，等待审核");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "陪玩师申请异常");
            return ApiResponse<ApplicationStatusResponse>.ErrorResponse(500, "申请失败，请稍后重试");
        }
    }

    /// <summary>
    /// 获取认证申请状态
    /// </summary>
    public async Task<ApiResponse<ApplicationStatusResponse>> GetApplicationStatusAsync()
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return ApiResponse<ApplicationStatusResponse>.ErrorResponse(401, "未授权，请先登录");
            }

            var companion = await _context.Companions
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (companion == null)
            {
                return ApiResponse<ApplicationStatusResponse>.ErrorResponse(404, "未找到陪玩师申请记录");
            }

            var response = new ApplicationStatusResponse
            {
                ApplicationId = companion.Id,
                CertificationStatus = GetCertificationStatus(companion.Status ?? 0),
                CertificationStatusText = GetCertificationStatusText(companion.Status ?? 0),
                CertificationApplyTime = companion.CreatedAt.ToDateTimeString(),
                CertificationTime = companion.UpdatedAt.ToDateTimeString(),
                RejectReason = companion.RejectReason
            };

            return ApiResponse<ApplicationStatusResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取认证申请状态失败");
            return ApiResponse<ApplicationStatusResponse>.ErrorResponse(500, "获取状态失败");
        }
    }

    /// <summary>
    /// 获取我的陪玩师信息
    /// </summary>
    public async Task<ApiResponse<CompanionInfoResponse>> GetMyCompanionInfoAsync()
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return ApiResponse<CompanionInfoResponse>.ErrorResponse(401, "未授权，请先登录");
            }

            var companion = await _context.Companions
                .Include(c => c.User)
                .Include(c => c.CompanionGames)
                    .ThenInclude(g => g.Game)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (companion == null || companion.Status != 1)
            {
                return ApiResponse<CompanionInfoResponse>.ErrorResponse(404, "未找到陪玩师信息或未认证");
            }

            // 取出第一个游戏技能（用于取服务类型、价格、段位）
            var firstGame = companion.CompanionGames.FirstOrDefault();

            var response = new CompanionInfoResponse
            {
                Id = companion.Id,
                UserId = companion.UserId,
                Nickname = companion.Nickname,
                AvatarUrl = companion.User?.Avatar ?? "",
                Level = companion.Level ?? null,
                
                // ======================
                // 【修改 1】从游戏表取服务类型
                // ======================
                ServiceType = firstGame?.ServiceType ?? "tech",
                
                // ======================
                // 【修改 2】从游戏表取价格
                // ======================
                Price = firstGame?.PricePerGame ?? 0,
                
                Rating = companion.Rating ?? 0,
                OrderCount = companion.TotalOrders ?? 0,
                RatingCount = 0, // 需要根据评价记录计算
                PositiveRate = companion.GoodReviewRate ?? 0,
                OnlineStatus = OnlineStatusToInt(companion.OnlineStatus),
                IsVerified = companion.Status == 1,
                Games = companion.CompanionGames.Select(g => g.Game.Name).ToList(),
                GameRank = firstGame?.GameLevel ?? "",
                Bio = companion.Bio ?? "",
                Tags = companion.Tags?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(),
                CertificationTime = companion.UpdatedAt.ToDateTimeString(),
                TodayOrders = 0, // 需要根据订单统计
                MonthOrders = 0  // 需要根据订单统计
            };

            return ApiResponse<CompanionInfoResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取陪玩师信息失败");
            return ApiResponse<CompanionInfoResponse>.ErrorResponse(500, "获取信息失败");
        }
    }

    /// <summary>
    /// 更新陪玩师信息
    /// </summary>
    public async Task<ApiResponse<object>> UpdateCompanionInfoAsync(UpdateCompanionInfoRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return ApiResponse<object>.Fail(401, "未授权，请先登录");
            }

            var companion = await _context.Companions
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (companion == null || companion.Status != 1)
            {
                return ApiResponse<object>.Fail(404, "未找到陪玩师信息或未认证");
            }

            // ======================
            // 【旧字段：只更新昵称、简介、标签】
            // ======================
            if (request.Nickname != null) companion.Nickname = request.Nickname;
            if (request.Bio != null) companion.Bio = request.Bio;
            if (request.Tags != null) companion.Tags = string.Join(",", request.Tags);

            // ======================
            // 【新逻辑：服务类型、价格 → 更新到 companion_games 表】
            // ======================
            if (request.ServiceType != null || request.Price.HasValue)
            {
                // 获取该陪玩师的游戏技能记录（默认取第一条，你也可以按 GameId 筛选）
                var companionGame = await _context.CompanionGames
                    .FirstOrDefaultAsync(cg => cg.CompanionId == companion.Id);

                if (companionGame == null)
                {
                    return ApiResponse<object>.Fail(404, "该陪玩师未设置游戏技能");
                }

                // 更新服务类型
                if (request.ServiceType != null)
                    companionGame.ServiceType = request.ServiceType;

                // 更新单价
                if (request.Price.HasValue)
                    companionGame.PricePerGame = request.Price.Value;
            }

            companion.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return ApiResponse<object>.Success("陪玩师信息更新成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "更新陪玩师信息失败");
            return ApiResponse<object>.Fail(500, "更新信息失败");
        }
    }

    /// <summary>
    /// 切换在线状态
    /// </summary>
    public async Task<ApiResponse<object>> UpdateOnlineStatusAsync(UpdateOnlineStatusRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return ApiResponse<object>.Fail(401, "未授权，请先登录");
            }

            var companion = await _context.Companions
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (companion == null || companion.Status != 1)
            {
                return ApiResponse<object>.Fail(404, "未找到陪玩师信息或未认证");
            }

            // 转换在线状态
            string onlineStatus = request.OnlineStatus switch
            {
                0 => "离线",
                1 => "在线",
                2 => "忙碌",
                _ => "离线"
            };

            companion.OnlineStatus = onlineStatus;
            companion.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return ApiResponse<object>.Success($"状态已切换为{onlineStatus}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "切换在线状态失败");
            return ApiResponse<object>.Fail(500, "切换状态失败");
        }
    }

    /// <summary>
    /// 获取接单列表
    /// </summary>
    public async Task<ApiResponse<CompanionOrdersResponse>> GetCompanionOrdersAsync(int? page = 1, int? pageSize = 20, int? status = null)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return ApiResponse<CompanionOrdersResponse>.ErrorResponse(401, "未授权，请先登录");
            }

            var query = _context.Orders
                .Include(o => o.User)
                .Where(o => o.CompanionId == userId);

            // 状态筛选
            if (status.HasValue)
            {
                var statusText = status.Value switch
                {
                    1 => "待接单",
                    2 => "服务中",
                    3 => "已完成",
                    _ => ""
                };
                if (!string.IsNullOrEmpty(statusText))
                {
                    query = query.Where(o => o.Status == statusText);
                }
            }

            var total = await query.CountAsync();
            var orders = await query
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page ?? 1 - 1) * (pageSize ?? 20))
                .Take(pageSize ?? 20)
                .ToListAsync();

            var orderInfos = orders.Select(o => new CompanionOrderInfo
            {
                OrderId = o.Id,
                OrderNo = o.OrderNo,
                Status = OrderStatusToInt(o.Status),
                StatusText = o.Status,
                User = new UserInfo
                {
                    Id = o.User.Id,
                    Nickname = o.User.Nickname,
                    AvatarUrl = o.User.Avatar ?? ""
                },
                GameName = "游戏名称", // 需要根据游戏ID获取
                ServiceCount = o.DurationValue,
                ServiceTime = o.PlayTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                SpecialRequirements = o.Remark,
                TotalAmount = o.FinalPrice,
                CreatedAt = o.CreatedAt.ToDateTimeString(),
                Countdown = 0 // 需要根据时间计算
            }).ToList();

            var response = new CompanionOrdersResponse
            {
                Items = orderInfos,
                Pagination = new PaginationInfo
                {
                    Page = page ?? 1,
                    PageSize = pageSize ?? 20,
                    Total = total
                }
            };

            return ApiResponse<CompanionOrdersResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取接单列表失败");
            return ApiResponse<CompanionOrdersResponse>.ErrorResponse(500, "获取列表失败");
        }
    }

    /// <summary>
    /// 接受订单
    /// </summary>
    public async Task<ApiResponse<object>> AcceptOrderAsync(int orderId)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return ApiResponse<object>.Fail(401, "未授权，请先登录");
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.CompanionId == userId);

            if (order == null)
            {
                return ApiResponse<object>.Fail(404, "未找到订单");
            }

            if (order.Status != "待接单")
            {
                return ApiResponse<object>.Fail(400, "订单状态不正确，无法接受");
            }

            order.Status = "服务中";
            order.StartTime = DateTime.Now;
            order.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return ApiResponse<object>.Success("订单接受成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "接受订单失败");
            return ApiResponse<object>.Fail(500, "接受订单失败");
        }
    }

    /// <summary>
    /// 拒绝订单
    /// </summary>
    public async Task<ApiResponse<object>> RejectOrderAsync(int orderId, string rejectReason)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return ApiResponse<object>.Fail(401, "未授权，请先登录");
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.CompanionId == userId);

            if (order == null)
            {
                return ApiResponse<object>.Fail(404, "未找到订单");
            }

            if (order.Status != "待接单")
            {
                return ApiResponse<object>.Fail(400, "订单状态不正确，无法拒绝");
            }

            order.Status = "已取消";
            order.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return ApiResponse<object>.Success("订单拒绝成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "拒绝订单失败");
            return ApiResponse<object>.Fail(500, "拒绝订单失败");
        }
    }

    /// <summary>
    /// 开始服务
    /// </summary>
    public async Task<ApiResponse<object>> StartOrderAsync(int orderId)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return ApiResponse<object>.Fail(401, "未授权，请先登录");
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.CompanionId == userId);

            if (order == null)
            {
                return ApiResponse<object>.Fail(404, "未找到订单");
            }

            if (order.Status != "服务中")
            {
                return ApiResponse<object>.Fail(400, "订单状态不正确，无法开始服务");
            }

            order.Status = "服务中";
            order.StartTime = DateTime.Now;
            order.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return ApiResponse<object>.Success("服务开始成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "开始服务失败");
            return ApiResponse<object>.Fail(500, "开始服务失败");
        }
    }

    /// <summary>
    /// 完成服务
    /// </summary>
    public async Task<ApiResponse<object>> CompleteOrderAsync(int orderId, string? serviceSummary = null)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return ApiResponse<object>.Fail(401, "未授权，请先登录");
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.CompanionId == userId);

            if (order == null)
            {
                return ApiResponse<object>.Fail(404, "未找到订单");
            }

            if (order.Status != "服务中")
            {
                return ApiResponse<object>.Fail(400, "订单状态不正确，无法完成服务");
            }

            order.Status = "已完成";
            order.EndTime = DateTime.Now;
            order.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            return ApiResponse<object>.Success("服务完成成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "完成服务失败");
            return ApiResponse<object>.Fail(500, "完成服务失败");
        }
    }

    /// <summary>
    /// 获取收益统计
    /// </summary>
    public async Task<ApiResponse<EarningsResponse>> GetEarningsAsync()
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return ApiResponse<EarningsResponse>.ErrorResponse(401, "未授权，请先登录");
            }

            // 获取陪玩师信息
            var companion = await _context.Companions
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (companion == null)
            {
                return ApiResponse<EarningsResponse>.ErrorResponse(404, "未找到陪玩师信息");
            }

            // 统计订单数据
            var now = DateTime.Now;
            var todayStart = now.Date;
            var monthStart = new DateTime(now.Year, now.Month, 1);

            var todayOrders = await _context.Orders
                .CountAsync(o => o.CompanionId == companion.Id &&
                                o.CreatedAt >= todayStart &&
                                o.CreatedAt < todayStart.AddDays(1));

            var monthOrders = await _context.Orders
                .CountAsync(o => o.CompanionId == companion.Id &&
                                o.CreatedAt >= monthStart &&
                                o.CreatedAt < monthStart.AddMonths(1));

            var totalOrders = await _context.Orders
                .CountAsync(o => o.CompanionId == companion.Id);

            var completedOrders = await _context.Orders
                .Where(o => o.CompanionId == companion.Id && o.Status == "已完成")
                .SumAsync(o => o.FinalPrice);

            var pendingOrders = await _context.Orders
                .Where(o => o.CompanionId == companion.Id && o.Status == "服务中")
                .SumAsync(o => o.FinalPrice);

            var response = new EarningsResponse
            {
                TotalEarnings = completedOrders,
                MonthEarnings = completedOrders,
                TodayEarnings = completedOrders,
                PendingAmount = pendingOrders,
                WithdrawnAmount = completedOrders - pendingOrders,
                OrdersCount = totalOrders,
                MonthOrders = monthOrders,
                TodayOrders = todayOrders,
                Rating = companion.Rating ?? 0,
                PositiveRate = companion.GoodReviewRate ?? 0
            };

            return ApiResponse<EarningsResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取收益统计失败");
            return ApiResponse<EarningsResponse>.ErrorResponse(500, "获取统计失败");
        }
    }

    /// <summary>
    /// 申请提现
    /// </summary>
    public async Task<ApiResponse<object>> ApplyWithdrawAsync(WithdrawRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return ApiResponse<object>.Fail(401, "未授权，请先登录");
            }

            // 获取陪玩师信息
            var companion = await _context.Companions
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (companion == null)
            {
                return ApiResponse<object>.Fail(404, "未找到陪玩师信息");
            }

            // 检查是否已认证
            if (companion.Status != 1)
            {
                return ApiResponse<object>.Fail(400, "未认证的陪玩师无法申请提现");
            }

            // 检查可提现金额（这里简化处理，实际需要计算已完成订单的金额）
            var availableAmount = await _context.Orders
                .Where(o => o.CompanionId == companion.Id && o.Status == "已完成")
                .SumAsync(o => o.FinalPrice);

            if (availableAmount < request.Amount)
            {
                return ApiResponse<object>.Fail(400, "可提现金额不足");
            }

            // 创建提现记录（这里简化处理，实际需要创建提现记录表）
            _logger.LogInformation("用户 {UserId} 申请提现 {Amount} 元到 {Method} ({Account})",
                userId, request.Amount, request.WithdrawMethod, request.Account);

            return ApiResponse<object>.Success("提现申请已提交，请等待审核");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "申请提现失败");
            return ApiResponse<object>.Fail(500, "申请提现失败");
        }
    }

    /// <summary>
    /// 获取提现记录
    /// </summary>
    public async Task<ApiResponse<WithdrawRecordsResponse>> GetWithdrawRecordsAsync(int? page = 1, int? pageSize = 20)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return ApiResponse<WithdrawRecordsResponse>.ErrorResponse(401, "未授权，请先登录");
            }

            // 这里简化处理，返回空列表
            // 实际需要查询提现记录表
            var response = new WithdrawRecordsResponse
            {
                Items = new List<WithdrawRecordInfo>(),
                Pagination = new PaginationInfo
                {
                    Page = page ?? 1,
                    PageSize = pageSize ?? 20,
                    Total = 0
                }
            };

            return ApiResponse<WithdrawRecordsResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取提现记录失败");
            return ApiResponse<WithdrawRecordsResponse>.ErrorResponse(500, "获取记录失败");
        }
    }


    #region 移动端 mobile
    
    /// <summary>
    /// 适配前端的陪玩师列表查询（游戏筛选/服务类型映射/价格排序）
    /// </summary>
    public async Task<ApiResponse<CompanionListFrontResponse>> GetCompanionListForFrontAsync(CompanionListFrontRequest request)
    {
        try
        {
            var query = _context.Companions
                .AsNoTracking()
                .Where(c => c.Status == 1);

            int? filterGameId = request.GameId > 0 ? request.GameId : null;

            // 搜索关键词
            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                query = query.Where(c => c.Nickname.Contains(request.Keyword) || c.User.RealName.Contains(request.Keyword));
            }

            // 游戏筛选
            if (filterGameId.HasValue)
            {
                query = query.Where(c => _context.CompanionGames
                    .Any(g => g.CompanionId == c.Id && g.GameId == filterGameId.Value));
            }

            // ==============================================
            // 【修改 1】服务类型从 companion_games 表查询
            // ==============================================
            if (!string.IsNullOrWhiteSpace(request.ServiceType))
            {
                query = query.Where(c => _context.CompanionGames
                    .Any(g => g.CompanionId == c.Id && g.ServiceType == request.ServiceType));
            }

            // 等级
            if (request.Level.HasValue)
                query = query.Where(c => c.Level == request.Level.Value);

            // 在线状态
            if (request.OnlineStatus == 1)
                query = query.Where(c => c.OnlineStatus == "online");

            // ==============================================
            // 【修改 2】排序关联 companion_games 表
            // ==============================================
            query = request.Sort switch
            {
                "price_asc" => query.OrderBy(c => _context.CompanionGames
                    .Where(g => g.CompanionId == c.Id && (filterGameId == null || g.GameId == filterGameId.Value))
                    .Select(g => g.PricePerGame)
                    .FirstOrDefault()),
                "price_desc" => query.OrderByDescending(c => _context.CompanionGames
                    .Where(g => g.CompanionId == c.Id && (filterGameId == null || g.GameId == filterGameId.Value))
                    .Select(g => g.PricePerGame)
                    .FirstOrDefault()),
                "rating_asc" => query.OrderBy(c => c.Rating),
                "rating_desc" => query.OrderByDescending(c => c.Rating),
                "total_orders_asc" => query.OrderBy(c => c.TotalOrders),
                "total_orders_desc" => query.OrderByDescending(c => c.TotalOrders),
                _ => query.OrderByDescending(c => c.Rating)
            };

            // 总数
            var total = await query.CountAsync();

            // 🔥 只查需要的字段，性能最高
            var data = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(c => new
                {
                    c.Id,
                    c.Nickname,
                    c.UserId,
                    c.Level,
                    c.Tags,
                    c.Rating,
                    c.TotalOrders,
                    c.OnlineStatus,
                    // 游戏ID（筛选游戏/第一个游戏）
                    GameId = filterGameId ?? _context.CompanionGames
                        .Where(g => g.CompanionId == c.Id)
                        .Select(g => g.GameId)
                        .FirstOrDefault(),
                    // ==============================================
                    // 【修改 3】从 companion_games 取价格
                    // ==============================================
                    PricePerGame = _context.CompanionGames
                        .Where(g => g.CompanionId == c.Id && (filterGameId == null || g.GameId == filterGameId.Value))
                        .Select(g => g.PricePerGame)
                        .FirstOrDefault()
                })
                .ToListAsync();

            // ===========================
            // 批量查用户头像（1次DB）
            // ===========================
            var userIds = data.Select(x => x.UserId).Distinct().ToList();
            var avatarDic = await _context.Users
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.Avatar ?? "");

            // ===========================
            // 批量查段位等级（优化：确保dictType为game_level_${游戏id}）
            // ===========================
            // 1. 按游戏ID分组收集需要翻译的段位值
            var gameLevelGroups = new Dictionary<int, List<string>>(); // key: GameId, value: 段位值列表
            var levelDataMap = new Dictionary<string, List<int>>();    // key: "GameId_Level", value: 陪玩师ID列表

            foreach (var x in data.Where(x => x.GameId > 0 && x.Level.HasValue))
            {
                var gameId = x.GameId;
                var levelValue = x.Level.Value.ToString();
                
                // 按游戏ID分组存储需要翻译的段位值
                if (!gameLevelGroups.ContainsKey(gameId))
                {
                    gameLevelGroups[gameId] = new List<string>();
                }
                if (!gameLevelGroups[gameId].Contains(levelValue))
                {
                    gameLevelGroups[gameId].Add(levelValue);
                }

                // 记录段位值与陪玩师ID的关联
                var mapKey = $"{gameId}_{levelValue}";
                if (!levelDataMap.ContainsKey(mapKey))
                {
                    levelDataMap[mapKey] = new List<int>();
                }
                levelDataMap[mapKey].Add(x.Id);
            }

            // 2. 按游戏ID批量翻译（dictType = game_level_${游戏id}）
            var levelTrans = new Dictionary<string, string>(); // key: "GameId_Level", value: 翻译后的段位名称
            foreach (var (gameId, levels) in gameLevelGroups)
            {
                // 确保dictType格式为 game_level_${游戏id}
                var dictType = $"game_level_{gameId}";
                var transMap = await _dictTranslateService.BatchTranslateAsync(dictType, levels);
                
                // 构建全局段位翻译映射
                foreach (var level in levels)
                {
                    var mapKey = $"{gameId}_{level}";
                    levelTrans[mapKey] = transMap.TryGetValue(level, out var name) ? name : level;
                }
            }

            // ===========================
            // 组装结果
            // ===========================
            var list = data.Select(x =>
            {
                string levelName = string.Empty;
                if (x.GameId > 0 && x.Level.HasValue)
                {
                    var mapKey = $"{x.GameId}_{x.Level.Value}";
                    levelName = levelTrans.TryGetValue(mapKey, out var name) ? name : x.Level.ToString() ?? "";
                }

                return new CompanionItemFrontResponse
                {
                    Id = x.Id,
                    Nickname = x.Nickname ?? "",
                    AvatarUrl = avatarDic.TryGetValue(x.UserId, out var url) ? url : "",
                    Level = x.Level ?? null,
                    LevelName = levelName,
                    Tags = x.Tags?.Split(',').Where(t => !string.IsNullOrWhiteSpace(t)).ToList() ?? new List<string>(),
                    Price = x.PricePerGame, // 这里已经是从游戏表取的价格
                    PriceUnit = "局",
                    Rating = x.Rating ?? 0,
                    OrderCount = x.TotalOrders ?? 0,
                    OnlineStatus = x.OnlineStatus
                };
            }).ToList();

            return ApiResponse<CompanionListFrontResponse>.SuccessResponse(new CompanionListFrontResponse
            {
                Items = list,
                Pagination = new cPaginationInfo
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    Total = total,
                    HasMore = total > request.Page * request.PageSize
                }
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "前端陪玩师列表查询失败");
            return ApiResponse<CompanionListFrontResponse>.ErrorResponse(500, "获取失败", "获取失败");
        }
    }

    /// <summary>
    /// 获取陪玩师详情（适配前端）
    /// </summary>
    public async Task<ApiResponse<CompanionListDetailDto>> GetCompanionDetailAsync(int companionId, int userId)
    {
        var companion = await _context.Companions
            .Include(c => c.User)
            // 👇 加入背景墙轮播图关联 + 文件表
            .Include(c => c.CompanionBackgroundImages.Where(img => img.IsDeleted == 0))
                .ThenInclude(img => img.File)
            .FirstOrDefaultAsync(c => c.Id == companionId && c.Status == 1);

        if (companion == null)
            return ApiResponse<CompanionListDetailDto>.Fail(404, "陪玩师不存在或未认证");

        // 检查是否收藏
        var isFavorite = await _context.UserCollections
            .AnyAsync(f => f.UserId == userId && f.ItemId == companionId && f.ItemType == "companion");

        var detail = new CompanionListDetailDto
        {
            Id = companion.Id,
            Nickname = companion.Nickname,
            Avatar = companion.User?.Avatar ?? string.Empty,
            OnlineStatus = companion.OnlineStatus,
            IsVip = companion.User?.VipLevel > 0,
            Rating = companion.Rating ?? 0,
            OrderCount = companion.TotalOrders ?? 0,
            GoodRate = companion.GoodReviewRate ?? 100,
            Tags = companion.Tags?.Split(',').Where(t => !string.IsNullOrWhiteSpace(t)).ToList() ?? new(),
            Intro = companion.Bio ?? "这个人很懒，什么都没留下~",
            
            // 👇 核心：取出轮播图URL列表（按sort排序）
            BackgroundImages = companion.CompanionBackgroundImages
                .OrderBy(img => img.Sort)
                .Select(img => img.File?.FileUrl ?? string.Empty)
                .Where(url => !string.IsNullOrEmpty(url))
                .ToList(),

            Gallery = new List<string>(), // 原有相册
            IsFavorite = isFavorite
        };

        return ApiResponse<CompanionListDetailDto>.Success(detail);
    }

    /// <summary>
    /// 获取陪玩师服务列表
    /// </summary>
    public async Task<ApiResponse<List<CompanionServiceDto>>> GetCompanionServicesAsync(int companionId)
    {
        // 1. 校验陪玩师是否存在
        var companionExists = await _context.Companions
            .AnyAsync(c => c.Id == companionId && c.Status == 1);
        
        if (!companionExists)
            return ApiResponse<List<CompanionServiceDto>>.Fail(404, "陪玩师不存在");

        // 2. 从 companion_games 表查询该陪玩师的所有游戏服务（最新表结构）
        var gameServices = await _context.CompanionGames
            .Where(g => g.CompanionId == companionId)
            .Select(g => new CompanionServiceDto
            {
                Id = g.Id,  // 这里用游戏技能表ID
                GameId = g.GameId,
                GameName = g.Game.Name ?? "",
                GameLevel = g.GameLevel,
                LevelName = "",
                Price = g.PricePerGame, // 从 companion_games 取
                PriceUnit = "局",
                Duration = 60,
                ServiceType = g.ServiceType, // 从 companion_games 取
                ServiceTypeName = string.Empty
            })
            .ToListAsync();

        if (!gameServices.Any())
            return ApiResponse<List<CompanionServiceDto>>.Success(gameServices);

        // 3. 批量翻译服务类型字典（原逻辑不变）
        var serviceTypes = gameServices
            .Select(x => x.ServiceType)
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct()
            .ToList();

        var serviceTypeMap = await _dictTranslateService.BatchTranslateAsync("service_type", serviceTypes);

        foreach (var item in gameServices)
        {
            item.ServiceTypeName = serviceTypeMap.TryGetValue(item.ServiceType!, out var sName) ? sName : item.ServiceType!;
        }

        // 4. 返回
        return ApiResponse<List<CompanionServiceDto>>.Success(gameServices);
    }

    /// <summary>
    /// 获取陪玩师评价列表
    /// </summary>
    public async Task<ApiResponse<List<CompanionReviewDto>>> GetCompanionReviewsAsync(int companionId, int page, int pageSize)
    {
        var reviews = await _context.OrderReviews
            .Include(r => r.User)
            .Where(r => r.CompanionId == companionId)
            .OrderByDescending(r => r.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var reviewList = reviews.Select(r => new CompanionReviewDto
        {
            Id = r.Id,
            UserAvatar = r.User?.Avatar ?? string.Empty,
            UserName = r.User?.Nickname ?? "匿名用户",
            Score = r.Rating == null ? 5 : (int)r.Rating,
            Content = r.Content ?? "好评",
            // Images = r.Images?.Split(',').Where(i => !string.IsNullOrWhiteSpace(i)).ToList() ?? new(),
            CreateTime = r.CreatedAt?.ToString("yyyy-MM-dd HH:mm") ?? string.Empty
        }).ToList();

        return ApiResponse<List<CompanionReviewDto>>.Success(reviewList);
    }


    #endregion

    #region 管理端 PC

    /// <summary>
    /// 获取陪玩师详情（适配后台）
    /// </summary>
    public async Task<ApiResponse<AdminCompanionDetailDto>> GetCompanionDetailAsync(int companionId, int userId, bool ignoreStatus = false)
    {
        var companion = await _context.Companions
            .Include(c => c.User)
            .Include(c => c.CompanionGames)
                .ThenInclude(cg => cg.Game)
            .Include(c => c.CompanionBackgroundImages.Where(img => img.IsDeleted == 0))
                .ThenInclude(img => img.File)
            .FirstOrDefaultAsync(c => c.Id == companionId && (ignoreStatus || c.Status == 1));

        if (companion == null)
            return ApiResponse<AdminCompanionDetailDto>.Fail(404, "陪玩师不存在或未认证");

        // 检查是否收藏
        var isFavorite = await _context.UserCollections
            .AnyAsync(f => f.UserId == userId && f.ItemId == companionId && f.ItemType == "companion");

        // ===================== 翻译逻辑（只在DTO用，不修改实体） =====================
        var gameSkills = companion.CompanionGames.ToList();
        var gameSkillItems = new List<GameSkillItem>();

        if (gameSkills.Any())
        {
            // 1. 批量翻译服务类型
            var serviceTypes = gameSkills
                .Select(g => g.ServiceType)
                .Where(t => !string.IsNullOrEmpty(t))
                .Distinct()
                .ToList();
            var serviceTypeMap = await _dictTranslateService.BatchTranslateAsync("service_type", serviceTypes);

            // 2. 批量翻译段位（按游戏ID）
            var levelTasks = new Dictionary<int, Task<Dictionary<string, string>>>();
            foreach (var g in gameSkills)
            {
                if (!levelTasks.ContainsKey(g.GameId))
                {
                    var dictType = $"game_level_{g.GameId}";
                    levelTasks[g.GameId] = _dictTranslateService.BatchTranslateAsync(dictType, new List<string> { g.GameLevel });
                }
            }
            await Task.WhenAll(levelTasks.Values);

            // 3. 逐个组装DTO并赋值翻译结果
            foreach (var g in gameSkills)
            {
                // 服务类型名称
                string serviceTypeName = "";
                if (!string.IsNullOrEmpty(g.ServiceType) && serviceTypeMap.TryGetValue(g.ServiceType, out var sName))
                {
                    serviceTypeName = sName;
                }

                // 段位名称
                string gameRankName = "";
                if (levelTasks.TryGetValue(g.GameId, out var levelTask))
                {
                    var levelMap = levelTask.Result;
                    if (!string.IsNullOrEmpty(g.GameLevel) && levelMap.TryGetValue(g.GameLevel, out var lName))
                    {
                        gameRankName = lName;
                    }
                }

                // 组装DTO（这里才是真正赋值的地方，不碰实体）
                gameSkillItems.Add(new GameSkillItem
                {
                    GameId = g.GameId,
                    GameName = g.Game.Name ?? "",
                    GameRank = g.GameLevel ?? "",
                    GameRankName = gameRankName,
                    ServiceType = g.ServiceType,
                    ServiceTypeName = serviceTypeName,
                    Price = g.PricePerGame ?? 0,
                    PriceUnit = "局"
                });
            }
        }
        // ===================== 翻译逻辑结束 =====================

        var detail = new AdminCompanionDetailDto
        {
            RealName = companion.RealName,
            IdCard = companion.IdCard,
            IdCardFrontUrl = companion.IdCardFront ?? string.Empty,
            IdCardBackUrl = companion.IdCardBack ?? string.Empty,
            Phone = companion.Phone,
            Nickname = companion.Nickname,
            Bio = companion.Bio,
            Tags = companion.Tags?.Split(',')?.ToList() ?? new(),
            CreatedAt = companion.CreatedAt.ToDateTimeString(),
            GameSkills = gameSkillItems, // 直接用组装好的DTO列表
            BackgroundImages = companion.CompanionBackgroundImages
                .OrderBy(img => img.Sort)
                .Select(img => img.File?.FileUrl ?? string.Empty)
                .Where(url => !string.IsNullOrEmpty(url))
                .ToList(),
        };

        return ApiResponse<AdminCompanionDetailDto>.Success(detail);
    }


    /// <summary>
    /// 获取陪玩师列表（高性能优化版 - 双字典翻译）
    /// </summary>
    public async Task<ApiResponse<CompanionListResponse>> GetCompanionListAsync(CompanionListRequest request)
    {
        try
        {
            // 基础查询（高性能：无跟踪、预生成SQL）
            var query = _context.Companions
                .AsNoTracking()
                .Include(x => x.CompanionGames)
                    .ThenInclude(cg => cg.Game)
                .AsQueryable();

            // 条件过滤
            if (int.TryParse(request.Status?.Trim(), out int status))
                query = query.Where(x => x.Status == status);

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                var k = request.Keyword.Trim();
                query = query.Where(x =>
                    x.Nickname.Contains(k) ||
                    x.RealName.Contains(k) ||
                    x.Phone.Contains(k) ||
                    x.User.Nickname.Contains(k) ||
                    x.User.Username.Contains(k));
            }

            if (request.GameId > 0)
                query = query.Where(x => x.CompanionGames.Any(g => g.GameId == request.GameId));

            if (!string.IsNullOrWhiteSpace(request.ServiceType))
                query = query.Where(x => x.CompanionGames.Any(g => g.ServiceType == request.ServiceType));

            if (request.StartTime.HasValue)
                query = query.Where(x => x.CreatedAt >= request.StartTime.Value);
            if (request.EndTime.HasValue)
                query = query.Where(x => x.CreatedAt <= request.EndTime.Value);

            // 总条数（只查 count，超快）
            var total = await query.CountAsync();

            // 分页查询（只查需要的字段，超快）
            var list = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CompanionListDto
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Nickname = x.Nickname,
                    RealName = x.User.RealName ?? "",
                    Phone = x.Phone,
                    Level = x.Level ?? null,
                    Status = x.Status ?? 0,
                    StatusName = "",      // 审核状态翻译占位
                    Rating = x.Rating,
                    TotalOrders = x.TotalOrders,
                    GoodReviewRate = x.GoodReviewRate,
                    Tags = x.Tags,
                    OnlineStatus = x.OnlineStatus ?? "",
                    CreatedAt = x.CreatedAt.ToDateTimeString(),

                    // 游戏关联
                    Games = x.CompanionGames.Select(cg => new CompanionGameItemDto
                    {
                        GameId = cg.GameId,
                        GameName = cg.Game.Name,
                        GameIcon = cg.Game.Icon ?? "",
                        GameLevel = cg.GameLevel ?? "",
                        ServiceType = cg.ServiceType ?? "",
                        ServiceTypeName = "",  // 占位，后面批量翻译
                        PricePerGame = cg.PricePerGame,
                        PricePerHour = cg.PricePerHour
                    }).ToList()
                })
                .ToListAsync();

            // ==============================================
            // 🔥 修正：批量翻译【游戏里的 ServiceTypeName】
            // ==============================================
            if (list.Count > 0)
            {
                // 1. 提取所有游戏中的 ServiceType（去重）
                var allServiceTypes = list
                    .SelectMany(x => x.Games)
                    .Select(g => g.ServiceType)
                    .Where(t => !string.IsNullOrEmpty(t))
                    .Distinct()
                    .ToList();

                // 2. 提取审核状态
                var statusValues = list
                    .Select(x => x.Status.ToString())
                    .Distinct()
                    .ToList();

                // 3. 批量翻译
                var serviceTypeMap = await _dictTranslateService.BatchTranslateAsync("service_type", allServiceTypes);
                var statusMap = await _dictTranslateService.BatchTranslateAsync("review_status", statusValues);

                // 4. 赋值：审核状态
                foreach (var item in list)
                {
                    var statusKey = item.Status.ToString();
                    item.StatusName = statusMap.TryGetValue(statusKey, out var stName) ? stName : statusKey;
                }

                // 5. 赋值：游戏服务类型名称（核心修正）
                foreach (var companion in list)
                {
                    foreach (var game in companion.Games)
                    {
                        if (!string.IsNullOrEmpty(game.ServiceType))
                        {
                            game.ServiceTypeName = serviceTypeMap.TryGetValue(game.ServiceType, out var name) 
                                ? name 
                                : game.ServiceType;
                        }
                    }
                }
            }

            // 返回结果
            return ApiResponse<CompanionListResponse>.Success(new CompanionListResponse
            {
                Total = total,
                Page = request.Page,
                PageSize = request.PageSize,
                list = list
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取陪玩师列表失败");
            return ApiResponse<CompanionListResponse>.Fail(500, "获取列表失败");
        }
    }


    /// <summary>
    /// 获取陪玩认证审核统计（按状态分组）
    /// </summary>
    public async Task<ApiResponse<List<CompanionStatusCountDto>>> GetCompanionStatusCountAsync()
    {
        try
        {
            // 按状态分组统计数量
            var countList = await _context.Companions
                .AsNoTracking()
                .GroupBy(x => x.Status)
                .Select(g => new CompanionStatusCountDto
                {
                    Status = g.Key ?? 0,
                    Count = g.Count()
                })
                .ToListAsync();

            // 补全 0/1/2 三个状态（即使数量为0也返回）
            var allStatus = new List<int> { 0, 1, 2 };
            var result = allStatus.Select(status => new CompanionStatusCountDto
            {
                Status = status,
                Count = countList.FirstOrDefault(x => x.Status == status)?.Count ?? 0
            }).ToList();

            return ApiResponse<List<CompanionStatusCountDto>>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取陪玩状态统计失败");
            return ApiResponse<List<CompanionStatusCountDto>>.Fail(500, "获取统计失败");
        }
    }

    /// <summary>
    /// 审核陪玩师
    /// </summary>
    /// <param name="auditDto">审核信息</param>
    /// <returns>审核结果</returns>
    public async Task<ApiResponse> AuditCompanionAsync(CompanionAuditDto auditDto)
    {
        // 1. 验证参数（拒绝时必须填写拒绝原因）
        if (auditDto.Status == 2 && string.IsNullOrWhiteSpace(auditDto.RejectReason))
        {
            return ApiResponse.Fail(400, "审核拒绝时必须填写拒绝原因");
        }

        // 2. 查询陪玩师信息
        var companion = await _context.Set<Companion>()
            .FirstOrDefaultAsync(c => c.Id == auditDto.CompanionId);

        if (companion == null)
        {
            return ApiResponse.Fail(404, "陪玩师不存在");
        }

        // 3. 检查当前状态是否为待审核（0）
        if (companion.Status != 0)
        {
            return ApiResponse.Fail(400, $"无法审核（仅待审核状态可审核）");
        }

        // 4. 更新审核状态
        companion.Status = auditDto.Status;
        companion.RejectReason = auditDto.Status == 2 ? auditDto.RejectReason : null;
        companion.UpdatedAt = DateTime.Now;

        // 5. 保存数据库
        await _context.SaveChangesAsync();

        // 6. 返回结果
        var message = auditDto.Status == 1 ? "审核通过" : "审核拒绝";
        return ApiResponse.Success($"陪玩师{message}成功");
    }
    
    #endregion

    #region 私有方法

    /// <summary>
    /// 获取当前用户ID（这里简化处理，实际应该从认证上下文获取）
    /// </summary>
    private int GetCurrentUserId()
    {
        // 这里应该从 HttpContext.User.Claims 中获取用户ID
        // 为了演示，返回一个模拟的用户ID
        return 1; // 模拟用户ID
    }

    /// <summary>
    /// 转换认证状态为数字
    /// </summary>
    private int GetCertificationStatus(int status)
    {
        return status switch
        {
            0 => 0,
            1 => 1,
            2 => 2,
            _ => 0
        };
    }

    /// <summary>
    /// 获取认证状态文本
    /// </summary>
    private string GetCertificationStatusText(int status)
    {
        return status switch
        {
            0 => "待审核",
            1 => "已认证",
            2 => "已拒绝",
            _ => "未知"
        };
    }

    /// <summary>
    /// 转换在线状态为数字
    /// </summary>
    private int OnlineStatusToInt(string status)
    {
        return status switch
        {
            "离线" => 0,
            "在线" => 1,
            "忙碌" => 2,
            _ => 0
        };
    }

    /// <summary>
    /// 转换订单状态为数字
    /// </summary>
    private int OrderStatusToInt(string status)
    {
        return status switch
        {
            "待接单" => 1,
            "服务中" => 2,
            "已完成" => 3,
            "已取消" => 4,
            _ => 0
        };
    }

    #endregion
}