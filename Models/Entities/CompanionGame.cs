using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 陪玩师游戏技能表
/// </summary>
[Table("companion_games")]
[Index("GameId", Name = "idx_game_id")]
[Index("CompanionId", "GameId", Name = "uk_companion_game", IsUnique = true)]
public partial class CompanionGame
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 陪玩师ID
    /// </summary>
    [Column("companion_id")]
    public int CompanionId { get; set; }

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

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("CompanionId")]
    [InverseProperty("CompanionGames")]
    public virtual Companion Companion { get; set; } = null!;
}
