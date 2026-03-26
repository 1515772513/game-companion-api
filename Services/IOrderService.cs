using GameCompanion.Api.DTOs.Order;
using GameCompanion.Api.Models;

namespace GameCompanion.Api.Services;

/// <summary>
/// 订单服务接口
/// </summary>
public interface IOrderService
{
    /// <summary>
    /// 创建订单
    /// </summary>
    Task<ApiResponse<CreateOrderResponse>> CreateOrderAsync(CreateOrderRequest request);

    /// <summary>
    /// 获取订单列表
    /// </summary>
    Task<ApiResponse<GetOrdersResponse>> GetOrdersAsync(GetOrdersRequest request);

    /// <summary>
    /// 获取订单详情
    /// </summary>
    Task<ApiResponse<GetOrderResponse>> GetOrderAsync(int orderId);

    /// <summary>
    /// 取消订单
    /// </summary>
    Task<ApiResponse<CancelOrderResponse>> CancelOrderAsync(int orderId, CancelOrderRequest request);

    /// <summary>
    /// 申请退款
    /// </summary>
    Task<ApiResponse<RefundOrderResponse>> RefundOrderAsync(int orderId, RefundOrderRequest request);

    /// <summary>
    /// 确认订单完成
    /// </summary>
    Task<ApiResponse<ConfirmOrderResponse>> ConfirmOrderAsync(int orderId);

    /// <summary>
    /// 订单评价
    /// </summary>
    Task<ApiResponse<CreateOrderReviewResponse>> ReviewOrderAsync(int orderId, CreateOrderReviewRequest request);
}