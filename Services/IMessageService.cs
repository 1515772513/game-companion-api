using GameCompanion.Api.DTOs.Messaging;
using GameCompanion.Api.Models;

namespace GameCompanion.Api.Services;

/// <summary>
/// 消息服务接口
/// </summary>
public interface IMessageService
{
    /// <summary>
    /// 获取会话列表
    /// </summary>
    Task<ApiResponse<ConversationsResponseDto>> GetConversationsAsync(int userId, string? type = "all", int page = 1, int pageSize = 20);

    /// <summary>
    /// 获取聊天详情
    /// </summary>
    Task<ApiResponse<ConversationDetailDto>> GetConversationDetailAsync(int userId, long conversationId, int page = 1, int pageSize = 50);

    /// <summary>
    /// 发送消息
    /// </summary>
    Task<ApiResponse<SendMessageResponseDto>> SendMessageAsync(int userId, long conversationId, SendMessageRequestDto request);

    /// <summary>
    /// 上传聊天图片
    /// </summary>
    Task<ApiResponse<UploadImageResponseDto>> UploadImageAsync(int userId, IFormFile image);

    /// <summary>
    /// 获取官方通知
    /// </summary>
    Task<ApiResponse<NotificationListResponseDto>> GetNotificationsAsync(int userId, int? type = null, int page = 1, int pageSize = 20);

    /// <summary>
    /// 标记通知已读
    /// </summary>
    Task<ApiResponse<MarkNotificationReadResponseDto>> MarkNotificationReadAsync(int userId, long notificationId);
}