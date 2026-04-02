using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 系统通知表
/// </summary>
[Table("notifications")]
[Index("CreatedAt", Name = "idx_created_at")]
[Index("IsRead", Name = "idx_is_read")]
[Index("Type", Name = "idx_type")]
[Index("UserId", Name = "idx_user_id")]
public partial class Notification
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// 通知类型
    /// </summary>
    [Column("type", TypeName = "enum('system','activity','order','important','warning','success')")]
    public string Type { get; set; } = null!;

    /// <summary>
    /// 通知标题
    /// </summary>
    [Column("title")]
    [StringLength(255)]
    public string Title { get; set; } = null!;

    /// <summary>
    /// 通知内容
    /// </summary>
    [Column("content", TypeName = "text")]
    public string Content { get; set; } = null!;

    /// <summary>
    /// 标签
    /// </summary>
    [Column("tag")]
    [StringLength(50)]
    public string? Tag { get; set; }

    /// <summary>
    /// 是否已读
    /// </summary>
    [Column("is_read")]
    public bool? IsRead { get; set; }

    /// <summary>
    /// 跳转链接
    /// </summary>
    [Column("action_url")]
    [StringLength(255)]
    public string? ActionUrl { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Notifications")]
    public virtual User User { get; set; } = null!;
}
