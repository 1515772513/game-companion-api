using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models;

/// <summary>
/// 动态表
/// </summary>
[Table("posts")]
public class Post : BaseEntity
{
    /// <summary>
    /// 发布用户ID
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 关联用户
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    /// <summary>
    /// 游戏ID
    /// </summary>
    public int? GameId { get; set; }

    /// <summary>
    /// 关联游戏
    /// </summary>
    [ForeignKey(nameof(GameId))]
    public Game? Game { get; set; }

    /// <summary>
    /// 动态内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 图片数组 (JSON)
    /// </summary>
    public string? Images { get; set; }

    /// <summary>
    /// 话题ID
    /// </summary>
    public int? TopicId { get; set; }

    /// <summary>
    /// 可见性: 0公开, 1仅粉丝, 2私密
    /// </summary>
    public int? Visibility { get; set; }

    /// <summary>
    /// 位置
    /// </summary>
    [MaxLength(100)]
    public string? Location { get; set; }

    /// <summary>
    /// 提醒的用户 (JSON)
    /// </summary>
    public string? MentionedUsers { get; set; }

    /// <summary>
    /// 点赞数
    /// </summary>
    public int LikeCount { get; set; } = 0;

    /// <summary>
    /// 评论数
    /// </summary>
    public int CommentCount { get; set; } = 0;

    /// <summary>
    /// 收藏数
    /// </summary>
    public int CollectCount { get; set; } = 0;

    /// <summary>
    /// 分享数
    /// </summary>
    public int ShareCount { get; set; } = 0;

    /// <summary>
    /// 状态: 0草稿, 1已发布
    /// </summary>
    public int Status { get; set; } = 0;

    /// <summary>
    /// 审核状态: 0待审核, 1已通过, 2已拒绝, 3已隐藏
    /// </summary>
    public int AuditStatus { get; set; } = 0;

    /// <summary>
    /// 审核时间
    /// </summary>
    public DateTime? AuditTime { get; set; }

    /// <summary>
    /// 审核管理员ID
    /// </summary>
    public int? AuditAdminId { get; set; }

    /// <summary>
    /// 审核拒绝原因
    /// </summary>
    [MaxLength(200)]
    public string? AuditReason { get; set; }
}
