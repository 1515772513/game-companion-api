namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 获取我的发布请求DTO
/// </summary>
public class GetMyPostsRequest
{
    /// <summary>
    /// 页码
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// 每页数量
    /// </summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// 状态筛选：0-审核中，1-已发布，2-已拒绝
    /// </summary>
    public int? Status { get; set; }
}