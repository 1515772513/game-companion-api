using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// VIP会员实体
/// </summary>
[Table("vip_memberships")]
public class VipMembership
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("level")]
    public int Level { get; set; } = 1;

    [Column("start_time")]
    public DateTime? StartTime { get; set; }

    [Column("expire_time")]
    public DateTime? ExpireTime { get; set; }

    [Column("status")]
    [MaxLength(20)]
    public string? Status { get; set; } = "激活";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}
