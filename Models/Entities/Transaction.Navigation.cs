using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

// 👇 关键：和自动生成的实体 同名、同命名空间、partial
public partial class Transaction
{
     // 导航属性
    [ForeignKey("UserId")]
    public virtual User? User { get; set; }
}