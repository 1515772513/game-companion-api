using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 代练服务实体
/// </summary>
[Table("power_leveling")]
public class PowerLeveling
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("game_id")]
    public int GameId { get; set; }

    [Column("service_name")]
    [MaxLength(100)]
    public string ServiceName { get; set; } = string.Empty;

    [Column("start_rank")]
    [MaxLength(50)]
    public string StartRank { get; set; } = string.Empty;

    [Column("end_rank")]
    [MaxLength(50)]
    public string EndRank { get; set; } = string.Empty;

    [Column("price")]
    public decimal Price { get; set; }

    [Column("original_price")]
    public decimal? OriginalPrice { get; set; }

    [Column("discount")]
    public decimal? Discount { get; set; }

    [Column("estimated_days")]
    public int? EstimatedDays { get; set; }

    [Column("description")]
    public string? Description { get; set; }

    [Column("requirements")]
    public string? Requirements { get; set; }

    [Column("process_steps")]
    public string? ProcessSteps { get; set; }

    [Column("status")]
    [MaxLength(20)]
    public string? Status { get; set; } = "上架";

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    [ForeignKey("GameId")]
    public virtual Game Game { get; set; } = null!;
}
