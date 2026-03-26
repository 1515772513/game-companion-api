using GameCompanion.Api.DTOs;
using GameCompanion.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameCompanion.Api.Controllers;

/// <summary>
/// 订单管理控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
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
    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> CreateOrder([FromBody] CreateOrderRequest request)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _orderService.CreateOrderAsync(userId, request);
        if (!result.Success)
        {
            return BadRequest(ApiResponse<object>.Fail(result.ErrorCode ?? 400, result.Message ?? ""));
        }
        return Ok(ApiResponse<object>.Success(result.Data, result.Message));
    }

    /// <summary>
    /// 获取订单列表
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetOrders(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] int? status = null,
        [FromQuery] string sortBy = "created_at",
        [FromQuery] string sortOrder = "desc")
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _orderService.GetOrdersAsync(userId, page, pageSize, status, sortBy, sortOrder);
        return Ok(ApiResponse<object>.Success(result));
    }

    /// <summary>
    /// 获取订单详情
    /// </summary>
    [HttpGet("{orderId}")]
    public async Task<ActionResult<ApiResponse<object>>> GetOrderDetail(int orderId)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _orderService.GetOrderDetailAsync(orderId, userId);
        if (result == null)
        {
            return NotFound(ApiResponse<object>.Fail(3001, "订单不存在", "Order not found"));
        }
        return Ok(ApiResponse<object>.Success(result));
    }

    /// <summary>
    /// 取消订单
    /// </summary>
    [HttpPost("{orderId}/cancel")]
    public async Task<ActionResult<ApiResponse<object>>> CancelOrder(int orderId, [FromBody] CancelOrderRequest request)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _orderService.CancelOrderAsync(orderId, userId, request.CancelReason);
        if (!result.Success)
        {
            return BadRequest(ApiResponse<object>.Fail(result.ErrorCode ?? 400, result.Message ?? ""));
        }
        return Ok(ApiResponse<object>.Success(result.Data, result.Message));
    }

    /// <summary>
    /// 申请退款
    /// </summary>
    [HttpPost("{orderId}/refund")]
    public async Task<ActionResult<ApiResponse<object>>> RequestRefund(int orderId, [FromBody] RefundRequest request)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _orderService.RequestRefundAsync(orderId, userId, request);
        if (!result.Success)
        {
            return BadRequest(ApiResponse<object>.Fail(result.ErrorCode ?? 400, result.Message ?? ""));
        }
        return Ok(ApiResponse<object>.Success(result.Data, result.Message));
    }

    /// <summary>
    /// 确认完成
    /// </summary>
    [HttpPost("{orderId}/confirm")]
    public async Task<ActionResult<ApiResponse<object>>> ConfirmOrder(int orderId)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _orderService.ConfirmOrderAsync(orderId, userId);
        if (!result.Success)
        {
            return BadRequest(ApiResponse<object>.Fail(result.ErrorCode ?? 400, result.Message ?? ""));
        }
        return Ok(ApiResponse<object>.Success(result.Data, result.Message));
    }

    /// <summary>
    /// 订单评价
    /// </summary>
    [HttpPost("{orderId}/review")]
    public async Task<ActionResult<ApiResponse<object>>> ReviewOrder(int orderId, [FromBody] ReviewOrderRequest request)
    {
        var userId = int.Parse(User.FindFirst("user_id")?.Value ?? "0");
        var result = await _orderService.ReviewOrderAsync(orderId, userId, request);
        if (!result.Success)
        {
            return BadRequest(ApiResponse<object>.Fail(result.ErrorCode ?? 400, result.Message ?? ""));
        }
        return Ok(ApiResponse<object>.Success(result.Data, result.Message));
    }
}
