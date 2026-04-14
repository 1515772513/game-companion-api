using GameCompanion.Api.DTOs.Home;
using GameCompanion.Api.Models;
using GameCompanion.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameCompanion.Api.Controllers;

/// <summary>
/// 首页服务接口
/// </summary>
[ApiController]
[Route("api/home")]
public class HomeController : ControllerBase
{
    private readonly IHomeService _homeService;

    public HomeController(IHomeService homeService)
    {
        _homeService = homeService;
    }

    /// <summary>
    /// 获取首页数据
    /// </summary>
    /// <returns>首页数据</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<HomeDataResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 503)]
    public async Task<IActionResult> GetHomeData()
    {
        var response = await _homeService.GetHomeDataAsync();
        return response.Code == 200 ? Ok(response) : StatusCode(response.Code, response);
    }

    /// <summary>
    /// 获取系统配置
    /// </summary>
    /// <returns>系统配置</returns>
    [HttpGet("system_config")]
    [ProducesResponseType(typeof(ApiResponse<object>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 503)]
    public async Task<IActionResult> GetSystemConfig()
    {
        var response = await _homeService.GetSystemConfigAsync();
        return response.Code == 200 ? Ok(response) : StatusCode(response.Code, response);
    }

    /// <summary>
    /// 获取陪玩师列表
    /// </summary>
    /// <param name="page">页码</param>
    /// <param name="page_size">每页数量</param>
    /// <param name="game_id">游戏ID</param>
    /// <param name="service_type">服务类型</param>
    /// <param name="level">等级</param>
    /// <param name="min_price">最低价格</param>
    /// <param name="max_price">最高价格</param>
    /// <param name="online_status">在线状态</param>
    /// <param name="keyword">搜索关键词</param>
    /// <param name="sort_by">排序字段</param>
    /// <param name="sort_order">排序方向</param>
    /// <returns>陪玩师列表</returns>
    [HttpGet("companions")]
    [ProducesResponseType(typeof(ApiResponse<CompanionListResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    public async Task<IActionResult> GetCompanions(
        [FromQuery] int page = 1,
        [FromQuery] int page_size = 20,
        [FromQuery] int? game_id = null,
        [FromQuery] string? service_type = null,
        [FromQuery] int? level = null,
        [FromQuery] decimal? min_price = null,
        [FromQuery] decimal? max_price = null,
        [FromQuery] int? online_status = null,
        [FromQuery] string? keyword = null,
        [FromQuery] string sort_by = "rating",
        [FromQuery] string sort_order = "desc")
    {
        // 验证参数
        if (page_size < 1 || page_size > 50)
        {
            var errorResponse = ApiResponse.Fail(400, "page_size参数超出范围，最大允许50", "请求参数错误");
            return BadRequest(errorResponse);
        }

        var request = new CompanionListRequest
        {
            Page = page,
            PageSize = page_size,
            GameId = game_id,
            ServiceType = service_type,
            Level = level,
            MinPrice = min_price,
            MaxPrice = max_price,
            OnlineStatus = online_status,
            Keyword = keyword,
            SortBy = sort_by,
            SortOrder = sort_order
        };

        var response = await _homeService.GetCompanionsAsync(request);
        return response.Code == 200 ? Ok(response) : StatusCode(response.Code, response);
    }

    /// <summary>
    /// 获取陪玩师详情
    /// </summary>
    /// <param name="id">陪玩师ID</param>
    /// <returns>陪玩师详情</returns>
    [HttpGet("companions/{id}")]
    [ProducesResponseType(typeof(ApiResponse<CompanionDetailResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 404)]
    [ProducesResponseType(typeof(ApiResponse), 403)]
    public async Task<IActionResult> GetCompanionDetail(int id)
    {
        var response = await _homeService.GetCompanionDetailAsync(id);
        return response.Code == 200 ? Ok(response) : StatusCode(response.Code, response);
    }

    /// <summary>
    /// 获取游戏列表
    /// </summary>
    /// <returns>游戏列表</returns>
    [HttpGet("games")]
    [ProducesResponseType(typeof(ApiResponse<GameListResponse>), 200)]
    public async Task<IActionResult> GetGames()
    {
        var response = await _homeService.GetGamesAsync();
        return Ok(response);
    }

    /// <summary>
    /// 搜索陪玩师
    /// </summary>
    /// <param name="keyword">搜索关键词</param>
    /// <param name="page">页码</param>
    /// <param name="page_size">每页数量</param>
    /// <returns>搜索结果</returns>
    [HttpGet("search/companions")]
    [ProducesResponseType(typeof(ApiResponse<SearchCompanionsResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse), 400)]
    public async Task<IActionResult> SearchCompanions(
        [FromQuery] string keyword,
        [FromQuery] int page = 1,
        [FromQuery] int page_size = 20)
    {
        // 验证参数
        if (string.IsNullOrWhiteSpace(keyword) || keyword.Length < 2)
        {
            var errorResponse = ApiResponse.Fail(400, "搜索关键词至少需要2个字符", "请求参数错误");
            return BadRequest(errorResponse);
        }

        if (page_size < 1 || page_size > 50)
        {
            var errorResponse = ApiResponse.Fail(400, "page_size参数超出范围，最大允许50", "请求参数错误");
            return BadRequest(errorResponse);
        }

        var request = new SearchCompanionsRequest
        {
            Keyword = keyword,
            Page = page,
            PageSize = page_size
        };

        var response = await _homeService.SearchCompanionsAsync(request);
        return response.Code == 200 ? Ok(response) : StatusCode(response.Code, response);
    }

    /// <summary>
    /// 获取游戏圈子列表
    /// </summary>
    /// <param name="game_id">游戏ID</param>
    /// <param name="page">页码</param>
    /// <param name="page_size">每页数量</param>
    /// <returns>圈子列表</returns>
    [HttpGet("circles")]
    [ProducesResponseType(typeof(ApiResponse<CirclesListResponse>), 200)]
    public async Task<IActionResult> GetCircles(
        [FromQuery] int? game_id = null,
        [FromQuery] int page = 1,
        [FromQuery] int page_size = 20)
    {
        // 验证参数
        if (page_size < 1 || page_size > 50)
        {
            var errorResponse = ApiResponse.Fail(400, "page_size参数超出范围，最大允许50", "请求参数错误");
            return BadRequest(errorResponse);
        }

        var request = new CirclesListRequest
        {
            GameId = game_id,
            Page = page,
            PageSize = page_size
        };

        var response = await _homeService.GetCirclesAsync(request);
        return response.Code == 200 ? Ok(response) : StatusCode(response.Code, response);
    }
}
