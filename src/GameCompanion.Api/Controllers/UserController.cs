using GameCompanion.Api.DTOs;
using GameCompanion.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameCompanion.Api.Controllers;

/// <summary>
/// 个人中心控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UserController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UserController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// 获取个人信息
    /// </summary>
    [HttpGet("profile")]
    public async Task<ActionResult<ApiResponse<object>>> GetProfile()
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _userService.GetUserProfileAsync(userId);
        if (result == null)
        {
            return NotFound(ApiResponse<object>.Fail(1001, "用户不存在", "User not found"));
        }
        return Ok(ApiResponse<object>.Success(result));
    }

    /// <summary>
    /// 更新个人资料
    /// </summary>
    [HttpPut("profile")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateProfile([FromBody] UpdateProfileRequest request)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _userService.UpdateProfileAsync(userId, request);
        if (!result.Success)
        {
            return BadRequest(ApiResponse<object>.Fail(result.ErrorCode ?? 400, result.Message ?? ""));
        }
        return Ok(ApiResponse<object>.Success(result.Data, result.Message));
    }

    /// <summary>
    /// 获取我的收藏
    /// </summary>
    [HttpGet("collections")]
    public async Task<ActionResult<ApiResponse<object>>> GetCollections(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _userService.GetCollectionsAsync(userId, page, pageSize);
        return Ok(ApiResponse<object>.Success(result));
    }

    /// <summary>
    /// 获取关注列表
    /// </summary>
    [HttpGet("following")]
    public async Task<ActionResult<ApiResponse<object>>> GetFollowing(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _userService.GetFollowingAsync(userId, page, pageSize);
        return Ok(ApiResponse<object>.Success(result));
    }

    /// <summary>
    /// 获取粉丝列表
    /// </summary>
    [HttpGet("followers")]
    public async Task<ActionResult<ApiResponse<object>>> GetFollowers(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _userService.GetFollowersAsync(userId, page, pageSize);
        return Ok(ApiResponse<object>.Success(result));
    }

    /// <summary>
    /// 关注/取消关注用户
    /// </summary>
    [HttpPost("follow")]
    public async Task<ActionResult<ApiResponse<object>>> FollowUser([FromBody] FollowUserRequest request)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _userService.FollowUserAsync(userId, request.TargetUserId, request.Action);
        if (!result.Success)
        {
            return BadRequest(ApiResponse<object>.Fail(result.ErrorCode ?? 400, result.Message ?? ""));
        }
        return Ok(ApiResponse<object>.Success(result.Data, result.Message));
    }

    /// <summary>
    /// 获取钱包信息
    /// </summary>
    [HttpGet("wallet")]
    public async Task<ActionResult<ApiResponse<object>>> GetWallet(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _userService.GetWalletAsync(userId, page, pageSize);
        return Ok(ApiResponse<object>.Success(result));
    }

    /// <summary>
    /// 实名认证
    /// </summary>
    [HttpPost("verify-real-name")]
    public async Task<ActionResult<ApiResponse<object>>> VerifyRealName([FromBody] VerifyRealNameRequest request)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _userService.VerifyRealNameAsync(userId, request);
        if (!result.Success)
        {
            return BadRequest(ApiResponse<object>.Fail(result.ErrorCode ?? 400, result.Message ?? ""));
        }
        return Ok(ApiResponse<object>.Success(result.Data, result.Message));
    }

    /// <summary>
    /// 申请成为陪玩师
    /// </summary>
    [HttpPost("apply-companion")]
    public async Task<ActionResult<ApiResponse<object>>> ApplyCompanion([FromBody] ApplyCompanionRequest request)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _userService.ApplyCompanionAsync(userId, request);
        if (!result.Success)
        {
            return BadRequest(ApiResponse<object>.Fail(result.ErrorCode ?? 400, result.Message ?? ""));
        }
        return Ok(ApiResponse<object>.Success(result.Data, result.Message));
    }
}
