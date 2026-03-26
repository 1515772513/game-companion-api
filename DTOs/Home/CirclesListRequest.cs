using System.Text.Json.Serialization;

namespace GameCompanion.Api.DTOs.Home;

/// <summary>
/// 获取游戏圈子列表请求参数
/// </summary>
public class CirclesListRequest
{
    /// <summary>
    /// 游戏ID，不传则查询所有圈子
    /// </summary>
    [JsonPropertyName("game_id")]
    public int? GameId { get; set; }

    /// <summary>
    /// 页码
    /// </summary>
    [JsonPropertyName("page")]
    public int Page { get; set; } = 1;

    /// <summary>
    /// 每页数量
    /// </summary>
    [JsonPropertyName("page_size")]
    public int PageSize { get; set; } = 20;
}

/// <summary>
/// 游戏圈子列表响应
/// </summary>
public class CirclesListResponse
{
    /// <summary>
    /// 圈子列表
    /// </summary>
    [JsonPropertyName("items")]
    public List<CircleDto> Items { get; set; } = new();

    /// <summary>
    /// 分页信息
    /// </summary>
    [JsonPropertyName("pagination")]
    public PaginationDto Pagination { get; set; } = new();
}

/// <summary>
/// 游戏圈子DTO
/// </summary>
public class CircleDto
{
    /// <summary>
    /// 圈子ID
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// 圈子名称
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    /// <summary>
    /// 游戏ID
    /// </summary>
    [JsonPropertyName("game_id")]
    public int GameId { get; set; }

    /// <summary>
    /// 游戏名称
    /// </summary>
    [JsonPropertyName("game_name")]
    public string GameName { get; set; } = "";

    /// <summary>
    /// 圈子头像URL
    /// </summary>
    [JsonPropertyName("avatar_url")]
    public string AvatarUrl { get; set; } = "";

    /// <summary>
    /// 成员总数
    /// </summary>
    [JsonPropertyName("member_count")]
    public int MemberCount { get; set; }

    /// <summary>
    /// 动态总数
    /// </summary>
    [JsonPropertyName("post_count")]
    public int PostCount { get; set; }

    /// <summary>
    /// 当前在线人数
    /// </summary>
    [JsonPropertyName("online_count")]
    public int OnlineCount { get; set; }

    /// <summary>
    /// 圈子描述
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = "";

    /// <summary>
    /// 是否官方圈子
    /// </summary>
    [JsonPropertyName("is_official")]
    public bool IsOfficial { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; } = "";
}