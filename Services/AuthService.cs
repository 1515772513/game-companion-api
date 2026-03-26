using GameCompanion.Api.DTOs.Auth;
using GameCompanion.Api.Data;
using GameCompanion.Api.Models;
using GameCompanion.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace GameCompanion.Api.Services;

/// <summary>
/// 认证服务实现
/// </summary>
public class AuthService : IAuthService
{
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(ApplicationDbContext context, IConfiguration configuration, ILogger<AuthService> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Phone == request.Phone);

        if (user == null)
        {
            return ApiResponse<LoginResponse>.ErrorResponse(1001, "用户不存在");
        }

        if (!VerifyPassword(request.Password, user.Password))
        {
            return ApiResponse<LoginResponse>.ErrorResponse(1002, "密码错误");
        }

        if (user.Status == "禁用")
        {
            return ApiResponse<LoginResponse>.ErrorResponse(1007, "账号已被禁用");
        }

        // 更新最后登录时间
        user.LastLoginTime = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        return ApiResponse<LoginResponse>.SuccessResponse(new LoginResponse
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            ExpiresIn = 7200,
            UserInfo = new UserInfo
            {
                Id = user.Id,
                Username = user.Username,
                Nickname = user.Nickname,
                Avatar = user.Avatar,
                Phone = MaskPhone(user.Phone),
                Gender = user.Gender,
                VipLevel = user.VipLevel,
                VipExpireTime = user.VipExpireDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                Balance = user.Balance,
                Points = user.Points,
                IsCompanion = false, // TODO: 从陪玩师表查询
                CompanionStatus = null,
                CreatedAt = user.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            }
        }, "登录成功");
    }

    public async Task<ApiResponse<LoginResponse>> RegisterAsync(RegisterRequest request)
    {
        // 检查手机号是否已注册
        var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Phone == request.Phone);
        if (existingUser != null)
        {
            return ApiResponse<LoginResponse>.ErrorResponse(1005, "手机号已注册");
        }

        // TODO: 验证验证码

        var user = new User
        {
            Username = "user" + request.Phone.Substring(7),
            Password = HashPassword(request.Password),
            Phone = request.Phone,
            Nickname = request.Nickname ?? "用户" + request.Phone.Substring(7),
            Status = "正常",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        var token = GenerateJwtToken(user);
        var refreshToken = GenerateRefreshToken();

        return ApiResponse<LoginResponse>.SuccessResponse(new LoginResponse
        {
            AccessToken = token,
            RefreshToken = refreshToken,
            ExpiresIn = 7200,
            UserInfo = new UserInfo
            {
                Id = user.Id,
                Username = user.Username,
                Nickname = user.Nickname,
                Phone = MaskPhone(user.Phone),
                CreatedAt = user.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            }
        }, "注册成功");
    }

    public async Task<ApiResponse<object>> SendCodeAsync(SendCodeRequest request)
    {
        // TODO: 实现发送短信验证码逻辑
        // 这里应该是调用短信服务商API

        await Task.Delay(100); // 模拟发送

        return ApiResponse<object>.SuccessResponse(new
        {
            expire_in = 300,
            phone = MaskPhone(request.Phone)
        }, "验证码已发送");
    }

    public async Task<ApiResponse<object>> ResetPasswordAsync(ResetPasswordRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Phone == request.Phone);
        if (user == null)
        {
            return ApiResponse<object>.ErrorResponse(1001, "用户不存在");
        }

        // TODO: 验证验证码

        user.Password = HashPassword(request.NewPassword);
        user.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return ApiResponse<object>.SuccessResponse(new
        {
            reset_time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
        }, "密码重置成功");
    }

    public async Task<ApiResponse<LoginResponse>> RefreshTokenAsync(RefreshTokenRequest request)
    {
        // TODO: 实现刷新Token逻辑，验证refresh_token
        // 这里需要从数据库或Redis中查找refresh_token

        await Task.Delay(100);

        // 简化示例
        return ApiResponse<LoginResponse>.ErrorResponse(1004, "刷新令牌无效");
    }

    public async Task<ApiResponse<object>> LogoutAsync(int userId)
    {
        // TODO: 将token加入黑名单或从Redis中删除
        await Task.Delay(100);

        return ApiResponse<object>.SuccessResponse(new
        {
            logout_time = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss")
        }, "登出成功");
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim(ClaimTypes.MobilePhone, user.Phone),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(int.Parse(jwtSettings["ExpirationMinutes"]!)),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256Signature),
            Issuer = jwtSettings["Issuer"],
            Audience = jwtSettings["Audience"]
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }

    private string GenerateRefreshToken()
    {
        return Guid.NewGuid().ToString() + "-" + Guid.NewGuid().ToString();
    }

    private string HashPassword(string password)
    {
        // TODO: 使用BCrypt或其他安全的哈希算法
        // 这里简化处理，实际项目中应使用 BCrypt.Net
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
    }

    private bool VerifyPassword(string password, string hash)
    {
        // var hashed = Convert.ToBase64String(Encoding.UTF8.GetBytes(password));
        return password == hash;
    }

    private string MaskPhone(string phone)
    {
        if (phone.Length == 11)
        {
            return phone.Substring(0, 3) + "****" + phone.Substring(7);
        }
        return phone;
    }
}
