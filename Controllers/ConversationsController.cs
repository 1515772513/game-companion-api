using GameCompanion.Api.DTOs.Messaging;
using GameCompanion.Api.Models;
using GameCompanion.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameCompanion.Api.Controllers;

/// <summary>
/// 会话控制器
/// </summary>
[ApiController]
[Route("api/conversations")]
[Authorize]
public class ConversationsController : ControllerBase
{
    private readonly IMessageService _messageService;
    private readonly ILogger<ConversationsController> _logger;

    public ConversationsController(IMessageService messageService, ILogger<ConversationsController> logger)
    {
        _messageService = messageService;
        _logger = logger;
    }

    /// <summary>
    /// 获取会话列表
    /// </summary>
    /// <param name="type">会话类型：all-全部，user-用户聊天，system-系统通知</param>
    /// <param name="page">页码</param>
    /// <param name="pageSize">每页数量</param>
    /// <returns>会话列表</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<ConversationsResponseDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> GetConversations(
        [FromQuery] string? type = "all",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        try
        {
            var userId = GetCurrentUserUserId();
            var response = await _messageService.GetConversationsAsync(userId, type, page, pageSize);
            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取会话列表失败");
            return StatusCode(500, ApiResponse.Fail(500, "服务器内部错误"));
        }
    }

    /// <summary>
    /// 获取聊天详情
    /// </summary>
    /// <param name="id">会话ID</param>
    /// <param name="page">页码</param>
    /// <param name="pageSize">每页数量</param>
    /// <returns>聊天详情</returns>
    [HttpGet("{id}/messages")]
    [ProducesResponseType(typeof(ApiResponse<ConversationDetailDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 403)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> GetConversationDetail(
        [FromRoute] long id,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 50)
    {
        try
        {
            var userId = GetCurrentUserUserId();
            var response = await _messageService.GetConversationDetailAsync(userId, id, page, pageSize);

            if (response.Code == 5001 || response.Code == 5003)
            {
                return StatusCode(response.Code == 5001 ? 404 : 403, response);
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取聊天详情失败");
            return StatusCode(500, ApiResponse.Fail(500, "服务器内部错误"));
        }
    }

    /// <summary>
    /// 发送消息
    /// </summary>
    /// <param name="id">会话ID</param>
    /// <param name="request">发送消息请求</param>
    /// <returns>发送结果</returns>
    [HttpPost("{id}/messages")]
    [ProducesResponseType(typeof(ApiResponse<SendMessageResponseDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 403)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    [Consumes("application/json")]
    public async Task<IActionResult> SendMessage(
        [FromRoute] long id,
        [FromBody] SendMessageRequestDto request)
    {
        try
        {
            var userId = GetCurrentUserUserId();
            var response = await _messageService.SendMessageAsync(userId, id, request);

            if (response.Code == 5001)
            {
                return NotFound(response);
            }
            else if (response.Code == 5003)
            {
                return StatusCode(403, response);
            }
            else if (response.Code == 400)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "发送消息失败");
            return StatusCode(500, ApiResponse.Fail(500, "服务器内部错误"));
        }
    }

    /// <summary>
    /// 上传聊天图片
    /// </summary>
    /// <param name="image">图片文件</param>
    /// <returns>上传结果</returns>
    [HttpPost("upload-image")]
    [ProducesResponseType(typeof(ApiResponse<UploadImageResponseDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> UploadImage([FromForm] IFormFile image)
    {
        try
        {
            var userId = GetCurrentUserUserId();
            var response = await _messageService.UploadImageAsync(userId, image);

            if (response.Code == 6001 || response.Code == 6002)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "上传图片失败");
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