using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models;

/// <summary>
/// 陪玩师游戏技能表
/// </summary>
[Table("companion_games")]
public class CompanionGame
{
    /// <summary>
    /// 主键ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 陪玩师ID
    /// </summary>
    public int CompanionId { get; set; }

    /// <summary>
    /// 关联陪玩师
    /// </summary>
    [ForeignKey(nameof(CompanionId))]
    public Companion? Companion { get; set; }

    /// <summary>
    /// 游戏ID
    /// </summary>
    public int GameId { get; set; }

    /// <summary>
    /// 关联游戏
    /// </summary>
    [ForeignKey(nameof(GameId))]
    public Game? Game { get; set; }

    /// <summary>
    /// 游戏段位/等级
    /// </summary>
    [MaxLength(50)]
    public string? GameLevel { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }
}
