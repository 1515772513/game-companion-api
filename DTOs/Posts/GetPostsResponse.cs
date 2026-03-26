namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 获取动态列表响应DTO
/// </summary>
public class GetPostsResponse
{
    /// <summary>
    /// 动态列表
    /// </summary>
    public List<PostListItemDto> Items { get; set; } = new List<PostListItemDto>();

    /// <summary>
    /// 分页信息
    /// </summary>
    public PaginationDto Pagination { get; set; } = new PaginationDto();
}