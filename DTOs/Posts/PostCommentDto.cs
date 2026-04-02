namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 评论DTO
/// </summary>
public class PostCommentDto
{
    /// <summary>
    /// 评论ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 评论用户信息
    /// </summary>
    public PostUserDto User { get; set; } = new PostUserDto();

    /// <summary>
    /// 评论内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 点赞数
    /// </summary>
    public int LikeCount { get; set; }

    /// <summary>
    /// 当前用户是否已点赞
    /// </summary>
    public bool IsLiked { get; set; }

    /// <summary>
    /// 评论时间
    /// </summary>
    public string CreatedAt { get; set; } = string.Empty;

    /// <summary>
    /// 回复列表
    /// </summary>
    public List<PostCommentReplyDto>? Replies { get; set; } = new List<PostCommentReplyDto>();
}