using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 消息表
/// </summary>
[Table("messages")]
[Index("ConversationId", Name = "idx_conversation_id")]
[Index("CreatedAt", Name = "idx_created_at")]
[Index("ReceiverId", Name = "idx_receiver_id")]
[Index("SenderId", Name = "idx_sender_id")]
public partial class Message
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 会话ID
    /// </summary>
    [Column("conversation_id")]
    public int ConversationId { get; set; }

    /// <summary>
    /// 发送者ID
    /// </summary>
    [Column("sender_id")]
    public int SenderId { get; set; }

    /// <summary>
    /// 接收者ID
    /// </summary>
    [Column("receiver_id")]
    public int ReceiverId { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    [Column("content", TypeName = "text")]
    public string Content { get; set; } = null!;

    /// <summary>
    /// 消息类型
    /// </summary>
    [Column("message_type", TypeName = "enum('text','image','voice','system')")]
    public string? MessageType { get; set; }

    /// <summary>
    /// 是否已读
    /// </summary>
    [Column("is_read")]
    public bool? IsRead { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("ConversationId")]
    [InverseProperty("Messages")]
    public virtual Conversation Conversation { get; set; } = null!;

    [ForeignKey("ReceiverId")]
    [InverseProperty("MessageReceivers")]
    public virtual User Receiver { get; set; } = null!;

    [ForeignKey("SenderId")]
    [InverseProperty("MessageSenders")]
    public virtual User Sender { get; set; } = null!;
}
