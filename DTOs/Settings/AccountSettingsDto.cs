using System.ComponentModel.DataAnnotations;

namespace GameCompanion.Api.DTOs.Settings;

/// <summary>
/// 账号设置DTO
/// </summary>
public class AccountSettingsDto
{
    public int UserId { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime LastLoginTime { get; set; }
}

/// <summary>
/// 修改手机号DTO
/// </summary>
public class ChangePhoneDto
{
    [Required]
    [MaxLength(11)]
    [RegularExpression(@"^1[3-9]\d{9}$", ErrorMessage = "手机号格式不正确")]
    public string NewPhone { get; set; } = string.Empty;

    [Required]
    [MaxLength(6)]
    public string VerificationCode { get; set; } = string.Empty;
}

/// <summary>
/// 修改密码DTO
/// </summary>
public class ChangePasswordDto
{
    [Required]
    [MinLength(6)]
    public string CurrentPassword { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string NewPassword { get; set; } = string.Empty;

    [Required]
    [MinLength(6)]
    public string ConfirmPassword { get; set; } = string.Empty;
}

/// <summary>
/// 隐私设置DTO
/// </summary>
public class PrivacySettingsDto
{
    public int UserId { get; set; }

    /// <summary>
    /// 是否显示在线状态
    /// </summary>
    public bool ShowOnlineStatus { get; set; } = true;

    /// <summary>
    /// 是否允许陌生人发消息
    /// </summary>
    public bool AllowStrangerMessage { get; set; } = true;

    /// <summary>
    /// 是否显示游戏动态
    /// </summary>
    public bool ShowGameActivity { get; set; } = true;

    /// <summary>
    /// 是否公开个人资料
    /// </summary>
    public bool PublicProfile { get; set; } = true;
}

/// <summary>
/// 通知设置DTO
/// </summary>
public class NotificationSettingsDto
{
    public int UserId { get; set; }

    /// <summary>
    /// 订单通知
    /// </summary>
    public bool OrderNotification { get; set; } = true;

    /// <summary>
    /// 消息通知
    /// </summary>
    public bool MessageNotification { get; set; } = true;

    /// <summary>
    /// 促销通知
    /// </summary>
    public bool PromotionNotification { get; set; } = false;

    /// <summary>
    /// 系统通知
    /// </summary>
    public bool SystemNotification { get; set; } = true;
}

/// <summary>
/// 意见反馈DTO
/// </summary>
public class FeedbackDto
{
    [Required]
    [MinLength(10)]
    [MaxLength(1000)]
    public string Content { get; set; } = string.Empty;

    [MaxLength(100)]
    public string? ContactInfo { get; set; }

    public string? Type { get; set; } = "建议";
}

/// <summary>
/// 意见反馈响应DTO
/// </summary>
public class FeedbackResponseDto
{
    public int Id { get; set; }
    public string Content { get; set; } = string.Empty;
    public string? ContactInfo { get; set; }
    public string Type { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? Response { get; set; }
    public DateTime? ResponseAt { get; set; }
}