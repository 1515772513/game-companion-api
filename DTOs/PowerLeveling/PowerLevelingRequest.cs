using System.ComponentModel.DataAnnotations;

namespace GameCompanion.Api.DTOs.PowerLeveling;

/// <summary>
/// 创建代练订单请求DTO
/// </summary>
public class CreatePowerLevelingOrderRequest
{
    [Required(ErrorMessage = "服务ID不能为空")]
    public int ServiceId { get; set; }

    [Required(ErrorMessage = "游戏账号不能为空")]
    [StringLength(50, ErrorMessage = "游戏账号长度不能超过50个字符")]
    public string GameAccount { get; set; } = string.Empty;

    [Required(ErrorMessage = "游戏密码不能为空")]
    [StringLength(100, ErrorMessage = "游戏密码长度不能超过100个字符")]
    public string GamePassword { get; set; } = string.Empty;

    [StringLength(50, ErrorMessage = "角色名称长度不能超过50个字符")]
    public string? GameRole { get; set; }

    [Required(ErrorMessage = "当前段位不能为空")]
    [StringLength(50, ErrorMessage = "当前段位长度不能超过50个字符")]
    public string CurrentRank { get; set; } = string.Empty;

    [Required(ErrorMessage = "目标段位不能为空")]
    [StringLength(50, ErrorMessage = "目标段位长度不能超过50个字符")]
    public string TargetRank { get; set; } = string.Empty;

    [StringLength(200, ErrorMessage = "特殊要求长度不能超过200个字符")]
    public string? SpecialRequirements { get; set; }

    [Required(ErrorMessage = "联系电话不能为空")]
    [Phone(ErrorMessage = "联系电话格式不正确")]
    [StringLength(11, MinimumLength = 11, ErrorMessage = "联系电话长度必须为11位")]
    public string ContactPhone { get; set; } = string.Empty;
}

/// <summary>
/// 代练服务信息DTO
/// </summary>
public class PowerLevelingServiceInfo
{
    public int Id { get; set; }
    public int GameId { get; set; }
    public string GameName { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public string StartRank { get; set; } = string.Empty;
    public string EndRank { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public decimal OriginalPrice { get; set; }
    public decimal Discount { get; set; }
    public int EstimatedDays { get; set; }
    public string Description { get; set; } = string.Empty;
    public List<string>? Requirements { get; set; }
    public List<string>? ProcessSteps { get; set; }
}

/// <summary>
/// 代练订单信息DTO
/// </summary>
public class PowerLevelingOrderInfo
{
    public int Id { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public int Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public string GameName { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public string CurrentRank { get; set; } = string.Empty;
    public string TargetRank { get; set; } = string.Empty;
    public int Progress { get; set; }
    public decimal TotalAmount { get; set; }
    public string CreatedAt { get; set; } = string.Empty;
    public string? EstimatedCompleteTime { get; set; }
}

/// <summary>
/// 代练订单详情DTO
/// </summary>
public class PowerLevelingOrderDetail
{
    public int Id { get; set; }
    public string OrderNo { get; set; } = string.Empty;
    public int Status { get; set; }
    public string StatusText { get; set; } = string.Empty;
    public string GameName { get; set; } = string.Empty;
    public string ServiceName { get; set; } = string.Empty;
    public string GameAccount { get; set; } = string.Empty;
    public string CurrentRank { get; set; } = string.Empty;
    public int CurrentStars { get; set; }
    public string TargetRank { get; set; } = string.Empty;
    public int Progress { get; set; }
    public decimal TotalAmount { get; set; }
    public string? SpecialRequirements { get; set; }
    public string? StartedAt { get; set; }
    public string? EstimatedCompleteTime { get; set; }
    public LevelerInfo Leveler { get; set; } = null!;
    public List<ProgressLogInfo> ProgressLogs { get; set; } = new();
    public string CreatedAt { get; set; } = string.Empty;
}

/// <summary>
/// 代练师信息DTO
/// </summary>
public class LevelerInfo
{
    public string Nickname { get; set; } = string.Empty;
    public string AvatarUrl { get; set; } = string.Empty;
}

/// <summary>
/// 进度日志DTO
/// </summary>
public class ProgressLogInfo
{
    public string Rank { get; set; } = string.Empty;
    public int Stars { get; set; }
    public string CompletedAt { get; set; } = string.Empty;
}

/// <summary>
/// 取消代练订单请求DTO
/// </summary>
public class CancelPowerLevelingOrderRequest
{
    [Required(ErrorMessage = "取消原因不能为空")]
    [StringLength(200, ErrorMessage = "取消原因长度不能超过200个字符")]
    public string CancelReason { get; set; } = string.Empty;
}

/// <summary>
/// 申请代练退款请求DTO
/// </summary>
public class RefundPowerLevelingOrderRequest
{
    [Required(ErrorMessage = "退款原因不能为空")]
    [StringLength(200, ErrorMessage = "退款原因长度不能超过200个字符")]
    public string RefundReason { get; set; } = string.Empty;

    [Range(0.01, 99999.99, ErrorMessage = "退款金额必须在0.01-99999.99之间")]
    public decimal? RefundAmount { get; set; }
}

/// <summary>
/// 代练订单评价请求DTO
/// </summary>
public class ReviewPowerLevelingOrderRequest
{
    [Required(ErrorMessage = "评分不能为空")]
    [Range(1, 5, ErrorMessage = "评分必须是1-5之间的整数")]
    public int Rating { get; set; }

    [Required(ErrorMessage = "评价内容不能为空")]
    [StringLength(500, ErrorMessage = "评价内容长度不能超过500个字符")]
    public string Comment { get; set; } = string.Empty;

    [Range(1, 5, ErrorMessage = "速度评分必须是1-5之间的整数")]
    public int? ServiceSpeed { get; set; }

    [Range(1, 5, ErrorMessage = "质量评分必须是1-5之间的整数")]
    public int? ServiceQuality { get; set; }
}

/// <summary>
/// 代练服务列表响应DTO
/// </summary>
public class PowerLevelingServicesResponse
{
    public List<PowerLevelingServiceInfo> Items { get; set; } = new();
}

/// <summary>
/// 代练订单列表响应DTO
/// </summary>
public class PowerLevelingOrdersResponse
{
    public List<PowerLevelingOrderInfo> Items { get; set; } = new();
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