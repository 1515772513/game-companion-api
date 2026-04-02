namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 发布动态响应DTO
/// </summary>
public class CreatePostResponse
{
    /// <summary>
    /// 动态ID
    /// </summary>
    public long PostId { get; set; }

    /// <summary>
    /// 动态状态：0-审核中，1-已发布，2-已拒绝，3-已删除
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 动态状态文本
    /// </summary>
    public string StatusText { get; set; } = string.Empty;

    /// <summary>
    /// 审核状态：0-待审核，1-审核通过，2-审核拒绝
    /// </summary>
    public int AuditStatus { get; set; }

    /// <summary>
    /// 审核状态文本
    /// </summary>
    public string AuditStatusText { get; set; } = string.Empty;

    /// <summary>
    /// 动态内容
    /// </summary>
    public string Content { get; set; } = string.Empty;

    /// <summary>
    /// 图片URL列表
    /// </summary>
    public List<string>? Images { get; set; } = new List<string>();

    /// <summary>
    /// 发布时间
    /// </summary>
    public string CreatedAt { get; set; } = string.Empty;

    /// <summary>
    /// 预计审核完成时间
    /// </summary>
    public string EstimatedAuditTime { get; set; } = string.Empty;
}