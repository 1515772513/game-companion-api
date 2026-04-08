using System.Text.Json.Serialization;

namespace GameCompanion.Api.DTOs.Home;

/// <summary>
/// 陪玩师详情响应
/// </summary>
public class CompanionDetailResponse
{
    /// <summary>
    /// 陪玩师ID
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    [JsonPropertyName("user_id")]
    public int UserId { get; set; }

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
    /// 等级名称
    /// </summary>
    [JsonPropertyName("level")]
    public int? Level { get; set; } = null;

    /// <summary>
    /// 等级代码
    /// </summary>
    [JsonPropertyName("level_code")]
    public int? LevelCode { get; set; } = null;

    /// <summary>
    /// 服务类型
    /// </summary>
    [JsonPropertyName("service_type")]
    public string ServiceType { get; set; } = "";

    /// <summary>
    /// 服务类型代码
    /// </summary>
    [JsonPropertyName("service_type_code")]
    public string ServiceTypeCode { get; set; } = "";

    /// <summary>
    /// 价格
    /// </summary>
    [JsonPropertyName("price")]
    public decimal Price { get; set; }

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
    /// 评价数量
    /// </summary>
    [JsonPropertyName("rating_count")]
    public int RatingCount { get; set; }

    /// <summary>
    /// 接单数量
    /// </summary>
    [JsonPropertyName("order_count")]
    public int OrderCount { get; set; }

    /// <summary>
    /// 好评率（百分比）
    /// </summary>
    [JsonPropertyName("positive_rate")]
    public decimal PositiveRate { get; set; }

    /// <summary>
    /// 在线状态：0-离线，1-在线
    /// </summary>
    [JsonPropertyName("online_status")]
    public int OnlineStatus { get; set; }

    /// <summary>
    /// 在线状态文本描述
    /// </summary>
    [JsonPropertyName("online_status_text")]
    public string OnlineStatusText { get; set; } = "";

    /// <summary>
    /// 是否已实名认证
    /// </summary>
    [JsonPropertyName("is_verified")]
    public bool IsVerified { get; set; }

    /// <summary>
    /// 认证时间
    /// </summary>
    [JsonPropertyName("verified_at")]
    public string VerifiedAt { get; set; } = "";

    /// <summary>
    /// 擅长的游戏列表
    /// </summary>
    [JsonPropertyName("games")]
    public List<GameSkillDto> Games { get; set; } = new();

    /// <summary>
    /// 服务时间安排
    /// </summary>
    [JsonPropertyName("service_times")]
    public List<ServiceTimeDto> ServiceTimes { get; set; } = new();

    /// <summary>
    /// 标签列表
    /// </summary>
    [JsonPropertyName("tags")]
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// 个人简介
    /// </summary>
    [JsonPropertyName("bio")]
    public string Bio { get; set; } = "";

    /// <summary>
    /// 优势/特长列表
    /// </summary>
    [JsonPropertyName("strengths")]
    public List<string> Strengths { get; set; } = new();

    /// <summary>
    /// 最近评价列表
    /// </summary>
    [JsonPropertyName("recent_reviews")]
    public List<CompanionReviewDto> RecentReviews { get; set; } = new();

    /// <summary>
    /// 统计数据
    /// </summary>
    [JsonPropertyName("statistics")]
    public CompanionStatisticsDto Statistics { get; set; } = new();
}

/// <summary>
/// 游戏技能DTO
/// </summary>
public class GameSkillDto
{
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
    /// 游戏段位/等级
    /// </summary>
    [JsonPropertyName("game_rank")]
    public string GameRank { get; set; } = "";
}

/// <summary>
/// 服务时间DTO
/// </summary>
public class ServiceTimeDto
{
    /// <summary>
    /// 星期几
    /// </summary>
    [JsonPropertyName("day")]
    public string Day { get; set; } = "";

    /// <summary>
    /// 开始时间
    /// </summary>
    [JsonPropertyName("start_time")]
    public string StartTime { get; set; } = "";

    /// <summary>
    /// 结束时间
    /// </summary>
    [JsonPropertyName("end_time")]
    public string EndTime { get; set; } = "";
}

/// <summary>
/// 陪玩师评价DTO
/// </summary>
public class CompanionReviewDto
{
    /// <summary>
    /// 评价ID
    /// </summary>
    [JsonPropertyName("id")]
    public int Id { get; set; }

    /// <summary>
    /// 订单ID
    /// </summary>
    [JsonPropertyName("order_id")]
    public int OrderId { get; set; }

    /// <summary>
    /// 评价用户昵称
    /// </summary>
    [JsonPropertyName("user_name")]
    public string UserName { get; set; } = "";

    /// <summary>
    /// 评价用户头像
    /// </summary>
    [JsonPropertyName("user_avatar")]
    public string UserAvatar { get; set; } = "";

    /// <summary>
    /// 评分（1-5）
    /// </summary>
    [JsonPropertyName("rating")]
    public int Rating { get; set; }

    /// <summary>
    /// 评价内容
    /// </summary>
    [JsonPropertyName("comment")]
    public string Comment { get; set; } = "";

    /// <summary>
    /// 服务日期
    /// </summary>
    [JsonPropertyName("service_date")]
    public string ServiceDate { get; set; } = "";

    /// <summary>
    /// 评价时间
    /// </summary>
    [JsonPropertyName("created_at")]
    public string CreatedAt { get; set; } = "";
}

/// <summary>
/// 陪玩师统计数据DTO
/// </summary>
public class CompanionStatisticsDto
{
    /// <summary>
    /// 总接单数
    /// </summary>
    [JsonPropertyName("total_orders")]
    public int TotalOrders { get; set; }

    /// <summary>
    /// 总服务时长（小时）
    /// </summary>
    [JsonPropertyName("total_hours")]
    public int TotalHours { get; set; }

    /// <summary>
    /// 平均响应时间（分钟）
    /// </summary>
    [JsonPropertyName("avg_response_time")]
    public int AvgResponseTime { get; set; }

    /// <summary>
    /// 订单完成率（百分比）
    /// </summary>
    [JsonPropertyName("completion_rate")]
    public decimal CompletionRate { get; set; }

    /// <summary>
    /// 准时率（百分比）
    /// </summary>
    [JsonPropertyName("on_time_rate")]
    public decimal OnTimeRate { get; set; }
}