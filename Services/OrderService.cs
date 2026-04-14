using GameCompanion.Api.DTOs.Order;
using GameCompanion.Api.Data;
using GameCompanion.Api.Models;
using GameCompanion.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using GameCompanion.Api.Utils;
using GameCompanion.Api.Services.DictTranslate;
using GameCompanion.Api.Dtos;

namespace GameCompanion.Api.Services;

/// <summary>
/// 订单服务实现
/// </summary>
public class OrderService : IOrderService
{
    private readonly GameCompanionContext _context;
    private readonly ILogger<OrderService> _logger;

    private readonly IDictTranslateService _dictTranslateService;

    public OrderService(GameCompanionContext context, ILogger<OrderService> logger, IDictTranslateService dictTranslateService)
    {
        _context = context;
        _logger = logger;
        _dictTranslateService = dictTranslateService;
    }

    /// <summary>
    /// 创建订单
    /// </summary>
    public async Task<ApiResponse<CreateOrderResponse>> CreateOrderAsync(CreateOrderRequest request)
    {
        try
        {
            // 1. 基础参数校验
            if (request == null)
            {
                return ApiResponse<CreateOrderResponse>.ErrorResponse(400, "请求参数不能为空");
            }
            if (request.CompanionId <= 0 || request.GameId <= 0 || request.ServiceCount <= 0)
            {
                return ApiResponse<CreateOrderResponse>.ErrorResponse(400, "陪玩师ID、游戏ID、服务数量不能为空且必须为正数");
            }

            // 2. 验证陪玩师状态（存在、已认证、在线接单）
            var companion = await _context.Companions
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == request.CompanionId);
            
            if (companion == null)
            {
                return ApiResponse<CreateOrderResponse>.ErrorResponse(2001, "陪玩师不存在");
            }
            if (companion.Status != 1) // 1=已认证
            {
                return ApiResponse<CreateOrderResponse>.ErrorResponse(2002, "陪玩师未认证，无法接单");
            }
            if (companion.OnlineStatus != "online") // 确保状态值和字典一致
            {
                return ApiResponse<CreateOrderResponse>.ErrorResponse(2003, "陪玩师暂不接单（当前状态：离线/忙碌）");
            }

            // 3. 验证游戏是否存在
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == request.GameId);
            if (game == null)
            {
                return ApiResponse<CreateOrderResponse>.ErrorResponse(3004, "游戏不存在");
            }

            // 4. 核心修改：验证陪玩师是否开通该游戏服务，并获取对应单价
            var companionGame = await _context.CompanionGames
                .AsNoTracking() // 无跟踪查询，减少EF解析压力
                .Where(cg => cg.CompanionId == request.CompanionId && cg.GameId == request.GameId)
                .Select(cg => new { cg.PricePerGame }) // 只查需要的字段，避免映射冲突
                .FirstOrDefaultAsync();
            
            if (companionGame == null)
            {
                return ApiResponse<CreateOrderResponse>.ErrorResponse(3007, $"陪玩师未开通【{game.Name}】的服务");
            }
            if (companionGame.PricePerGame <= 0)
            {
                return ApiResponse<CreateOrderResponse>.ErrorResponse(3008, $"陪玩师【{game.Name}】的服务未设置价格，请联系客服");
            }

            // 5. 预约时间校验（格式 + 提前30分钟）
            if (!DateTime.TryParseExact(request.ServiceTime, "yyyy-MM-dd HH:mm:ss", 
                System.Globalization.CultureInfo.InvariantCulture, 
                System.Globalization.DateTimeStyles.None, out var serviceTime))
            {
                return ApiResponse<CreateOrderResponse>.ErrorResponse(3006, "预约时间格式错误，正确格式：yyyy-MM-dd HH:mm:ss");
            }
            // 统一使用UTC时间，避免时区问题
            var utcServiceTime = TimeZoneInfo.ConvertTimeToUtc(serviceTime);
            // if (utcServiceTime <= DateTime.UtcNow.AddMinutes(30))
            // {
            //     return ApiResponse<CreateOrderResponse>.ErrorResponse(3006, "预约时间必须至少提前30分钟");
            // }

