namespace GameCompanion.Api.DTOs.Order;

/// <summary>
/// 创建订单响应
/// </summary>
public class CreateOrderResponse
{
    /// <summary>
    /// 订单ID
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// 订单号
    /// </summary>
    public string OrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 陪玩师ID
    /// </summary>
    public int CompanionId { get; set; }

    /// <summary>
    /// 陪玩师名称
    /// </summary>
    public string CompanionName { get; set; } = string.Empty;

    /// <summary>
    /// 游戏名称
    /// </summary>
    public string GameName { get; set; } = string.Empty;

    /// <summary>
    /// 服务数量
    /// </summary>
    public int ServiceCount { get; set; }

    /// <summary>
    /// 预约服务时间
    /// </summary>
    public string ServiceTime { get; set; } = string.Empty;

    /// <summary>
    /// 单价（元/局或元/小时）
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 订单总额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 服务手续费
    /// </summary>
    public decimal ServiceFee { get; set; }

    /// <summary>
    /// 优惠金额
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 实付金额
    /// </summary>
    public decimal FinalAmount { get; set; }

    /// <summary>
    /// 订单状态：1-待付款，2-待服务，3-服务中，4-待确认，5-已完成，6-已取消，7-退款中
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 订单状态文本
    /// </summary>
    public string StatusText { get; set; } = string.Empty;

    /// <summary>
    /// 支付倒计时（秒），超时订单自动取消
    /// </summary>
    public int PaymentTimeout { get; set; }

    /// <summary>
    /// 支付链接
    /// </summary>
    public string PaymentUrl { get; set; } = string.Empty;

    /// <summary>
    /// 订单创建时间
    /// </summary>
    public string CreatedAt { get; set; } = string.Empty;
}