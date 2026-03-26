using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 交易记录实体
/// </summary>
[Table("transactions")]
public class Transaction
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("type")]
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty; // 收入、支出、退款

    [Column("amount", TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }

    [Column("description")]
    [MaxLength(255)]
    public string Description { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("order_id")]
    [MaxLength(50)]
    public string? OrderId { get; set; }

    [Column("status")]
    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;

    // 导航属性
    [ForeignKey("UserId")]
    public virtual User? User { get; set; }
}
