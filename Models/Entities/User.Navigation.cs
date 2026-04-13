using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

// 👇 关键：和自动生成的实体 同名、同命名空间、partial
public partial class User
{
  /// <summary>
  /// 收藏项目
  /// </summary>
  [ForeignKey("Id")]
  public virtual UserCollection UserCollection { get; set; } = null!;
}