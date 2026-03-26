using System.ComponentModel.DataAnnotations;

namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 发布动态请求DTO
/// </summary>
public class CreatePostRequest
{
    /// <summary>
    /// 动态内容
    /// </summary>
    [Required(ErrorMessage = "动态内容不能为空")]
    [StringLength(2000, MinimumLength = 10, ErrorMessage = "动态内容长度必须在10-2000个字符之间")]
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 图片URL数组
    /// </summary>
    [MaxLength(9, ErrorMessage = "最多支持9张图片")]
    public List<string>? Images { get; set; } = new List<string>();

    /// <summary>
    /// 关联游戏ID
    /// </summary>
    public int? GameId { get; set; }

    /// <summary>
    /// 话题ID
    /// </summary>
    public int? TopicId { get; set; }

    /// <summary>
    /// 可见性：0-公开，1-仅粉丝可见，2-私密
    /// </summary>
    public int? Visibility { get; set; } = 0;

    /// <summary>
    /// 位置信息
    /// </summary>
    [StringLength(255, ErrorMessage = "位置信息长度不能超过255个字符")]
    public string? Location { get; set; }

    /// <summary>
    /// @提醒的用户ID数组
    /// </summary>
    [MaxLength(10, ErrorMessage = "最多可以@10个用户")]
    public List<int>? MentionUsers { get; set; } = new List<int>();
}