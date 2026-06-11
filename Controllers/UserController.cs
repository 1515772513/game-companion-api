using System.Security.Claims;
using System.Text.Json;
using GameCompanion.Api.DTOs.User;
using GameCompanion.Api.Helpers;
using GameCompanion.Api.Models;
using GameCompanion.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameCompanion.Api.Controllers;

/// <summary>
/// 用户控制器
/// </summary>
[ApiController]
[Route("api/user")]
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
    /// 获取用户个人信息
    /// </summary>
    [HttpGet("info")]
    [ProducesResponseType(typeof(ApiResponse<UserProfileDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<>), 404)]
    [ProducesResponseType(typeof(ApiResponse<>), 500)]
    public async Task<IActionResult> GetProfile()
    {
        var openId = GetOpenId();
        _logger.LogInformation($"openId: {openId}");
        if (string.IsNullOrEmpty(openId))
        {
            return ApiResponse<UserProfileDto>.Fail(401, "用户未授权").ToActionResult();
        }

        var result = await _userService.GetProfileAsync(openId);
        return result.ToActionResult();
    }

    /// <summary>
    /// 更新用户个人资料
    /// </summary>
    [HttpPut("info")]
    [ProducesResponseType(typeof(ApiResponse<UserProfileDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<>), 404)]
    [ProducesResponseType(typeof(ApiResponse<>), 400)]
    [ProducesResponseType(typeof(ApiResponse<>), 500)]
    public async Task<IActionResult> UpdateProfile([FromBody] UpdateProfileDto updateDto)
    {
        var userId = GetUserId();
        if (userId == 0)
        {
            return ApiResponse<UserProfileDto>.Fail(401, "用户未授权").ToActionResult();
        }

        var result = await _userService.UpdateProfileAsync(userId, updateDto);
        return result.ToActionResult();
    }

    /// <summary>
    /// 上传头像
    /// </summary>
    [HttpPost("upload-avatar")]
    [ProducesResponseType(typeof(ApiResponse<string>), 200)]
    [ProducesResponseType(typeof(ApiResponse<>), 404)]
    [ProducesResponseType(typeof(ApiResponse<>), 400)]
    [ProducesResponseType(typeof(ApiResponse<>), 500)]
    public async Task<IActionResult> UploadAvatar([FromBody] UploadAvatarDto uploadDto)
    {
        var userId = GetUserId();
        if (userId == 0)
        {
            return ApiResponse<string>.Fail(401, "用户未授权").ToActionResult();
        }

        if (string.IsNullOrEmpty(uploadDto.AvatarUrl))
        {
            return ApiResponse<string>.Fail(400, "头像URL不能为空").ToActionResult();
        }

        var result = await _userService.UploadAvatarAsync(userId, uploadDto.AvatarUrl);
        return result.ToActionResult();
    }

    /// <summary>
    /// 实名认证
    /// </summary>
    [HttpPost("verify-real-name")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(typeof(ApiResponse<>), 404)]
    [ProducesResponseType(typeof(ApiResponse<>), 400)]
    [ProducesResponseType(typeof(ApiResponse<>), 500)]
    public async Task<IActionResult> VerifyRealName([FromBody] VerifyRealNameDto verifyDto)
    {
        var userId = GetUserId();
        if (userId == 0)
        {
            return ApiResponse<bool>.Fail(401, "用户未授权").ToActionResult();
        }

        var result = await _userService.VerifyRealNameAsync(userId, verifyDto);
        return result.ToActionResult();
    }

    /// <summary>
    /// 获取我的收藏
    /// </summary>
    [HttpGet("collections")]
    [ProducesResponseType(typeof(ApiResponse<CollectionsResponseDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<>), 404)]
    [ProducesResponseType(typeof(ApiResponse<>), 500)]
    public async Task<IActionResult> GetCollections([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userId = GetUserId();
        if (userId == 0)
        {
            return ApiResponse<CollectionsResponseDto>.Fail(401, "用户未授权").ToActionResult();
        }

        var result = await _userService.GetCollectionsAsync(userId, page, pageSize);
        return result.ToActionResult();
    }

    /// <summary>
    /// 获取关注列表
    /// </summary>
    [HttpGet("following")]
    [ProducesResponseType(typeof(ApiResponse<FollowingListResponseDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<>), 404)]
    [ProducesResponseType(typeof(ApiResponse<>), 500)]
    public async Task<IActionResult> GetFollowing([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userId = GetUserId();
        if (userId == 0)
        {
            return ApiResponse<FollowingListResponseDto>.Fail(401, "用户未授权").ToActionResult();
        }

        var result = await _userService.GetFollowingAsync(userId, page, pageSize);
        return result.ToActionResult();
    }

    /// <summary>
    /// 获取粉丝列表
    /// </summary>
    [HttpGet("followers")]
    [ProducesResponseType(typeof(ApiResponse<FollowersListResponseDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<>), 404)]
    [ProducesResponseType(typeof(ApiResponse<>), 500)]
    public async Task<IActionResult> GetFollowers([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        var userId = GetUserId();
        if (userId == 0)
        {
            return ApiResponse<FollowersListResponseDto>.Fail(401, "用户未授权").ToActionResult();
        }

        var result = await _userService.GetFollowersAsync(userId, page, pageSize);
        return result.ToActionResult();
    }

    /// <summary>
    /// 关注/取消关注用户
    /// </summary>
    [HttpPost("follow")]
    [ProducesResponseType(typeof(ApiResponse<bool>), 200)]
    [ProducesResponseType(typeof(ApiResponse<>), 404)]
    [ProducesResponseType(typeof(ApiResponse<>), 400)]
    [ProducesResponseType(typeof(ApiResponse<>), 500)]
    public async Task<IActionResult> FollowUser([FromBody] FollowUserDto followDto)
    {
        var userId = GetUserId();
        if (userId == 0)
        {
            return ApiResponse<bool>.Fail(401, "用户未授权").ToActionResult();
        }

        var result = await _userService.FollowUserAsync(userId, followDto);
        return result.ToActionResult();
    }

    /// <summary>
    /// 申请成为陪玩师
    /// </summary>
    [HttpPost("apply-companion")]
    [ProducesResponseType(typeof(ApiResponse<GameCompanion.Api.Models.Entities.CompanionApplication>), 200)]
    [ProducesResponseType(typeof(ApiResponse<>), 404)]
    [ProducesResponseType(typeof(ApiResponse<>), 400)]
    [ProducesResponseType(typeof(ApiResponse<>), 500)]
    public async Task<IActionResult> ApplyCompanion([FromBody] ApplyCompanionDto applyDto)
    {
        var userId = GetUserId();
        if (userId == 0)
        {
            return ApiResponse<GameCompanion.Api.Models.Entities.CompanionApplication>.Fail(401, "用户未授权").ToActionResult();
        }

        var result = await _userService.ApplyCompanionAsync(userId, applyDto);
        return result.ToActionResult();
    }

    /// <summary>
    /// 获取钱包信息
    /// </summary>
    [HttpGet("wallet")]
    [ProducesResponseType(typeof(ApiResponse<WalletDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<>), 404)]
    [ProducesResponseType(typeof(ApiResponse<>), 500)]
    public async Task<IActionResult> GetWallet()
    {
        var userId = GetUserId();
        if (userId == 0)
        {
            return ApiResponse<WalletDto>.Fail(401, "用户未授权").ToActionResult();
        }

        var result = await _userService.GetWalletAsync(userId);
        return result.ToActionResult();
    }


    #region 客户端 mobile
    
    /// <summary>
    /// 添加收藏
    /// </summary>
    [HttpPost("favorite")]
    public async Task<ActionResult<ApiResponse<FavoriteResultDto>>> AddFavorite([FromBody] AddFavoriteDto dto)
    {
        var userId = GetUserId();
        var result = await _userService.AddFavoriteAsync(userId, dto);
        return Ok(result);
    }

    /// <summary>
    /// 取消收藏
    /// </summary>
    [HttpPost("favorite-remove")]
    public async Task<ActionResult<ApiResponse<FavoriteResultDto>>> RemoveFavorite([FromBody] RemoveFavoriteDto dto)
    {
        var userId = GetUserId();
        var result = await _userService.RemoveFavoriteAsync(userId, dto);
        return Ok(result);
    }

    #endregion

    #region 私有方法
    /// <summary>
    /// 从Token获取当前登录用户ID
    /// </summary>
    private int GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
        if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int userId))
            return userId;
        return 0;
    }

    /// <summary>
    /// 从Token获取当前登录用户OpenID
    /// </summary>
    private string GetOpenId()
    {
        var claim = User.FindFirst("Openid");
        return claim != null ? claim.Value : string.Empty;
    }
    #endregion
}