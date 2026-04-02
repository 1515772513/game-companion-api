using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 游戏圈子表
/// </summary>
[Table("game_circles")]
[Index("GameId", Name = "idx_game_id")]
[Index("Status", Name = "idx_status")]
public partial class GameCircle
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 游戏ID
    /// </summary>
    [Column("game_id")]
    public int GameId { get; set; }

    /// <summary>
    /// 圈子名称
    /// </summary>
    [Column("name")]
    [StringLength(50)]
    public string Name { get; set; } = null!;

    /// <summary>
    /// 圈子描述
    /// </summary>
    [Column("description", TypeName = "text")]
    public string? Description { get; set; }

    /// <summary>
    /// 圈子图标
    /// </summary>
    [Column("icon")]
    [StringLength(255)]
    public string? Icon { get; set; }

    /// <summary>
    /// 成员数
    /// </summary>
    [Column("member_count")]
    public int? MemberCount { get; set; }

    /// <summary>
    /// 动态数
    /// </summary>
    [Column("post_count")]
    public int? PostCount { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [Column("status", TypeName = "enum('active','inactive')")]
    public string? Status { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("GameId")]
    [InverseProperty("GameCircles")]
    public virtual Game Game { get; set; } = null!;
}
