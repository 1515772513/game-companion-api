using System.Security.Claims;
using GameCompanion.Api.Dtos;
using GameCompanion.Api.DTOs.Order;
using GameCompanion.Api.Models;
using GameCompanion.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameCompanion.Api.Controllers;

/// <summary>
/// 订单管理接口
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Produces("application/json")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;
    private readonly ILogger<OrdersController> _logger;

    public OrdersController(IOrderService orderService, ILogger<OrdersController> logger)
    {
        _orderService = orderService;
        _logger = logger;
    }

    /// <summary>
    /// 创建订单
    /// </summary>
    /// <param name="request">创建订单请求</param>
    /// <returns>创建订单结果</returns>
    [HttpPost("create")]
    [ProducesResponseType(typeof(ApiResponse<CreateOrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ApiResponse<CreateOrderResponse>>> CreateOrder([FromBody] CreateOrderRequest request)
    {
        _logger.LogInformation("用户创建订单: CompanionId={CompanionId}, GameId={GameId}", request.CompanionId, request.GameId);

        // 从Claims中获取用户ID（简化处理，实际应该从JWT token中解析）
        var userId = GetUserId();

        // 将用户ID添加到请求中
        request.GetType().GetProperty("UserId")?.SetValue(request, userId, null);

        var result = await _orderService.CreateOrderAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// 获取订单列表
    /// </summary>
    /// <param name="request">获取订单列表请求</param>
    /// <returns>订单列表</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ApiResponse<GetOrdersResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ApiResponse<GetOrdersResponse>>> GetOrders([FromQuery] GetOrdersRequest request)
    {
        _logger.LogInformation("用户获取订单列表: Page={Page}, PageSize={PageSize}, Status={Status}",
            request.Page, request.PageSize, request.Status);

        // 从Claims中获取用户ID（简化处理，实际应该从JWT token中解析）
        var userId = 1; // TODO: 从JWT token中获取用户ID

        // 将用户ID添加到请求中
        request.GetType().GetProperty("UserId")?.SetValue(request, userId, null);

        var result = await _orderService.GetOrdersAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// 获取订单详情
    /// </summary>
    /// <param name="id">订单ID</param>
    /// <returns>订单详情</returns>
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(ApiResponse<GetOrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<GetOrderResponse>>> GetOrder(int id)
    {
        _logger.LogInformation("用户获取订单详情: OrderId={OrderId}", id);

        var result = await _orderService.GetOrderAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// 取消订单
    /// </summary>
    /// <param name="id">订单ID</param>
    /// <param name="request">取消订单请求</param>
    /// <returns>取消订单结果</returns>
    [HttpPost("{id}/cancel")]
    [ProducesResponseType(typeof(ApiResponse<CancelOrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ApiResponse<CancelOrderResponse>>> CancelOrder(int id, [FromBody] CancelOrderRequest request)
    {
        _logger.LogInformation("用户取消订单: OrderId={OrderId}, Reason={Reason}", id, request.CancelReason);

        var result = await _orderService.CancelOrderAsync(id, request);
        return Ok(result);
    }

    /// <summary>
    /// 申请退款
    /// </summary>
    /// <param name="id">订单ID</param>
    /// <param name="request">申请退款请求</param>
    /// <returns>申请退款结果</returns>
    [HttpPost("{id}/refund")]
    [ProducesResponseType(typeof(ApiResponse<RefundOrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ApiResponse<RefundOrderResponse>>> RefundOrder(int id, [FromBody] RefundOrderRequest request)
    {
        _logger.LogInformation("用户申请退款: OrderId={OrderId}, Amount={Amount}", id, request.RefundAmount);

        var result = await _orderService.RefundOrderAsync(id, request);
        return Ok(result);
    }

    /// <summary>
    /// 确认订单完成
    /// </summary>
    /// <param name="id">订单ID</param>
    /// <returns>确认订单完成结果</returns>
    [HttpPost("{id}/confirm")]
    [ProducesResponseType(typeof(ApiResponse<ConfirmOrderResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ApiResponse<ConfirmOrderResponse>>> ConfirmOrder(int id)
    {
        _logger.LogInformation("用户确认订单完成: OrderId={OrderId}", id);

        var result = await _orderService.ConfirmOrderAsync(id);
        return Ok(result);
    }

    /// <summary>
    /// 订单评价
    /// </summary>
    /// <param name="id">订单ID</param>
    /// <param name="request">订单评价请求</param>
    /// <returns>订单评价结果</returns>
    [HttpPost("{id}/review")]
    [ProducesResponseType(typeof(ApiResponse<CreateOrderReviewResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status422UnprocessableEntity)]
    public async Task<ActionResult<ApiResponse<CreateOrderReviewResponse>>> ReviewOrder(int id, [FromBody] CreateOrderReviewRequest request)
    {
        _logger.LogInformation("用户订单评价: OrderId={OrderId}, Rating={Rating}", id, request.Rating);

        var result = await _orderService.ReviewOrderAsync(id, request);
        return Ok(result);
    }

    #region 客户端 mobile

    /// <summary>
    /// 获取订单统计数据
    /// </summary>
    /// <returns>订单统计结果</returns>
    [HttpGet("order-status")]
    public async Task<ApiResponse<OrderStatusDto>> GetOrderStatus()
    {
        var openId = GetOpenId();
        return await _orderService.GetOrderStatusAsync(openId);
    }

    #endregion


    #region PC端接口

    /// <summary>
    /// 订单分页列表
    /// </summary>
    /// <param name="request">订单分页列表请求</param>
    /// <returns>订单分页列表</returns>
    [HttpGet("list")]
    [ProducesResponseType(typeof(ApiResponse<GetOrdersResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status403Forbidden)]
    public async Task<ActionResult<ApiResponse<GetOrdersPaginationResponse>>> GetList([FromQuery] GetOrdersPaginationRequest request)
    {
        var result = await _orderService.GetListAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// 获取订单统计数据
    /// </summary>
    /// <returns>订单统计结果</returns>
    [HttpGet("statistics")]
    public async Task<ApiResponse<OrderStatisticsDto>> GetStatistics()
    {
        return await _orderService.GetStatisticsAsync();
    }

    #endregion


    #region 私有接口
    
    /// <summary>
    /// 从Token获取当前登录用户ID
    /// </summary>
    private int GetUserId()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier);
        return claim != null && int.TryParse(claim.Value, out int id) ? id : 0;
    }

    /// <summary>
    /// 从Token获取当前登录用户OpenID
    /// </summary>
    private string GetOpenId()
    {
        var claim = User.FindFirst("openId");
        return claim != null ? claim.Value : string.Empty;
    }

    #endregion
}