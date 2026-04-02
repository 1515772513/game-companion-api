namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 我的发布动态项DTO
/// </summary>
public class MyPostItemDto
{
    /// <summary>
    /// 动态ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 动态内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 图片URL列表
    /// </summary>
    public List<string>? Images { get; set; } = new List<string>();

    /// <summary>
    /// 动态状态：0-审核中，1-已发布，2-已拒绝，3-已删除
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 动态状态文本
    /// </summary>
    public string StatusText { get; set; } = string.Empty;

    /// <summary>
    /// 点赞数
    /// </summary>
    public int LikeCount { get; set; }

    /// <summary>
    /// 评论数
    /// </summary>
    public int CommentCount { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    public string CreatedAt { get; set; }
}