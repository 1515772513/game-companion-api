using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 优惠券实体
/// </summary>
[Table("coupons")]
public class Coupon
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("code")]
    [MaxLength(50)]
    public string Code { get; set; } = string.Empty;

    [Column("type")]
    [MaxLength(20)]
    public string? Type { get; set; }

    [Column("amount")]
    public decimal? Amount { get; set; }

    [Column("min_amount")]
    public decimal? MinAmount { get; set; }

    [Column("status")]
    [MaxLength(20)]
    public string? Status { get; set; } = "未使用";

    [Column("expire_time")]
    public DateTime? ExpireTime { get; set; }

    [Column("used_time")]
    public DateTime? UsedTime { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}
