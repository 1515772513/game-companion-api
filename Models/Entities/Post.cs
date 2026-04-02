using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 动态表
/// </summary>
[Table("posts")]
[Index("CircleId", Name = "idx_circle_id")]
[Index("CreatedAt", Name = "idx_created_at")]
[Index("Status", Name = "idx_status")]
[Index("UserId", Name = "idx_user_id")]
public partial class Post
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 发布用户ID
    /// </summary>
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// 圈子ID
    /// </summary>
    [Column("circle_id")]
    public int? CircleId { get; set; }

    /// <summary>
    /// 动态内容
    /// </summary>
    [Column("content", TypeName = "text")]
    public string Content { get; set; } = null!;

    /// <summary>
    /// 图片URL(多个用逗号分隔)
    /// </summary>
    [Column("images")]
    [StringLength(1000)]
    public string? Images { get; set; }

    /// <summary>
    /// 位置
    /// </summary>
    [Column("location")]
    [StringLength(255)]
    public string? Location { get; set; }

    /// <summary>
    /// 话题标签
    /// </summary>
    [Column("tags")]
    [StringLength(500)]
    public string? Tags { get; set; }

    /// <summary>
    /// 提醒的用户ID
    /// </summary>
    [Column("mention_users")]
    [StringLength(500)]
    public string? MentionUsers { get; set; }

    /// <summary>
    /// 可见性
    /// </summary>
    [Column("visibility", TypeName = "enum('public','followers','private')")]
    public string? Visibility { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [Column("status", TypeName = "enum('draft','pending','approved','rejected','published')")]
    public string? Status { get; set; }

    /// <summary>
    /// 点赞数
    /// </summary>
    [Column("like_count")]
    public int? LikeCount { get; set; }

    /// <summary>
    /// 评论数
    /// </summary>
    [Column("comment_count")]
    public int? CommentCount { get; set; }

    /// <summary>
    /// 分享数
    /// </summary>
    [Column("share_count")]
    public int? ShareCount { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 收藏数
    /// </summary>
    public int? CollectCount { get; set; }

    [InverseProperty("Post")]
    public virtual ICollection<PostComment> PostComments { get; set; } = new List<PostComment>();

    [InverseProperty("Post")]
    public virtual ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();

    [ForeignKey("UserId")]
    [InverseProperty("Posts")]
    public virtual User User { get; set; } = null!;
}