            // 6. 检查陪玩师服务时间冲突（核心：避免同一时间段接单）
            var conflictingOrder = await _context.Orders
                .Where(o => o.CompanionId == request.CompanionId)
                .Where(o => o.Status != "已取消" && o.Status != "退款/售后") // 排除已取消/退款的订单
                .Where(o => o.StartTime.HasValue && o.EndTime.HasValue)
                .Where(o => 
                    // 新订单开始时间 在 已有订单的时间范围内（前后缓冲30分钟）
                    utcServiceTime >= o.StartTime.Value.AddMinutes(-30) && 
                    utcServiceTime <= o.EndTime.Value.AddMinutes(30)
                )
                .FirstOrDefaultAsync();
            
            if (conflictingOrder != null)
            {
                return ApiResponse<CreateOrderResponse>.ErrorResponse(3005, $"陪玩师该时间段已接单（订单号：{conflictingOrder.OrderNo}），请更换时间");
            }

            // 7. 价格计算（核心：从companionGame取单价）
            var unitPrice = companionGame.PricePerGame; // 从关联表获取单价
            var totalPrice = unitPrice * request.ServiceCount; // 总价 = 单价 * 数量
            var discountAmount = CalculateDiscount((totalPrice ?? 0), request.ServiceCount); // 计算优惠
            var finalAmount = totalPrice - discountAmount; // 实付金额
            if (finalAmount <= 0)
            {
                return ApiResponse<CreateOrderResponse>.ErrorResponse(3009, "实付金额不能为0，请检查服务数量和价格");
            }

