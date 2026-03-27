using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 订单评价实体
/// </summary>
[Table("order_reviews")]
public class OrderReview
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("order_id")]
    public int OrderId { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("companion_id")]
    public int CompanionId { get; set; }

    [Column("rating")]
    public int Rating { get; set; }

    [Column("content")]
    public string? Content { get; set; }

    [Column("tags")]
    [MaxLength(255)]
    public string? Tags { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    [ForeignKey("OrderId")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("CompanionId")]
    public virtual Companion Companion { get; set; } = null!;
}
