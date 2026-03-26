using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 陪玩师申请实体
/// </summary>
[Table("companion_applications")]
public class CompanionApplication
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("game_category")]
    [MaxLength(100)]
    public string GameCategory { get; set; } = string.Empty;

    [Column("skill_level")]
    [MaxLength(100)]
    public string SkillLevel { get; set; } = string.Empty;

    [Column("self_introduction")]
    [MaxLength(500)]
    public string SelfIntroduction { get; set; } = string.Empty;

    [Column("hourly_rate")]
    public decimal HourlyRate { get; set; }

    [Column("available_time")]
    [MaxLength(100)]
    public string AvailableTime { get; set; } = string.Empty;

    [Column("status")]
    [MaxLength(20)]
    public string Status { get; set; } = "待审核";

    [Column("admin_notes")]
    [MaxLength(500)]
    public string? AdminNotes { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    [ForeignKey("UserId")]
    public virtual User? User { get; set; }
}
