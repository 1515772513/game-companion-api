using GameCompanion.Api.DTOs;
using GameCompanion.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameCompanion.Api.Controllers;

/// <summary>
/// 首页服务控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class HomeController : ControllerBase
{
    private readonly IHomeService _homeService;
    private readonly ILogger<HomeController> _logger;

    public HomeController(IHomeService homeService, ILogger<HomeController> logger)
    {
        _homeService = homeService;
        _logger = logger;
    }

    /// <summary>
    /// 获取首页数据
    /// </summary>
    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<HomeResponse>>> GetHomeData()
    {
        try
        {
            var data = await _homeService.GetHomeDataAsync();
            return Ok(ApiResponse<HomeResponse>.Success(data));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取首页数据失败");
            return StatusCode(500, ApiResponse<HomeResponse>.Fail(9001, "系统维护中", ex.Message));
        }
    }

    /// <summary>
    /// 获取陪玩师列表
    /// </summary>
    [HttpGet("companions")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<PagedResponse<CompanionDetailInfo>>>> GetCompanions(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] int? gameId = null,
        [FromQuery] string? serviceType = null,
        [FromQuery] string? level = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        [FromQuery] int? onlineStatus = null,
        [FromQuery] string? keyword = null,
        [FromQuery] string sortBy = "rating",
        [FromQuery] string sortOrder = "desc")
    {
        try
        {
            if (pageSize < 1 || pageSize > 50)
            {
                return BadRequest(ApiResponse<PagedResponse<CompanionDetailInfo>>.Fail(400, "请求参数错误", "page_size参数超出范围，最大允许50"));
            }

            var data = await _homeService.GetCompanionsAsync(page, pageSize, gameId, serviceType, level,
                minPrice, maxPrice, onlineStatus, keyword, sortBy, sortOrder);
            return Ok(ApiResponse<PagedResponse<CompanionDetailInfo>>.Success(data));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取陪玩师列表失败");
            return StatusCode(500, ApiResponse<PagedResponse<CompanionDetailInfo>>.Fail(500, "服务器内部错误", ex.Message));
        }
    }

    /// <summary>
    /// 获取陪玩师详情
    /// </summary>
    [HttpGet("companions/{companionId}")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<CompanionDetailInfo>>> GetCompanionDetail(int companionId)
    {
        try
        {
            var data = await _homeService.GetCompanionDetailAsync(companionId);
            if (data == null)
            {
                return NotFound(ApiResponse<CompanionDetailInfo>.Fail(2001, "陪玩师不存在", "指定的陪玩师ID不存在或已被删除"));
            }
            return Ok(ApiResponse<CompanionDetailInfo>.Success(data));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取陪玩师详情失败, companionId: {CompanionId}", companionId);
            return StatusCode(500, ApiResponse<CompanionDetailInfo>.Fail(500, "服务器内部错误", ex.Message));
        }
    }

    /// <summary>
    /// 获取游戏列表
    /// </summary>
    [HttpGet("games")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<List<GameDetailInfo>>>> GetGames()
    {
        try
        {
            var data = await _homeService.GetGamesAsync();
            return Ok(ApiResponse<List<GameDetailInfo>>.Success(data));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取游戏列表失败");
            return StatusCode(500, ApiResponse<List<GameDetailInfo>>.Fail(500, "服务器内部错误", ex.Message));
        }
    }

    /// <summary>
    /// 搜索陪玩师
    /// </summary>
    [HttpGet("search/companions")]
    [AllowAnonymous]
    public async Task<ActionResult<ApiResponse<PagedResponse<CompanionSimpleInfo>>>> SearchCompanions(
        [FromQuery] string keyword = "",
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(keyword) || keyword.Length < 2)
            {
                return BadRequest(ApiResponse<PagedResponse<CompanionSimpleInfo>>.Fail(400, "请求参数错误", "搜索关键词至少需要2个字符"));
            }

            var data = await _homeService.SearchCompanionsAsync(keyword, page, pageSize);
            return Ok(ApiResponse<PagedResponse<CompanionSimpleInfo>>.Success(data));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "搜索陪玩师失败, keyword: {Keyword}", keyword);
            return StatusCode(500, ApiResponse<PagedResponse<CompanionSimpleInfo>>.Fail(500, "服务器内部错误", ex.Message));
        }
    }
}
