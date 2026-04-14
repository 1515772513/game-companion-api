using GameCompanion.Api.Data;
using GameCompanion.Api.DTOs.Messaging;
using GameCompanion.Api.Models;
using GameCompanion.Api.Models.Entities;
using GameCompanion.Api.Utils;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace GameCompanion.Api.Services;

/// <summary>
/// 消息服务实现
/// </summary>
public class MessageService : IMessageService
{
    private readonly GameCompanionContext _context;
    private readonly ILogger<MessageService> _logger;

    public MessageService(GameCompanionContext context, ILogger<MessageService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// 获取会话列表
    /// </summary>
    public async Task<ApiResponse<ConversationsResponseDto>> GetConversationsAsync(int userId, string? type = "all", int page = 1, int pageSize = 20)
    {
        var query = _context.Conversations
            .Include(c => c.Messages.OrderByDescending(m => m.CreatedAt).Take(1))
            .Include(c => c.User)
            .Include(c => c.Companion)
            .Where(c => c.UserId == userId);

        // 根据类型筛选
        if (type == "user")
        {
            query = query.Where(c => c.CompanionId != null);
        }
        else if (type == "system")
        {
            // 系统通知的特殊处理
            query = query.Where(c => c.CompanionId == null);
        }

        // 分页查询
        var totalCount = await query.CountAsync();
        var conversations = await query
            .OrderByDescending(c => c.LastMessageTime ?? c.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // 计算总未读数
        var totalUnread = await _context.Conversations
            .Where(c => c.UserId == userId)
            .SumAsync(c => c.UnreadCount ?? 0);

        // 转换为DTO
        var items = new List<ConversationItemDto>();
        foreach (var conv in conversations)
        {
            var lastMessage = conv.Messages.FirstOrDefault();
            items.Add(new ConversationItemDto
            {
                ConversationId = conv.Id,
                ConversationType = conv.CompanionId != null ? "user" : "system",
                User = conv.Companion != null ? new UserDto
                {
                    Id = conv.Companion.Id,
                    Nickname = conv.Companion.Nickname,
                    AvatarUrl = conv.Companion.User?.Avatar,
                    Level = conv.Companion.Level,
                    IsCompanion = true,
                    IsOnline = conv.Companion.OnlineStatus == "在线"
                } : null,
                LastMessage = lastMessage != null ? new LastMessageDto
                {
                    Id = lastMessage.Id,
                    Content = lastMessage.Content,
                    MessageType = ConvertMessageType(lastMessage.MessageType),
                    SenderId = lastMessage.SenderId,
                    Time = lastMessage.CreatedAt.ToDateTimeString(),
                    Timestamp = new DateTimeOffset(lastMessage.CreatedAt ?? DateTime.UtcNow).ToUniversalTime().ToUnixTimeSeconds()
                } : null,
                UnreadCount = conv.UnreadCount ?? 0,
                IsOnline = conv.Companion?.OnlineStatus == "在线",
                // IsPinned = conv.IsPinned == true,
                // IsBlocked = conv.IsBlocked == true,
                UpdatedAt = conv.UpdatedAt.ToDateTimeString()
            });
        }

        var response = new ConversationsResponseDto
        {
            Items = items,
            TotalUnread = totalUnread
        };

        return ApiResponse<ConversationsResponseDto>.SuccessResponse(response);
    }

    /// <summary>
    /// 获取聊天详情
    /// </summary>
    public async Task<ApiResponse<ConversationDetailDto>> GetConversationDetailAsync(int userId, long conversationId, int page = 1, int pageSize = 50)
    {
        var conversation = await _context.Conversations
            .Include(c => c.User)
            .Include(c => c.Companion)
            .FirstOrDefaultAsync(c => c.Id == conversationId);

        if (conversation == null)
        {
            return ApiResponse<ConversationDetailDto>.ErrorResponse(5001, "对话不存在");
        }

        // 检查是否有权限访问这个会话
        if (conversation.UserId != userId && conversation.CompanionId != userId)
        {
            return ApiResponse<ConversationDetailDto>.ErrorResponse(5003, "已被对方拉黑");
        }

        // 获取消息列表
        var messagesQuery = _context.Messages
            .Include(m => m.Sender)
            .Include(m => m.Receiver)
            .Where(m => m.ConversationId == conversationId)
            .OrderBy(m => m.CreatedAt);

        var totalCount = await messagesQuery.CountAsync();
        var messages = await messagesQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // 转换为DTO
        var messageDtos = new List<MessageDetailDto>();
        foreach (var msg in messages)
        {
            messageDtos.Add(new MessageDetailDto
            {
                Id = msg.Id,
                SenderId = msg.SenderId,
                SenderType = userId == msg.SenderId ? 0 : 1,
                SenderNickname = msg.Sender.Nickname,
                Content = msg.Content,
                MessageType = ConvertMessageType(msg.MessageType),
                Time = msg.CreatedAt.ToDateTimeString(),
                Timestamp = new DateTimeOffset(msg.CreatedAt ?? DateTime.UtcNow).ToUniversalTime().ToUnixTimeSeconds(),
                IsSelf = userId == msg.SenderId,
                IsRead = msg.IsRead == true
            });
        }

        var pagination = new PaginationDto
        {
            Page = page,
            PageSize = pageSize,
            Total = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            HasMore = page < (int)Math.Ceiling((double)totalCount / pageSize)
        };

        var conversationInfo = new ConversationInfoDto
        {
            Id = conversation.Id,
            User = new UserInfoDto
            {
                Id = conversation.Companion?.Id ?? conversation.User.Id,
                Nickname = conversation.Companion?.Nickname ?? conversation.User.Nickname,
                AvatarUrl = conversation.Companion?.User?.Avatar ?? conversation.User.Avatar,
                Level = conversation.Companion?.Level,
                IsOnline = conversation.Companion?.OnlineStatus == "在线"
            }
        };

        var response = new ConversationDetailDto
        {
            Conversation = conversationInfo,
            Messages = messageDtos,
            Pagination = pagination
        };

        return ApiResponse<ConversationDetailDto>.SuccessResponse(response);
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    public async Task<ApiResponse<SendMessageResponseDto>> SendMessageAsync(int userId, long conversationId, SendMessageRequestDto request)
    {
        // 检查会话是否存在且有权限
        var conversation = await _context.Conversations
            .Include(c => c.User)
            .Include(c => c.Companion)
            .FirstOrDefaultAsync(c => c.Id == conversationId);

        if (conversation == null)
        {
            return ApiResponse<SendMessageResponseDto>.ErrorResponse(5001, "对话不存在");
        }

        // 检查是否被对方拉黑
        if (conversation.CompanionId == userId && conversation.User?.IsBlocked == true)
        {
            return ApiResponse<SendMessageResponseDto>.ErrorResponse(5003, "已被对方拉黑");
        }

        // 检查消息内容
        if (string.IsNullOrWhiteSpace(request.Content))
        {
            return ApiResponse<SendMessageResponseDto>.ErrorResponse(400, "请求参数错误", "消息内容不能为空");
        }

        // 创建新消息
        var message = new Message
        {
            ConversationId = (int)conversationId,
            SenderId = userId,
            ReceiverId = conversation.UserId == userId ? conversation.CompanionId : conversation.UserId,
            Content = request.Content,
            MessageType = ConvertMessageTypeToDb(request.MessageType),
            IsRead = false,
            CreatedAt = DateTime.UtcNow
        };

        _context.Messages.Add(message);
        await _context.SaveChangesAsync();

        // 更新会话的最后消息和未读数
        conversation.LastMessage = request.Content;
        conversation.LastMessageTime = DateTime.UtcNow;
        if (conversation.UserId != userId)
        {
            conversation.UnreadCount = (conversation.UnreadCount ?? 0) + 1;
        }

        await _context.SaveChangesAsync();

        // 返回响应
        var response = new SendMessageResponseDto
        {
            MessageId = message.Id,
            ConversationId = conversationId,
            Content = request.Content,
            MessageType = request.MessageType,
            SenderId = userId,
            IsSelf = true,
            SentAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
        };

        return ApiResponse<SendMessageResponseDto>.SuccessResponse(response);
    }

    /// <summary>
    /// 上传聊天图片
    /// </summary>
    public async Task<ApiResponse<UploadImageResponseDto>> UploadImageAsync(int userId, IFormFile image)
    {
        if (image == null || image.Length == 0)
        {
            return ApiResponse<UploadImageResponseDto>.ErrorResponse(6001, "文件格式不支持");
        }

        // 验证文件类型
        var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
        var fileExtension = Path.GetExtension(image.FileName).ToLower();
        if (!allowedExtensions.Contains(fileExtension))
        {
            return ApiResponse<UploadImageResponseDto>.ErrorResponse(6001, "文件格式不支持");
        }

        // 验证文件大小 (5MB)
        if (image.Length > 5 * 1024 * 1024)
        {
            return ApiResponse<UploadImageResponseDto>.ErrorResponse(6002, "文件大小超限");
        }

        // 生成文件名
        var fileName = $"chat_{userId}_{DateTime.Now:yyyyMMddHHmmss}{fileExtension}";
        var folderPath = Path.Combine("uploads", "chat", DateTime.Now.ToString("yyyy-MM-dd"));
        Directory.CreateDirectory(folderPath);
        var filePath = Path.Combine(folderPath, fileName);

        // 保存文件
        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await image.CopyToAsync(stream);
        }

        // 获取文件信息
        var fileInfo = new FileInfo(filePath);
        var imageUrl = $"/uploads/chat/{DateTime.Now:yyyy-MM-dd}/{fileName}";

        var response = new UploadImageResponseDto
        {
            ImageUrl = imageUrl,
            Width = 1080, // 模拟宽度
            Height = 1920, // 模拟高度
            Size = (int)fileInfo.Length,
            MimeType = image.ContentType
        };

        return ApiResponse<UploadImageResponseDto>.SuccessResponse(response);
    }

    /// <summary>
    /// 获取官方通知
    /// </summary>
    public async Task<ApiResponse<NotificationListResponseDto>> GetNotificationsAsync(int userId, int? type = null, int page = 1, int pageSize = 20)
    {
        var query = _context.Notifications
            .Where(n => n.UserId == userId);

        // 根据类型筛选
        if (type.HasValue)
        {
            query = query.Where(n => n.Type == type.ToString());
        }

        // 分页查询
        var totalCount = await query.CountAsync();
        var notifications = await query
            .OrderByDescending(n => n.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        // 计算未读数
        var unreadCount = await _context.Notifications
            .Where(n => n.UserId == userId && n.IsRead != true)
            .CountAsync();

        // 转换为DTO
        var items = new List<NotificationDto>();
        foreach (var notif in notifications)
        {
            items.Add(new NotificationDto
            {
                Id = notif.Id,
                Title = notif.Title,
                Content = notif.Content,
                Type = ConvertNotificationType(notif.Type),
                TypeText = GetNotificationTypeText(notif.Type),
                IsRead = notif.IsRead == true,
                // Priority = notif.Priority ?? 2,
                // PriorityText = notif.Priority == 1 ? "重要" : "普通",
                // JumpUrl = notif.JumpUrl,
                // JumpType = notif.JumpType,
                CreatedAt = notif.CreatedAt.ToDateTimeString(),
                TimeText = notif.CreatedAt.ToDateTimeString()
            });
        }

        var pagination = new PaginationDto
        {
            Page = page,
            PageSize = pageSize,
            Total = totalCount,
            TotalPages = (int)Math.Ceiling((double)totalCount / pageSize),
            HasMore = page < (int)Math.Ceiling((double)totalCount / pageSize)
        };

        var response = new NotificationListResponseDto
        {
            Items = items,
            UnreadCount = unreadCount,
            Pagination = pagination
        };

        return ApiResponse<NotificationListResponseDto>.SuccessResponse(response);
    }

    /// <summary>
    /// 标记通知已读
    /// </summary>
    public async Task<ApiResponse<MarkNotificationReadResponseDto>> MarkNotificationReadAsync(int userId, long notificationId)
    {
        var notification = await _context.Notifications
            .FirstOrDefaultAsync(n => n.Id == notificationId && n.UserId == userId);

        if (notification == null)
        {
            return ApiResponse<MarkNotificationReadResponseDto>.ErrorResponse(5010, "通知不存在");
        }

        notification.IsRead = true;
        // notification.ReadAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var response = new MarkNotificationReadResponseDto
        {
            NotificationId = notificationId,
            IsRead = true,
            ReadAt = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
        };

        return ApiResponse<MarkNotificationReadResponseDto>.SuccessResponse(response);
    }

    #region 私有方法

    /// <summary>
    /// 转换消息类型
    /// </summary>
    private int ConvertMessageType(string? dbType)
    {
        return dbType switch
        {
            "文本" => 0,
            "图片" => 1,
            "语音" => 2,
            _ => 0
        };
    }

    /// <summary>
    /// 转换消息类型到数据库
    /// </summary>
    private string ConvertMessageTypeToDb(int messageType)
    {
        return messageType switch
        {
            0 => "文本",
            1 => "图片",
            2 => "语音",
            _ => "文本"
        };
    }

    /// <summary>
    /// 转换通知类型
    /// </summary>
    private int ConvertNotificationType(string? dbType)
    {
        return dbType switch
        {
            "系统通知" => 0,
            "活动通知" => 1,
            "订单通知" => 2,
            _ => 0
        };
    }

    /// <summary>
    /// 获取通知类型文本
    /// </summary>
    private string GetNotificationTypeText(string? dbType)
    {
        return dbType switch
        {
            "系统通知" => "系统通知",
            "活动通知" => "活动通知",
            "订单通知" => "订单通知",
            _ => "系统通知"
        };
    }

    /// <summary>
    /// 格式化时间
    /// </summary>
    private string FormatTime(DateTime dateTime)
    {
        var now = DateTime.Now;
        var diff = now - dateTime;

        if (diff.TotalMinutes < 1)
        {
            return "刚刚";
        }
        else if (diff.TotalMinutes < 60)
        {
            return $"{(int)diff.TotalMinutes}分钟前";
        }
        else if (diff.TotalHours < 24)
        {
            return $"{(int)diff.TotalHours}小时前";
        }
        else if (diff.TotalDays < 7)
        {
            return $"{(int)diff.TotalDays}天前";
        }
        else if (dateTime.Year == now.Year)
        {
            return dateTime.ToString("MM-dd");
        }
        else
        {
            return dateTime.ToString("yyyy-MM-dd");
        }
    }

    #endregion
}