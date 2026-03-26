using GameCompanion.Api.DTOs;
using GameCompanion.Api.Models;
using GameCompanion.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace GameCompanion.Api.Controllers;

/// <summary>
/// 认证授权控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
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
    /// 管理员登录
    /// </summary>
    [HttpPost("login")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<object>>> Login([FromBody] LoginRequest request)
    {
        try
        {
            var (accessToken, refreshToken, adminInfo) = await _authService.LoginAsync(
                request.Username,
                request.Password,
                request.RememberMe ?? false
            );

            if (accessToken == null)
            {
                return Unauthorized(ApiResponse<object>.Fail(401, "账号或密码错误", "Invalid username or password"));
            }

            var data = new
            {
                access_token = accessToken,
                refresh_token = refreshToken,
                expires_in = 7200,
                admin_info = adminInfo
            };

            return Ok(ApiResponse<object>.Success(data, "登录成功"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "登录失败");
            return BadRequest(ApiResponse<object>.Fail(400, "登录失败", ex.Message));
        }
    }

    /// <summary>
    /// 管理员登出
    /// </summary>
    [HttpPost("logout")]
    public async Task<ActionResult<ApiResponse<object>>> Logout()
    {
        try
        {
            var token = Request.Headers.Authorization.ToString().Replace("Bearer ", "");
            await _authService.LogoutAsync(token);
            return Ok(ApiResponse<object>.Success(null, "登出成功"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "登出失败");
            return BadRequest(ApiResponse<object>.Fail(400, "登出失败", ex.Message));
        }
    }

    /// <summary>
    /// 刷新Token
    /// </summary>
    [HttpPost("refresh")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<object>>> Refresh([FromBody] RefreshRequest request)
    {
        try
        {
            var (accessToken, message) = await _authService.RefreshTokenAsync(request.RefreshToken);

            if (accessToken == null)
            {
                return Unauthorized(ApiResponse<object>.Fail(401, message ?? "刷新失败", "Invalid or expired refresh token"));
            }

            var data = new
            {
                access_token = accessToken,
                expires_in = 7200
            };

            return Ok(ApiResponse<object>.Success(data, "Token刷新成功"));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Token刷新失败");
            return BadRequest(ApiResponse<object>.Fail(400, "Token刷新失败", ex.Message));
        }
    }

    /// <summary>
    /// 获取当前管理员信息
    /// </summary>
    [HttpGet("me")]
    public async Task<ActionResult<ApiResponse<object>>> GetCurrentUser()
    {
        try
        {
            var adminIdStr = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(adminIdStr) || !int.TryParse(adminIdStr, out var adminId))
            {
                return Unauthorized(ApiResponse<object>.Fail(401, "未授权", "Invalid token"));
            }

            var adminInfo = await _authService.GetCurrentUserAsync(adminId);
            if (adminInfo == null)
            {
                return NotFound(ApiResponse<object>.Fail(404, "管理员不存在", "Admin not found"));
            }

            return Ok(ApiResponse<object>.Success(adminInfo));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取管理员信息失败");
            return BadRequest(ApiResponse<object>.Fail(400, "获取信息失败", ex.Message));
        }
    }
}

/// <summary>
/// 登录请求
/// </summary>
public class LoginRequest
{
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool? RememberMe { get; set; }
}

/// <summary>
/// 刷新Token请求
/// </summary>
public class RefreshRequest
{
    public string RefreshToken { get; set; } = string.Empty;
}
