using System.Text.Json.Serialization;

namespace GameCompanion.Api.DTOs.Messaging;

/// <summary>
/// 会话项DTO
/// </summary>
public class ConversationItemDto
{
    /// <summary>
    /// 会话ID
    /// </summary>
    public long ConversationId { get; set; }

    /// <summary>
    /// 会话类型：user-用户聊天，system-系统通知
    /// </summary>
    public string ConversationType { get; set; } = "user";

    /// <summary>
    /// 对话用户信息（系统通知时为null）
    /// </summary>
    public UserDto? User { get; set; }

    /// <summary>
    /// 最后一条消息
    /// </summary>
    public LastMessageDto? LastMessage { get; set; }

    /// <summary>
    /// 未读消息数
    /// </summary>
    public int UnreadCount { get; set; }

    /// <summary>
    /// 对方是否在线
    /// </summary>
    public bool IsOnline { get; set; }

    /// <summary>
    /// 是否置顶
    /// </summary>
    public bool IsPinned { get; set; }

    /// <summary>
    /// 是否被拉黑
    /// </summary>
    public bool IsBlocked { get; set; }

    /// <summary>
    /// 最后更新时间
    /// </summary>
    public string UpdatedAt { get; set; } = string.Empty;
}

/// <summary>
/// 用户DTO
/// </summary>
public class UserDto
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string Nickname { get; set; } = string.Empty;

    /// <summary>
    /// 头像URL
    /// </summary>
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// 等级（陪玩师）
    /// </summary>
    public string? Level { get; set; }

    /// <summary>
    /// 是否为陪玩师
    /// </summary>
    public bool IsCompanion { get; set; }

    /// <summary>
    /// 是否在线
    /// </summary>
    public bool? IsOnline { get; set; }
}

/// <summary>
/// 最后一条消息DTO
/// </summary>
public class LastMessageDto
{
    /// <summary>
    /// 消息ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 消息类型：0-文本，1-图片，2-语音
    /// </summary>
    public int MessageType { get; set; }

    /// <summary>
    /// 发送者ID
    /// </summary>
    public int SenderId { get; set; }

    /// <summary>
    /// 时间文本（友好格式）
    /// </summary>
    public string Time { get; set; } = string.Empty;

    /// <summary>
    /// 时间戳
    /// </summary>
    public long Timestamp { get; set; }
}