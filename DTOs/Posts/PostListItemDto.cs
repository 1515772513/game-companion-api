namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 动态列表项DTO
/// </summary>
public class PostListItemDto
{
    /// <summary>
    /// 动态ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 发布者信息
    /// </summary>
    public PostUserDto User { get; set; } = new PostUserDto();

    /// <summary>
    /// 动态内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 图片URL列表
    /// </summary>
    public List<string>? Images { get; set; } = new List<string>();

    /// <summary>
    /// 关联游戏信息
    /// </summary>
    public PostGameDto? Game { get; set; }

    /// <summary>
    /// 关联话题信息
    /// </summary>
    public PostTopicDto? Topic { get; set; }

    /// <summary>
    /// 位置信息
    /// </summary>
    public string? Location { get; set; }

    /// <summary>
    /// 点赞数
    /// </summary>
    public int LikeCount { get; set; }

    /// <summary>
    /// 评论数
    /// </summary>
    public int CommentCount { get; set; }

    /// <summary>
    /// 收藏数
    /// </summary>
    public int CollectCount { get; set; }

    /// <summary>
    /// 分享数
    /// </summary>
    public int ShareCount { get; set; }

    /// <summary>
    /// 当前用户是否已点赞
    /// </summary>
    public bool IsLiked { get; set; }

    /// <summary>
    /// 当前用户是否已收藏
    /// </summary>
    public bool IsCollected { get; set; }

    /// <summary>
    /// 发布时间
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 友好时间显示
    /// </summary>
    public string TimeText { get; set; } = string.Empty;
}