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
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<UserController> _logger;

    public UsersController(IUserService userService, ILogger<UserController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    /// <summary>
    /// 获取用户列表
    /// </summary>
    [HttpPost("list")]
    [ProducesResponseType(typeof(ApiResponse<UserListListDto>), 200)]
    [ProducesResponseType(typeof(ApiResponse<>), 404)]
    [ProducesResponseType(typeof(ApiResponse<>), 500)]
    public async Task<IActionResult> GetList([FromBody] GetUserListDto? request = null)
    {
        request ??= new GetUserListDto();
        var result = await _userService.GetListAsync(request);
        return result.ToActionResult();
    }

    /// <summary>
    /// 获取用户统计卡片
    /// </summary>
    [HttpGet("stats")]
    [ProducesResponseType(typeof(ApiResponse<List<StatCardDto>>), 200)]
    [ProducesResponseType(typeof(ApiResponse<>), 404)]
    [ProducesResponseType(typeof(ApiResponse<>), 500)]
    public async Task<IActionResult> GetUserStatCards()
    {
        var result = await _userService.GetUserStatCardsAsync();
        return result.ToActionResult();
    }
}