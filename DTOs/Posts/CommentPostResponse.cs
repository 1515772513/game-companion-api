namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 评论动态响应DTO
/// </summary>
public class CommentPostResponse
{
    /// <summary>
    /// 评论ID
    /// </summary>
    public int CommentId { get; set; }

    /// <summary>
    /// 评论内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 评论用户信息
    /// </summary>
    public PostUserDto User { get; set; } = new PostUserDto();

    /// <summary>
    /// 点赞数
    /// </summary>
    public int LikeCount { get; set; }

    /// <summary>
    /// 评论时间
    /// </summary>
    public string CreatedAt { get; set; } = string.Empty;
}