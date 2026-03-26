namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 获取我的发布响应DTO
/// </summary>
public class GetMyPostsResponse
{
    /// <summary>
    /// 动态列表
    /// </summary>
    public List<MyPostItemDto> Items { get; set; } = new List<MyPostItemDto>();

    /// <summary>
    /// 分页信息
    /// </summary>
    public PaginationDto Pagination { get; set; } = new PaginationDto();
}