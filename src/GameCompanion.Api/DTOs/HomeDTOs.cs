namespace GameCompanion.Api.DTOs;

/// <summary>
/// 首页数据响应
/// </summary>
public class HomeResponse
{
    public List<BannerItem> Banners { get; set; } = new();
    public List<CompanionSimpleInfo> Hot_companions { get; set; } = new();
    public List<GameSimpleInfo> Hot_games { get; set; } = new();
    public List<PostSimpleInfo> Hot_posts { get; set; } = new();
}

/// <summary>
/// 轮播图项
/// </summary>
public class BannerItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Image_url { get; set; } = string.Empty;
    public string Jump_url { get; set; } = string.Empty;
    public string Jump_type { get; set; } = string.Empty; // url, internal
}

/// <summary>
/// 陪玩师简单信息
/// </summary>
public class CompanionSimpleInfo
{
    public int Id { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string Avatar_url { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty; // 银牌、金牌、钻石、王者
    public string Service_type { get; set; } = string.Empty; // 技术陪玩、娱乐陪玩
    public decimal Price { get; set; }
    public string Price_unit { get; set; } = string.Empty; // 局、小时
    public decimal Rating { get; set; }
    public int Online_status { get; set; }
    public string Online_status_text { get; set; } = string.Empty;
    public List<string> Tags { get; set; } = new();
}

/// <summary>
/// 游戏简单信息
/// </summary>
public class GameSimpleInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon_url { get; set; } = string.Empty;
    public int Companion_count { get; set; }
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// 动态简单信息
/// </summary>
public class PostSimpleInfo
{
    public long Id { get; set; }
    public int User_id { get; set; }
    public string User_name { get; set; } = string.Empty;
    public string User_avatar { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<string> Images { get; set; } = new();
    public int Like_count { get; set; }
    public int Comment_count { get; set; }
    public string Created_at { get; set; } = string.Empty; // 友好时间格式
}

/// <summary>
/// 陪玩师详细信息
/// </summary>
public class CompanionDetailInfo
{
    public int Id { get; set; }
    public int User_id { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string Avatar_url { get; set; } = string.Empty;
    public string Level { get; set; } = string.Empty;
    public string Level_code { get; set; } = string.Empty; // silver, gold, diamond, king
    public string Service_type { get; set; } = string.Empty;
    public string Service_type_code { get; set; } = string.Empty; // tech, entertainment
    public decimal Price { get; set; }
    public string Price_unit { get; set; } = string.Empty;
    public decimal Rating { get; set; }
    public int Rating_count { get; set; }
    public int Order_count { get; set; }
    public decimal Positive_rate { get; set; }
    public int Online_status { get; set; }
    public string Online_status_text { get; set; } = string.Empty;
    public bool Is_verified { get; set; }
    public string? Verified_at { get; set; }
    public List<GameSkillInfo> Games { get; set; } = new();
    public List<ServiceTimeInfo> Service_times { get; set; } = new();
    public List<string> Tags { get; set; } = new();
    public string Bio { get; set; } = string.Empty;
    public List<string> Strengths { get; set; } = new();
    public string Created_at { get; set; } = string.Empty;
    public List<ReviewInfo> Recent_reviews { get; set; } = new();
    public CompanionStatistics Statistics { get; set; } = new();
}

/// <summary>
/// 游戏技能信息
/// </summary>
public class GameSkillInfo
{
    public int Game_id { get; set; }
    public string Game_name { get; set; } = string.Empty;
    public string Game_rank { get; set; } = string.Empty;
}

/// <summary>
/// 服务时间信息
/// </summary>
public class ServiceTimeInfo
{
    public string Day { get; set; } = string.Empty;
    public string Start_time { get; set; } = string.Empty;
    public string End_time { get; set; } = string.Empty;
}

/// <summary>
/// 评价信息
/// </summary>
public class ReviewInfo
{
    public long Id { get; set; }
    public long Order_id { get; set; }
    public string User_name { get; set; } = string.Empty;
    public string User_avatar { get; set; } = string.Empty;
    public int Rating { get; set; }
    public string Comment { get; set; } = string.Empty;
    public string Service_date { get; set; } = string.Empty;
    public string Created_at { get; set; } = string.Empty;
}

/// <summary>
/// 陪玩师统计数据
/// </summary>
public class CompanionStatistics
{
    public int Total_orders { get; set; }
    public int Total_hours { get; set; }
    public int Avg_response_time { get; set; }
    public decimal Completion_rate { get; set; }
    public decimal On_time_rate { get; set; }
}

/// <summary>
/// 游戏详细信息
/// </summary>
public class GameDetailInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Icon_url { get; set; } = string.Empty;
    public int Companion_count { get; set; }
    public int Online_companion_count { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool Is_hot { get; set; }
}
