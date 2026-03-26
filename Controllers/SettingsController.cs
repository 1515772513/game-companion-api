using GameCompanion.Api.DTOs.Settings;
using GameCompanion.Api.Helpers;
using GameCompanion.Api.Models;
using GameCompanion.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameCompanion.Api.Controllers;

/// <summary>
/// 设置控制器
/// </summary>
[ApiController]
[Route("api/settings")]
[Authorize]
public class SettingsController : ControllerBase
{
    private readonly ISettingsService _settingsService;
    private readonly ILogger<SettingsController> _logger;

    public SettingsController(ISettingsService settingsService, ILogger<SettingsController> logger)
    {
        _settingsService = settingsService;
        _logger = logger;
    }

    /// <summary>
    /// 获取账号设置
    /// </summary>
    [HttpGet("account")]
    [ProducesResponseType(typeof(ApiResponse<AccountSettingsDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<>), 404)]
    [ProducesResponseType(typeof(ApiResponse<>), 500)]
    public async Task<IActionResult> GetAccountSettings()
    {
        var userId = GetUserIdFromClaims();
        if (userId == 0)
        {
            return ApiResponse<AccountSettingsDto>.Fail(401, "用户未授权").ToActionResult();
        }

        var result = await _settingsService.GetAccountSettingsAsync(userId);
        return result.ToActionResult();
    }

    /// <summary>
    /// 修改手机号
    /// </summary>
    [HttpPost("change-phone")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(typeof(ApiResponse<>), 404)]
    [ProducesResponseType(typeof(ApiResponse<>), 400)]
    [ProducesResponseType(typeof(ApiResponse<>), 500)]
    public async Task<IActionResult> ChangePhone([FromBody] ChangePhoneDto changeDto)
    {
        var userId = GetUserIdFromClaims();
        if (userId == 0)
        {
            return ApiResponse<bool>.Fail(401, "用户未授权").ToActionResult();
        }

        var result = await _settingsService.ChangePhoneAsync(userId, changeDto);
        return result.ToActionResult();
    }

    /// <summary>
    /// 修改密码
    /// </summary>
    [HttpPost("change-password")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(typeof(ApiResponse<>), 404)]
    [ProducesResponseType(typeof(ApiResponse<>), 400)]
    [ProducesResponseType(typeof(ApiResponse<>), 500)]
    public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordDto changeDto)
    {
        var userId = GetUserIdFromClaims();
        if (userId == 0)
        {
            return ApiResponse<bool>.Fail(401, "用户未授权").ToActionResult();
        }

        var result = await _settingsService.ChangePasswordAsync(userId, changeDto);
        return result.ToActionResult();
    }

    /// <summary>
    /// 更新隐私设置
    /// </summary>
    [HttpPut("privacy")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(typeof(ApiResponse<>), 404)]
    [ProducesResponseType(typeof(ApiResponse<>), 400)]
    [ProducesResponseType(typeof(ApiResponse<>), 500)]
    public async Task<IActionResult> UpdatePrivacy([FromBody] PrivacySettingsDto privacyDto)
    {
        var userId = GetUserIdFromClaims();
        if (userId == 0)
        {
            return ApiResponse<bool>.Fail(401, "用户未授权").ToActionResult();
        }

        var result = await _settingsService.UpdatePrivacyAsync(userId, privacyDto);
        return result.ToActionResult();
    }

    /// <summary>
    /// 更新通知设置
    /// </summary>
    [HttpPut("notification")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(typeof(ApiResponse<>), 404)]
    [ProducesResponseType(typeof(ApiResponse<>), 400)]
    [ProducesResponseType(typeof(ApiResponse<>), 500)]
    public async Task<IActionResult> UpdateNotification([FromBody] NotificationSettingsDto notificationDto)
    {
        var userId = GetUserIdFromClaims();
        if (userId == 0)
        {
            return ApiResponse<bool>.Fail(401, "用户未授权").ToActionResult();
        }

        var result = await _settingsService.UpdateNotificationAsync(userId, notificationDto);
        return result.ToActionResult();
    }

    /// <summary>
    /// 意见反馈
    /// </summary>
    [HttpPost("feedback")]
    [ProducesResponseType(typeof(ApiResponse<FeedbackResponseDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<>), 404)]
    [ProducesResponseType(typeof(ApiResponse<>), 400)]
    [ProducesResponseType(typeof(ApiResponse<>), 500)]
    public async Task<IActionResult> Feedback([FromBody] FeedbackDto feedbackDto)
    {
        var userId = GetUserIdFromClaims();
        if (userId == 0)
        {
            return ApiResponse<FeedbackResponseDto>.Fail(401, "用户未授权").ToActionResult();
        }

        var result = await _settingsService.SubmitFeedbackAsync(userId, feedbackDto);
        return result.ToActionResult();
    }

    /// <summary>
    /// 从用户声明中获取用户ID
    /// </summary>
    private int GetUserIdFromClaims()
    {
        var userIdClaim = User.FindFirst("userId");
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
        {
            return userId;
        }
        return 0;
    }
}