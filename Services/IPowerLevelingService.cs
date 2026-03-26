using GameCompanion.Api.DTOs.PowerLeveling;
using GameCompanion.Api.Models;

namespace GameCompanion.Api.Services;

/// <summary>
/// 代练服务接口
/// </summary>
public interface IPowerLevelingService
{
    /// <summary>
    /// 获取代练服务列表
    /// </summary>
    Task<ApiResponse<PowerLevelingServicesResponse>> GetServicesAsync(int? gameId = null);

    /// <summary>
    /// 获取代练服务详情
    /// </summary>
    Task<ApiResponse<PowerLevelingServiceInfo>> GetServiceDetailAsync(int serviceId);

    /// <summary>
    /// 创建代练订单
    /// </summary>
    Task<ApiResponse<object>> CreateOrderAsync(CreatePowerLevelingOrderRequest request);

    /// <summary>
    /// 获取代练订单列表
    /// </summary>
    Task<ApiResponse<PowerLevelingOrdersResponse>> GetOrdersAsync(int? page = 1, int? pageSize = 20, int? status = null);

    /// <summary>
    /// 获取代练订单详情
    /// </summary>
    Task<ApiResponse<PowerLevelingOrderDetail>> GetOrderDetailAsync(int orderId);

    /// <summary>
    /// 取消代练订单
    /// </summary>
    Task<ApiResponse<object>> CancelOrderAsync(int orderId, CancelPowerLevelingOrderRequest request);

    /// <summary>
    /// 申请代练退款
    /// </summary>
    Task<ApiResponse<object>> ApplyRefundAsync(int orderId, RefundPowerLevelingOrderRequest request);

    /// <summary>
    /// 代练订单评价
    /// </summary>
    Task<ApiResponse<object>> ReviewOrderAsync(int orderId, ReviewPowerLevelingOrderRequest request);
}