namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 草稿项DTO
/// </summary>
public class DraftItemDto
{
    /// <summary>
    /// 草稿ID
    /// </summary>
    public int DraftId { get; set; }

    /// <summary>
    /// 动态内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 图片URL列表
    /// </summary>
    public List<string>? Images { get; set; } = new List<string>();

    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime CreatedAt { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime UpdatedAt { get; set; }
}