using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 消息实体
/// </summary>
[Table("messages")]
public class Message
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("conversation_id")]
    public int ConversationId { get; set; }

    [Column("sender_id")]
    public int SenderId { get; set; }

    [Column("receiver_id")]
    public int ReceiverId { get; set; }

    [Column("content")]
    public string Content { get; set; } = string.Empty;

    [Column("message_type")]
    [MaxLength(20)]
    public string? MessageType { get; set; } = "文本";

    [Column("is_read")]
    public int? IsRead { get; set; } = 0;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    [ForeignKey("SenderId")]
    public virtual User Sender { get; set; } = null!;

    [ForeignKey("ReceiverId")]
    public virtual User Receiver { get; set; } = null!;

    [ForeignKey("ConversationId")]
    public virtual Conversation Conversation { get; set; } = null!;
}
