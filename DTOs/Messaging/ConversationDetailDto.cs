using System.Text.Json.Serialization;

namespace GameCompanion.Api.DTOs.Messaging;

/// <summary>
/// 会话详情响应DTO
/// </summary>
public class ConversationDetailDto
{
    /// <summary>
    /// 会话信息
    /// </summary>
    public ConversationInfoDto Conversation { get; set; } = null!;

    /// <summary>
    /// 消息列表
    /// </summary>
    public List<MessageDetailDto> Messages { get; set; } = new();

    /// <summary>
    /// 分页信息
    /// </summary>
    public PaginationDto Pagination { get; set; } = null!;
}

/// <summary>
/// 会话信息DTO
/// </summary>
public class ConversationInfoDto
{
    /// <summary>
    /// 会话ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 对话用户信息
    /// </summary>
    public UserInfoDto User { get; set; } = null!;
}

/// <summary>
/// 用户信息DTO
/// </summary>
public class UserInfoDto
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
    public int? Level { get; set; } = null;

    /// <summary>
    /// 是否在线
    /// </summary>
    public bool? IsOnline { get; set; }
}

/// <summary>
/// 消息详情DTO
/// </summary>
public class MessageDetailDto
{
    /// <summary>
    /// 消息ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 发送者ID
    /// </summary>
    public int SenderId { get; set; }

    /// <summary>
    /// 发送者类型：0-自己，1-对方
    /// </summary>
    public int SenderType { get; set; }

    /// <summary>
    /// 发送者昵称
    /// </summary>
    public string SenderNickname { get; set; } = string.Empty;

    /// <summary>
    /// 消息内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 消息类型：0-文本，1-图片，2-语音
    /// </summary>
    public int MessageType { get; set; }

    /// <summary>
    /// 时间文本
    /// </summary>
    public string Time { get; set; } = string.Empty;

    /// <summary>
    /// 时间戳
    /// </summary>
    public long Timestamp { get; set; }

    /// <summary>
    /// 是否为自己发送
    /// </summary>
    public bool IsSelf { get; set; }

    /// <summary>
    /// 是否已读
    /// </summary>
    public bool IsRead { get; set; }
}

/// <summary>
/// 分页DTO
/// </summary>
public class PaginationDto
{
    /// <summary>
    /// 当前页码
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// 每页数量
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// 总数量
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// 总页数
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// 是否有更多数据
    /// </summary>
    public bool HasMore { get; set; }
}