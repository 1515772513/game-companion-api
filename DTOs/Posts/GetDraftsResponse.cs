namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 获取草稿列表响应DTO
/// </summary>
public class GetDraftsResponse
{
    /// <summary>
    /// 草稿列表
    /// </summary>
    public List<DraftItemDto> Items { get; set; } = new List<DraftItemDto>();
}