using GameCompanion.Api.DTOs.Companion;
using GameCompanion.Api.Models;

namespace GameCompanion.Api.Services;

/// <summary>
/// 陪玩师服务接口
/// </summary>
public interface ICompanionService
{
    /// <summary>
    /// 申请成为陪玩师
    /// </summary>
    Task<ApiResponse<ApplicationStatusResponse>> ApplyCompanionAsync(ApplyCompanionRequest request);

    /// <summary>
    /// 获取认证申请状态
    /// </summary>
    Task<ApiResponse<ApplicationStatusResponse>> GetApplicationStatusAsync();

    /// <summary>
    /// 获取我的陪玩师信息
    /// </summary>
    Task<ApiResponse<CompanionInfoResponse>> GetMyCompanionInfoAsync();

    /// <summary>
    /// 更新陪玩师信息
    /// </summary>
    Task<ApiResponse<object>> UpdateCompanionInfoAsync(UpdateCompanionInfoRequest request);

    /// <summary>
    /// 切换在线状态
    /// </summary>
    Task<ApiResponse<object>> UpdateOnlineStatusAsync(UpdateOnlineStatusRequest request);

    /// <summary>
    /// 获取接单列表
    /// </summary>
    Task<ApiResponse<CompanionOrdersResponse>> GetCompanionOrdersAsync(int? page = 1, int? pageSize = 20, int? status = null);

    /// <summary>
    /// 接受订单
    /// </summary>
    Task<ApiResponse<object>> AcceptOrderAsync(int orderId);

    /// <summary>
    /// 拒绝订单
    /// </summary>
    Task<ApiResponse<object>> RejectOrderAsync(int orderId, string rejectReason);

    /// <summary>
    /// 开始服务
    /// </summary>
    Task<ApiResponse<object>> StartOrderAsync(int orderId);

    /// <summary>
    /// 完成服务
    /// </summary>
    Task<ApiResponse<object>> CompleteOrderAsync(int orderId, string? serviceSummary = null);

    /// <summary>
    /// 获取收益统计
    /// </summary>
    Task<ApiResponse<EarningsResponse>> GetEarningsAsync();

    /// <summary>
    /// 申请提现
    /// </summary>
    Task<ApiResponse<object>> ApplyWithdrawAsync(WithdrawRequest request);

    /// <summary>
    /// 获取提现记录
    /// </summary>
    Task<ApiResponse<WithdrawRecordsResponse>> GetWithdrawRecordsAsync(int? page = 1, int? pageSize = 20);
}