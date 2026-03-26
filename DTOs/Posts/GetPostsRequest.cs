namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 获取动态列表请求DTO
/// </summary>
public class GetPostsRequest
{
    /// <summary>
    /// 页码，从1开始
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// 每页数量，范围1-50
    /// </summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// feed类型：recommend-推荐，follow-关注
    /// </summary>
    public string FeedType { get; set; } = "recommend";

    /// <summary>
    /// 游戏圈子筛选
    /// </summary>
    public int? GameId { get; set; }

    /// <summary>
    /// 话题筛选
    /// </summary>
    public int? TopicId { get; set; }
}