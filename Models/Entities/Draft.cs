using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 草稿表
/// </summary>
[Table("drafts")]
[Index("Type", Name = "idx_type")]
[Index("UserId", Name = "idx_user_id")]
public partial class Draft
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
    /// 草稿类型
    /// </summary>
    [Column("type", TypeName = "enum('post','request')")]
    public string Type { get; set; } = null!;

    /// <summary>
    /// 标题
    /// </summary>
    [Column("title")]
    [StringLength(255)]
    public string? Title { get; set; }

    /// <summary>
    /// 内容
    /// </summary>
    [Column("content", TypeName = "text")]
    public string? Content { get; set; }

    /// <summary>
    /// 图片URL
    /// </summary>
    [Column("images")]
    [StringLength(1000)]
    public string? Images { get; set; }

    /// <summary>
    /// 草稿数据(JSON格式)
    /// </summary>
    [Column("draft_data", TypeName = "json")]
    public string? DraftData { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Drafts")]
    public virtual User User { get; set; } = null!;
}
