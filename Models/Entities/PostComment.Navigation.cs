using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

// 👇 关键：和自动生成的实体 同名、同命名空间、partial
public partial class PostComment
{
    /// <summary>
    /// 软关联：回复
    /// </summary>
    [NotMapped] // 👈 最重要！告诉 EF Core：这不是数据库字段
    public ICollection<PostComment> Replies { get; set; } = new List<PostComment>();

    /// <summary>
    /// 软关联：父评论
    /// </summary>
    [NotMapped] // 👈 最重要！告诉 EF Core：这不是数据库字段
    public virtual PostComment? Parent { get; set; }
}