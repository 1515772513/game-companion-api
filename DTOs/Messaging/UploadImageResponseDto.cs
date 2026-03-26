namespace GameCompanion.Api.DTOs.Messaging;

/// <summary>
/// 上传图片响应DTO
/// </summary>
public class UploadImageResponseDto
{
    /// <summary>
    /// 图片URL
    /// </summary>
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// 图片宽度
    /// </summary>
    public int Width { get; set; }

    /// <summary>
    /// 图片高度
    /// </summary>
    public int Height { get; set; }

    /// <summary>
    /// 图片大小（字节）
    /// </summary>
    public int Size { get; set; }

    /// <summary>
    /// 图片类型
    /// </summary>
    public string MimeType { get; set; } = string.Empty;
}