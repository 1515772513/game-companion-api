using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 订单实体
/// </summary>
[Table("orders")]
public class Order
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("order_no")]
    [MaxLength(50)]
    public string OrderNo { get; set; } = string.Empty;

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("companion_id")]
    public int CompanionId { get; set; }

    [Column("game_id")]
    public int GameId { get; set; }

    [Column("service_type")]
    [MaxLength(20)]
    public string? ServiceType { get; set; }

    [Column("play_time")]
    public DateTime? PlayTime { get; set; }

    [Column("duration_type")]
    [MaxLength(20)]
    public string? DurationType { get; set; }

    [Column("duration_value")]
    public int DurationValue { get; set; }

    [Column("unit_price")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal UnitPrice { get; set; }

    [Column("total_price")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal TotalPrice { get; set; }

    [Column("discount_amount")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal? DiscountAmount { get; set; }

    [Column("final_price")]
    [Column(TypeName = "decimal(10,2)")]
    public decimal FinalPrice { get; set; }

    [Column("remark")]
    public string? Remark { get; set; }

    [Column("status")]
    [MaxLength(20)]
    public string Status { get; set; } = "待付款"; // 待付款、待服务、服务中、待确认、已完成、已取消

    [Column("pay_time")]
    public DateTime? PayTime { get; set; }

    [Column("start_time")]
    public DateTime? StartTime { get; set; }

    [Column("end_time")]
    public DateTime? EndTime { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    [ForeignKey("CompanionId")]
    public virtual Companion Companion { get; set; } = null!;
}
