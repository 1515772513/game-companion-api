namespace GameCompanion.Api.DTOs;

/// <summary>
/// 个人信息响应
/// </summary>
public class UserProfileResponse
{
    public long Id { get; set; }
    public string? Username { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string Avatar_url { get; set; } = string.Empty;
    public int Gender { get; set; }
    public string Gender_text { get; set; } = string.Empty;
    public string? Birthday { get; set; }
    public int? Age { get; set; }
    public string? Location { get; set; }
    public string? Bio { get; set; }
    public string Phone { get; set; } = string.Empty;
    public bool Phone_verified { get; set; }
    public string? Real_name { get; set; }
    public bool Id_card_verified { get; set; }
    public int Vip_level { get; set; }
    public string Vip_level_text { get; set; } = string.Empty;
    public string? Vip_expire_time { get; set; }
    public int? Vip_days_left { get; set; }
    public decimal Balance { get; set; }
    public decimal Frozen_amount { get; set; }
    public int Points { get; set; }
    public bool Is_companion { get; set; }
    public int? Companion_status { get; set; }
    public UserStatistics Statistics { get; set; } = new();
    public string Created_at { get; set; } = string.Empty;
    public string? Last_login_at { get; set; }
}

/// <summary>
/// 用户统计数据
/// </summary>
public class UserStatistics
{
    public int Followers_count { get; set; }
    public int Following_count { get; set; }
    public int Posts_count { get; set; }
    public int Likes_count { get; set; }
    public int Orders_count { get; set; }
    public int Collections_count { get; set; }
}

/// <summary>
/// 更新个人资料请求
/// </summary>
public class UpdateProfileRequest
{
    public string? Nickname { get; set; }
    public string? Avatar_url { get; set; }
    public int? Gender { get; set; }
    public string? Birthday { get; set; }
    public string? Location { get; set; }
    public string? Bio { get; set; }
}

/// <summary>
/// 实名认证请求
/// </summary>
public class VerifyRealNameRequest
{
    public string Real_name { get; set; } = string.Empty;
    public string Id_card { get; set; } = string.Empty;
    public string Id_card_front_url { get; set; } = string.Empty;
    public string Id_card_back_url { get; set; } = string.Empty;
}

/// <summary>
/// 关注/取消关注请求
/// </summary>
public class FollowRequest
{
    public int Target_user_id { get; set; }
    public string Action { get; set; } = string.Empty; // follow, unfollow
}

/// <summary>
/// 申请成为陪玩师请求
/// </summary>
public class ApplyCompanionRequest
{
    public string Real_name { get; set; } = string.Empty;
    public string Id_card { get; set; } = string.Empty;
    public string Id_card_front_url { get; set; } = string.Empty;
    public string Id_card_back_url { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string Avatar_url { get; set; } = string.Empty;
    public string Service_type { get; set; } = string.Empty; // tech, entertainment
    public decimal Price { get; set; }
    public List<int> Games { get; set; } = new();
    public string Game_rank { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public List<string>? Tags { get; set; }
}

/// <summary>
/// 钱包信息响应
/// </summary>
public class WalletResponse
{
    public decimal Balance { get; set; }
    public decimal Frozen_amount { get; set; }
    public decimal Available_amount { get; set; }
    public decimal Total_income { get; set; }
    public decimal Total_expense { get; set; }
    public decimal Pending_amount { get; set; }
    public WalletRecords Records { get; set; } = new();
}

/// <summary>
/// 钱包流水记录
/// </summary>
public class WalletRecords
{
    public List<WalletRecordItem> Items { get; set; } = new();
    public PaginationInfo Pagination { get; set; } = new();
}

/// <summary>
/// 钱包流水项
/// </summary>
public class WalletRecordItem
{
    public long Id { get; set; }
    public int Type { get; set; } // 1-充值，2-消费，3-退款，4-提现，5-收入
    public string Type_text { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal Balance_after { get; set; }
    public string Description { get; set; } = string.Empty;
    public string? Order_no { get; set; }
    public string Created_at { get; set; } = string.Empty;
}
