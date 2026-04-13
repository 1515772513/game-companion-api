using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 用户收藏表
/// </summary>
[Table("user_collections")]
[Index("UserId", Name = "idx_user_id")]
[Index("UserId", "ItemType", "ItemId", Name = "uk_user_item", IsUnique = true)]
public partial class UserCollection
{
    /// <summary>
    /// 主键ID
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// 收藏标题
    /// </summary>
    [Column("title")]
    [StringLength(100)]
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 收藏描述
    /// </summary>
    [Column("description")]
    [StringLength(500)]
    public string? Description { get; set; } = string.Empty;

    /// <summary>
    /// 收藏分类
    /// </summary>
    [Column("category")]
    [StringLength(50)]
    public string Category { get; set; } = string.Empty;

    /// <summary>
    /// 关联项目ID
    /// </summary>
    [Column("item_id")]
    public int ItemId { get; set; }

    /// <summary>
    /// 关联项目类型(companion/post等)
    /// </summary>
    [Column("item_type")]
    [StringLength(20)]
    public string ItemType { get; set; } = null!;

    /// <summary>
    /// 创建时间
    /// </summary>
    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}
