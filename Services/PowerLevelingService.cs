using GameCompanion.Api.DTOs.PowerLeveling;
using GameCompanion.Api.Models.Entities;
using GameCompanion.Api.Models;
using Microsoft.EntityFrameworkCore;
using GameCompanion.Api.Data;
using GameCompanion.Api.Utils;

namespace GameCompanion.Api.Services;

/// <summary>
/// 代练服务实现
/// </summary>
public class PowerLevelingService : IPowerLevelingService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PowerLevelingService> _logger;

    public PowerLevelingService(ApplicationDbContext context, ILogger<PowerLevelingService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// 获取代练服务列表
    /// </summary>
    public async Task<ApiResponse<PowerLevelingServicesResponse>> GetServicesAsync(int? gameId = null)
    {
        try
        {
            var query = _context.Games.AsQueryable();

            if (gameId.HasValue)
            {
                query = query.Where(g => g.Id == gameId.Value);
            }

            var games = await query
                .Where(g => g.IsActive == 1)
                .OrderBy(g => g.SortOrder)
                .ToListAsync();

            var serviceInfos = new List<PowerLevelingServiceInfo>();

            // 这里简化处理，实际应该从代练服务表中获取
            // 每个游戏创建几个示例服务
            foreach (var game in games)
            {
                // 示例服务 - 实际应该从代练服务表获取
                var services = new List<PowerLevelingServiceInfo>
                {
                    new PowerLevelingServiceInfo
                    {
                        Id = game.Id * 1000 + 1,
                        GameId = game.Id,
                        GameName = game.Name,
                        ServiceName = "青铜到白银",
                        StartRank = "青铜III",
                        EndRank = "白银I",
                        Price = 50.00m,
                        OriginalPrice = 80.00m,
                        Discount = 30.00m,
                        EstimatedDays = 2,
                        Description = "包含所有段位，保证胜率60%以上",
                        Requirements = new List<string>
                        {
                            "账号需实名认证",
                            "需提供账号密码",
                            "代练期间不可登录"
                        },
                        ProcessSteps = new List<string>
                        {
                            "下单后客服联系确认",
                            "代练师开始上号",
                            "完成目标段位",
                            "验收确认"
                        }
                    },
                    new PowerLevelingServiceInfo
                    {
                        Id = game.Id * 1000 + 2,
                        GameId = game.Id,
                        GameName = game.Name,
                        ServiceName = "白银到黄金",
                        StartRank = "白银III",
                        EndRank = "黄金I",
                        Price = 100.00m,
                        OriginalPrice = 150.00m,
                        Discount = 50.00m,
                        EstimatedDays = 3,
                        Description = "专业代练，安全高效",
                        Requirements = new List<string>
                        {
                            "账号需实名认证",
                            "需提供账号密码",
                            "代练期间不可登录"
                        },
                        ProcessSteps = new List<string>
                        {
                            "下单后客服联系确认",
                            "代练师开始上号",
                            "完成目标段位",
                            "验收确认"
                        }
                    },
                    new PowerLevelingServiceInfo
                    {
                        Id = game.Id * 1000 + 3,
                        GameId = game.Id,
                        GameName = game.Name,
                        ServiceName = "黄金到铂金",
                        StartRank = "黄金III",
                        EndRank = "铂金I",
                        Price = 200.00m,
                        OriginalPrice = 300.00m,
                        Discount = 100.00m,
                        EstimatedDays = 5,
                        Description = "实力代练，五星好评",
                        Requirements = new List<string>
                        {
                            "账号需实名认证",
                            "需提供账号密码",
                            "代练期间不可登录"
                        },
                        ProcessSteps = new List<string>
                        {
                            "下单后客服联系确认",
                            "代练师开始上号",
                            "完成目标段位",
                            "验收确认"
                        }
                    }
                };

                serviceInfos.AddRange(services);
            }

            var response = new PowerLevelingServicesResponse
            {
                Items = serviceInfos
            };

            return ApiResponse<PowerLevelingServicesResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取代练服务列表失败");
            return ApiResponse<PowerLevelingServicesResponse>.ErrorResponse(500, "获取服务列表失败");
        }
    }

    /// <summary>
    /// 获取代练服务详情
    /// </summary>
    public async Task<ApiResponse<PowerLevelingServiceInfo>> GetServiceDetailAsync(int serviceId)
    {
        try
        {
            // 这里简化处理，实际应该从代练服务表获取
            var game = await _context.Games
                .FirstOrDefaultAsync(g => g.Id == serviceId / 1000);

            if (game == null)
            {
                return ApiResponse<PowerLevelingServiceInfo>.ErrorResponse(404, "服务不存在");
            }

            var serviceType = (serviceId % 1000) switch
            {
                1 => "青铜到白银",
                2 => "白银到黄金",
                3 => "黄金到铂金",
                _ => "未知服务"
            };

            var service = new PowerLevelingServiceInfo
            {
                Id = serviceId,
                GameId = game.Id,
                GameName = game.Name,
                ServiceName = serviceType,
                StartRank = GetStartRank(serviceType),
                EndRank = GetEndRank(serviceType),
                Price = GetPrice(serviceType),
                OriginalPrice = GetOriginalPrice(serviceType),
                Discount = GetDiscount(serviceType),
                EstimatedDays = GetEstimatedDays(serviceType),
                Description = GetDescription(serviceType),
                Requirements = new List<string>
                {
                    "账号需实名认证",
                    "需提供账号密码",
                    "代练期间不可登录"
                },
                ProcessSteps = new List<string>
                {
                    "下单后客服联系确认",
                    "代练师开始上号",
                    "完成目标段位",
                    "验收确认"
                }
            };

            return ApiResponse<PowerLevelingServiceInfo>.SuccessResponse(service);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取代练服务详情失败");
            return ApiResponse<PowerLevelingServiceInfo>.ErrorResponse(500, "获取服务详情失败");
        }
    }

    /// <summary>
    /// 创建代练订单
    /// </summary>
    public async Task<ApiResponse<object>> CreateOrderAsync(CreatePowerLevelingOrderRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return ApiResponse<object>.Fail(401, "未授权，请先登录");
            }

            // 验证服务是否存在
            var service = await GetServiceDetailAsync(request.ServiceId);
            if (service.Code != 200)
            {
                return ApiResponse<object>.Fail(404, "服务不存在");
            }

            // 创建订单
            var order = new Order
            {
                OrderNo = GenerateOrderNo("DL"),
                UserId = userId,
                CompanionId = 0, // 代练订单不需要陪玩师ID
                GameId = request.ServiceId / 1000, // 从服务ID推导游戏ID
                ServiceType = "代练",
                DurationValue = 0,
                UnitPrice = service.Data?.Price ?? 0,
                TotalPrice = service.Data?.Price ?? 0,
                FinalPrice = service.Data?.Price ?? 0,
                Status = "待付款",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            // 创建代练订单详情（需要创建代练订单详情表）
            // 这里简化处理，只记录到备注中
            order.Remark = $"代练服务：{service.Data?.GameName}-{service.Data?.ServiceName}|" +
                          $"账号：{request.GameAccount}|" +
                          $"角色：{request.GameRole ?? "无"}|" +
                          $"当前段位：{request.CurrentRank}|" +
                          $"目标段位：{request.TargetRank}|" +
                          $"特殊要求：{request.SpecialRequirements ?? "无"}|" +
                          $"联系电话：{request.ContactPhone}";

            order.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            var response = new
            {
                OrderId = order.Id,
                OrderNo = order.OrderNo,
                TotalAmount = order.FinalPrice,
                PaymentTimeout = 1800, // 30分钟支付超时
                PaymentUrl = $"https://pay.example.com/order/{order.Id}"
            };

            return ApiResponse<object>.SuccessResponse(response, "订单创建成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "创建代练订单失败");
            return ApiResponse<object>.Fail(500, "创建订单失败");
        }
    }

    /// <summary>
    /// 获取代练订单列表
    /// </summary>
    public async Task<ApiResponse<PowerLevelingOrdersResponse>> GetOrdersAsync(int? page = 1, int? pageSize = 20, int? status = null)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return ApiResponse<PowerLevelingOrdersResponse>.ErrorResponse(401, "未授权，请先登录");
            }

            var query = _context.Orders
                .Where(o => o.UserId == userId && o.ServiceType == "代练");

            // 状态筛选
            if (status.HasValue)
            {
                var statusText = status.Value switch
                {
                    1 => "待付款",
                    2 => "代练中",
                    3 => "已完成",
                    4 => "已取消",
                    5 => "已退款",
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

            var orderInfos = orders.Select(o => new PowerLevelingOrderInfo
            {
                Id = o.Id,
                OrderNo = o.OrderNo,
                Status = OrderStatusToInt(o.Status),
                StatusText = o.Status,
                GameName = "游戏名称", // 需要从服务信息获取
                ServiceName = "代练服务", // 需要从服务信息获取
                CurrentRank = "当前段位", // 需要从订单详情获取
                TargetRank = "目标段位", // 需要从订单详情获取
                Progress = 0, // 需要根据进度计算
                TotalAmount = o.FinalPrice ?? 0,
                CreatedAt = o.CreatedAt.ToDateTimeString(),
                EstimatedCompleteTime = o.EndTime?.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList();

            var response = new PowerLevelingOrdersResponse
            {
                Items = orderInfos,
                Pagination = new PaginationInfo
                {
                    Page = page ?? 1,
                    PageSize = pageSize ?? 20,
                    Total = total
                }
            };

            return ApiResponse<PowerLevelingOrdersResponse>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取代练订单列表失败");
            return ApiResponse<PowerLevelingOrdersResponse>.ErrorResponse(500, "获取订单列表失败");
        }
    }

    /// <summary>
    /// 获取代练订单详情
    /// </summary>
    public async Task<ApiResponse<PowerLevelingOrderDetail>> GetOrderDetailAsync(int orderId)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return ApiResponse<PowerLevelingOrderDetail>.ErrorResponse(401, "未授权，请先登录");
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId && o.ServiceType == "代练");

            if (order == null)
            {
                return ApiResponse<PowerLevelingOrderDetail>.ErrorResponse(404, "订单不存在");
            }

            // 解析订单详情
            var details = ParseOrderDetails(order.Remark);

            var response = new PowerLevelingOrderDetail
            {
                Id = order.Id,
                OrderNo = order.OrderNo,
                Status = OrderStatusToInt(order.Status),
                StatusText = order.Status,
                GameName = details.GameName,
                ServiceName = details.ServiceName,
                GameAccount = details.GameAccount,
                CurrentRank = details.CurrentRank,
                CurrentStars = details.CurrentStars,
                TargetRank = details.TargetRank,
                Progress = details.Progress,
                TotalAmount = order.FinalPrice ?? 0,
                SpecialRequirements = details.SpecialRequirements,
                StartedAt = order.StartTime?.ToString("yyyy-MM-dd HH:mm:ss"),
                EstimatedCompleteTime = order.EndTime?.ToString("yyyy-MM-dd HH:mm:ss"),
                Leveler = new LevelerInfo
                {
                    Nickname = "代练师",
                    AvatarUrl = "https://example.com/avatar/leveler.jpg"
                },
                ProgressLogs = details.ProgressLogs,
                CreatedAt = order.CreatedAt?.ToDateTimeString() ?? string.Empty
            };

            return ApiResponse<PowerLevelingOrderDetail>.SuccessResponse(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取代练订单详情失败");
            return ApiResponse<PowerLevelingOrderDetail>.ErrorResponse(500, "获取订单详情失败");
        }
    }

    /// <summary>
    /// 取消代练订单
    /// </summary>
    public async Task<ApiResponse<object>> CancelOrderAsync(int orderId, CancelPowerLevelingOrderRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return ApiResponse<object>.Fail(401, "未授权，请先登录");
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId && o.ServiceType == "代练");

            if (order == null)
            {
                return ApiResponse<object>.Fail(404, "订单不存在");
            }

            // 检查订单状态
            if (order.Status != "待付款")
            {
                return ApiResponse<object>.Fail(400, "订单状态不正确，无法取消");
            }

            order.Status = "已取消";
            order.Remark = $"{order.Remark}|取消原因：{request.CancelReason}";
            order.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return ApiResponse<object>.Success("订单取消成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "取消代练订单失败");
            return ApiResponse<object>.Fail(500, "取消订单失败");
        }
    }

    /// <summary>
    /// 申请代练退款
    /// </summary>
    public async Task<ApiResponse<object>> ApplyRefundAsync(int orderId, RefundPowerLevelingOrderRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return ApiResponse<object>.Fail(401, "未授权，请先登录");
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId && o.ServiceType == "代练");

            if (order == null)
            {
                return ApiResponse<object>.Fail(404, "订单不存在");
            }

            // 检查订单状态
            if (order.Status != "代练中" && order.Status != "已完成")
            {
                return ApiResponse<object>.Fail(400, "订单状态不正确，无法申请退款");
            }

            // 创建退款记录（需要创建退款记录表）
            _logger.LogInformation("用户 {UserId} 申请订单 {OrderId} 退款，金额：{Amount}，原因：{Reason}",
                userId, orderId, request.RefundAmount ?? order.FinalPrice, request.RefundReason);

            order.Status = "已退款";
            order.Remark = $"{order.Remark}|退款原因：{request.RefundReason}";
            order.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return ApiResponse<object>.Success("退款申请已提交");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "申请代练退款失败");
            return ApiResponse<object>.Fail(500, "申请退款失败");
        }
    }

    /// <summary>
    /// 代练订单评价
    /// </summary>
    public async Task<ApiResponse<object>> ReviewOrderAsync(int orderId, ReviewPowerLevelingOrderRequest request)
    {
        try
        {
            var userId = GetCurrentUserId();
            if (userId == 0)
            {
                return ApiResponse<object>.Fail(401, "未授权，请先登录");
            }

            var order = await _context.Orders
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId && o.ServiceType == "代练");

            if (order == null)
            {
                return ApiResponse<object>.Fail(404, "订单不存在");
            }

            // 检查订单状态
            if (order.Status != "已完成")
            {
                return ApiResponse<object>.Fail(400, "订单状态不正确，无法评价");
            }

            // 创建评价记录（需要创建评价记录表）
            _logger.LogInformation("用户 {UserId} 订单 {OrderId} 评价：{Rating}星，内容：{Comment}",
                userId, orderId, request.Rating, request.Comment);

            return ApiResponse<object>.Success("评价成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "代练订单评价失败");
            return ApiResponse<object>.Fail(500, "评价失败");
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
    /// 生成订单号
    /// </summary>
    private string GenerateOrderNo(string prefix)
    {
        return $"{prefix}{DateTime.Now:yyyyMMdd}{Guid.NewGuid().ToString("N").Substring(0, 6)}";
    }

    /// <summary>
    /// 转换订单状态为数字
    /// </summary>
    private int OrderStatusToInt(string status)
    {
        return status switch
        {
            "待付款" => 1,
            "代练中" => 2,
            "已完成" => 3,
            "已取消" => 4,
            "已退款" => 5,
            _ => 0
        };
    }

    /// <summary>
    /// 获取起始段位
    /// </summary>
    private string GetStartRank(string serviceType)
    {
        return serviceType switch
        {
            "青铜到白银" => "青铜III",
            "白银到黄金" => "白银III",
            "黄金到铂金" => "黄金III",
            _ => "未知"
        };
    }

    /// <summary>
    /// 获取目标段位
    /// </summary>
    private string GetEndRank(string serviceType)
    {
        return serviceType switch
        {
            "青铜到白银" => "白银I",
            "白银到黄金" => "黄金I",
            "黄金到铂金" => "铂金I",
            _ => "未知"
        };
    }

    /// <summary>
    /// 获取价格
    /// </summary>
    private decimal GetPrice(string serviceType)
    {
        return serviceType switch
        {
            "青铜到白银" => 50.00m,
            "白银到黄金" => 100.00m,
            "黄金到铂金" => 200.00m,
            _ => 0
        };
    }

    /// <summary>
    /// 获取原价
    /// </summary>
    private decimal GetOriginalPrice(string serviceType)
    {
        return serviceType switch
        {
            "青铜到白银" => 80.00m,
            "白银到黄金" => 150.00m,
            "黄金到铂金" => 300.00m,
            _ => 0
        };
    }

    /// <summary>
    /// 获取折扣
    /// </summary>
    private decimal GetDiscount(string serviceType)
    {
        return serviceType switch
        {
            "青铜到白银" => 30.00m,
            "白银到黄金" => 50.00m,
            "黄金到铂金" => 100.00m,
            _ => 0
        };
    }

    /// <summary>
    /// 获取预计天数
    /// </summary>
    private int GetEstimatedDays(string serviceType)
    {
        return serviceType switch
        {
            "青铜到白银" => 2,
            "白银到黄金" => 3,
            "黄金到铂金" => 5,
            _ => 0
        };
    }

    /// <summary>
    /// 获取描述
    /// </summary>
    private string GetDescription(string serviceType)
    {
        return serviceType switch
        {
            "青铜到白银" => "包含所有段位，保证胜率60%以上",
            "白银到黄金" => "专业代练，安全高效",
            "黄金到铂金" => "实力代练，五星好评",
            _ => "未知服务"
        };
    }

    /// <summary>
    /// 解析订单详情
    /// </summary>
    private (string GameName, string ServiceName, string GameAccount, string CurrentRank, string TargetRank, int CurrentStars, int Progress, string SpecialRequirements, List<ProgressLogInfo> ProgressLogs) ParseOrderDetails(string? remark)
    {
        if (string.IsNullOrEmpty(remark))
        {
            return ("游戏名称", "代练服务", "游戏账号", "当前段位", "目标段位", 0, 0, "无", new List<ProgressLogInfo>());
        }

        var parts = remark.Split('|');
        var details = new Dictionary<string, string>();

        foreach (var part in parts)
        {
            if (part.Contains(':') || part.Contains('：'))
            {
                var keyValuePair = part.Split(new[] { ':', '：' }, 2);
                if (keyValuePair.Length == 2)
                {
                    details[keyValuePair[0].Trim()] = keyValuePair[1].Trim();
                }
            }
        }

        var progressLogs = new List<ProgressLogInfo>
        {
            new ProgressLogInfo { Rank = "青铜III", Stars = 3, CompletedAt = DateTime.Now.AddDays(-2).ToString("yyyy-MM-dd HH:mm:ss") },
            new ProgressLogInfo { Rank = "青铜II", Stars = 3, CompletedAt = DateTime.Now.AddDays(-1).ToString("yyyy-MM-dd HH:mm:ss") }
        };

        return (
            details.GetValueOrDefault("代练服务", "游戏名称").Split('-')[0],
            details.GetValueOrDefault("代练服务", "代练服务"),
            details.GetValueOrDefault("账号", "游戏账号"),
            details.GetValueOrDefault("当前段位", "当前段位"),
            details.GetValueOrDefault("目标段位", "目标段位"),
            2, // 当前星星数
            60, // 进度
            details.GetValueOrDefault("特殊要求", "无"),
            progressLogs
        );
    }

    #endregion
}