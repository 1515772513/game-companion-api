using GameCompanion.Api.DTOs.Auth;
using GameCompanion.Api.Models;
using GameCompanion.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameCompanion.Api.Controllers;

/// <summary>
/// 用户认证接口
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly ILogger<AuthController> _logger;

    public AuthController(IAuthService authService, ILogger<AuthController> logger)
    {
        _authService = authService;
        _logger = logger;
    }

    /// <summary>
    /// 用户登录
    /// </summary>
    /// <param name="request">登录请求</param>
    /// <returns>登录结果</returns>
    [HttpPost("login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Login([FromBody] LoginRequest request)
    {
        _logger.LogInformation("用户登录: {Phone}", request.Phone);
        var result = await _authService.LoginAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// 用户注册
    /// </summary>
    /// <param name="request">注册请求</param>
    /// <returns>注册结果</returns>
    [HttpPost("register")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> Register([FromBody] RegisterRequest request)
    {
        _logger.LogInformation("用户注册: {Phone}", request.Phone);
        var result = await _authService.RegisterAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// 发送验证码
    /// </summary>
    /// <param name="request">发送验证码请求</param>
    /// <returns>发送结果</returns>
    [HttpPost("send-code")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<ApiResponse<object>>> SendCode([FromBody] SendCodeRequest request)
    {
        var result = await _authService.SendCodeAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// 重置密码
    /// </summary>
    /// <param name="request">重置密码请求</param>
    /// <returns>重置结果</returns>
    [HttpPost("reset-password")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<object>>> ResetPassword([FromBody] ResetPasswordRequest request)
    {
        var result = await _authService.ResetPasswordAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// 刷新Token
    /// </summary>
    /// <param name="request">刷新Token请求</param>
    /// <returns>新的Token</returns>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> RefreshToken([FromBody] RefreshTokenRequest request)
    {
        var result = await _authService.RefreshTokenAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// 退出登录
    /// </summary>
    /// <returns>退出结果</returns>
    [HttpPost("logout")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<object>>> Logout()
    {
        // TODO: 从JWT Token中获取用户ID
        var userId = 1; // 示例
        var result = await _authService.LogoutAsync(userId);
        return Ok(result);
    }

    /// <summary>
    /// 手机号一键登录（无验证码）
    /// </summary>
    /// <param name="request">一键登录请求</param>
    /// <returns>登录结果</returns>
    [HttpPost("sms-login")]
    [ProducesResponseType(typeof(ApiResponse<LoginResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    public async Task<ActionResult<ApiResponse<LoginResponse>>> SmsLogin([FromBody] SmsLoginRequest request)
    {
        _logger.LogInformation("手机号一键登录: {Phone}", request.Phone);
        var result = await _authService.SmsLoginAsync(request);
        return Ok(result);
    }
}
