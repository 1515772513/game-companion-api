using GameCompanion.Api.DTOs.Companion;
using GameCompanion.Api.Models.Entities;
using GameCompanion.Api.Models;
using Microsoft.EntityFrameworkCore;
using GameCompanion.Api.Data;
using GameCompanion.Api.Utils;

namespace GameCompanion.Api.Services;

/// <summary>
/// 陪玩师服务实现
/// </summary>
public class CompanionService : ICompanionService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CompanionService> _logger;

    public CompanionService(ApplicationDbContext context, ILogger<CompanionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// 申请成为陪玩师
    /// </summary>
    public async Task<ApiResponse<ApplicationStatusResponse>> ApplyCompanionAsync(ApplyCompanionRequest request)
    {
        try
        {
            // 获取当前用户（这里假设用户已经通过认证）
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return ApiResponse<ApplicationStatusResponse>.ErrorResponse(401, "未授权，请先登录");
            }

            // 检查是否已经申请过
            var existingApplication = await _context.Companions
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (existingApplication != null && existingApplication.Status == 0)
            {
                return ApiResponse<ApplicationStatusResponse>.ErrorResponse(400, "您已经有待审核的申请，请耐心等待");
            }

            // 检查是否已认证
            if (existingApplication != null && existingApplication.Status == 1)
            {
                return ApiResponse<ApplicationStatusResponse>.ErrorResponse(400, "您已经是认证陪玩师，无需再次申请");
            }

            // 创建陪玩师申请记录
            var companion = new Companion
            {
                UserId = userId,
                RealName = request.RealName,
                IdCard = request.IdCard,
                IdCardFront = request.IdCardFrontUrl,
                IdCardBack = request.IdCardBackUrl,
                Phone = request.Phone,
                Nickname = request.Nickname,
                ServiceType = request.ServiceType,
                PricePerGame = request.Price,
                Bio = request.Bio,
                Tags = request.Tags != null ? string.Join(",", request.Tags) : null,
                Status = 0,
                OnlineStatus = "离线",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Companions.Add(companion);
            await _context.SaveChangesAsync();

            // 创建游戏技能记录
            foreach (var gameId in request.Games)
            {
                var companionGame = new CompanionGame
                {
                    CompanionId = companion.Id,
                    GameId = gameId,
                    GameLevel = request.GameRank,
                    CreatedAt = DateTime.UtcNow
                };
                _context.CompanionGames.Add(companionGame);
            }
            await _context.SaveChangesAsync();

            var response = new ApplicationStatusResponse
            {
                ApplicationId = companion.Id,
                CertificationStatus = 0,
                CertificationStatusText = "待审核",
                CertificationApplyTime = companion.CreatedAt.ToDateTimeString()
            };

            return ApiResponse<ApplicationStatusResponse>.SuccessResponse(response, "申请提交成功，请等待审核");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "陪玩师申请失败");
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
                .Include(c => c.Games)
                .ThenInclude(g => g.Game)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (companion == null || companion.Status != 1)
            {
                return ApiResponse<CompanionInfoResponse>.ErrorResponse(404, "未找到陪玩师信息或未认证");
            }

            var response = new CompanionInfoResponse
            {
                Id = companion.Id,
                UserId = companion.UserId,
                Nickname = companion.Nickname,
                AvatarUrl = companion.User?.Avatar ?? "",
                Level = companion.Level ?? "银牌",
                ServiceType = companion.ServiceType ?? "技术陪玩",
                Price = companion.PricePerGame,
                Rating = companion.Rating ?? 0,
                OrderCount = companion.TotalOrders ?? 0,
                RatingCount = 0, // 需要根据评价记录计算
                PositiveRate = companion.GoodReviewRate ?? 0,
                OnlineStatus = OnlineStatusToInt(companion.OnlineStatus),
                IsVerified = companion.Status == 1,
                Games = companion.Games.Select(g => g.Game.Name).ToList(),
                GameRank = companion.Games.FirstOrDefault()?.GameLevel ?? "",
                Bio = companion.Bio ?? "",
                Tags = companion.Tags?.Split(',').ToList() ?? new List<string>(),
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

            // 更新信息
            if (request.Nickname != null) companion.Nickname = request.Nickname;
            if (request.ServiceType != null) companion.ServiceType = request.ServiceType;
            if (request.Price.HasValue) companion.PricePerGame = request.Price.Value;
            if (request.Bio != null) companion.Bio = request.Bio;
            if (request.Tags != null) companion.Tags = string.Join(",", request.Tags);

            companion.UpdatedAt = DateTime.UtcNow;
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
            companion.UpdatedAt = DateTime.UtcNow;
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
            order.StartTime = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;
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
            order.UpdatedAt = DateTime.UtcNow;
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
            order.StartTime = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;
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
            order.EndTime = DateTime.UtcNow;
            order.UpdatedAt = DateTime.UtcNow;
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
            var now = DateTime.UtcNow;
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


    /// <summary>
    /// 获取陪玩师列表
    /// </summary>
    public async Task<ApiResponse<CompanionListResponse>> GetCompanionListAsync(CompanionListRequest request)
    {
        try
        {
            var query = _context.Companions
                .AsNoTracking()
                .AsQueryable();

            int? status = null;
            if (!string.IsNullOrWhiteSpace(request.Status) && int.TryParse(request.Status, out var s))
            {
                status = s;
            }
            if (status.HasValue)
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

            if (request.GameId.HasValue)
                query = query.Where(x => x.CompanionGames.Any(g => g.GameId == request.GameId));

            if (!string.IsNullOrWhiteSpace(request.ServiceType))
                query = query.Where(x => x.ServiceType == request.ServiceType);

            if (request.StartTime.HasValue)
                query = query.Where(x => x.CreatedAt >= request.StartTime.Value);
            if (request.EndTime.HasValue)
                query = query.Where(x => x.CreatedAt <= request.EndTime.Value);

            var total = await query.CountAsync();

            var list = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)

                // 👇 重要：EF Core 自动在 Select 里加载关联，不需要 Include！
                .Select(x => new CompanionListDto
                {
                    Id = x.Id,
                    UserId = x.UserId,
                    Nickname = x.Nickname,
                    RealName = x.User.RealName ?? "",
                    Phone = x.Phone,
                    Level = x.Level ?? "",
                    ServiceType = x.ServiceType ?? "",
                    PricePerGame = x.PricePerGame,
                    PricePerHour = x.PricePerHour,
                    Rating = x.Rating,
                    TotalOrders = x.TotalOrders,
                    GoodReviewRate = x.GoodReviewRate,
                    Tags = x.Tags,
                    Status = x.Status ?? 0,
                    OnlineStatus = x.OnlineStatus ?? "",
                    CreatedAt = x.CreatedAt.ToDateTimeString(),

                    // 这里 EF 会自动关联 Game，不会报错！
                    Games = x.CompanionGames.Select(cg => new CompanionGameItemDto
                    {
                        GameId = cg.GameId,
                        GameName = cg.Game.Name,
                        GameIcon = cg.Game.Icon ?? "",
                        GameLevel = cg.GameLevel ?? "",
                    }).ToList()
                })
                .ToListAsync();

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