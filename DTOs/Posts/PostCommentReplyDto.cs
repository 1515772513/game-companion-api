namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 评论回复DTO
/// </summary>
public class PostCommentReplyDto
{
    /// <summary>
    /// 回复ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 回复用户信息
    /// </summary>
    public PostUserDto User { get; set; } = new PostUserDto();

    /// <summary>
    /// 回复内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 回复时间
    /// </summary>
    public string CreatedAt { get; set; } = string.Empty;
}