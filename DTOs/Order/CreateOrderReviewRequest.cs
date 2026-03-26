using System.ComponentModel.DataAnnotations;

namespace GameCompanion.Api.DTOs.Order;

/// <summary>
/// 创建订单评价请求
/// </summary>
public class CreateOrderReviewRequest
{
    /// <summary>
    /// 评分，1-5星
    /// </summary>
    [Range(1, 5, ErrorMessage = "评分必须在1-5之间")]
    [Required(ErrorMessage = "评分不能为空")]
    public int Rating { get; set; }

    /// <summary>
    /// 评价内容
    /// </summary>
    [Required(ErrorMessage = "评价内容不能为空")]
    [StringLength(500, MinimumLength = 10, ErrorMessage = "评价内容必须在10-500字符之间")]
    public string Comment { get; set; } = string.Empty;

    /// <summary>
    /// 评价图片URL数组
    /// </summary>
    public List<string>? Images { get; set; }

    /// <summary>
    /// 评价标签
    /// </summary>
    public List<string>? Tags { get; set; }
}