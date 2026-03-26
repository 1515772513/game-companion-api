using GameCompanion.Api.DTOs;
using GameCompanion.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameCompanion.Api.Controllers;

/// <summary>
/// 动态社区控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;
    private readonly ILogger<PostsController> _logger;

    public PostsController(IPostService postService, ILogger<PostsController> logger)
    {
        _postService = postService;
        _logger = logger;
    }

    /// <summary>
    /// 发布动态
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> CreatePost([FromBody] CreatePostRequest request)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _postService.CreatePostAsync(userId, request);
        if (!result.Success)
        {
            return BadRequest(ApiResponse<object>.Fail(result.ErrorCode ?? 400, result.Message ?? ""));
        }
        return Ok(ApiResponse<object>.Success(result.Data, result.Message));
    }

    /// <summary>
    /// 获取动态列表
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetPosts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string feedType = "recommend",
        [FromQuery] int? gameId = null,
        [FromQuery] int? topicId = null)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _postService.GetPostsAsync(userId, page, pageSize, feedType, gameId, topicId);
        return Ok(ApiResponse<object>.Success(result));
    }

    /// <summary>
    /// 获取动态详情
    /// </summary>
    [HttpGet("{postId}")]
    public async Task<ActionResult<ApiResponse<object>>> GetPostDetail(int postId)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _postService.GetPostDetailAsync(postId, userId);
        if (result == null)
        {
            return NotFound(ApiResponse<object>.Fail(4001, "动态不存在", "Post not found"));
        }
        return Ok(ApiResponse<object>.Success(result));
    }

    /// <summary>
    /// 点赞/取消点赞动态
    /// </summary>
    [HttpPost("{postId}/like")]
    public async Task<ActionResult<ApiResponse<object>>> LikePost(int postId, [FromBody] LikePostRequest request)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _postService.LikePostAsync(postId, userId, request.Action);
        return Ok(ApiResponse<object>.Success(result.Data, result.Message));
    }

    /// <summary>
    /// 收藏/取消收藏动态
    /// </summary>
    [HttpPost("{postId}/collect")]
    public async Task<ActionResult<ApiResponse<object>>> CollectPost(int postId, [FromBody] CollectPostRequest request)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _postService.CollectPostAsync(postId, userId, request.Action);
        return Ok(ApiResponse<object>.Success(result.Data, result.Message));
    }

    /// <summary>
    /// 评论动态
    /// </summary>
    [HttpPost("{postId}/comments")]
    public async Task<ActionResult<ApiResponse<object>> CommentPost(int postId, [FromBody] CommentPostRequest request)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _postService.CommentPostAsync(postId, userId, request);
        if (!result.Success)
        {
            return BadRequest(ApiResponse<object>.Fail(result.ErrorCode ?? 400, result.Message ?? ""));
        }
        return Ok(ApiResponse<object>.Success(result.Data, result.Message));
    }

    /// <summary>
    /// 获取我的发布
    /// </summary>
    [HttpGet("my")]
    public async Task<ActionResult<ApiResponse<object>>> GetMyPosts(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] int? status = null)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _postService.GetMyPostsAsync(userId, page, pageSize, status);
        return Ok(ApiResponse<object>.Success(result));
    }

    /// <summary>
    /// 删除动态
    /// </summary>
    [HttpDelete("{postId}")]
    public async Task<ActionResult<ApiResponse<object>>> DeletePost(int postId)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _postService.DeletePostAsync(postId, userId);
        if (!result.Success)
        {
            return BadRequest(ApiResponse<object>.Fail(result.ErrorCode ?? 400, result.Message ?? ""));
        }
        return Ok(ApiResponse<object>.Success(null, result.Message));
    }

    /// <summary>
    /// 保存草稿
    /// </summary>
    [HttpPost("draft")]
    public async Task<ActionResult<ApiResponse<object>>> SaveDraft([FromBody] SaveDraftRequest request)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _postService.SaveDraftAsync(userId, request);
        return Ok(ApiResponse<object>.Success(result.Data, result.Message));
    }

    /// <summary>
    /// 获取草稿列表
    /// </summary>
    [HttpGet("drafts")]
    public async Task<ActionResult<ApiResponse<object>>> GetDrafts()
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _postService.GetDraftsAsync(userId);
        return Ok(ApiResponse<object>.Success(result));
    }
}
