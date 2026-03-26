using GameCompanion.Api.DTOs.Order;
using GameCompanion.Api.Data;
using GameCompanion.Api.Models;
using GameCompanion.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace GameCompanion.Api.Services;

/// <summary>
/// 订单服务实现
/// </summary>
public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OrderService> _logger;

    public OrderService(ApplicationDbContext context, ILogger<OrderService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// 创建订单
    /// </summary>
    public async Task<ApiResponse<CreateOrderResponse>> CreateOrderAsync(CreateOrderRequest request)
    {
        try
        {
            // 验证陪玩师是否存在
            var companion = await _context.Companions
                .Include(c => c.User)
                .FirstOrDefaultAsync(c => c.Id == request.CompanionId && c.Status == "已认证");

            if (companion == null)
            {
                return ApiResponse<CreateOrderResponse>.ErrorResponse(2001, "陪玩师不存在");
            }

            if (companion.Status != "已认证")
            {
                return ApiResponse<CreateOrderResponse>.ErrorResponse(2002, "陪玩师未认证");
            }

            if (companion.OnlineStatus == "离线")
            {
                return ApiResponse<CreateOrderResponse>.ErrorResponse(2003, "陪玩师暂不接单");
            }

            // 验证游戏是否存在
            var game = await _context.Games.FirstOrDefaultAsync(g => g.Id == request.GameId);
            if (game == null)
            {
                return ApiResponse<CreateOrderResponse>.ErrorResponse(3004, "游戏不存在");
            }

            // 验证预约时间格式
            if (!DateTime.TryParseExact(request.ServiceTime, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out var serviceTime))
            {
                return ApiResponse<CreateOrderResponse>.ErrorResponse(3006, "预约时间格式错误");
            }

            // 验证预约时间必须至少提前30分钟
            if (serviceTime <= DateTime.UtcNow.AddMinutes(30))
            {
                return ApiResponse<CreateOrderResponse>.ErrorResponse(3006, "预约时间必须至少提前30分钟");
            }

            // 检查服务时间冲突
            var conflictingOrder = await _context.Orders
                .FirstOrDefaultAsync(o => o.CompanionId == request.CompanionId &&
                                        o.Status != "已取消" &&
                                        o.Status != "退款中" &&
                                        o.StartTime.HasValue &&
                                        o.EndTime.HasValue &&
                                        serviceTime >= o.StartTime.Value.AddMinutes(-30) &&
                                        serviceTime <= o.EndTime.Value.AddMinutes(30));

            if (conflictingOrder != null)
            {
                return ApiResponse<CreateOrderResponse>.ErrorResponse(3005, "服务时间冲突");
            }

            // 计算订单金额
            var unitPrice = companion.PricePerGame;
            var totalPrice = unitPrice * request.ServiceCount;
            var discountAmount = CalculateDiscount(totalPrice, request.ServiceCount);
            var finalAmount = totalPrice - discountAmount;

            // 检查用户余额
            var user = await _context.Users.FindAsync(request.UserId);
            if (user == null)
            {
                return ApiResponse<CreateOrderResponse>.ErrorResponse(404, "用户不存在");
            }

            if (user.Balance < finalAmount)
            {
                return ApiResponse<CreateOrderResponse>.ErrorResponse(3003, "余额不足");
            }

            // 生成订单号
            var orderNo = GenerateOrderNo();

            // 计算服务时长
            var durationValue = request.ServiceCount;
            var durationType = request.ServiceCount == 1 ? "局" : "小时";
            var startTime = serviceTime;
            var endTime = serviceTime.AddHours(durationType == "小时" ? 1 : 0.5); // 每局30分钟

            // 创建订单
            var order = new Order
            {
                OrderNo = orderNo,
                UserId = request.UserId,
                CompanionId = request.CompanionId,
                GameId = request.GameId,
                ServiceType = "陪玩",
                PlayTime = serviceTime,
                DurationType = durationType,
                DurationValue = durationValue,
                UnitPrice = unitPrice,
                TotalPrice = totalPrice,
                DiscountAmount = discountAmount,
                FinalPrice = finalAmount,
                Remark = request.SpecialRequirements,
                Status = "待付款",
                PayTime = null,
                StartTime = startTime,
                EndTime = endTime,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // 生成支付链接
            var paymentUrl = GeneratePaymentUrl(orderNo);

            var response = new CreateOrderResponse
            {
                OrderId = order.Id,
                OrderNo = orderNo,
                CompanionId = companion.Id,
                CompanionName = companion.Nickname,
                GameName = game.Name,
                ServiceCount = request.ServiceCount,
                ServiceTime = request.ServiceTime,
                UnitPrice = unitPrice,
                TotalAmount = totalPrice,
                ServiceFee = 0,
                DiscountAmount = discountAmount,
                FinalAmount = finalAmount,
                Status = 1, // 1-待付款
                StatusText = "待付款",
                PaymentTimeout = 1800, // 30分钟
                PaymentUrl = paymentUrl,
                CreatedAt = order.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            };

            return ApiResponse<CreateOrderResponse>.SuccessResponse(response, "订单创建成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "创建订单失败");
            return ApiResponse<CreateOrderResponse>.ErrorResponse(500, "系统错误");
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
                    OrderType = 1, // 1-陪玩订单
                    OrderTypeText = "陪玩订单",
                    Status = GetOrderStatusValue(o.Status),
                    StatusText = o.Status,
                    PaymentStatus = GetPaymentStatusValue(o.Status),
                    PaymentStatusText = GetPaymentStatusText(o.Status),
                    Companion = new GetOrdersResponse.CompanionInfo
                    {
                        Id = o.Companion.Id,
                        Nickname = o.Companion.Nickname,
                        // AvatarUrl = o.Companion.AvatarUrl ?? "",
                        Level = o.Companion.Level ?? ""
                    },
                    GameName = o.Game?.Name ?? "",
                    GameRank = "",
                    ServiceCount = o.DurationValue,
                    ServiceTime = o.PlayTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                    TotalAmount = o.FinalPrice,
                    CreatedAt = o.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
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
                    // AvatarUrl = order.Companion.AvatarUrl ?? "",
                    Level = order.Companion.Level ?? "",
                    Phone = order.Companion.Phone ?? "",
                    Wechat = order.Companion.User?.Wechat ?? ""
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
                CreatedAt = order.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                UpdatedAt = order.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
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
                Comment = request.Comment,
                Images = request.Images != null ? string.Join(",", request.Images) : null,
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
                CreatedAt = review.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            };

            return ApiResponse<CreateOrderReviewResponse>.SuccessResponse(response, "评价成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "订单评价失败");
            return ApiResponse<CreateOrderReviewResponse>.ErrorResponse(500, "系统错误");
        }
    }

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
            Time = order.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
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

    #endregion
}