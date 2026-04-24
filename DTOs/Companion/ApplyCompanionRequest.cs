using System.ComponentModel.DataAnnotations;

namespace GameCompanion.Api.DTOs.Companion;

/// <summary>
/// 申请成为陪玩师请求DTO
/// </summary>
public class ApplyCompanionRequest
{
    // 基础信息（不变）
    [Required(ErrorMessage = "真实姓名不能为空")]
    [StringLength(50)]
    public string RealName { get; set; } = string.Empty;

    [Required(ErrorMessage = "身份证号不能为空")]
    [StringLength(18, MinimumLength = 18)]
    public string IdCard { get; set; } = string.Empty;

    [Required(ErrorMessage = "身份证正面照URL不能为空")]
    [Url]
    public string IdCardFrontUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "身份证反面照URL不能为空")]
    [Url]
    public string IdCardBackUrl { get; set; } = string.Empty;

    [Required(ErrorMessage = "联系电话不能为空")]
    [Phone]
    [StringLength(11, MinimumLength = 11)]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "昵称不能为空")]
    [StringLength(50)]
    public string Nickname { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "个人简介不能为空")]
    [StringLength(500)]
    public string Bio { get; set; } = string.Empty;

    public List<string>? Tags { get; set; }

    // ====================== 核心改动：多游戏 + 每个游戏独立配置 ======================
    [Required(ErrorMessage = "游戏技能列表不能为空")]
    public List<ApplyGameSkillItem> GameSkills { get; set; } = new();

     // ====================== 【核心】多张背景轮播图 ======================
    public List<ApplyBackgroundImageItem> BackgroundImages { get; set; } = new();
}

/// <summary>
/// 单张背景图信息（支持排序）
/// </summary>
public class ApplyBackgroundImageItem
{
    [Required(ErrorMessage = "文件ID不能为空")]
    public Guid FileId { get; set; }

    /// <summary>
    /// 排序（越小越靠前）
    /// </summary>
    public int Sort { get; set; }
}

/// <summary>
/// 每个游戏的技能 + 服务类型 + 价格
/// </summary>
public class ApplyGameSkillItem
{
    [Required(ErrorMessage = "游戏ID不能为空")]
    public int GameId { get; set; }

    [Required(ErrorMessage = "游戏段位不能为空")]
    [StringLength(50)]
    public string GameRank { get; set; } = string.Empty;

    [Required(ErrorMessage = "服务类型不能为空")]
    [StringLength(20)]
    public string ServiceType { get; set; } = string.Empty;

    [Required(ErrorMessage = "单价不能为空")]
    [Range(0.01, 99999.99)]
    public decimal Price { get; set; }
}

/// <summary>
/// 认证申请状态响应DTO
/// </summary>
public class ApplicationStatusResponse
{
    public int ApplicationId { get; set; }
    public int CertificationStatus { get; set; }
    public string CertificationStatusText { get; set; } = string.Empty;
    public string? CertificationApplyTime { get; set; }
    public string? CertificationTime { get; set; }
    public string? RejectReason { get; set; }
}

/// <summary>
/// 陪玩师信息响应DTO
/// </summary>
public class CompanionInfoResponse
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
    public int? Level { get; set; } = null;
    public string ServiceType { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal Rating { get; set; }
    public int OrderCount { get; set; }
    public int RatingCount { get; set; }
    public decimal PositiveRate { get; set; }
    public int OnlineStatus { get; set; }
    public bool IsVerified { get; set; }
    public List<string> Games { get; set; } = new();
    public string GameRank { get; set; } = string.Empty;
    public string Bio { get; set; } = string.Empty;
    public List<string>? Tags { get; set; }
    public string? CertificationTime { get; set; }
    public int TodayOrders { get; set; }
    public int MonthOrders { get; set; }
}

/// <summary>
/// 更新陪玩师信息请求DTO
/// </summary>
public class UpdateCompanionInfoRequest
{
    [StringLength(50, ErrorMessage = "昵称长度不能超过50个字符")]
    public string? Nickname { get; set; }

    [Url(ErrorMessage = "头像URL格式不正确")]
    public string? AvatarUrl { get; set; }

    [StringLength(20, ErrorMessage = "服务类型长度不能超过20个字符")]
    public string? ServiceType { get; set; }

