using GameCompanion.Api.DTOs.PowerLeveling;
using GameCompanion.Api.Models;
using GameCompanion.Api.Services;
using Microsoft.AspNetCore.Mvc;
using GameCompanion.Api.Data;

namespace GameCompanion.Api.Controllers;

/// <summary>
/// 代练服务接口
/// </summary>
[ApiController]
[Route("api/power-leveling")]
[Produces("application/json")]
public class PowerLevelingController : ControllerBase
{
    private readonly IPowerLevelingService _powerLevelingService;
    private readonly ILogger<PowerLevelingController> _logger;
    private readonly ApplicationDbContext _context;

    public PowerLevelingController(IPowerLevelingService powerLevelingService, ILogger<PowerLevelingController> logger, ApplicationDbContext context)
    {
        _powerLevelingService = powerLevelingService;
        _logger = logger;
        _context = context;
    }

    /// <summary>
    /// 获取代练服务列表
    /// </summary>
    /// <param name="gameId">游戏ID</param>
    /// <returns>服务列表</returns>
    [HttpGet("services")]
    [ProducesResponseType(typeof(ApiResponse<PowerLevelingServicesResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<PowerLevelingServicesResponse>>> GetServices([FromQuery] int? gameId = null)
    {
        _logger.LogInformation("获取代练服务列表，游戏ID：{GameId}", gameId);
        var result = await _powerLevelingService.GetServicesAsync(gameId);
        return Ok(result);
    }

    /// <summary>
    /// 获取代练服务详情
    /// </summary>
    /// <param name="serviceId">服务ID</param>
    /// <returns>服务详情</returns>
    [HttpGet("services/{serviceId}")]
    [ProducesResponseType(typeof(ApiResponse<PowerLevelingServiceInfo>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<PowerLevelingServiceInfo>>> GetServiceDetail(int serviceId)
    {
        _logger.LogInformation("获取代练服务详情，服务ID：{ServiceId}", serviceId);
        var result = await _powerLevelingService.GetServiceDetailAsync(serviceId);
        return Ok(result);
    }

    /// <summary>
    /// 创建代练订单
    /// </summary>
    /// <param name="request">创建订单请求</param>
    /// <returns>订单创建结果</returns>
    [HttpPost("orders")]
    [ProducesResponseType(typeof(ApiResponse<object>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<object>>> CreateOrder([FromBody] CreatePowerLevelingOrderRequest request)
    {
        _logger.LogInformation("创建代练订单，服务ID：{ServiceId}", request.ServiceId);
        var result = await _powerLevelingService.CreateOrderAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// 获取代练订单列表
    /// </summary>
    /// <param name="page">页码</param>
    /// <param name="pageSize">每页数量</param>
    /// <param name="status">订单状态筛选</param>
    /// <returns>订单列表</returns>
    [HttpGet("orders")]
    [ProducesResponseType(typeof(ApiResponse<PowerLevelingOrdersResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<PowerLevelingOrdersResponse>>> GetOrders(
        [FromQuery] int? page = 1,
        [FromQuery] int? pageSize = 20,
        [FromQuery] int? status = null)
    {
        _logger.LogInformation("获取代练订单列表，页码：{Page}，状态：{Status}", page, status);
        var result = await _powerLevelingService.GetOrdersAsync(page, pageSize, status);
        return Ok(result);
    }

    /// <summary>
    /// 获取代练订单详情
    /// </summary>
    /// <param name="orderId">订单ID</param>
    /// <returns>订单详情</returns>
    [HttpGet("orders/{orderId}")]
    [ProducesResponseType(typeof(ApiResponse<PowerLevelingOrderDetail>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<PowerLevelingOrderDetail>>> GetOrderDetail(int orderId)
    {
        _logger.LogInformation("获取代练订单详情，订单ID：{OrderId}", orderId);
        var result = await _powerLevelingService.GetOrderDetailAsync(orderId);
        return Ok(result);
    }

    /// <summary>
    /// 取消代练订单
    /// </summary>
    /// <param name="orderId">订单ID</param>
    /// <param name="request">取消订单请求</param>
    /// <returns>取消结果</returns>
    [HttpPost("orders/{orderId}/cancel")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> CancelOrder(int orderId, [FromBody] CancelPowerLevelingOrderRequest request)
    {
        _logger.LogInformation("取消代练订单：{OrderId}，原因：{CancelReason}", orderId, request.CancelReason);
        var result = await _powerLevelingService.CancelOrderAsync(orderId, request);
        return Ok(result);
    }

    /// <summary>
    /// 申请代练退款
    /// </summary>
    /// <param name="orderId">订单ID</param>
    /// <param name="request">退款申请请求</param>
    /// <returns>退款申请结果</returns>
    [HttpPost("orders/{orderId}/refund")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> ApplyRefund(int orderId, [FromBody] RefundPowerLevelingOrderRequest request)
    {
        _logger.LogInformation("申请代练退款：{OrderId}，金额：{RefundAmount}，原因：{RefundReason}",
            orderId, request.RefundAmount, request.RefundReason);
        var result = await _powerLevelingService.ApplyRefundAsync(orderId, request);
        return Ok(result);
    }

    /// <summary>
    /// 代练订单评价
    /// </summary>
    /// <param name="orderId">订单ID</param>
    /// <param name="request">评价请求</param>
    /// <returns>评价结果</returns>
    [HttpPost("orders/{orderId}/review")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> ReviewOrder(int orderId, [FromBody] ReviewPowerLevelingOrderRequest request)
    {
        _logger.LogInformation("代练订单评价：{OrderId}，评分：{Rating}，评价：{Comment}",
            orderId, request.Rating, request.Comment);
        var result = await _powerLevelingService.ReviewOrderAsync(orderId, request);
        return Ok(result);
    }
}