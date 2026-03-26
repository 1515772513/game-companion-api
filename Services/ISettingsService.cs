using GameCompanion.Api.DTOs.Settings;
using GameCompanion.Api.Models;
using GameCompanion.Api.Models.Entities;

namespace GameCompanion.Api.Services;

/// <summary>
/// 设置服务接口
/// </summary>
public interface ISettingsService
{
    /// <summary>
    /// 获取账号设置
    /// </summary>
    Task<ApiResponse<AccountSettingsDto>> GetAccountSettingsAsync(int userId);

    /// <summary>
    /// 修改手机号
    /// </summary>
    Task<ApiResponse<bool>> ChangePhoneAsync(int userId, ChangePhoneDto changeDto);

    /// <summary>
    /// 修改密码
    /// </summary>
    Task<ApiResponse<bool>> ChangePasswordAsync(int userId, ChangePasswordDto changeDto);

    /// <summary>
    /// 更新隐私设置
    /// </summary>
    Task<ApiResponse<bool>> UpdatePrivacyAsync(int userId, PrivacySettingsDto privacyDto);

    /// <summary>
    /// 更新通知设置
    /// </summary>
    Task<ApiResponse<bool>> UpdateNotificationAsync(int userId, NotificationSettingsDto notificationDto);

    /// <summary>
    /// 提交意见反馈
    /// </summary>
    Task<ApiResponse<FeedbackResponseDto>> SubmitFeedbackAsync(int userId, FeedbackDto feedbackDto);
}