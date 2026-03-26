namespace GameCompanion.Api.DTOs.Order;

/// <summary>
/// 取消订单响应
/// </summary>
public class CancelOrderResponse
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
    /// 订单状态
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 订单状态文本
    /// </summary>
    public string StatusText { get; set; } = string.Empty;

    /// <summary>
    /// 取消时间
    /// </summary>
    public string CancelledAt { get; set; } = string.Empty;

    /// <summary>
    /// 退款金额
    /// </summary>
    public decimal RefundAmount { get; set; }

    /// <summary>
    /// 退款到
    /// </summary>
    public string RefundTo { get; set; } = string.Empty;

    /// <summary>
    /// 退款到文本
    /// </summary>
    public string RefundToText { get; set; } = string.Empty;
}