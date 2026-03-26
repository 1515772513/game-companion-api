using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 陪玩师游戏技能实体
/// </summary>
[Table("companion_games")]
public class CompanionGame
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("companion_id")]
    public int CompanionId { get; set; }

    [Column("game_id")]
    public int GameId { get; set; }

    [Column("game_rank")]
    [MaxLength(50)]
    public string? GameRank { get; set; }

    [Column("level")]
    [MaxLength(20)]
    public string? Level { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    [ForeignKey("CompanionId")]
    public virtual Companion Companion { get; set; } = null!;

    [ForeignKey("GameId")]
    public virtual Game Game { get; set; } = null!;
}
