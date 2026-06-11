namespace GameCompanion.Api.DTOs.Companion;

/// <summary>
/// 陪玩师详情DTO - 管理端
/// </summary>
public class AdminCompanionDetailDto
{
    public int Id { get; set; }
    public string RealName { get; set; } = string.Empty;
    public string IdCard { get; set; } = string.Empty;
    public string IdCardFrontUrl { get; set; } = string.Empty;
    public string IdCardBackUrl { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;

    public List<string>? Tags { get; set; } = new();
    public List<GameSkillItem> GameSkills { get; set; } = new();
    public List<string> BackgroundImages { get; set; } = new();
}

/// <summary>
/// 每个游戏的技能 + 服务类型 + 价格
/// </summary>
public class GameSkillItem
{
    public int GameId { get; set; }
    public string GameName { get; set; } = string.Empty;
    public string GameRank { get; set; } = string.Empty;

    public string GameRankName { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty;

    public string ServiceTypeName { get; set; } = string.Empty;
    public decimal Price { get; set; }
    
    public string PriceUnit { get; set; } = string.Empty;
}

/// <summary>
/// 陪玩师详情DTO
/// </summary>
public class CompanionListDetailDto
{
    /// <summary>
    /// 陪玩师ID
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// 昵称
    /// </summary>
    public string Nickname { get; set; } = string.Empty;
    
    /// <summary>
    /// 头像URL
    /// </summary>
    public string Avatar { get; set; } = string.Empty;
    
    /// <summary>
    /// 是否在线
    /// </summary>
    public string OnlineStatus { get; set; } = string.Empty;
    
    /// <summary>
    /// 是否VIP
    /// </summary>
    public bool IsVip { get; set; }
    
    /// <summary>
    /// 评分
    /// </summary>
    public decimal Rating { get; set; }
    
    /// <summary>
    /// 订单数
    /// </summary>
    public int OrderCount { get; set; }
    
    /// <summary>
    /// 好评率
    /// </summary>
    public decimal GoodRate { get; set; }
    
    /// <summary>
    /// 标签
    /// </summary>
    public List<string> Tags { get; set; } = new();
    
    /// <summary>
    /// 个人简介
    /// </summary>
    public string Intro { get; set; } = string.Empty;
    
    /// <summary>
    /// 相册图片
    /// </summary>
    public List<string> Gallery { get; set; } = new();

    /// <summary>
    /// 背景墙图片
    /// </summary>
    public List<string> BackgroundImages { get; set; } = new();
    
    /// <summary>
    /// 是否收藏
    /// </summary>
    public bool IsFavorite { get; set; }
}

/// <summary>
/// 陪玩师服务DTO
/// </summary>
public class CompanionServiceDto
{
    /// <summary>
    /// 服务ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 游戏ID
    /// </summary>
    public int GameId { get; set; }

    /// <summary>
    /// 游戏名称
    /// </summary>
    public string GameName { get; set; } = string.Empty;

    /// <summary>
    /// 游戏段位/等级
    /// </summary>
    public string GameLevel { get; set; } = string.Empty;

    /// <summary>
    /// 游戏段位/等级名称
    /// </summary>
    public string LevelName { get; set; } = string.Empty;
    
    /// <summary>
    /// 服务名称
    /// </summary>
    public string Name { get; set; } = string.Empty;
    
    /// <summary>
    /// 服务描述
    /// </summary>
    public string Description { get; set; } = string.Empty;
    
    /// <summary>
    /// 价格
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// 服务类型
    /// </summary>
    public string ServiceType { get; set; } = string.Empty;

    /// <summary>
    /// 服务类型名称
    /// </summary>
    public string ServiceTypeName { get; set; } = string.Empty;
    
    /// <summary>
    /// 时长(分钟)
    /// </summary>
    public int Duration { get; set; }

    /// <summary>
    /// 价格单位
    /// </summary>
    public string PriceUnit { get; set; } = string.Empty;
}

/// <summary>
/// 陪玩师评价DTO
/// </summary>
public class CompanionReviewDto
{
    /// <summary>
    /// 评价ID
    /// </summary>
    public int Id { get; set; }
    
    /// <summary>
    /// 用户头像
    /// </summary>
    public string UserAvatar { get; set; } = string.Empty;
    
    /// <summary>
    /// 用户昵称
    /// </summary>
    public string UserName { get; set; } = string.Empty;
    
    /// <summary>
    /// 评分
    /// </summary>
    public int Score { get; set; }
    
    /// <summary>
    /// 评价内容
    /// </summary>
    public string Content { get; set; } = string.Empty;
    
    /// <summary>
    /// 评价图片
    /// </summary>
    public List<string> Images { get; set; } = new();
    
    /// <summary>
    /// 创建时间
    /// </summary>
    public string CreateTime { get; set; } = string.Empty;
}