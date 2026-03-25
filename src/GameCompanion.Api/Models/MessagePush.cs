using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models;

/// <summary>
/// 消息推送记录表
/// </summary>
[Table("message_pushes")]
public class MessagePush : BaseEntity
{
    /// <summary>
    /// 推送标题
    /// </summary>
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 推送内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 推送对象: all全部用户, vip VIP用户, active活跃用户, custom自定义
    /// </summary>
    [MaxLength(50)]
    public string? TargetType { get; set; }

    /// <summary>
    /// 自定义筛选条件 (JSON)
    /// </summary>
    public string? TargetCriteria { get; set; }

    /// <summary>
    /// 跳转链接
    /// </summary>
    [MaxLength(255)]
    public string? JumpUrl { get; set; }

    /// <summary>
    /// 推送方式: immediate立即, scheduled定时
    /// </summary>
    [MaxLength(20)]
    public string? PushType { get; set; }

    /// <summary>
    /// 定时推送时间
    /// </summary>
    public DateTime? ScheduleTime { get; set; }

    /// <summary>
    /// 状态: pending待发送, sending发送中, sent已完成, failed失败
    /// </summary>
    [MaxLength(20)]
    public string Status { get; set; } = "pending";

    /// <summary>
    /// 目标用户总数
    /// </summary>
    public int? TotalCount { get; set; }

    /// <summary>
    /// 成功发送数
    /// </summary>
    public int? SentCount { get; set; }

    /// <summary>
    /// 打开数
    /// </summary>
    public int? OpenCount { get; set; }

    /// <summary>
    /// 点击数
    /// </summary>
    public int? ClickCount { get; set; }

    /// <summary>
    /// 创建人ID
    /// </summary>
    public int? AdminId { get; set; }

    /// <summary>
    /// 发送时间
    /// </summary>
    public DateTime? SentAt { get; set; }
}
