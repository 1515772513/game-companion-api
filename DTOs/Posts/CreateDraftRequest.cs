using System.ComponentModel.DataAnnotations;

namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 保存草稿请求DTO
/// </summary>
public class CreateDraftRequest
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
    /// 草稿ID（编辑时传）
    /// </summary>
    public int? DraftId { get; set; }
}