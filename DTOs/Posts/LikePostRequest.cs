namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 点赞动态请求DTO
/// </summary>
public class LikePostRequest
{
    /// <summary>
    /// 操作类型：like-点赞，unlike-取消点赞
    /// </summary>
    public string Action { get; set; } = string.Empty;
}