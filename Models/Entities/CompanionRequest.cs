using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 陪玩需求实体
/// </summary>
[Table("companion_requests")]
public class CompanionRequest
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("game_id")]
    public int GameId { get; set; }

    [Column("service_type")]
    [MaxLength(20)]
    public string? ServiceType { get; set; }

    [Column("min_price")]
    public decimal? MinPrice { get; set; }

    [Column("max_price")]
    public decimal? MaxPrice { get; set; }

    [Column("requirements")]
    [MaxLength(500)]
    public string? Requirements { get; set; }

    [Column("status")]
    [MaxLength(20)]
    public string? Status { get; set; } = "待接单";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}
