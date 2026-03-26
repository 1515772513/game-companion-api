using System.ComponentModel.DataAnnotations;

namespace GameCompanion.Api.DTOs.Order;

/// <summary>
/// 取消订单请求
/// </summary>
public class CancelOrderRequest
{
    /// <summary>
    /// 取消原因
    /// </summary>
    [Required(ErrorMessage = "取消原因不能为空")]
    [MaxLength(200, ErrorMessage = "取消原因最多200字符")]
    public string CancelReason { get; set; } = string.Empty;
}