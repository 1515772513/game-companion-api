using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 陪玩需求表
/// </summary>
[Table("companion_requests")]
[Index("GameId", Name = "idx_game_id")]
[Index("PlayTime", Name = "idx_play_time")]
[Index("Status", Name = "idx_status")]
[Index("UserId", Name = "idx_user_id")]
public partial class CompanionRequest
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 发布用户ID
    /// </summary>
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// 游戏ID
    /// </summary>
    [Column("game_id")]
    public int GameId { get; set; }

    /// <summary>
    /// 游戏段位/等级
    /// </summary>
    [Column("game_level")]
    [StringLength(50)]
    public string? GameLevel { get; set; }

    /// <summary>
    /// 期望陪玩时间
    /// </summary>
    [Column("play_time", TypeName = "datetime")]
    public DateTime PlayTime { get; set; }

    /// <summary>
    /// 时长类型:局/小时
    /// </summary>
    [Column("duration_type", TypeName = "enum('game','hour')")]
    public string? DurationType { get; set; }

    /// <summary>
    /// 时长数量
    /// </summary>
    [Column("duration_value")]
    public int DurationValue { get; set; }

    /// <summary>
    /// 预算价格
    /// </summary>
    [Column("budget")]
    [Precision(10, 2)]
    public decimal? Budget { get; set; }

    /// <summary>
    /// 陪玩要求(多个标签用逗号分隔)
    /// </summary>
    [Column("requirements")]
    [StringLength(500)]
    public string? Requirements { get; set; }

    /// <summary>
    /// 需求描述
    /// </summary>
    [Column("description", TypeName = "text")]
    public string? Description { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [Column("status", TypeName = "enum('pending','matched','completed','cancelled')")]
    public string? Status { get; set; }

    /// <summary>
    /// 匹配的陪玩师ID
    /// </summary>
    [Column("matched_companion_id")]
    public int? MatchedCompanionId { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("CompanionRequests")]
    public virtual User User { get; set; } = null!;
}
