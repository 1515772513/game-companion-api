using System.Text.Json.Serialization;

namespace GameCompanion.Api.DTOs.Home;

/// <summary>
/// 游戏列表响应
/// </summary>
public class GameListResponse
{
    /// <summary>
    /// 游戏列表
    /// </summary>
    [JsonPropertyName("items")]
    public List<GameDetailDto> Items { get; set; } = new();
}

/// <summary>
/// 游戏详细信息DTO
/// </summary>
public class GameDetailDto
{
    /// <summary>
    /// 游戏ID
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// 游戏名称
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = "";

    /// <summary>
    /// 游戏图标URL
    /// </summary>
    [JsonPropertyName("icon_url")]
    public string IconUrl { get; set; } = "";

    /// <summary>
    /// 该游戏的陪玩师总数
    /// </summary>
    [JsonPropertyName("companion_count")]
    public int CompanionCount { get; set; }

    /// <summary>
    /// 该游戏当前在线的陪玩师数
    /// </summary>
    [JsonPropertyName("online_companion_count")]
    public int OnlineCompanionCount { get; set; }

    /// <summary>
    /// 游戏描述
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = "";

    /// <summary>
    /// 是否热门游戏
    /// </summary>
    [JsonPropertyName("is_hot")]
    public bool IsHot { get; set; }
}