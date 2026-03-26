using System.ComponentModel.DataAnnotations;

namespace GameCompanion.Api.DTOs.Order;

/// <summary>
/// 创建订单请求
/// </summary>
public class CreateOrderRequest
{
    /// <summary>
    /// 陪玩师ID
    /// </summary>
    [Required(ErrorMessage = "陪玩师ID不能为空")]
    public int CompanionId { get; set; }

    /// <summary>
    /// 游戏ID
    /// </summary>
    [Required(ErrorMessage = "游戏ID不能为空")]
    public int GameId { get; set; }

    /// <summary>
    /// 游戏段位/等级
    /// </summary>
    [MaxLength(50, ErrorMessage = "游戏段位最多50字符")]
    public string? GameRank { get; set; }

    /// <summary>
    /// 服务数量（局数/小时数）
    /// </summary>
    [Range(1, 10, ErrorMessage = "服务数量必须在1-10之间")]
    [Required(ErrorMessage = "服务数量不能为空")]
    public int ServiceCount { get; set; }

    /// <summary>
    /// 预约服务时间
    /// </summary>
    [Required(ErrorMessage = "预约服务时间不能为空")]
    public string ServiceTime { get; set; } = string.Empty;

    /// <summary>
    /// 特殊要求/备注
    /// </summary>
    [MaxLength(200, ErrorMessage = "特殊要求最多200字符")]
    public string? SpecialRequirements { get; set; }
}