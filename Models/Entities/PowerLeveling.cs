using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 代练服务表
/// </summary>
[Table("power_leveling")]
[Index("GameId", Name = "idx_game_id")]
[Index("Status", Name = "idx_status")]
[Index("UserId", Name = "idx_user_id")]
public partial class PowerLeveling
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 发布用户ID(陪玩师)
    /// </summary>
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// 游戏ID
    /// </summary>
    [Column("game_id")]
    public int GameId { get; set; }

    /// <summary>
    /// 代练项目
    /// </summary>
    [Column("service_type", TypeName = "enum('rank','star','achievement','hero','custom')")]
    public string ServiceType { get; set; } = null!;

    /// <summary>
    /// 当前段位
    /// </summary>
    [Column("current_rank")]
    [StringLength(50)]
    public string? CurrentRank { get; set; }

    /// <summary>
    /// 目标段位
    /// </summary>
    [Column("target_rank")]
    [StringLength(50)]
    public string TargetRank { get; set; } = null!;

    /// <summary>
    /// 预计完成天数
    /// </summary>
    [Column("estimated_days")]
    public int? EstimatedDays { get; set; }

    /// <summary>
    /// 报价
    /// </summary>
    [Column("price")]
    [Precision(10, 2)]
    public decimal Price { get; set; }

    /// <summary>
    /// 特殊要求
    /// </summary>
    [Column("special_requirements", TypeName = "text")]
    public string? SpecialRequirements { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [Column("status", TypeName = "enum('active','inactive','completed')")]
    public string? Status { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("PowerLevelings")]
    public virtual User User { get; set; } = null!;
}
