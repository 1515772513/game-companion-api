using GameCompanion.Api.Data;
using GameCompanion.Api.DTOs.Settings;
using GameCompanion.Api.Models;
using GameCompanion.Api.Models.Entities;
using GameCompanion.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Services;

/// <summary>
/// 设置服务实现
/// </summary>
public class SettingsService : ISettingsService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SettingsService> _logger;

    public SettingsService(ApplicationDbContext context, ILogger<SettingsService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// 获取账号设置
    /// </summary>
    public async Task<ApiResponse<AccountSettingsDto>> GetAccountSettingsAsync(int userId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return ApiResponse<AccountSettingsDto>.Fail(404, "用户不存在");
            }

            var settingsDto = new AccountSettingsDto
            {
                UserId = user.Id,
                Username = user.Username,
                Email = "", // 实际项目中应该从用户表获取
                Phone = user.Phone,
                Nickname = user.Nickname,
                Status = user.Status ?? "正常",
                CreatedAt = user.CreatedAt,
                LastLoginTime = user.LastLoginTime ?? user.CreatedAt
            };

            return ApiResponse<AccountSettingsDto>.Success(settingsDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取账号设置失败");
            return ApiResponse<AccountSettingsDto>.Fail(500, "获取账号设置失败");
        }
    }

    /// <summary>
    /// 修改手机号
    /// </summary>
    public async Task<ApiResponse<bool>> ChangePhoneAsync(int userId, ChangePhoneDto changeDto)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return ApiResponse<bool>.Fail(404, "用户不存在");
            }

            // 验证验证码（实际项目中应该调用短信服务验证）
            // 这里简化处理，只验证手机号格式
            if (!IsValidPhone(changeDto.NewPhone))
            {
                return ApiResponse<bool>.Fail(400, "手机号格式不正确");
            }

            // 检查手机号是否已被其他用户使用
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Phone == changeDto.NewPhone && u.Id != userId);
            if (existingUser != null)
            {
                return ApiResponse<bool>.Fail(400, "该手机号已被其他用户使用");
            }

            user.Phone = changeDto.NewPhone;
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "修改手机号失败");
            return ApiResponse<bool>.Fail(500, "修改手机号失败");
        }
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    public async Task<ApiResponse<bool>> ChangePasswordAsync(int userId, ChangePasswordDto changeDto)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return ApiResponse<bool>.Fail(404, "用户不存在");
            }

            // 验证密码格式
            if (changeDto.NewPassword != changeDto.ConfirmPassword)
            {
                return ApiResponse<bool>.Fail(400, "两次输入的密码不一致");
            }

            if (changeDto.NewPassword.Length < 6)
            {
                return ApiResponse<bool>.Fail(400, "密码长度至少为6位");
            }

            // 验证当前密码（实际项目中应该使用密码哈希验证）
            // 这里简化处理
            if (user.Password != changeDto.CurrentPassword)
            {
                return ApiResponse<bool>.Fail(400, "当前密码不正确");
            }

            // 更新密码（实际项目中应该哈希处理）
            user.Password = changeDto.NewPassword;
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "修改密码失败");
            return ApiResponse<bool>.Fail(500, "修改密码失败");
        }
    }

    /// <summary>
    /// 更新隐私设置
    /// </summary>
    public async Task<ApiResponse<bool>> UpdatePrivacyAsync(int userId, PrivacySettingsDto privacyDto)
    {
        try
        {
            var setting = await _context.UserSettings.FirstOrDefaultAsync(s => s.UserId == userId);
            if (setting == null)
            {
                // 如果设置不存在，创建新的设置
                setting = new UserSetting
                {
                    UserId = userId,
                    ShowOnlineStatus = privacyDto.ShowOnlineStatus ? 1 : 0,
                    AllowStrangerMessage = privacyDto.AllowStrangerMessage ? 1 : 0,
                    ShowGameActivity = privacyDto.ShowGameActivity ? 1 : 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.UserSettings.Add(setting);
            }
            else
            {
                // 更新现有设置
                setting.ShowOnlineStatus = privacyDto.ShowOnlineStatus ? 1 : 0;
                setting.AllowStrangerMessage = privacyDto.AllowStrangerMessage ? 1 : 0;
                setting.ShowGameActivity = privacyDto.ShowGameActivity ? 1 : 0;
                setting.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return ApiResponse<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "更新隐私设置失败");
            return ApiResponse<bool>.Fail(500, "更新隐私设置失败");
        }
    }

    /// <summary>
    /// 更新通知设置
    /// </summary>
    public async Task<ApiResponse<bool>> UpdateNotificationAsync(int userId, NotificationSettingsDto notificationDto)
    {
        try
        {
            var setting = await _context.UserSettings.FirstOrDefaultAsync(s => s.UserId == userId);
            if (setting == null)
            {
                // 如果设置不存在，创建新的设置
                setting = new UserSetting
                {
                    UserId = userId,
                    OrderNotification = notificationDto.OrderNotification ? 1 : 0,
                    MessageNotification = notificationDto.MessageNotification ? 1 : 0,
                    PromotionNotification = notificationDto.PromotionNotification ? 1 : 0,
                    SystemNotification = notificationDto.SystemNotification ? 1 : 0,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };
                _context.UserSettings.Add(setting);
            }
            else
            {
                // 更新现有设置
                setting.OrderNotification = notificationDto.OrderNotification ? 1 : 0;
                setting.MessageNotification = notificationDto.MessageNotification ? 1 : 0;
                setting.PromotionNotification = notificationDto.PromotionNotification ? 1 : 0;
                setting.SystemNotification = notificationDto.SystemNotification ? 1 : 0;
                setting.UpdatedAt = DateTime.UtcNow;
            }

            await _context.SaveChangesAsync();
            return ApiResponse<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "更新通知设置失败");
            return ApiResponse<bool>.Fail(500, "更新通知设置失败");
        }
    }

    /// <summary>
    /// 提交意见反馈
    /// </summary>
    public async Task<ApiResponse<FeedbackResponseDto>> SubmitFeedbackAsync(int userId, FeedbackDto feedbackDto)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return ApiResponse<FeedbackResponseDto>.Fail(404, "用户不存在");
            }

            var feedback = new Feedback
            {
                UserId = userId,
                Content = feedbackDto.Content,
                Contact = feedbackDto.ContactInfo,
                FeedbackType = feedbackDto.Type,
                Status = "待处理",
                CreatedAt = DateTime.UtcNow
            };

            _context.Feedbacks.Add(feedback);
            await _context.SaveChangesAsync();

            var responseDto = new FeedbackResponseDto
            {
                Id = feedback.Id,
                Content = feedback.Content,
                ContactInfo = feedback.Contact,
                Type = feedback.FeedbackType,
                Status = feedback.Status,
                CreatedAt = feedback.CreatedAt,
                Response = feedback.Reply,
                ResponseAt = null // Feedback实体没有ResponseAt字段
            };

            return ApiResponse<FeedbackResponseDto>.Success(responseDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "提交意见反馈失败");
            return ApiResponse<FeedbackResponseDto>.Fail(500, "提交意见反馈失败");
        }
    }

    /// <summary>
    /// 验证手机号格式
    /// </summary>
    private bool IsValidPhone(string phone)
    {
        return System.Text.RegularExpressions.Regex.IsMatch(phone, @"^1[3-9]\d{9}$");
    }
}