using System.Text.Json.Serialization;

namespace GameCompanion.Api.DTOs.Messaging;

/// <summary>
/// 通知项DTO
/// </summary>
public class NotificationDto
{
    /// <summary>
    /// 通知ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 通知标题
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 通知内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 通知类型：0-系统通知，1-活动通知，2-订单通知
    /// </summary>
    public int Type { get; set; }

    /// <summary>
    /// 通知类型文本
    /// </summary>
    public string TypeText { get; set; } = string.Empty;

    /// <summary>
    /// 是否已读
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// 优先级：1-重要，2-普通
    /// </summary>
    public int Priority { get; set; }

    /// <summary>
    /// 优先级文本
    /// </summary>
    public string PriorityText { get; set; } = string.Empty;

    /// <summary>
    /// 跳转链接（可为null）
    /// </summary>
    public string? JumpUrl { get; set; }

    /// <summary>
    /// 跳转类型：webview-网页，internal-内部页面
    /// </summary>
    public string? JumpType { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    public string CreatedAt { get; set; } = string.Empty;

    /// <summary>
    /// 友好时间显示
    /// </summary>
    public string TimeText { get; set; } = string.Empty;
}

/// <summary>
/// 通知列表响应DTO
/// </summary>
public class NotificationListResponseDto
{
    /// <summary>
    /// 通知列表
    /// </summary>
    public List<NotificationDto> Items { get; set; } = new();

    /// <summary>
    /// 未读通知数
    /// </summary>
    public int UnreadCount { get; set; }

    /// <summary>
    /// 分页信息
    /// </summary>
    public PaginationDto Pagination { get; set; } = null!;
}

/// <summary>
/// 标记通知已读响应DTO
/// </summary>
public class MarkNotificationReadResponseDto
{
    /// <summary>
    /// 通知ID
    /// </summary>
    public long NotificationId { get; set; }

    /// <summary>
    /// 是否已读
    /// </summary>
    public bool IsRead { get; set; }

    /// <summary>
    /// 读取时间
    /// </summary>
    public string ReadAt { get; set; } = string.Empty;
}