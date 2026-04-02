using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 动态评论表
/// </summary>
[Table("post_comments")]
[Index("ParentId", Name = "idx_parent_id")]
[Index("PostId", Name = "idx_post_id")]
[Index("UserId", Name = "idx_user_id")]
public partial class PostComment
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 动态ID
    /// </summary>
    [Column("post_id")]
    public int PostId { get; set; }

    /// <summary>
    /// 评论用户ID
    /// </summary>
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// 父评论ID(0为一级评论)
    /// </summary>
    [Column("parent_id")]
    public int? ParentId { get; set; }

    /// <summary>
    /// 回复给的用户ID
    /// </summary>
    [Column("reply_to_user_id")]
    public int? ReplyToUserId { get; set; }

    /// <summary>
    /// 评论内容
    /// </summary>
    [Column("content", TypeName = "text")]
    public string Content { get; set; } = null!;

    /// <summary>
    /// 状态
    /// </summary>
    [Column("status", TypeName = "enum('normal','hidden','deleted')")]
    public string? Status { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    /// <summary>
    /// 点赞数
    /// </summary>
    public int? LikeCount { get; set; }

    [ForeignKey("PostId")]
    [InverseProperty("PostComments")]
    public virtual Post Post { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("PostComments")]
    public virtual User User { get; set; } = null!;
}
