using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

public partial class Game
{
    /// <summary>
    /// 软关联：陪玩游戏列表
    /// </summary>
    // [ForeignKey("id")]
    public virtual ICollection<CompanionGame>? CompanionGames { get; set; }
}