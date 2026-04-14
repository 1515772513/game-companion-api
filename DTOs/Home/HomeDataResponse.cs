using System.Text.Json.Serialization;

namespace GameCompanion.Api.DTOs.Home;

/// <summary>
/// 首页数据响应DTO
/// </summary>
public class HomeDataResponse
{
    /// <summary>
    /// 轮播图列表
    /// </summary>
    [JsonPropertyName("banners")]
    public object? Banners { get; set; } = null;

    /// <summary>
    /// 热门陪玩师列表
    /// </summary>
    [JsonPropertyName("hot_companions")]
    public List<CompanionSummaryDto> HotCompanions { get; set; } = new();

    /// <summary>
    /// 热门游戏列表
    /// </summary>
    [JsonPropertyName("hot_games")]
    public List<GameDto> HotGames { get; set; } = new();

    /// <summary>
    /// 热门动态列表
    /// </summary>
    [JsonPropertyName("hot_posts")]
    public List<PostDto> HotPosts { get; set; } = new();
}

public class BannerDto
{
    /// <summary>
    /// 编号
    /// </summary>
    [JsonPropertyName("Id")] // 👇 对应 JSON 的 id
    public int Id { get; set; }

    /// <summary>
    /// 图片地址
    /// </summary>
    [JsonPropertyName("ImageUrl")] // 👇 严格对应 JSON 的 ImageUrl
    public string ImageUrl { get; set; } = string.Empty;

    /// <summary>
    /// 跳转地址
    /// </summary>
    [JsonPropertyName("LinkUrl")] // 👇 严格对应 JSON 的 LinkUrl
    public string LinkUrl { get; set; } = string.Empty;

    /// <summary>
    /// 标题
    /// </summary>
    [JsonPropertyName("Title")] // 👇 严格对应 JSON 的 Title
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// 排序
    /// </summary>
    [JsonPropertyName("sort")] // 👇 严格对应 JSON 的 Sort
    public int Sort { get; set; }
}

/// <summary>
/// 陪玩师摘要DTO
/// </summary>
public class CompanionSummaryDto
{
    /// <summary>
    /// 陪玩师ID
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [JsonPropertyName("nickname")]
    public string Nickname { get; set; } = "";

    /// <summary>
    /// 头像URL
    /// </summary>
    [JsonPropertyName("avatar_url")]
    public string AvatarUrl { get; set; } = "";

    /// <summary>
    /// 等级
    /// </summary>
    [JsonPropertyName("level")]
    public int? Level { get; set; } = null;

    /// <summary>
    /// 服务类型
    /// </summary>
    [JsonPropertyName("service_type")]
    public string ServiceType { get; set; } = "";

    /// <summary>
    /// 价格
    /// </summary>
    [JsonPropertyName("price")]
    public decimal? Price { get; set; }

    /// <summary>
    /// 价格单位
    /// </summary>
    [JsonPropertyName("price_unit")]
    public string PriceUnit { get; set; } = "";

    /// <summary>
    /// 评分
    /// </summary>
    [JsonPropertyName("rating")]
    public decimal Rating { get; set; }

    /// <summary>
    /// 在线状态
    /// </summary>
    [JsonPropertyName("online_status")]
    public int OnlineStatus { get; set; }

    /// <summary>
    /// 在线状态文本描述
    /// </summary>
    [JsonPropertyName("online_status_text")]
    public string OnlineStatusText { get; set; } = "";

    /// <summary>
    /// 标签列表
    /// </summary>
    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; } = new();
}

/// <summary>
/// 游戏DTO
/// </summary>
public class GameDto
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
    /// 该游戏的陪玩师数量
    /// </summary>
    [JsonPropertyName("companion_count")]
    public int CompanionCount { get; set; }

    /// <summary>
    /// 游戏描述
    /// </summary>
    [JsonPropertyName("description")]
    public string Description { get; set; } = "";
}

/// <summary>
/// 动态DTO
/// </summary>
public class PostDto
{
    /// <summary>
    /// 动态ID
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// 发布者用户ID
    /// </summary>
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// 发布者昵称
    /// </summary>
    [JsonPropertyName("user_name")]
    public string UserName { get; set; } = "";

    /// <summary>
    /// 发布者头像URL
    /// </summary>
    [JsonPropertyName("user_avatar")]
    public string UserAvatar { get; set; } = "";

    /// <summary>
    /// 动态内容
    /// </summary>
    [JsonPropertyName("content")]
    public string Content { get; set; } = "";

    /// <summary>
    /// 动态图片URL列表
    /// </summary>
    [JsonPropertyName("images")]
    public List<string> Images { get; set; } = new();

    /// <summary>
    /// 点赞数
    /// </summary>
    [JsonPropertyName("like_count")]
    public int LikeCount { get; set; }

    /// <summary>
    /// 评论数
    /// </summary>
    [JsonPropertyName("comment_count")]
    public int CommentCount { get; set; }

    /// <summary>
    /// 发布时间（友好格式）
    /// </summary>
    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; } = "";
}