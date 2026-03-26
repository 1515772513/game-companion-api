namespace GameCompanion.Api.DTOs.Order;

/// <summary>
/// 创建订单评价响应
/// </summary>
public class CreateOrderReviewResponse
{
    /// <summary>
    /// 评价ID
    /// </summary>
    public long ReviewId { get; set; }

    /// <summary>
    /// 订单ID
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// 陪玩师ID
    /// </summary>
    public int CompanionId { get; set; }

    /// <summary>
    /// 评分（1-5）
    /// </summary>
    public int Rating { get; set; }

    /// <summary>
    /// 评价内容
    /// </summary>
    public string Comment { get; set; } = string.Empty;

    /// <summary>
    /// 评价图片URL列表
    /// </summary>
    public List<string>? Images { get; set; }

    /// <summary>
    /// 评价标签列表
    /// </summary>
    public List<string>? Tags { get; set; }

    /// <summary>
    /// 评价奖励金额
    /// </summary>
    public decimal RewardAmount { get; set; }

    /// <summary>
    /// 奖励积分
    /// </summary>
    public int RewardPoints { get; set; }

    /// <summary>
    /// 评价时间
    /// </summary>
    public string CreatedAt { get; set; } = string.Empty;
}