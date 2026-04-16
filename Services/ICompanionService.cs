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
    Task<ApiResponse<ApplicationStatusResponse>> ApplyCompanionAsync(ApplyCompanionRequest request, int userId);

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

    /// <summary>
    /// 获取陪玩师列表
    /// </summary>
    Task<ApiResponse<CompanionListResponse>> GetCompanionListAsync(CompanionListRequest request);

    /// <summary>
    /// 获取陪玩认证审核统计（tab数量）
    /// </summary>
    Task<ApiResponse<List<CompanionStatusCountDto>>> GetCompanionStatusCountAsync();
    
    /// <summary>
    /// 新增：适配前端的陪玩师列表查询
    /// </summary>
    /// <returns></returns>
    Task<ApiResponse<CompanionListFrontResponse>> GetCompanionListForFrontAsync(CompanionListFrontRequest request);
    
    /// <summary>
    /// 获取陪玩师详情（适配前端）
    /// </summary>
    Task<ApiResponse<CompanionListDetailDto>> GetCompanionDetailAsync(int companionId, int userId);

    /// <summary>
    /// 获取陪玩师详情（适配后台）
    /// </summary>
    Task<ApiResponse<AdminCompanionDetailDto>> GetCompanionDetailAsync(int companionId, int userId, bool ignoreStatus = false);

    /// <summary>
    /// 获取陪玩师服务列表
    /// </summary>
    Task<ApiResponse<List<CompanionServiceDto>>> GetCompanionServicesAsync(int companionId);

    /// <summary>
    /// 获取陪玩师评价列表
    /// </summary>
    Task<ApiResponse<List<CompanionReviewDto>>> GetCompanionReviewsAsync(int companionId, int page, int pageSize);
}