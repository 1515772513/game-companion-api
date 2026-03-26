using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 系统通知实体
/// </summary>
[Table("notifications")]
public class Notification
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("title")]
    [MaxLength(200)]
    public string Title { get; set; } = string.Empty;

    [Column("content")]
    public string Content { get; set; } = string.Empty;

    [Column("type")]
    [MaxLength(20)]
    public string? Type { get; set; } = "系统通知";

    [Column("is_read")]
    public int? IsRead { get; set; } = 0;

    [Column("priority")]
    public int? Priority { get; set; } = 2;

    [Column("jump_url")]
    [MaxLength(500)]
    public string? JumpUrl { get; set; }

    [Column("jump_type")]
    [MaxLength(20)]
    public string? JumpType { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("read_at")]
    public DateTime? ReadAt { get; set; }

    // 导航属性
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}
