namespace GameCompanion.Api.DTOs.Order;

/// <summary>
/// 确认订单完成响应
/// </summary>
public class ConfirmOrderResponse
{
    /// <summary>
    /// 订单ID
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// 订单状态
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 订单状态文本
    /// </summary>
    public string StatusText { get; set; } = string.Empty;

    /// <summary>
    /// 确认时间
    /// </summary>
    public string ConfirmedAt { get; set; } = string.Empty;

    /// <summary>
    /// 奖励积分
    /// </summary>
    public int RewardPoints { get; set; }
}