    [Range(0.01, 99999.99, ErrorMessage = "价格必须在0.01-99999.99之间")]
    public decimal? Price { get; set; }

    [StringLength(500, ErrorMessage = "个人简介长度不能超过500个字符")]
    public string? Bio { get; set; }

    [StringLength(200, ErrorMessage = "标签长度不能超过200个字符")]
    public List<string>? Tags { get; set; }
}

/// <summary>
/// 切换在线状态请求DTO
/// </summary>
public class UpdateOnlineStatusRequest
{
    [Required(ErrorMessage = "在线状态不能为空")]
    [Range(0, 2, ErrorMessage = "在线状态必须是0-2之间的整数")]
    public int OnlineStatus { get; set; }
}

/// <summary>
/// 陪玩师订单信息DTO
/// </summary>
public class CompanionOrderInfo
{
    public int OrderId { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public int Status { get; set; }
    public string StatusText { get; set; } = string.Empty;

    public UserInfo User { get; set; } = null!;

    public string GameName { get; set; } = string.Empty;
    public int ServiceCount { get; set; }
    public string ServiceTime { get; set; } = string.Empty;
    public string? SpecialRequirements { get; set; }
    public decimal TotalAmount { get; set; }
    public string CreatedAt { get; set; } = string.Empty;
    public long Countdown { get; set; }
}

/// <summary>
/// 用户信息DTO
/// </summary>
public class UserInfo
{
    public int Id { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
}

/// <summary>
/// 陪玩师订单列表响应DTO
/// </summary>
public class CompanionOrdersResponse
{
    public List<CompanionOrderInfo> Items { get; set; } = new();
    public PaginationInfo Pagination { get; set; } = null!;
}

/// <summary>
/// 分页信息DTO
/// </summary>
public class PaginationInfo
{
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
}

/// <summary>
/// 收益统计响应DTO
/// </summary>
public class EarningsResponse
{
    public decimal TotalEarnings { get; set; }
    public decimal MonthEarnings { get; set; }
    public decimal TodayEarnings { get; set; }
    public decimal PendingAmount { get; set; }
    public decimal WithdrawnAmount { get; set; }
    public int OrdersCount { get; set; }
    public int MonthOrders { get; set; }
    public int TodayOrders { get; set; }
    public decimal Rating { get; set; }
    public decimal PositiveRate { get; set; }
}

/// <summary>
// 申请提现请求DTO
/// </summary>
public class WithdrawRequest
{
    [Required(ErrorMessage = "提现金额不能为空")]
    [Range(100, 99999.99, ErrorMessage = "提现金额必须在100-99999.99之间")]
    public decimal Amount { get; set; }

    [Required(ErrorMessage = "提现方式不能为空")]
    public string WithdrawMethod { get; set; } = string.Empty;

    [Required(ErrorMessage = "收款账号不能为空")]
    public string Account { get; set; } = string.Empty;
}

/// <summary>
/// 陪玩师审核请求DTO
/// </summary>
public class CompanionAuditDto
{
    /// <summary>
    /// 陪玩师ID
    /// </summary>
    [Required(ErrorMessage = "陪玩师ID不能为空")]
    public int CompanionId { get; set; }

    /// <summary>
    /// 审核状态（1=审核通过,2=审核拒绝）
    /// </summary>
    [Required(ErrorMessage = "审核状态不能为空")]
    [Range(1, 2, ErrorMessage = "审核状态只能是1(通过)或2(拒绝)")]
    public int Status { get; set; }

    /// <summary>
    /// 拒绝原因（审核拒绝时必填）
    /// </summary>
    public string? RejectReason { get; set; }
}

/// <summary>
/// 提现记录信息DTO
/// </summary>
public class WithdrawRecordInfo
{
    public int Id { get; set; }
    public decimal Amount { get; set; }
    public string WithdrawMethod { get; set; } = string.Empty;
    public string Account { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public string? RejectReason { get; set; }
    public string ApplyTime { get; set; } = string.Empty;
    public string? ProcessTime { get; set; }
}

/// <summary>
/// 提现记录响应DTO
/// </summary>
public class WithdrawRecordsResponse
{
    public List<WithdrawRecordInfo> Items { get; set; } = new();
    public PaginationInfo Pagination { get; set; } = null!;
}