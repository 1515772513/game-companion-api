using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 游戏表
/// </summary>
[Table("games")]
[Index("SortOrder", Name = "idx_sort_order")]
[Index("Status", Name = "idx_status")]
[Index("Name", Name = "name", IsUnique = true)]
public partial class Game
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 游戏名称
    /// </summary>
    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 英文名
    /// </summary>
    [Column("name_en")]
    [StringLength(50)]
    public string? NameEn { get; set; }

    /// <summary>
    /// 游戏图标
    /// </summary>
    [Column("icon")]
    [StringLength(255)]
    public string? Icon { get; set; }

    /// <summary>
    /// 封面图片
    /// </summary>
    [Column("cover_image")]
    [StringLength(255)]
    public string? CoverImage { get; set; }

    /// <summary>
    /// 游戏类型
    /// </summary>
    [Column("type")]
    [StringLength(50)]
    public string? Type { get; set; }

    /// <summary>
    /// 游戏描述
    /// </summary>
    [Column("description", TypeName = "text")]
    public string? Description { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [Column("status", TypeName = "enum('active','inactive')")]
    public string? Status { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    [Column("sort_order")]
    public int? SortOrder { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 是否启用
    /// </summary>
    public int? IsActive { get; set; }

    [InverseProperty("Game")]
    public virtual ICollection<GameCircle> GameCircles { get; set; } = new List<GameCircle>();
}