            // 8. 校验用户余额
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return ApiResponse<CreateOrderResponse>.ErrorResponse(404, "用户不存在");
            }
            if (user.Balance < finalAmount)
            {
                return ApiResponse<CreateOrderResponse>.ErrorResponse(3003, $"余额不足（当前余额：{user.Balance:F2}，需支付：{finalAmount:F2}）");
            }

            // 9. 生成订单号 + 计算服务时长
            var orderNo = GenerateOrderNo();
            var durationType = request.ServiceCount == 1 ? "game" : "hour"; // 对齐数据库enum类型（game=局，hour=小时）
            var (startTime, endTime) = CalculateServiceTimeRange(utcServiceTime, durationType, request.ServiceCount);

            // 10. 创建订单实体
            var order = new Order
            {
                OrderNo = orderNo,
                UserId = request.UserId,
                CompanionId = request.CompanionId,
                GameId = request.GameId,
                ServiceType = "1", // 对齐数据库enum（1=陪玩）
                PlayTime = utcServiceTime,
                DurationType = durationType,
                DurationValue = request.ServiceCount,
                UnitPrice = unitPrice ?? 0,
                TotalPrice = totalPrice ?? 0,
                DiscountAmount = discountAmount,
                FinalPrice = finalAmount ?? 0,
                Remark = request.SpecialRequirements,
                Status = "0", // 0=待付款（对齐数据库状态定义）
                PayTime = null,
                StartTime = startTime,
                EndTime = endTime,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            // 11. 保存订单（建议用事务，避免部分保存）
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                _logger.LogError(ex, "创建订单保存失败（订单号：{OrderNo}）", orderNo);
                return ApiResponse<CreateOrderResponse>.ErrorResponse(500, "订单创建失败，请重试");
            }

            // 12. 构造返回结果
            var paymentUrl = GeneratePaymentUrl(orderNo);
            var response = new CreateOrderResponse
            {
                OrderId = order.Id,
                OrderNo = orderNo,
                CompanionId = companion.Id,
                CompanionName = companion.Nickname,
                GameName = game.Name,
                ServiceCount = request.ServiceCount,
                ServiceTime = serviceTime.ToString("yyyy-MM-dd HH:mm:ss"), // 转回本地时间展示
                UnitPrice = unitPrice,
                TotalAmount = totalPrice,
                ServiceFee = 0, // 可根据业务调整
                DiscountAmount = discountAmount,
                FinalAmount = finalAmount,
                Status = 1, // 1=待付款（前端展示状态）
                StatusText = "待付款",
                PaymentTimeout = 1800, // 30分钟超时
                PaymentUrl = paymentUrl,
                CreatedAt = order.CreatedAt.ToDateTimeString()
            };

            return ApiResponse<CreateOrderResponse>.SuccessResponse(response, "订单创建成功，请尽快支付");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "创建订单异常（UserId：{UserId}，CompanionId：{CompanionId}）", 
                request?.UserId, request?.CompanionId);
            return ApiResponse<CreateOrderResponse>.ErrorResponse(500, "系统错误，请联系客服");
        }
    }

    /// <summary>
    /// 获取订单列表
    /// </summary>
    public async Task<ApiResponse<GetOrdersResponse>> GetOrdersAsync(GetOrdersRequest request)
    {
        try
        {
            var query = _context.Orders
                .Include(o => o.User)
                .Include(o => o.Companion)
                .Include(o => o.Game)
                .Where(o => o.UserId == request.UserId);

            // 状态筛选
            if (request.Status.HasValue)
            {
                query = query.Where(o => GetOrderStatusValue(o.Status) == request.Status.Value);
            }

            // 排序
            var sortBy = request.SortBy.ToLower();
            var sortOrder = request.SortOrder.ToLower();

            query = sortBy switch
            {
                "service_time" => sortOrder == "asc" ? query.OrderBy(o => o.PlayTime) : query.OrderByDescending(o => o.PlayTime),
                _ => sortOrder == "asc" ? query.OrderBy(o => o.CreatedAt) : query.OrderByDescending(o => o.CreatedAt)
            };

            // 分页
            var total = await query.CountAsync();
            var orders = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .ToListAsync();

            var response = new GetOrdersResponse
            {
                Items = orders.Select(o => new GetOrdersResponse.OrderItem
                {
                    Id = o.Id,
                    OrderNo = o.OrderNo,
                    OrderType = "1", // 1-陪玩订单
                    OrderTypeText = "陪玩订单",
                    Status = o.Status,
                    StatusText = o.Status,
                    PaymentStatus = GetPaymentStatusValue(o.Status),
                    PaymentStatusText = GetPaymentStatusText(o.Status),
                    Companion = new GetOrdersResponse.CompanionInfo
                    {
                        Id = o.Companion.Id,
                        Nickname = o.Companion.Nickname,
                        // AvatarUrl = o.Companion.AvatarUrl ?? "",
                        Level = o.Companion.Level?.ToString() ?? "",
                    },
                    GameName = o.Game?.Name ?? "",
                    GameRank = "",
                    ServiceCount = o.DurationValue,
                    ServiceTime = o.PlayTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                    TotalAmount = o.FinalPrice,
                    CreatedAt = o.CreatedAt.ToDateTimeString()
                }).ToList(),
                Pagination = new GetOrdersResponse.OrderPagination
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    Total = total,
                    TotalPages = (int)Math.Ceiling((double)total / request.PageSize),
                    HasMore = request.Page * request.PageSize < total
                }
            };

            return ApiResponse<GetOrdersResponse>.SuccessResponse(response, "获取成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取订单列表失败");
            return ApiResponse<GetOrdersResponse>.ErrorResponse(500, "系统错误");
        }
    }

    /// <summary>
    /// 获取订单详情
    /// </summary>
    public async Task<ApiResponse<GetOrderResponse>> GetOrderAsync(int orderId)
    {
        try
        {
            var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Companion)
                    .ThenInclude(c => c.User)
                .Include(o => o.Game)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null)
            {
                return ApiResponse<GetOrderResponse>.ErrorResponse(3001, "订单不存在");
            }

            // 检查权限（这里简化处理，实际应该检查当前用户是否是订单的创建者）
            var currentUser = await _context.Users.FindAsync(1); // 简化处理
            if (currentUser == null || order.UserId != currentUser.Id)
            {
                return ApiResponse<GetOrderResponse>.ErrorResponse(403, "无权访问");
            }

            var response = new GetOrderResponse
            {
                Id = order.Id,
                OrderNo = order.OrderNo,
                OrderType = 1,
                OrderTypeText = "陪玩订单",
                Status = GetOrderStatusValue(order.Status),
                StatusText = order.Status,
                PaymentStatus = GetPaymentStatusValue(order.Status),
                PaymentStatusText = GetPaymentStatusText(order.Status),
                Companion = new GetOrderResponse.CompanionDetail
                {
                    Id = order.Companion.Id,
                    UserId = order.Companion.UserId,
                    Nickname = order.Companion.Nickname,
                    AvatarUrl = order.Companion.User?.Avatar ?? "",
                    Level = order.Companion.Level ?? null,
                    Phone = order.Companion.Phone ?? ""
                },
                Game = new GetOrderResponse.GameInfo
                {
                    Id = order.Game.Id,
                    Name = order.Game.Name
                },
                GameRank = "",
                ServiceCount = order.DurationValue,
                ServiceUnit = order.DurationType ?? "局",
                ServiceTime = order.PlayTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                SpecialRequirements = order.Remark ?? "",
                UnitPrice = order.UnitPrice,
                ServiceFee = 0,
                DiscountAmount = order.DiscountAmount ?? 0,
                TotalAmount = order.TotalPrice,
                FinalAmount = order.FinalPrice,
                PaymentMethod = "balance",
                PaymentMethodText = "余额支付",
                PaymentTime = order.PayTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                CreatedAt = order.CreatedAt.ToDateTimeString(),
                UpdatedAt = order.UpdatedAt.ToDateTimeString(),
                Countdown = new GetOrderResponse.CountdownInfo
                {
                    ServiceStartIn = order.StartTime.HasValue ?
                        (int)(order.StartTime.Value - DateTime.UtcNow).TotalSeconds : 0,
                    AutoConfirmIn = 0
                },
                Actions = new GetOrderResponse.OrderActions
                {
                    CanCancel = order.Status == "待付款" || order.Status == "待服务",
                    CanRefund = order.Status == "已完成" && DateTime.UtcNow <= order.PayTime?.AddHours(24),
                    CanConfirm = order.Status == "服务中",
                    CanReview = order.Status == "已完成" && !HasReviewed(order.Id)
                },
                Timeline = GenerateOrderTimeline(order)
            };

            return ApiResponse<GetOrderResponse>.SuccessResponse(response, "获取成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取订单详情失败");
            return ApiResponse<GetOrderResponse>.ErrorResponse(500, "系统错误");
        }
    }

    /// <summary>
    /// 取消订单
    /// </summary>
    public async Task<ApiResponse<CancelOrderResponse>> CancelOrderAsync(int orderId, CancelOrderRequest request)
    {
        try
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return ApiResponse<CancelOrderResponse>.ErrorResponse(3001, "订单不存在");
            }

            // 检查订单状态
            if (order.Status != "待付款" && order.Status != "待服务")
            {
                return ApiResponse<CancelOrderResponse>.ErrorResponse(3002, "订单状态不允许取消");
            }

            // 检查取消时间
            if (order.StartTime.HasValue && order.StartTime.Value <= DateTime.UtcNow.AddHours(1))
            {
                return ApiResponse<CancelOrderResponse>.ErrorResponse(3007, "取消时间过晚");
            }

            // 更新订单状态
            order.Status = "已取消";
            order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var response = new CancelOrderResponse
            {
                OrderId = order.Id,
                OrderNo = order.OrderNo,
                Status = 6,
                StatusText = "已取消",
                CancelledAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                RefundAmount = order.FinalPrice,
                RefundTo = "balance",
                RefundToText = "退回余额"
            };

            return ApiResponse<CancelOrderResponse>.SuccessResponse(response, "订单已取消");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "取消订单失败");
            return ApiResponse<CancelOrderResponse>.ErrorResponse(500, "系统错误");
        }
    }

    /// <summary>
    /// 申请退款
    /// </summary>
    public async Task<ApiResponse<RefundOrderResponse>> RefundOrderAsync(int orderId, RefundOrderRequest request)
    {
        try
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return ApiResponse<RefundOrderResponse>.ErrorResponse(3001, "订单不存在");
            }

            // 检查订单状态
            if (order.Status != "已完成" && order.Status != "服务中")
            {
                return ApiResponse<RefundOrderResponse>.ErrorResponse(3004, "不满足退款条件");
            }

            // 检查是否已申请退款
            // TODO: 查询退款记录表

            // 创建退款申请
            var refundAmount = request.RefundAmount ?? order.FinalPrice;
            var refundType = request.RefundType ?? 1; // 默认全额退款

            // TODO: 保存退款申请到数据库

            var response = new RefundOrderResponse
            {
                RefundId = 5001, // TODO: 从数据库获取
                OrderId = order.Id,
                RefundAmount = refundAmount,
                RefundStatus = 0,
                RefundStatusText = "审核中",
                EstimatedRefundTime = "1-3个工作日",
                SubmittedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
            };

            return ApiResponse<RefundOrderResponse>.SuccessResponse(response, "退款申请已提交");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "申请退款失败");
            return ApiResponse<RefundOrderResponse>.ErrorResponse(500, "系统错误");
        }
    }

    /// <summary>
    /// 确认订单完成
    /// </summary>
    public async Task<ApiResponse<ConfirmOrderResponse>> ConfirmOrderAsync(int orderId)
    {
        try
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return ApiResponse<ConfirmOrderResponse>.ErrorResponse(3001, "订单不存在");
                ;
            }

            // 检查订单状态
            if (order.Status != "服务中")
            {
                return ApiResponse<ConfirmOrderResponse>.ErrorResponse(3002, "订单状态不允许确认");
            }

            // 更新订单状态
            order.Status = "已完成";
            order.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var response = new ConfirmOrderResponse
            {
                OrderId = order.Id,
                Status = 5,
                StatusText = "已完成",
                ConfirmedAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss"),
                RewardPoints = (int)order.FinalPrice // 按金额给予积分
            };

            return ApiResponse<ConfirmOrderResponse>.SuccessResponse(response, "订单已确认完成");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "确认订单完成失败");
            return ApiResponse<ConfirmOrderResponse>.ErrorResponse(500, "系统错误");
        }
    }

    /// <summary>
    /// 订单评价
    /// </summary>
    public async Task<ApiResponse<CreateOrderReviewResponse>> ReviewOrderAsync(int orderId, CreateOrderReviewRequest request)
    {
        try
        {
            var order = await _context.Orders.FindAsync(orderId);
            if (order == null)
            {
                return ApiResponse<CreateOrderReviewResponse>.ErrorResponse(3001, "订单不存在");
            }

            // 检查订单状态
            if (order.Status != "已完成")
            {
                return ApiResponse<CreateOrderReviewResponse>.ErrorResponse(3009, "订单未完成");
            }

            // 检查是否已评价
            var existingReview = await _context.OrderReviews.FirstOrDefaultAsync(r => r.OrderId == orderId);
            if (existingReview != null)
            {
                return ApiResponse<CreateOrderReviewResponse>.ErrorResponse(3010, "已评价过");
            }

            // 创建评价
            var review = new OrderReview
            {
                OrderId = orderId,
                CompanionId = order.CompanionId,
                Rating = request.Rating,
                Content = request.Comment,
                Tags = request.Tags != null ? string.Join(",", request.Tags) : null,
                CreatedAt = DateTime.UtcNow
            };

            _context.OrderReviews.Add(review);
            await _context.SaveChangesAsync();

            var response = new CreateOrderReviewResponse
            {
                ReviewId = review.Id,
                OrderId = orderId,
                CompanionId = order.CompanionId,
                Rating = request.Rating,
                Comment = request.Comment,
                Images = request.Images,
                Tags = request.Tags,
                RewardAmount = 5.00m, // 固定奖励金额
                RewardPoints = 100, // 固定奖励积分
                CreatedAt = review.CreatedAt.ToDateTimeString()
            };

            return ApiResponse<CreateOrderReviewResponse>.SuccessResponse(response, "评价成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "订单评价失败");
            return ApiResponse<CreateOrderReviewResponse>.ErrorResponse(500, "系统错误");
        }
    }

    #region 客户端 mobile
    /// <summary>
    /// 获取订单统计数据（高性能：单次SQL查询，5种状态一次算出）
    /// </summary>
    public async Task<ApiResponse<OrderStatusDto>> GetOrderStatusAsync(string openId)
    {
        try
        {
            // 🔥 高性能：单次EF Core查询，一次性统计所有状态，只查1次DB！
            var statistics = await _context.Orders
                .Where(x => x.User.Openid == openId)
                .AsNoTracking() // 无跟踪，极致性能
                .GroupBy(x => 1) // 虚拟分组，一次性聚合所有数据
                .Select(g => new OrderStatusDto
                {
                    // TotalOrders = g.Count(),
                    // 对应你字典的状态值：0=待付款，1=进行中，2=已完成，3=退款/售后
                    PendingPayment = g.Count(x => x.Status == "0"),
                    InProgress = g.Count(x => x.Status == "1"),
                    Completed = g.Count(x => x.Status == "2"),
                    RefundAfterSale = g.Count(x => x.Status == "3")
                })
                .FirstOrDefaultAsync();

            // 兜底：如果没有数据，返回0
            var result = statistics ?? new OrderStatusDto();

            return ApiResponse<OrderStatusDto>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取订单统计数据失败");
            return ApiResponse<OrderStatusDto>.Fail(500, "获取统计数据失败");
        }
    }

    #endregion

    #region PC端

    /// <summary>
    /// 订单分页列表
    /// </summary>
    public async Task<ApiResponse<GetOrdersPaginationResponse>> GetListAsync(GetOrdersPaginationRequest request)
    {
        try
        {
            // 基础查询（高性能：无跟踪、预生成SQL）
            var query = _context.Orders
                .AsNoTracking()
                .Include(x => x.User)
                .Include(x => x.Companion)
                .Include(x => x.Game)
                .AsQueryable();

            // 条件过滤
            if (!string.IsNullOrWhiteSpace(request.Status))
                query = query.Where(x => x.Status == request.Status);

            if (!string.IsNullOrWhiteSpace(request.Keyword))
            {
                var k = request.Keyword.Trim();
                query = query.Where(x =>
                    x.OrderNo.Contains(k) ||
                    x.User.Nickname.Contains(k) ||
                    x.User.Username.Contains(k));
            }

            if (request.GameId > 0)
                query = query.Where(x => x.GameId == request.GameId);

            if (!string.IsNullOrWhiteSpace(request.OrderType))
                query = query.Where(x => x.ServiceType == request.OrderType);

            if (request.StartTime.HasValue)
                query = query.Where(x => x.CreatedAt >= request.StartTime.Value);
            if (request.EndTime.HasValue)
                query = query.Where(x => x.CreatedAt <= request.EndTime.Value);

            // 总条数
            var total = await query.CountAsync();

            // 分页查询
            var list = await query
                .OrderByDescending(x => x.CreatedAt)
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new GetOrdersPaginationResponse.OrderItem
                {
                    Id = x.Id,
                    OrderNo = x.OrderNo,
                    Status = x.Status ?? "",
                    StatusText = "",
                    OrderType = x.ServiceType ?? "",
                    OrderTypeText = "",

                    // ✅ 正确映射 Companion（一对一）
                    Companion = new GetOrdersPaginationResponse.CompanionInfo
                    {
                        Id = x.Companion.Id,
                        Nickname = x.Companion.Nickname ?? "",
                        // ======================
                        // 【唯一修改】从 companion_games 取当前游戏的等级
                        // ======================
                        Level = _context.CompanionGames
                            .Where(cg => cg.CompanionId == x.CompanionId && cg.GameId == x.GameId)
                            .Select(cg => cg.GameLevel)
                            .FirstOrDefault(),
                        RealName = x.User.RealName ?? ""
                    },

                    GameName = x.Game.Name ?? "",
                    Username = x.User.RealName ?? "",
                    GameRank = "", // 可自己补充
                    ServiceCount = x.DurationValue,
                    ServiceTime = x.PlayTime.HasValue ? x.PlayTime.Value.ToString("yyyy-MM-dd HH:mm") : "",
                    TotalPrice = x.TotalPrice,
                    DiscountAmount = x.DiscountAmount ?? 0,
                    FinalPrice = x.FinalPrice,
                    CreatedAt = x.CreatedAt.HasValue ? x.CreatedAt.Value.ToString("yyyy-MM-dd HH:mm") : ""
                })
                .ToListAsync();

            // 批量翻译字典
            if (list.Count > 0)
            {
                var serviceTypes = list.Select(x => x.OrderType).Where(x => !string.IsNullOrEmpty(x)).Distinct().ToList();
                var statusValues = list.Select(x => x.Status).Distinct().ToList();

                // 2. 批量翻译（一次数据库请求）
                var serviceTypeMap = await _dictTranslateService.BatchTranslateAsync("service_type", serviceTypes);
                var statusMap = await _dictTranslateService.BatchTranslateAsync("order_status", statusValues);

                // 3. 内存赋值（极快）
                foreach (var item in list)
                {
                    // 翻译服务类型
                    item.OrderTypeText = serviceTypeMap.TryGetValue(item.OrderType!, out var sName) ? sName : item.OrderType!;
                    
                    // 翻译审核状态
                    var statusKey = item.Status.ToString();
                    item.StatusText = statusMap.TryGetValue(statusKey, out var stName) ? stName : statusKey;
                }
            }

            return ApiResponse<GetOrdersPaginationResponse>.Success(new GetOrdersPaginationResponse
            {
                Total = total,
                Page = request.Page,
                PageSize = request.PageSize,
                list = list
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取订单列表失败");
            return ApiResponse<GetOrdersPaginationResponse>.Fail(500, "获取列表失败");
        }
    }

    /// <summary>
    /// 获取订单统计数据（高性能：单次SQL查询，5种状态一次算出）
    /// </summary>
    public async Task<ApiResponse<OrderStatisticsDto>> GetStatisticsAsync()
    {
        try
        {
            // 🔥 高性能：单次EF Core查询，一次性统计所有状态，只查1次DB！
            var statistics = await _context.Orders
                .AsNoTracking() // 无跟踪，极致性能
                .GroupBy(x => 1) // 虚拟分组，一次性聚合所有数据
                .Select(g => new OrderStatisticsDto
                {
                    TotalOrders = g.Count(),
                    // 对应你字典的状态值：0=待付款，1=进行中，2=已完成，3=退款/售后
                    PendingPayment = g.Count(x => x.Status == "0"),
                    InProgress = g.Count(x => x.Status == "1"),
                    Completed = g.Count(x => x.Status == "2"),
                    RefundAfterSale = g.Count(x => x.Status == "3")
                })
                .FirstOrDefaultAsync();

            // 兜底：如果没有数据，返回0
            var result = statistics ?? new OrderStatisticsDto();

            return ApiResponse<OrderStatisticsDto>.Success(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取订单统计数据失败");
            return ApiResponse<OrderStatisticsDto>.Fail(500, "获取统计数据失败");
        }
    }

    #endregion

    #region 辅助方法

    /// <summary>
    /// 生成订单号
    /// </summary>
    private string GenerateOrderNo()
    {
        var prefix = "PW";
        var timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
        var random = new Random().Next(1000, 9999);
        return $"{prefix}{timestamp}{random}";
    }

    /// <summary>
    /// 生成支付链接
    /// </summary>
    private string GeneratePaymentUrl(string orderNo)
    {
        return $"https://pay.example.com/checkout?order_no={orderNo}";
    }

    /// <summary>
    /// 计算优惠金额
    /// </summary>
    private decimal CalculateDiscount(decimal totalPrice, int serviceCount)
    {
        if (serviceCount >= 5)
        {
            return totalPrice * 0.2m; // 满5局打8折
        }
        else if (serviceCount >= 3)
        {
            return totalPrice * 0.1m; // 满3局打9折
        }
        return 0;
    }

    /// <summary>
    /// 获取订单状态值
    /// </summary>
    private int GetOrderStatusValue(string statusText)
    {
        return statusText switch
        {
            "待付款" => 1,
            "待服务" => 2,
            "服务中" => 3,
            "待确认" => 4,
            "已完成" => 5,
            "已取消" => 6,
            "退款中" => 7,
            _ => 1
        };
    }

    /// <summary>
    /// 获取支付状态值
    /// </summary>
    private int GetPaymentStatusValue(string statusText)
    {
        return statusText switch
        {
            "待付款" => 0,
            "待服务" or "服务中" or "待确认" or "已完成" => 1,
            "退款中" => 2,
            _ => 0
        };
    }

    /// <summary>
    /// 获取支付状态文本
    /// </summary>
    private string GetPaymentStatusText(string statusText)
    {
        return statusText switch
        {
            "待付款" => "未支付",
            "待服务" or "服务中" or "待确认" or "已完成" => "已支付",
            "退款中" => "退款中",
            _ => "未支付"
        };
    }

    /// <summary>
    /// 生成订单时间线
    /// </summary>
    private List<GetOrderResponse.OrderTimeline> GenerateOrderTimeline(Order order)
    {
        var timeline = new List<GetOrderResponse.OrderTimeline>();

        timeline.Add(new GetOrderResponse.OrderTimeline
        {
            Status = "待付款",
            Time = order.CreatedAt.ToDateTimeString(),
            Description = "订单创建成功"
        });

        if (order.PayTime.HasValue)
        {
            timeline.Add(new GetOrderResponse.OrderTimeline
            {
                Status = order.Status switch
                {
                    "待服务" => "待服务",
                    "服务中" => "服务中",
                    "已完成" => "已完成",
                    _ => order.Status
                },
                Time = order.PayTime.Value.ToString("yyyy-MM-dd HH:mm:ss"),
                Description = "支付成功"
            });
        }

        return timeline;
    }

    /// <summary>
    /// 检查是否已评价
    /// </summary>
    private bool HasReviewed(int orderId)
    {
        return _context.OrderReviews.Any(r => r.OrderId == orderId);
    }

    // 新增：计算服务开始/结束时间（抽离为独立方法，便于维护）
    private (DateTime StartTime, DateTime EndTime) CalculateServiceTimeRange(DateTime serviceTime, string durationType, int serviceCount)
    {
        DateTime startTime = serviceTime;
        DateTime endTime;

        if (durationType == "game") // 按局（每局30分钟）
        {
            endTime = startTime.AddMinutes(30 * serviceCount);
        }
        else // 按小时
        {
            endTime = startTime.AddHours(serviceCount);
        }

        return (startTime, endTime);
    }

    #endregion
}