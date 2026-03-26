using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 游戏实体
/// </summary>
[Table("games")]
public class Game
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Column("icon")]
    [MaxLength(255)]
    public string? Icon { get; set; }

    [Column("icon_url")]
    [MaxLength(255)]
    public string? IconUrl { get; set; }

    [Column("description")]
    [MaxLength(500)]
    public string? Description { get; set; }

    [Column("is_hot")]
    public int? IsHot { get; set; } = 0;

    [Column("sort_order")]
    public int? SortOrder { get; set; } = 0;

    [Column("status")]
    [MaxLength(20)]
    public string? Status { get; set; } = "active";

    [Column("is_active")]
    public int? IsActive { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    public virtual ICollection<CompanionGame> CompanionGames { get; set; } = new List<CompanionGame>();
    public virtual ICollection<GameCircle> Circles { get; set; } = new List<GameCircle>();
}
