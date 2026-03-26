using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 动态评论实体
/// </summary>
[Table("post_comments")]
public class PostComment
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("post_id")]
    public int PostId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("parent_id")]
    public int? ParentId { get; set; }

    [Column("content")]
    public string Content { get; set; } = string.Empty;

    [Column("like_count")]
    public int? LikeCount { get; set; } = 0;

    [Column("likes")]
    public int? Likes { get; set; } = 0;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    [ForeignKey("PostId")]
    public virtual Post Post { get; set; } = null!;

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("ParentId")]
    public virtual PostComment? Parent { get; set; }

    public virtual ICollection<PostComment> Replies { get; set; } = new List<PostComment>();
}
