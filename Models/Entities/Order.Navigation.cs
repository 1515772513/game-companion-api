using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

// 👇 关键：和自动生成的实体 同名、同命名空间、partial
public partial class Order
{
    /// <summary>
    /// 软关联：订单
    /// </summary>
    [ForeignKey(nameof(GameId))]
    public virtual Game? Game { get; set; } = null!; // 👈 最重要！告诉 EF Core：这不是数据库字段
}