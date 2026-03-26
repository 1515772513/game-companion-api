using GameCompanion.Api.DTOs.Posts;
using GameCompanion.Api.Models;
using GameCompanion.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameCompanion.Api.Controllers;

/// <summary>
/// 动态社区接口
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize] // 需要登录才能访问
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
    /// <param name="request">发布动态请求</param>
    /// <returns>发布结果</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ApiResponse<CreatePostResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<ApiResponse<CreatePostResponse>>> CreatePost([FromBody] CreatePostRequest request)
    {
        var userId = GetUserIdFromClaims();
        _logger.LogInformation("用户 {UserId} 发布动态", userId);

        var result = await _postService.CreatePostAsync(request, userId);
        return Ok(result);
    }

    /// <summary>
    /// 获取动态列表
    /// </summary>
    /// <param name="request">获取动态列表请求</param>
    /// <returns>动态列表</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<GetPostsResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<GetPostsResponse>>> GetPosts([FromQuery] GetPostsRequest request)
    {
        var userId = GetUserIdFromClaims();
        _logger.LogInformation("用户 {UserId} 获取动态列表，页码: {Page}, 每页数: {PageSize}, 类型: {FeedType}",
            userId, request.Page, request.PageSize, request.FeedType);

        var result = await _postService.GetPostsAsync(request, userId);
        return Ok(result);
    }

    /// <summary>
    /// 获取动态详情
    /// </summary>
    /// <param name="id">动态ID</param>
    /// <returns>动态详情</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<GetPostDetailResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status410Gone)]
    public async Task<ActionResult<ApiResponse<GetPostDetailResponse>>> GetPostDetail(long id)
    {
        var userId = GetUserIdFromClaims();
        _logger.LogInformation("用户 {UserId} 获取动态详情，动态ID: {PostId}", userId, id);

        var result = await _postService.GetPostDetailAsync(id, userId);

        // 处理特定的错误码映射
        if (result.Code == 4001)
        {
            return NotFound(result);
        }
        else if (result.Code == 4002)
        {
            return StatusCode(StatusCodes.Status410Gone, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// 点赞动态
    /// </summary>
    /// <param name="id">动态ID</param>
    /// <param name="request">点赞请求</param>
    /// <returns>点赞结果</returns>
    [HttpPost("{id}/like")]
    [ProducesResponseType(typeof(ApiResponse<LikePostResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status409Conflict)]
    public async Task<ActionResult<ApiResponse<LikePostResponse>>> LikePost(long id, [FromBody] LikePostRequest request)
    {
        var userId = GetUserIdFromClaims();
        _logger.LogInformation("用户 {UserId} {Action} 动态 {PostId}", userId, request.Action, id);

        var result = await _postService.LikePostAsync(id, request, userId);

        // 处理重复点赞错误
        if (result.Code == 4003)
        {
            return StatusCode(StatusCodes.Status409Conflict, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// 收藏动态
    /// </summary>
    /// <param name="id">动态ID</param>
    /// <param name="request">收藏请求</param>
    /// <returns>收藏结果</returns>
    [HttpPost("{id}/collect")]
    [ProducesResponseType(typeof(ApiResponse<CollectPostResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<CollectPostResponse>>> CollectPost(long id, [FromBody] CollectPostRequest request)
    {
        var userId = GetUserIdFromClaims();
        _logger.LogInformation("用户 {UserId} {Action} 收藏动态 {PostId}", userId, request.Action, id);

        var result = await _postService.CollectPostAsync(id, request, userId);
        return Ok(result);
    }

    /// <summary>
    /// 评论动态
    /// </summary>
    /// <param name="id">动态ID</param>
    /// <param name="request">评论请求</param>
    /// <returns>评论结果</returns>
    [HttpPost("{id}/comments")]
    [ProducesResponseType(typeof(ApiResponse<CommentPostResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status429TooManyRequests)]
    public async Task<ActionResult<ApiResponse<CommentPostResponse>>> CommentPost(long id, [FromBody] CommentPostRequest request)
    {
        var userId = GetUserIdFromClaims();
        _logger.LogInformation("用户 {UserId} 评论动态 {PostId}", userId, id);

        var result = await _postService.CommentPostAsync(id, request, userId);

        // 处理评论频率限制
        if (result.Code == 429)
        {
            return StatusCode(StatusCodes.Status429TooManyRequests, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// 获取我的发布
    /// </summary>
    /// <param name="request">获取我的发布请求</param>
    /// <returns>我的发布列表</returns>
    [HttpGet("my")]
    [ProducesResponseType(typeof(ApiResponse<GetMyPostsResponse>), StatusCodes.Status200OK)]
    [Authorize(Roles = "User")] // 只有普通用户可以访问
    public async Task<ActionResult<ApiResponse<GetMyPostsResponse>>> GetMyPosts([FromQuery] GetMyPostsRequest request)
    {
        var userId = GetUserIdFromClaims();
        _logger.LogInformation("用户 {UserId} 获取我的发布，页码: {Page}, 每页数: {PageSize}", userId, request.Page, request.PageSize);

        var result = await _postService.GetMyPostsAsync(request, userId);
        return Ok(result);
    }

    /// <summary>
    /// 删除动态
    /// </summary>
    /// <param name="id">动态ID</param>
    /// <returns>删除结果</returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse>> DeletePost(long id)
    {
        var userId = GetUserIdFromClaims();
        _logger.LogInformation("用户 {UserId} 删除动态 {PostId}", userId, id);

        var result = await _postService.DeletePostAsync(id, userId);

        // 处理权限错误
        if (result.Code == 403)
        {
            return StatusCode(StatusCodes.Status403Forbidden, result);
        }

        return Ok(result);
    }

    /// <summary>
    /// 保存草稿
    /// </summary>
    /// <param name="request">保存草稿请求</param>
    /// <returns>保存结果</returns>
    [HttpPost("draft")]
    [ProducesResponseType(typeof(ApiResponse<CreateDraftResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<CreateDraftResponse>>> SaveDraft([FromBody] CreateDraftRequest request)
    {
        var userId = GetUserIdFromClaims();
        _logger.LogInformation("用户 {UserId} 保存草稿", userId);

        var result = await _postService.SaveDraftAsync(request, userId);
        return Ok(result);
    }

    /// <summary>
    /// 获取草稿列表
    /// </summary>
    /// <returns>草稿列表</returns>
    [HttpGet("drafts")]
    [ProducesResponseType(typeof(ApiResponse<GetDraftsResponse>), StatusCodes.Status200OK)]
    public async Task<ActionResult<ApiResponse<GetDraftsResponse>>> GetDrafts()
    {
        var userId = GetUserIdFromClaims();
        _logger.LogInformation("用户 {UserId} 获取草稿列表", userId);

        var result = await _postService.GetDraftsAsync(userId);
        return Ok(result);
    }

    /// <summary>
    /// 从JWT Token中获取用户ID
    /// </summary>
    /// <returns>用户ID</returns>
    private int GetUserIdFromClaims()
    {
        // TODO: 从JWT Token的Claim中获取用户ID
        // 这里返回1作为示例，实际应该从HttpContext.User中解析
        return 1;
    }
}