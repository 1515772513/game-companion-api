namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 收藏动态请求DTO
/// </summary>
public class CollectPostRequest
{
    /// <summary>
    /// 操作类型：collect-收藏，uncollect-取消收藏
    /// </summary>
    public string Action { get; set; } = string.Empty;
}