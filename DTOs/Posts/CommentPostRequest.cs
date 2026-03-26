using System.ComponentModel.DataAnnotations;

namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 评论动态请求DTO
/// </summary>
public class CommentPostRequest
{
    /// <summary>
    /// 评论内容
    /// </summary>
    [Required(ErrorMessage = "评论内容不能为空")]
    [StringLength(500, MinimumLength = 1, ErrorMessage = "评论内容长度必须在1-500个字符之间")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 父评论ID（回复评论时必填）
    /// </summary>
    public int? ParentId { get; set; }

    /// <summary>
    /// 回复的用户ID（回复评论时必填）
    /// </summary>
    public int? ReplyToUserId { get; set; }
}