using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

// 👇 关键：和自动生成的实体 同名、同命名空间、partial
public partial class CompanionGame
{
    /// <summary>
    /// 软关联：游戏
    /// </summary>
    [ForeignKey("GameId")]
    public virtual Game? Game { get; set; }
}