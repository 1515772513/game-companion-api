namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 动态关联游戏信息DTO
/// </summary>
public class PostGameDto
{
    /// <summary>
    /// 游戏ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 游戏名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 游戏图标URL
    /// </summary>
    public string? IconUrl { get; set; }
}