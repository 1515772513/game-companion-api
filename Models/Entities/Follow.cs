using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 关注表
/// </summary>
[Table("follows")]
[Index("FollowerId", Name = "idx_follower_id")]
[Index("FollowingId", Name = "idx_following_id")]
[Index("FollowerId", "FollowingId", Name = "uk_follower_following", IsUnique = true)]
public partial class Follow
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 关注者ID
    /// </summary>
    [Column("follower_id")]
    public int FollowerId { get; set; }

    /// <summary>
    /// 被关注者ID
    /// </summary>
    [Column("following_id")]
    public int FollowingId { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("FollowerId")]
    [InverseProperty("FollowFollowers")]
    public virtual User Follower { get; set; } = null!;

    [ForeignKey("FollowingId")]
    [InverseProperty("FollowFollowings")]
    public virtual User Following { get; set; } = null!;
}
