using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 会话实体
/// </summary>
[Table("conversations")]
public class Conversation
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("companion_id")]
    public int? CompanionId { get; set; }

    [Column("last_message")]
    [MaxLength(500)]
    public string? LastMessage { get; set; }

    [Column("last_message_time")]
    public DateTime? LastMessageTime { get; set; }

    [Column("unread_count")]
    public int? UnreadCount { get; set; } = 0;

    [Column("is_pinned")]
    public int? IsPinned { get; set; } = 0;

    [Column("is_blocked")]
    public int? IsBlocked { get; set; } = 0;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;

    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();
}
