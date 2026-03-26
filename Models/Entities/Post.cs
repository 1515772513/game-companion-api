using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 动态实体
/// </summary>
[Table("posts")]
public class Post
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("circle_id")]
    public int? CircleId { get; set; }

    [Column("game_id")]
    public int? GameId { get; set; }

    [Column("content")]
    public string Content { get; set; } = string.Empty;

    [Column("images")]
    [MaxLength(1000)]
    public string? Images { get; set; }

    [Column("location")]
    [MaxLength(255)]
    public string? Location { get; set; }

    [Column("tags")]
    [MaxLength(500)]
    public string? Tags { get; set; }

    [Column("mention_users")]
    [MaxLength(500)]
    public string? MentionUsers { get; set; }

    [Column("visibility")]
    [MaxLength(20)]
    public string? Visibility { get; set; } = "公开";

    [Column("status")]
    [MaxLength(20)]
    public string? Status { get; set; } = "已发布";

    [Column("like_count")]
    public int? LikeCount { get; set; }

    [Column("comment_count")]
    public int? CommentCount { get; set; }

    [Column("share_count")]
    public int? ShareCount { get; set; }

    [Column("collect_count")]
    public int? CollectCount { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
    public virtual ICollection<PostComment> Comments { get; set; } = new List<PostComment>();
    public virtual ICollection<PostLike> Likes { get; set; } = new List<PostLike>();
    public virtual Game? Game { get; set; }
}
