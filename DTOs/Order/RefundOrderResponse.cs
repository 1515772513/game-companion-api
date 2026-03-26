namespace GameCompanion.Api.DTOs.Order;

/// <summary>
/// 申请退款响应
/// </summary>
public class RefundOrderResponse
{
    /// <summary>
    /// 退款ID
    /// </summary>
    public long RefundId { get; set; }

    /// <summary>
    /// 订单ID
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// 退款金额
    /// </summary>
    public decimal RefundAmount { get; set; }

    /// <summary>
    /// 退款状态：0-审核中，1-已退款，2-已拒绝
    /// </summary>
    public int RefundStatus { get; set; }

    /// <summary>
    /// 退款状态文本
    /// </summary>
    public string RefundStatusText { get; set; } = string.Empty;

    /// <summary>
    /// 预计退款时间
    /// </summary>
    public string EstimatedRefundTime { get; set; } = string.Empty;

    /// <summary>
    /// 提交时间
    /// </summary>
    public string SubmittedAt { get; set; } = string.Empty;
}