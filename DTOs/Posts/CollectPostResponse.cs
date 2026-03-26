namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 收藏动态响应DTO
/// </summary>
public class CollectPostResponse
{
    /// <summary>
    /// 动态ID
    /// </summary>
    public long PostId { get; set; }

    /// <summary>
    /// 当前用户是否已收藏
    /// </summary>
    public bool IsCollected { get; set; }

    /// <summary>
    /// 收藏数
    /// </summary>
    public int CollectCount { get; set; }
}