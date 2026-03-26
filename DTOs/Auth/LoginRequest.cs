using System.ComponentModel.DataAnnotations;

namespace GameCompanion.Api.DTOs.Auth;

/// <summary>
/// 用户登录请求DTO
/// </summary>
public class LoginRequest
{
    [Required(ErrorMessage = "手机号不能为空")]
    [Phone(ErrorMessage = "手机号格式不正确")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "密码不能为空")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度应为6-20位")]
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// 用户注册请求DTO
/// </summary>
public class RegisterRequest
{
    [Required(ErrorMessage = "手机号不能为空")]
    [Phone(ErrorMessage = "手机号格式不正确")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "密码不能为空")]
    [StringLength(20, MinimumLength = 6, ErrorMessage = "密码长度应为6-20位")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "验证码不能为空")]
    [StringLength(6, MinimumLength = 6)]
    public string VerifyCode { get; set; } = string.Empty;

    [StringLength(20, MinimumLength = 2)]
    public string? Nickname { get; set; }

    [StringLength(8)]
    public string? InviteCode { get; set; }
}

/// <summary>
/// 登录响应DTO
/// </summary>
public class LoginResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
    public string TokenType { get; set; } = "Bearer";
    public int ExpiresIn { get; set; } = 7200;
    public UserInfo UserInfo { get; set; } = null!;
}

/// <summary>
/// 用户信息DTO
/// </summary>
public class UserInfo
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string Phone { get; set; } = string.Empty;
    public int Gender { get; set; }
    public int VipLevel { get; set; }
    public string? VipExpireTime { get; set; }
    public decimal Balance { get; set; }
    public int Points { get; set; }
    public bool IsCompanion { get; set; }
    public string? CompanionStatus { get; set; }
    public string CreatedAt { get; set; } = string.Empty;
}

/// <summary>
/// 发送验证码请求DTO
/// </summary>
public class SendCodeRequest
{
    [Required(ErrorMessage = "手机号不能为空")]
    [Phone(ErrorMessage = "手机号格式不正确")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "验证码类型不能为空")]
    public string Type { get; set; } = string.Empty; // register, login, reset_password
}

/// <summary>
/// 重置密码请求DTO
/// </summary>
public class ResetPasswordRequest
{
    [Required(ErrorMessage = "手机号不能为空")]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "验证码不能为空")]
    public string VerifyCode { get; set; } = string.Empty;

    [Required(ErrorMessage = "新密码不能为空")]
    [StringLength(20, MinimumLength = 6)]
    public string NewPassword { get; set; } = string.Empty;
}

/// <summary>
/// 刷新Token请求DTO
/// </summary>
public class RefreshTokenRequest
{
    [Required]
    public string RefreshToken { get; set; } = string.Empty;
}
