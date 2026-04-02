namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 保存草稿响应DTO
/// </summary>
public class CreateDraftResponse
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
    /// 保存时间
    /// </summary>
    public string SavedAt { get; set; }
}