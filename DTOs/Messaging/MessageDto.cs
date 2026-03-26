using System.Text.Json.Serialization;

namespace GameCompanion.Api.DTOs.Messaging;

/// <summary>
/// 发送消息请求DTO
/// </summary>
public class SendMessageRequestDto
{
    /// <summary>
    /// 消息内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 消息类型：0-文本，1-图片，2-语音
    /// </summary>
    public int MessageType { get; set; }

    /// <summary>
    /// 扩展信息
    /// </summary>
    public MessageExtraDto? Extra { get; set; }
}

/// <summary>
/// 消息扩展信息DTO
/// </summary>
public class MessageExtraDto
{
    /// <summary>
    /// 图片宽度（图片消息）
    /// </summary>
    public int? Width { get; set; }

    /// <summary>
    /// 图片高度（图片消息）
    /// </summary>
    public int? Height { get; set; }

    /// <summary>
    /// 图片大小（图片消息）
    /// </summary>
    public int? Size { get; set; }

    /// <summary>
    /// 语音时长（语音消息）
    /// </summary>
    public int? Duration { get; set; }
}

/// <summary>
/// 发送消息响应DTO
/// </summary>
public class SendMessageResponseDto
{
    /// <summary>
    /// 消息ID
    /// </summary>
    public long MessageId { get; set; }

    /// <summary>
    /// 会话ID
    /// </summary>
    public long ConversationId { get; set; }

    /// <summary>
    /// 消息内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 消息类型
    /// </summary>
    public int MessageType { get; set; }

    /// <summary>
    /// 发送者ID
    /// </summary>
    public int SenderId { get; set; }

    /// <summary>
    /// 是否为自己发送
    /// </summary>
    public bool IsSelf { get; set; }

    /// <summary>
    /// 发送时间
    /// </summary>
    public string SentAt { get; set; } = string.Empty;
}