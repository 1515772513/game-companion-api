using System.ComponentModel.DataAnnotations;

namespace GameCompanion.Api.DTOs.Order;

/// <summary>
/// 申请退款请求
/// </summary>
public class RefundOrderRequest
{
    /// <summary>
    /// 退款原因
    /// </summary>
    [Required(ErrorMessage = "退款原因不能为空")]
    [MaxLength(500, ErrorMessage = "退款原因最多500字符")]
    public string RefundReason { get; set; } = string.Empty;

    /// <summary>
    /// 退款金额（不填则全额退款）
    /// </summary>
    public decimal? RefundAmount { get; set; }

    /// <summary>
    /// 退款类型：1-全额退款，2-部分退款
    /// </summary>
    public int? RefundType { get; set; }
}