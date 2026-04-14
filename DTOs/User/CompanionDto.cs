using System.ComponentModel.DataAnnotations;

namespace GameCompanion.Api.DTOs.Companion;


/// <summary>
/// 陪玩师擅长游戏项
/// </summary>
public class CompanionGameItemDto
{
    /// <summary>游戏ID</summary>
    public int GameId { get; set; }
    /// <summary>游戏名称</summary>
    public string GameName { get; set; } = string.Empty;
    /// <summary>游戏段位/等级</summary>
    public string GameLevel { get; set; } = string.Empty;
    public string GameIcon { get; set; } = string.Empty;
    /// <summary>服务类型</summary>
    public string ServiceType { get; set; } = string.Empty;
    /// <summary>服务类型名称</summary>
    public string ServiceTypeName { get; set; } = string.Empty;
    /// <summary>单价(元/局)</summary>
    public decimal? PricePerGame { get; set; }
    /// <summary>单价(元/小时)</summary>
    public decimal? PricePerHour { get; set; }
}

/// <summary>
/// 陪玩师列表DTO
/// </summary>
public class CompanionListDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string? RealName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int? Level { get; set; } = null; // 银牌/金牌/钻石/王者
    public string LevelName { get; set; } = string.Empty;
    public string ServiceType { get; set; } = string.Empty; // 技术陪玩/娱乐陪玩
    public string ServiceTypeName { get; set; } = string.Empty; // 技术陪玩/娱乐陪玩
    public decimal PricePerGame { get; set; }
    public decimal? PricePerHour { get; set; }
    public decimal? Rating { get; set; }
    public int? TotalOrders { get; set; }
    public decimal? GoodReviewRate { get; set; }
    public string? Tags { get; set; }
    public int Status { get; set; } = 0; // 0=待审核,1=审核通过,2=审核拒绝
    public string StatusName { get; set; } = string.Empty; // 待审核/审核通过/审核拒绝
    public string OnlineStatus { get; set; } = string.Empty;
    public string CreatedAt { get; set; } = string.Empty;

    /// <summary>
    /// 主游戏ID
    /// </summary>
    public int MainGameId { get; set; }
    
    public List<CompanionGameItemDto> Games { get; set; } = new();
}

/// <summary>
/// 陪玩师详情DTO
/// </summary>
public class CompanionDetailDto
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string? RealName { get; set; }
    public string? IdCard { get; set; }
    public string? IdCardFront { get; set; }
    public string? IdCardBack { get; set; }
    public string Phone { get; set; } = string.Empty;

    public string? Level { get; set; }
    public string? ServiceType { get; set; }
    public decimal PricePerGame { get; set; }
    public decimal? PricePerHour { get; set; }

    public decimal? Rating { get; set; }
    public int? TotalOrders { get; set; }
    public decimal? GoodReviewRate { get; set; }

    public string? Bio { get; set; }
    public string? Tags { get; set; }

    public string Status { get; set; } = string.Empty;
    public string? RejectReason { get; set; }
    public string OnlineStatus { get; set; } = string.Empty;

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// 陪玩师申请DTO
/// </summary>
public class ApplyCompanionDto
{
    /// <summary>
    /// 昵称
    /// </summary>
    public string Nickname { get; set; } = string.Empty;

    /// <summary>
    /// 真实姓名
    /// </summary>
    public string RealName { get; set; } = string.Empty;

    /// <summary>
    /// 身份证号
    /// </summary>
    public string IdCard { get; set; } = string.Empty;

    /// <summary>
    /// 身份证正面
    /// </summary>
    public string? IdCardFront { get; set; }

    /// <summary>
    /// 身份证反面
    /// </summary>
    public string? IdCardBack { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    public string Phone { get; set; } = string.Empty;

    /// <summary>
    /// 等级：银牌/金牌/钻石/王者
    /// </summary>
    public string Level { get; set; } = "银牌";

    /// <summary>
    /// 服务类型：技术陪玩/娱乐陪玩
    /// </summary>
    public string ServiceType { get; set; } = "技术陪玩";

    /// <summary>
    /// 单价/局
    /// </summary>
    public decimal PricePerGame { get; set; }

    /// <summary>
    /// 单价/小时
    /// </summary>
    public decimal? PricePerHour { get; set; }

    /// <summary>
    /// 个人简介
    /// </summary>
    public string? Bio { get; set; }

    /// <summary>
    /// 标签，逗号分隔
    /// </summary>
    public string? Tags { get; set; }
}

/// <summary>
/// 陪玩师列表请求DTO
/// </summary>/// <summary>
/// 陪玩认证审核列表请求参数
/// </summary>
public class CompanionListRequest
{
    /// <summary>
    /// 审核状态：待审核/已通过/已拒绝（不传则查全部）
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// 搜索关键词：申请人姓名/昵称
    /// </summary>
    public string? Keyword { get; set; }

    /// <summary>
    /// 擅长游戏ID（或游戏类型，前端下拉选择）
    /// </summary>
    public int? GameId { get; set; }

    /// <summary>
    /// 服务类型：技术陪玩/娱乐陪玩
    /// </summary>
    public string? ServiceType { get; set; }

    /// <summary>
    /// 申请时间范围 - 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 申请时间范围 - 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 页码（默认1）
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// 每页条数（默认10）
    /// </summary>
    public int PageSize { get; set; } = 10;
}

/// <summary>
/// 陪玩师列表响应DTO
/// </summary>
public class CompanionListResponse
{
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<CompanionListDto> list { get; set; } = new List<CompanionListDto>();
}

/// <summary>
/// 陪玩状态统计DTO
/// </summary>
public class CompanionStatusCountDto
{
    /// <summary>
    /// 状态：0=待审核,1=审核通过,2=审核拒绝
    /// </summary>
    public int Status { get; set; }

    /// <summary>
    /// 数量
    /// </summary>
    public int Count { get; set; }
}