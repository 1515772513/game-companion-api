using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 关注实体
/// </summary>
[Table("follows")]
public class Follow
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("follower_id")]
    public int FollowerId { get; set; }

    [Column("following_id")]
    public int FollowingId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    [ForeignKey("FollowerId")]
    public virtual User? Follower { get; set; }

    [ForeignKey("FollowingId")]
    public virtual User? Following { get; set; }
}
