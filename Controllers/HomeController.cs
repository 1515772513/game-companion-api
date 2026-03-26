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
        [FromQuery] string? level = null,
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
            var errorResponse = ApiResponse.Error(400, "page_size参数超出范围，最大允许50", "请求参数错误");
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
            var errorResponse = ApiResponse.Error(400, "搜索关键词至少需要2个字符", "请求参数错误");
            return BadRequest(errorResponse);
        }

        if (page_size < 1 || page_size > 50)
        {
            var errorResponse = ApiResponse.Error(400, "page_size参数超出范围，最大允许50", "请求参数错误");
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
            var errorResponse = ApiResponse.Error(400, "page_size参数超出范围，最大允许50", "请求参数错误");
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

/// <summary>
/// 订单管理接口
/// </summary>
[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    // TODO: 实现订单相关接口
    // 参考接口文档：
    // - POST /api/orders - 创建订单
    // - GET /api/orders - 获取订单列表
    // - GET /api/orders/{id} - 获取订单详情
    // - POST /api/orders/{id}/cancel - 取消订单
    // - POST /api/orders/{id}/refund - 申请退款
    // - POST /api/orders/{id}/confirm - 确认完成
    // - POST /api/orders/{id}/review - 订单评价
}

/// <summary>
/// 动态社区接口
/// </summary>
[ApiController]
[Route("api/posts")]
public class PostsController : ControllerBase
{
    // TODO: 实现动态相关接口
    // 参考接口文档：
    // - POST /api/posts - 发布动态
    // - GET /api/posts - 获取动态列表
    // - GET /api/posts/{id} - 获取动态详情
    // - POST /api/posts/{id}/like - 点赞动态
    // - POST /api/posts/{id}/collect - 收藏动态
    // - POST /api/posts/{id}/comments - 评论动态
    // - GET /api/posts/my - 获取我的发布
    // - DELETE /api/posts/{id} - 删除动态
    // - POST /api/posts/draft - 保存草稿
    // - GET /api/posts/drafts - 获取草稿列表
}

/// <summary>
/// 消息聊天接口
/// </summary>
[ApiController]
[Route("api/conversations")]
public class ConversationsController : ControllerBase
{
    // TODO: 实现消息相关接口
    // 参考接口文档：
    // - GET /api/conversations - 获取会话列表
    // - GET /api/conversations/{id}/messages - 获取聊天详情
    // - POST /api/conversations/{id}/messages - 发送消息
    // - POST /api/conversations/upload-image - 上传聊天图片
}

/// <summary>
/// 系统通知接口
/// </summary>
[ApiController]
[Route("api/notifications")]
public class NotificationsController : ControllerBase
{
    // TODO: 实现通知相关接口
    // 参考接口文档：
    // - GET /api/notifications - 获取官方通知
    // - POST /api/notifications/{id}/read - 标记通知已读
}

/// <summary>
/// 个人中心接口
/// </summary>
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    // TODO: 实现个人中心相关接口
    // 参考接口文档：
    // - GET /api/user/profile - 获取个人信息
    // - PUT /api/user/profile - 更新个人资料
    // - POST /api/user/upload-avatar - 上传头像
    // - POST /api/user/verify-real-name - 实名认证
    // - GET /api/user/collections - 获取我的收藏
    // - GET /api/user/following - 获取关注列表
    // - GET /api/user/followers - 获取粉丝列表
    // - POST /api/user/follow - 关注/取消关注用户
    // - POST /api/user/apply-companion - 申请成为陪玩师
    // - GET /api/user/wallet - 获取钱包信息
}

/// <summary>
/// 设置相关接口
/// </summary>
[ApiController]
[Route("api/settings")]
public class SettingsController : ControllerBase
{
    // TODO: 实现设置相关接口
    // 参考接口文档：
    // - GET /api/settings/account - 获取账号设置
    // - POST /api/settings/change-phone - 修改手机号
    // - POST /api/settings/change-password - 修改密码
    // - PUT /api/settings/privacy - 更新隐私设置
    // - PUT /api/settings/notification - 更新通知设置
}

/// <summary>
/// 意见反馈接口
/// </summary>
[ApiController]
[Route("api/feedback")]
public class FeedbackController : ControllerBase
{
    // TODO: 实现反馈相关接口
}

/// <summary>
/// 陪玩师认证接口
/// </summary>
[ApiController]
[Route("api/companion")]
public class CompanionController : ControllerBase
{
    // TODO: 实现陪玩师相关接口
    // 参考接口文档中的陪玩师认证部分
}

/// <summary>
/// 代练服务接口
/// </summary>
[ApiController]
[Route("api/power-leveling")]
public class PowerLevelingController : ControllerBase
{
    // TODO: 实现代练服务相关接口
    // 参考接口文档中的代练服务部分
}
