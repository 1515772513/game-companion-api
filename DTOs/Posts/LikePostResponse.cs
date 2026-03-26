namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 点赞动态响应DTO
/// </summary>
public class LikePostResponse
{
    /// <summary>
    /// 动态ID
    /// </summary>
    public long PostId { get; set; }

    /// <summary>
    /// 当前用户是否已点赞
    /// </summary>
    public bool IsLiked { get; set; }

    /// <summary>
    /// 点赞数
    /// </summary>
    public int LikeCount { get; set; }
}