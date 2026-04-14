using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 陪玩师申请表
/// </summary>
[Table("companion_applications")]
public partial class CompanionApplication
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
    /// 游戏类型
    /// </summary>
    [Column("game_category")]
    [StringLength(100)]
    public string GameCategory { get; set; } = null!;

    /// <summary>
    /// 技能等级
    /// </summary>
    [Column("skill_level")]
    [StringLength(100)]
    public string SkillLevel { get; set; } = null!;

    /// <summary>
    /// 自我介绍
    /// </summary>
    [Column("self_introduction")]
    [StringLength(500)]
    public string SelfIntroduction { get; set; } = null!;

    /// <summary>
    /// 时薪
    /// </summary>
    [Column("hourly_rate")]
    [Precision(18, 2)]
    public decimal HourlyRate { get; set; }

    /// <summary>
    /// 可接单时间
    /// </summary>
    [Column("available_time")]
    [StringLength(100)]
    public string AvailableTime { get; set; } = null!;

    /// <summary>
    /// 状态：待审核/已通过/已拒绝
    /// </summary>
    [Column("status")]
    [StringLength(20)]
    public string Status { get; set; } = null!;

    /// <summary>
    /// 管理员备注
    /// </summary>
    [Column("admin_notes")]
    [StringLength(500)]
    public string? AdminNotes { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [Column("updated_at", TypeName = "datetime")]
    public DateTime UpdatedAt { get; set; }
}
