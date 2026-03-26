namespace GameCompanion.Api.DTOs;

/// <summary>
/// 会话信息
/// </summary>
public class ConversationInfo
{
    public long Conversation_id { get; set; }
    public string Conversation_type { get; set; } = string.Empty; // user, system
    public ConversationUserInfo? User { get; set; }
    public LastMessageInfo Last_message { get; set; } = new();
    public int Unread_count { get; set; }
    public bool Is_online { get; set; }
    public bool Is_pinned { get; set; }
    public bool Is_blocked { get; set; }
    public string Updated_at { get; set; } = string.Empty;
}

/// <summary>
/// 会话用户信息
/// </summary>
public class ConversationUserInfo
{
    public int Id { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string Avatar_url { get; set; } = string.Empty;
    public string? Level { get; set; } // 陪玩师等级
    public bool Is_companion { get; set; }
}

/// <summary>
/// 最后一条消息信息
/// </summary>
public class LastMessageInfo
{
    public long Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Message_type { get; set; } // 0-文本，1-图片，2-语音
    public int Sender_id { get; set; }
    public string Time { get; set; } = string.Empty; // 友好时间格式
    public long Timestamp { get; set; }
}

/// <summary>
/// 消息详情响应
/// </summary>
public class MessageDetailResponse
{
    public ConversationDetailInfo Conversation { get; set; } = new();
    public List<MessageInfo> Messages { get; set; } = new();
}

/// <summary>
/// 会话详情信息
/// </summary>
public class ConversationDetailInfo
{
    public long Id { get; set; }
    public ConversationUserInfo User { get; set; } = new();
}

/// <summary>
/// 消息信息
/// </summary>
public class MessageInfo
{
    public long Id { get; set; }
    public int Sender_id { get; set; }
    public int Sender_type { get; set; } // 0-自己，1-对方
    public string Sender_nickname { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Message_type { get; set; }
    public string Time { get; set; } = string.Empty;
    public long Timestamp { get; set; }
    public bool Is_self { get; set; }
    public bool Is_read { get; set; }
}

/// <summary>
/// 发送消息请求
/// </summary>
public class SendMessageRequest
{
    public string Content { get; set; } = string.Empty;
    public int Message_type { get; set; } // 0-文本，1-图片，2-语音
    public object? Extra { get; set; } // 扩展信息：图片URL、语音时长等
}

/// <summary>
/// 发送消息响应
/// </summary>
public class SendMessageResponse
{
    public long Message_id { get; set; }
    public long Conversation_id { get; set; }
    public string Content { get; set; } = string.Empty;
    public int Message_type { get; set; }
    public int Sender_id { get; set; }
    public bool Is_self { get; set; }
    public string Sent_at { get; set; } = string.Empty;
}

/// <summary>
/// 通知信息
/// </summary>
public class NotificationInfo
{
    public long Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public int Type { get; set; } // 0-系统通知，1-活动通知，2-订单通知
    public string Type_text { get; set; } = string.Empty;
    public bool Is_read { get; set; }
    public int Priority { get; set; } // 1-重要，2-普通
    public string Priority_text { get; set; } = string.Empty;
    public string? Jump_url { get; set; }
    public string? Jump_type { get; set; } // webview, internal
    public string Created_at { get; set; } = string.Empty;
    public string Time_text { get; set; } = string.Empty;
}
