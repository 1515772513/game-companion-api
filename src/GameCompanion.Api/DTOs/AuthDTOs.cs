namespace GameCompanion.Api.DTOs;

/// <summary>
/// 用户注册请求
/// </summary>
public class RegisterRequest
{
    public string Phone { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string VerifyCode { get; set; } = string.Empty;
    public string? Nickname { get; set; }
    public string? InviteCode { get; set; }
}

/// <summary>
/// 用户登录请求
/// </summary>
public class LoginRequest
{
    public string Phone { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}

/// <summary>
/// 发送验证码请求
/// </summary>
public class SendCodeRequest
{
    public string Phone { get; set; } = string.Empty;
    public string Type { get; set; } = string.Empty; // register, login, reset_password, bind_phone
}

/// <summary>
/// 重置密码请求
/// </summary>
public class ResetPasswordRequest
{
    public string Phone { get; set; } = string.Empty;
    public string VerifyCode { get; set; } = string.Empty;
    public string NewPassword { get; set; } = string.Empty;
}

/// <summary>
/// 刷新Token请求
/// </summary>
public class RefreshTokenRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}

/// <summary>
/// 登录/注册响应数据
/// </summary>
public class AuthResponse
{
    public long User_id { get; set; }
    public string Access_token { get; set; } = string.Empty;
    public string Refresh_token { get; set; } = string.Empty;
    public string Token_type { get; set; } = "Bearer";
    public int Expires_in { get; set; }
    public UserInfo User_info { get; set; } = new();
}

/// <summary>
/// 用户基本信息
/// </summary>
public class UserInfo
{
    public long Id { get; set; }
    public string? Username { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string Avatar { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public int Gender { get; set; }
    public int Vip_level { get; set; }
    public string? Vip_expire_time { get; set; }
    public decimal Balance { get; set; }
    public int Points { get; set; }
    public bool Is_companion { get; set; }
    public int? Companion_status { get; set; }
    public string Created_at { get; set; } = string.Empty;
}
