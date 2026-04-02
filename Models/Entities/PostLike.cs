using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 动态点赞表
/// </summary>
[Table("post_likes")]
[Index("PostId", Name = "idx_post_id")]
[Index("PostId", "UserId", Name = "uk_post_user", IsUnique = true)]
[Index("UserId", Name = "user_id")]
public partial class PostLike
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
    /// 点赞用户ID
    /// </summary>
    [Column("user_id")]
    public int UserId { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("PostId")]
    [InverseProperty("PostLikes")]
    public virtual Post Post { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("PostLikes")]
    public virtual User User { get; set; } = null!;
}
