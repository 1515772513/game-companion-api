using GameCompanion.Api.DTOs.Auth;
using GameCompanion.Api.Models;

namespace GameCompanion.Api.Services;

/// <summary>
/// 认证服务接口
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// 用户登录
    /// </summary>
    Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request);

    /// <summary>
    /// 用户注册
    /// </summary>
    Task<ApiResponse<LoginResponse>> RegisterAsync(RegisterRequest request);

    /// <summary>
    /// 发送验证码
    /// </summary>
    Task<ApiResponse<object>> SendCodeAsync(SendCodeRequest request);

    /// <summary>
    /// 重置密码
    /// </summary>
    Task<ApiResponse<object>> ResetPasswordAsync(ResetPasswordRequest request);

    /// <summary>
    /// 刷新Token
    /// </summary>
    Task<ApiResponse<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request);

    /// <summary>
    /// 退出登录
    /// </summary>
    Task<ApiResponse<object>> LogoutAsync(int userId);

    /// <summary>
    /// 手机号一键登录（无验证码）
    /// </summary>
    Task<ApiResponse<LoginResponse>> SmsLoginAsync(SmsLoginRequest request);
}
