using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 会话表
/// </summary>
[Table("conversations")]
[Index("CompanionId", Name = "idx_companion_id")]
[Index("UserId", Name = "idx_user_id")]
[Index("UserId", "CompanionId", Name = "uk_user_companion", IsUnique = true)]
public partial class Conversation
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// 陪玩师ID
    /// </summary>
    [Column("companion_id")]
    public int CompanionId { get; set; }

    /// <summary>
    /// 最后一条消息
    /// </summary>
    [Column("last_message", TypeName = "text")]
    public string? LastMessage { get; set; }

    /// <summary>
    /// 最后消息时间
    /// </summary>
    [Column("last_message_time", TypeName = "datetime")]
    public DateTime? LastMessageTime { get; set; }

    /// <summary>
    /// 未读数
    /// </summary>
    [Column("unread_count")]
    public int? UnreadCount { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("CompanionId")]
    [InverseProperty("Conversations")]
    public virtual Companion Companion { get; set; } = null!;

    [InverseProperty("Conversation")]
    public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    [ForeignKey("UserId")]
    [InverseProperty("Conversations")]
    public virtual User User { get; set; } = null!;
}
