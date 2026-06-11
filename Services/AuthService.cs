using GameCompanion.Api.DTOs.Auth;
using GameCompanion.Api.Data;
using GameCompanion.Api.Models;
using GameCompanion.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using GameCompanion.Api.Utils;
using System.Text.Json;

namespace GameCompanion.Api.Services;

/// <summary>
/// 认证服务实现
/// </summary>
public class AuthService : IAuthService
{
    private readonly GameCompanionContext _context;
    private readonly IConfiguration _configuration;
    private readonly ILogger<AuthService> _logger;

    public AuthService(GameCompanionContext context, IConfiguration configuration, ILogger<AuthService> logger)
    {
        _context = context;
        _configuration = configuration;
        _logger = logger;
    }

    public async Task<ApiResponse<LoginResponse>> LoginAsync(LoginRequest request)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Phone == request.Phone && u.IsAdmin == true);

        if (user == null)
        {
            return ApiResponse<LoginResponse>.ErrorResponse(1001, "用户不存在");
        }

        if (!VerifyPassword(request.Password, user.Password))
        {
            return ApiResponse<LoginResponse>.ErrorResponse(1002, "密码错误");
        }

        if (user.Status != true)
        {
            return ApiResponse<LoginResponse>.ErrorResponse(1007, "账号已被禁用");
        }

        // 更新最后登录时间
        user.LastLoginTime = DateTime.Now;
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
                Gender = user.Gender.GetSafeInt(),
                VipLevel = user.VipLevel.GetSafeInt(),
                VipExpireTime = user.VipExpireDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                Balance = user.Balance.GetSafeDecimal(),
                Points = user.Points.GetSafeInt(),
                IsCompanion = false, // TODO: 从陪玩师表查询
                CompanionStatus = null,
                CreatedAt = user.CreatedAt.ToDateTimeString()
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
            Status = true,
            CreatedAt = DateTime.Now,
            UpdatedAt = DateTime.Now
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
                CreatedAt = user.CreatedAt.ToDateTimeString()
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
        user.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();

        return ApiResponse<object>.SuccessResponse(new
        {
            reset_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
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
            logout_time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
        }, "登出成功");
    }

    private string GenerateJwtToken(User user)
    {
        var jwtSettings = _configuration.GetSection("JwtSettings");
        var secretKey = Encoding.UTF8.GetBytes(jwtSettings["SecretKey"]!);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Username ?? string.Empty),
            new Claim(ClaimTypes.MobilePhone, user.Phone ?? string.Empty),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim("Openid", user.Openid ?? string.Empty)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.Now.AddMinutes(int.Parse(jwtSettings["ExpirationMinutes"]!)),
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
        if (string.IsNullOrEmpty(hash)) return false;
        // 与 HashPassword 保持一致（Base64）；同时兼容历史明文存储的数据
        return hash == HashPassword(password) || hash == password;
    }

    private string MaskPhone(string phone)
    {
        if (string.IsNullOrEmpty(phone)) return string.Empty;
        if (phone.Length == 11)
        {
            return phone.Substring(0, 3) + "****" + phone.Substring(7);
        }
        return phone;
    }

    #region 客户端 mobile

    /// <summary>
    /// 手机号一键登录（无验证码 + 无账号自动创建）
    /// </summary>
    public async Task<ApiResponse<LoginResponse>> SmsLoginAsync(SmsLoginRequest request)
    {
        try
        {
            // 1. 根据手机号查询用户
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Phone == request.Phone);

            // 2. 如果用户不存在 → 自动创建账号
            if (user == null)
            {
                user = new User
                {
                    Phone = request.Phone,
                    Username = $"user_{request.Phone}", // 默认用户名
                    Nickname = $"用户{request.Phone[^4..]}", // 尾号4位
                    Status = true, // 启用
                    IsBlocked = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    LastLoginTime = DateTime.Now
                    // 你有其他默认字段，在这里继续加
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            else
            {
                // 3. 已有账号 → 校验状态
                if (user.Status != true)
                    return ApiResponse<LoginResponse>.ErrorResponse(1007, "账号已被禁用");

                if (user.IsBlocked == true)
                    return ApiResponse<LoginResponse>.ErrorResponse(1008, "账号已被封禁");
            }

            // 4. 统一更新登录时间
            user.LastLoginTime = DateTime.Now;
            user.UpdatedAt = DateTime.Now;
            await _context.SaveChangesAsync();

            // 5. 生成 Token
            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            // 6. 返回登录成功
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
                    Gender = user.Gender ?? 0,
                    VipLevel = user.VipLevel.GetSafeInt(),
                    VipExpireTime = user.VipExpireDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                    Balance = user.Balance.GetSafeDecimal(),
                    Points = user.Points.GetSafeInt(),
                    IsCompanion = user.Companions.Any(),
                    CompanionStatus = user.Companions.FirstOrDefault()?.Status,
                    CreatedAt = user.CreatedAt?.ToDateTimeString() ?? string.Empty
                }
            }, "登录成功");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "一键登录异常：{Phone}", request.Phone);
            return ApiResponse<LoginResponse>.ErrorResponse(500, "服务器异常");
        }
    }

    /// <summary>
    /// 微信一键登录
    /// </summary>
    public async Task<ApiResponse<LoginResponse>> WechatLoginAsync(WechatLoginRequest request)
    {
        using var transaction = await _context.Database.BeginTransactionAsync();

        try
        {
            // 1. 获取微信 openid
            var wechatResult = await GetWechatOpenIdAsync(request.Openid);
            string openid = wechatResult.openid;

            if (string.IsNullOrEmpty(openid))
            {
                return ApiResponse<LoginResponse>.ErrorResponse(1001, "获取微信授权失败");
            }

            // 2. 查询用户（❌ 删掉 AsNoTracking，这是报错根源）
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Openid == openid);

            // 3. 新用户
            if (user == null)
            {
                user = new User
                {
                    Openid = openid,
                    Nickname = request.Nickname ?? $"用户{openid[^4..]}",
                    Avatar = request.Avatar ?? string.Empty,
                    Status = true,
                    IsBlocked = false,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    LastLoginTime = DateTime.Now,
                    Phone = string.Empty,
                    Gender = 0,
                    VipLevel = 0
                };

                _context.Users.Add(user);
            }
            else
            {
                // 4. 老用户校验
                if (user.Status != true)
                    return ApiResponse<LoginResponse>.ErrorResponse(1007, "账号已被禁用");
                if (user.IsBlocked == true)
                    return ApiResponse<LoginResponse>.ErrorResponse(1008, "账号已被封禁");
            }

            // 5. 统一更新登录时间
            user.LastLoginTime = DateTime.Now;
            user.UpdatedAt = DateTime.Now;

            // 6. 一次保存
            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            // 7. 生成token
            var token = GenerateJwtToken(user);
            var refreshToken = GenerateRefreshToken();

            // 8. 查询是否是陪玩师
            bool isCompanion = await _context.Companions.AnyAsync(c => c.UserId == user.Id);
            int? companionStatus = isCompanion ? await _context.Companions
                .Where(c => c.UserId == user.Id)
                .Select(c => c.Status)
                .FirstOrDefaultAsync() : null;

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
                    Gender = user.Gender ?? 0,
                    VipLevel = user.VipLevel.GetSafeInt(),
                    VipExpireTime = user.VipExpireDate?.ToString("yyyy-MM-dd HH:mm:ss"),
                    Balance = user.Balance.GetSafeDecimal(),
                    Points = user.Points.GetSafeInt(),
                    IsCompanion = isCompanion,
                    CompanionStatus = companionStatus,
                    CreatedAt = user.CreatedAt?.ToDateTimeString() ?? string.Empty
                }
            }, "登录成功");
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync();
            _logger.LogError(ex, "微信登录异常 Openid:{Openid}", request.Openid);
            return ApiResponse<LoginResponse>.ErrorResponse(500, "登录失败");
        }
    }
    
    #endregion


    #region 私有方法

    /// <summary>
    /// 根据微信小程序 code 获取 openid
    /// </summary>
    private async Task<WechatSessionResponse> GetWechatOpenIdAsync(string code)
    {
        // 你小程序的 appid 和 secret（从微信公众平台拿）
        string appId = "wx0cbe906910dac8d2";
        string secret = "8a43a08a58fa33f72f0c77118cbc92f0";

        // 微信官方接口
        string url = $"https://api.weixin.qq.com/sns/jscode2session" +
            $"?appid={appId}" +
            $"&secret={secret}" +
            $"&js_code={code}" +
            $"&grant_type=authorization_code";

        using var http = new HttpClient();
        var response = await http.GetAsync(url);
        var json = await response.Content.ReadAsStringAsync();

        // 解析返回结果
        var result = JsonSerializer.Deserialize<WechatSessionResponse>(json);

        // 如果 errcode 不为 0，说明获取失败
        if (!string.IsNullOrEmpty(result.errcode) && result.errcode != "0")
        {
            throw new Exception($"微信授权失败：{result.errmsg}");
        }

        return result;
    }

    // 接收微信返回的模型
    public class WechatSessionResponse
    {
        public string openid { get; set; } = "";
        public string session_key { get; set; } = "";
        public string errcode { get; set; } = "";
        public string errmsg { get; set; } = "";
    }

    #endregion


}