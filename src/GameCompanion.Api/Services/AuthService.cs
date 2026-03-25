using GameCompanion.Api.Configuration;
using GameCompanion.Api.Helpers;
using GameCompanion.Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;

namespace GameCompanion.Api.Services;

/// <summary>
/// 认证服务实现
/// </summary>
public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IJwtService _jwtService;
    private readonly ILogger<AuthService> _logger;

    public AuthService(ApplicationDbContext context, IJwtService jwtService, ILogger<AuthService> logger)
    {
        _context = context;
        _jwtService = jwtService;
        _logger = logger;
    }

    /// <summary>
    /// 管理员登录
    /// </summary>
    public async Task<(string? accessToken, string? refreshToken, object? adminInfo)> LoginAsync(string username, string password, bool rememberMe)
    {
        try
        {
            // 查找管理员
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Username == username);
            if (admin == null)
            {
                return (null, null, null);
            }

            // 验证密码（这里使用简单的BCrypt验证，实际项目中应该使用哈希）
            if (!VerifyPassword(password, admin.Password))
            {
                return (null, null, null);
            }

            // 检查账号状态
            if (admin.AccountStatus != 0)
            {
                return (null, null, null);
            }

            // 解析权限
            var permissions = ParsePermissions(admin.Permissions);

            // 生成Token
            var accessToken = _jwtService.GenerateAccessToken(admin.Id, admin.Username, admin.Role ?? "admin", permissions);
            var refreshToken = _jwtService.GenerateRefreshToken();

            // 更新登录信息
            admin.LastLoginTime = DateTime.Now;
            admin.LastLoginIp = ""; // 从请求中获取IP
            await _context.SaveChangesAsync();

            // 构建返回的管理员信息
            var adminInfo = new
            {
                id = admin.Id,
                username = admin.Username,
                real_name = admin.RealName,
                avatar_url = admin.AvatarUrl,
                role = admin.Role,
                permissions = permissions,
                last_login_time = admin.LastLoginTime?.ToString("yyyy-MM-dd HH:mm:ss"),
                last_login_ip = admin.LastLoginIp
            };

            return (accessToken, refreshToken, adminInfo);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "登录失败: {Username}", username);
            return (null, null, null);
        }
    }

    /// <summary>
    /// 管理员登出
    /// </summary>
    public Task<bool> LogoutAsync(string token)
    {
        // 实际项目中应该将token加入黑名单
        return Task.FromResult(true);
    }

    /// <summary>
    /// 刷新Token
    /// </summary>
    public async Task<(string? accessToken, string? message)> RefreshTokenAsync(string refreshToken)
    {
        // 实际项目中应该验证refreshToken是否在数据库中或缓存中
        // 这里简化处理，直接返回失败
        await Task.CompletedTask;
        return (null, "refresh_token已过期或无效");
    }

    /// <summary>
    /// 获取当前管理员信息
    /// </summary>
    public async Task<object?> GetCurrentUserAsync(int adminId)
    {
        try
        {
            var admin = await _context.Admins.FirstOrDefaultAsync(a => a.Id == adminId);
            if (admin == null)
            {
                return null;
            }

            var permissions = ParsePermissions(admin.Permissions);

            return new
            {
                id = admin.Id,
                username = admin.Username,
                real_name = admin.RealName,
                avatar_url = admin.AvatarUrl,
                role = admin.Role,
                permissions = permissions,
                last_login_time = admin.LastLoginTime?.ToString("yyyy-MM-dd HH:mm:ss"),
                last_login_ip = admin.LastLoginIp,
                created_at = admin.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            };
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取管理员信息失败: {AdminId}", adminId);
            return null;
        }
    }

    /// <summary>
    /// 验证密码
    /// </summary>
    private bool VerifyPassword(string password, string hashedPassword)
    {
        // 简化实现，实际应使用BCrypt等哈希算法
        return password == hashedPassword; // 仅用于演示，生产环境必须使用哈希
    }

    /// <summary>
    /// 解析权限
    /// </summary>
    private string[] ParsePermissions(string? permissionsJson)
    {
        if (string.IsNullOrEmpty(permissionsJson))
        {
            return Array.Empty<string>();
        }

        try
        {
            // 简化处理，实际应解析JSON
            if (permissionsJson == "[\"*\"]")
            {
                return new[] { "*" };
            }
            return Array.Empty<string>();
        }
        catch
        {
            return Array.Empty<string>();
        }
    }
}
