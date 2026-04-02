using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 意见反馈表
/// </summary>
[Table("feedbacks")]
[Index("Status", Name = "idx_status")]
[Index("Type", Name = "idx_type")]
[Index("UserId", Name = "idx_user_id")]
public partial class Feedback
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
    /// 反馈类型
    /// </summary>
    [Column("type", TypeName = "enum('feature','bug','order','other')")]
    public string Type { get; set; } = null!;

    /// <summary>
    /// 反馈内容
    /// </summary>
    [Column("content", TypeName = "text")]
    public string Content { get; set; } = null!;

    /// <summary>
    /// 截图URL
    /// </summary>
    [Column("images")]
    [StringLength(1000)]
    public string? Images { get; set; }

    /// <summary>
    /// 联系方式
    /// </summary>
    [Column("contact")]
    [StringLength(100)]
    public string? Contact { get; set; }

    /// <summary>
    /// 处理状态
    /// </summary>
    [Column("status", TypeName = "enum('pending','processing','resolved','closed')")]
    public string? Status { get; set; }

    /// <summary>
    /// 回复内容
    /// </summary>
    [Column("reply", TypeName = "text")]
    public string? Reply { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Feedbacks")]
    public virtual User User { get; set; } = null!;
}
