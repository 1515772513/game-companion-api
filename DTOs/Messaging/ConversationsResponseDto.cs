namespace GameCompanion.Api.DTOs.Messaging;

/// <summary>
/// 会话列表响应DTO
/// </summary>
public class ConversationsResponseDto
{
    /// <summary>
    /// 会话列表
    /// </summary>
    public List<ConversationItemDto> Items { get; set; } = new();

    /// <summary>
    /// 总未读消息数
    /// </summary>
    public int TotalUnread { get; set; }
}