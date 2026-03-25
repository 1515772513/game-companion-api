using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models;

/// <summary>
/// 系统通知表
/// </summary>
[Table("notifications")]
public class Notification : BaseEntity
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 关联用户
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    /// <summary>
    /// 通知类型
    /// </summary>
    [MaxLength(50)]
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// 标题
    /// </summary>
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 内容
    /// </summary>
    public string? Content { get; set; }

    /// <summary>
    /// 是否已读
    /// </summary>
    public int IsRead { get; set; } = 0;

    /// <summary>
    /// 优先级: 0普通, 1重要, 2紧急
    /// </summary>
    public int Priority { get; set; } = 0;

    /// <summary>
    /// 扩展数据 (JSON)
    /// </summary>
    public string? ExtraData { get; set; }
}
