using GameCompanion.Api.DTOs.Messaging;
using GameCompanion.Api.Models;
using GameCompanion.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameCompanion.Api.Controllers;

/// <summary>
/// 通知控制器
/// </summary>
[ApiController]
[Route("api/notifications")]
[Authorize]
public class NotificationsController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly ILogger<NotificationsController> _logger;

    public NotificationsController(IMessageService messageService, ILogger<NotificationsController> logger)
    {
        _messageService = messageService;
        _logger = logger;
    }

    /// <summary>
    /// 获取官方通知
    /// </summary>
    /// <param name="type">通知类型筛选：0-系统通知，1-活动通知，2-订单通知</param>
    /// <param name="page">页码</param>
    /// <param name="pageSize">每页数量</param>
    /// <returns>通知列表</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<NotificationListResponseDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> GetNotifications(
        [FromQuery] int? type = null,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        try
        {
            var userId = GetCurrentUserUserId();
            var response = await _messageService.GetNotificationsAsync(userId, type, page, pageSize);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取通知列表失败");
            return StatusCode(500, ApiResponse.Fail(500, "服务器内部错误"));
        }
    }

    /// <summary>
    /// 标记通知已读
    /// </summary>
    /// <param name="id">通知ID</param>
    /// <returns>标记结果</returns>
    [HttpPost("{id}/read")]
    [ProducesResponseType(typeof(ApiResponse<MarkNotificationReadResponseDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> MarkNotificationRead([FromRoute] long id)
    {
        try
        {
            var userId = GetCurrentUserUserId();
            var response = await _messageService.MarkNotificationReadAsync(userId, id);

            if (response.Code == 5010)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "标记通知已读失败");
            return StatusCode(500, ApiResponse.Fail(500, "服务器内部错误"));
        }
    }

    /// <summary>
    /// 获取当前用户ID
    /// </summary>
    private int GetCurrentUserUserId()
    {
        // 这里应该从JWT Token中解析用户ID
        // 临时返回一个固定值，实际使用时需要从token中获取
        return 10086888; // 示例用户ID
    }
}