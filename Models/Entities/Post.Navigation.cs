using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

// 👇 关键：和自动生成的实体 同名、同命名空间、partial
public partial class Post
{
    /// <summary>
    /// 软关联：评论
    /// </summary>
    [NotMapped] // 👈 最重要！告诉 EF Core：这不是数据库字段
    public virtual ICollection<PostComment> Comments { get; set; } = new List<PostComment>();

    /// <summary>
    /// 关联的游戏
    /// </summary>
    [NotMapped]
    public virtual Game Game { get; set; } = null!;
}