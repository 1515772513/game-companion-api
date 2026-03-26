namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 动态关联话题信息DTO
/// </summary>
public class PostTopicDto
{
    /// <summary>
    /// 话题ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 话题名称
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 参与人数
    /// </summary>
    public int ParticipantCount { get; set; }
}