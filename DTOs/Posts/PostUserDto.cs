namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 动态发布者信息DTO
/// </summary>
public class PostUserDto
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string Nickname { get; set; } = string.Empty;

    /// <summary>
    /// 头像URL
    /// </summary>
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// 是否实名认证
    /// </summary>
    public bool IsVerified { get; set; }

    /// <summary>
    /// 等级（陪玩师）
    /// </summary>
    public string? Level { get; set; }

    /// <summary>
    /// 简介
    /// </summary>
    public string? Bio { get; set; }

    /// <summary>
    /// 粉丝数
    /// </summary>
    public int FollowersCount { get; set; }

    /// <summary>
    /// 关注数
    /// </summary>
    public int FollowingCount { get; set; }
}