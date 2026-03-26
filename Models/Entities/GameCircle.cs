using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 游戏圈子实体
/// </summary>
[Table("game_circles")]
public class GameCircle
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("name")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Column("game_id")]
    public int GameId { get; set; }

    [Column("avatar_url")]
    [MaxLength(255)]
    public string? AvatarUrl { get; set; }

    [Column("description")]
    [MaxLength(500)]
    public string? Description { get; set; }

    [Column("is_official")]
    public int? IsOfficial { get; set; } = 0;

    [Column("member_count")]
    public int? MemberCount { get; set; } = 0;

    [Column("post_count")]
    public int? PostCount { get; set; } = 0;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    [ForeignKey("GameId")]
    public virtual Game Game { get; set; } = null!;
}
