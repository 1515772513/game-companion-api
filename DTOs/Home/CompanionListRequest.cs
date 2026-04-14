namespace GameCompanion.Api.DTOs.Home;

/// <summary>
/// 获取陪玩师列表请求参数
/// </summary>
public class CompanionListRequest
{
    /// <summary>
    /// 页码，从1开始
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// 每页数量，范围1-50
    /// </summary>
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// 游戏ID，不传则查询所有游戏
    /// </summary>
    public int? GameId { get; set; }

    /// <summary>
    /// 服务类型：tech-技术陪玩，entertainment-娱乐陪玩
    /// </summary>
    public string? ServiceType { get; set; }

    /// <summary>
    /// 等级：silver-银牌，gold-金牌，diamond-钻石，king-王者
    /// </summary>
    public int? Level { get; set; } = null;

    /// <summary>
    /// 最低价格（元）
    /// </summary>
    public decimal? MinPrice { get; set; }

    /// <summary>
    /// 最高价格（元）
    /// </summary>
    public decimal? MaxPrice { get; set; }

    /// <summary>
    /// 在线状态：0-全部，1-仅在线，2-仅离线
    /// </summary>
    public int? OnlineStatus { get; set; }

    /// <summary>
    /// 搜索关键词（模糊匹配昵称）
    /// </summary>
    public string? Keyword { get; set; }

    /// <summary>
    /// 排序字段：rating-评分，price-价格，order_count-接单数
    /// </summary>
    public string SortBy { get; set; } = "rating";

    /// <summary>
    /// 排序方向：asc-升序，desc-降序
    /// </summary>
    public string SortOrder { get; set; } = "desc";
}

/// <summary>
/// 陪玩师列表响应
/// </summary>
public class CompanionListResponse
{
    /// <summary>
    /// 陪玩师列表
    /// </summary>
    public List<CompanionDetailDto> Items { get; set; } = new();

    /// <summary>
    /// 分页信息
    /// </summary>
    public PaginationDto Pagination { get; set; } = new();
}

/// <summary>
/// 陪玩师详细信息DTO
/// </summary>
public class CompanionDetailDto
{
    /// <summary>
    /// 陪玩师ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 对应的用户ID
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string Nickname { get; set; } = "";

    /// <summary>
    /// 头像URL
    /// </summary>
    public string AvatarUrl { get; set; } = "";

    /// <summary>
    /// 等级名称（中文）
    /// </summary>
    public int? Level { get; set; } = null;

    /// <summary>
    /// 等级代码
    /// </summary>
    public int? LevelCode { get; set; } = null;

    /// <summary>
    /// 服务类型名称
    /// </summary>
    public string ServiceType { get; set; } = "";

    /// <summary>
    /// 服务类型代码
    /// </summary>
    public string ServiceTypeCode { get; set; } = "";

    /// <summary>
    /// 价格
    /// </summary>
    public decimal? Price { get; set; }

    /// <summary>
    /// 价格单位
    /// </summary>
    public string PriceUnit { get; set; } = "";

    /// <summary>
    /// 评分（0-5）
    /// </summary>
    public decimal Rating { get; set; }

    /// <summary>
    /// 评价数量
    /// </summary>
    public int RatingCount { get; set; }

    /// <summary>
    /// 接单数量
    /// </summary>
    public int OrderCount { get; set; }

    /// <summary>
    /// 好评率（百分比）
    /// </summary>
    public decimal PositiveRate { get; set; }

    /// <summary>
    /// 在线状态：0-离线，1-在线
    /// </summary>
    public int OnlineStatus { get; set; }

    /// <summary>
    /// 在线状态文本描述
    /// </summary>
    public string OnlineStatusText { get; set; } = "";

    /// <summary>
    /// 擅长的游戏列表
    /// </summary>
    public List<string> Games { get; set; } = new();

    /// <summary>
    /// 游戏段位/等级
    /// </summary>
    public string GameRank { get; set; } = "";

    /// <summary>
    /// 标签列表
    /// </summary>
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// 个人简介
    /// </summary>
    public string Bio { get; set; } = "";
}

/// <summary>
/// 分页DTO
/// </summary>
public class PaginationDto
{
    /// <summary>
    /// 当前页码
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// 每页数量
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// 总记录数
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// 总页数
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// 是否有更多数据
    /// </summary>
    public bool HasMore { get; set; }
}