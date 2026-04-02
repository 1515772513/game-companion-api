using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

// 👇 关键：和自动生成的实体 同名、同命名空间、partial
public partial class Companion
{
    /// <summary>
    /// 软关联：游戏列表
    /// </summary>
    [NotMapped] // 👈 最重要！告诉 EF Core：这不是数据库字段
    public virtual ICollection<CompanionGame> Games { get; set; } = new List<CompanionGame>();
}