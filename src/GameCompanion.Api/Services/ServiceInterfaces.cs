using GameCompanion.Api.DTOs;

namespace GameCompanion.Api.Services;

/// <summary>
/// 认证服务接口
/// </summary>
public interface IAuthService
{
    Task<(string? accessToken, string? refreshToken, object? adminInfo)> LoginAsync(string username, string password, bool rememberMe);
    Task<bool> LogoutAsync(string token);
    Task<(string? accessToken, string? message)> RefreshTokenAsync(string refreshToken);
    Task<object?> GetCurrentUserAsync(int adminId);
}

/// <summary>
/// 数据概览服务接口
/// </summary>
public interface IDashboardService
{
    Task<object> GetStatsAsync();
    Task<object> GetRevenueTrendAsync(int days = 30);
    Task<object> GetOrderDistributionAsync();
    Task<object> GetLatestOrdersAsync(int limit = 10);
    Task<object> GetQuickActionsAsync();
}

/// <summary>
/// 用户服务接口
/// </summary>
public interface IUserService
{
    Task<object> GetUserListAsync(int page, int pageSize, string? keyword, int? accountStatus, int? vipLevel, string? registerStart, string? registerEnd, string orderBy, string order);
    Task<object?> GetUserDetailAsync(int userId);
    Task<bool> UpdateUserStatusAsync(int userId, int accountStatus, string? reason);
    Task<bool> UpdateUserAsync(int userId, object updateData);
    Task<object> GetUserOrdersAsync(int userId, int page, int pageSize, int? status);
    Task<object> GetUserStatsAsync();
    Task<string> ExportUsersAsync(object filters);
    Task<bool> ResetUserPasswordAsync(int userId, string newPassword);
}

/// <summary>
/// 陪玩服务接口
/// </summary>
public interface ICompanionService
{
    Task<object> GetApplicationsAsync(int page, int pageSize, string? keyword, int? certificationStatus, string? serviceType, int? gameId, string? applyStart, string? applyEnd, string orderBy, string order);
    Task<object?> GetApplicationDetailAsync(int applicationId);
    Task<bool> AuditApplicationAsync(int applicationId, string action, string? rejectReason);
    Task<object> GetCompanionsAsync(int page, int pageSize, string? keyword, string? level, string? serviceType, int? gameId, int? onlineStatus, int certificationStatus, string orderBy, string order);
    Task<bool> UpdateCompanionAsync(int companionId, object updateData);
    Task<object> GetCompanionStatsAsync();
}

/// <summary>
/// 订单服务接口
/// </summary>
public interface IOrderService
{
    Task<object> GetOrderListAsync(int page, int pageSize, string? keyword, int? orderType, int? status, int? paymentStatus, int? gameId, int? userId, int? companionId, string? startTime, string? endTime, string orderBy, string order);
    Task<object?> GetOrderDetailAsync(int orderId);
    Task<bool> UpdateOrderAsync(int orderId, object updateData);
    Task<bool> ProcessRefundAsync(int orderId, string action, decimal? refundAmount, string? rejectReason);
    Task<bool> CancelOrderAsync(int orderId, string cancelReason);
    Task<object> GetOrderStatsAsync();
    Task<string> ExportOrdersAsync(object filters);
}

/// <summary>
/// 内容服务接口
/// </summary>
public interface IPostService
{
    Task<object> GetPostListAsync(int page, int pageSize, string? keyword, int? gameId, int? auditStatus, int? status, int? userId, string? startTime, string? endTime, string orderBy, string order);
    Task<object?> GetPostDetailAsync(int postId);
    Task<bool> AuditPostAsync(int postId, string action, string? auditReason);
    Task<bool> UpdatePostVisibilityAsync(int postId, int auditStatus);
    Task<bool> DeletePostAsync(int postId);
}

/// <summary>
/// 消息服务接口
/// </summary>
public interface IMessageService
{
    Task<object> CreateMessageAsync(object messageData);
    Task<object> GetMessageRecordsAsync(int page, int pageSize, string? keyword, string? targetType, string? status, string? startTime, string? endTime, string orderBy, string order);
    Task<object> GetMessageStatsAsync();
    Task<bool> CancelMessageAsync(int pushId);
}

/// <summary>
/// 设置服务接口
/// </summary>
public interface ISettingService
{
    Task<object> GetSystemConfigAsync(string? configGroup);
    Task<bool> UpdateSystemConfigAsync(object configData);
    Task<object> GetAdminLogsAsync(int page, int pageSize, int? adminId, string? action, string? module, string? startTime, string? endTime, string orderBy, string order);
    Task<bool> ResetConfigAsync(string configGroup);
    Task<object> GetPermissionsAsync();
    Task<object> GetAdminsAsync();
    Task<bool> CreateAdminAsync(object adminData);
    Task<bool> UpdateAdminAsync(int adminId, object adminData);
}

/// <summary>
/// 首页服务接口
/// </summary>
public interface IHomeService
{
    Task<HomeResponse> GetHomeDataAsync();
    Task<PagedResponse<CompanionDetailInfo>> GetCompanionsAsync(int page, int pageSize, int? gameId, string? serviceType, string? level, decimal? minPrice, decimal? maxPrice, int? onlineStatus, string? keyword, string sortBy, string sortOrder);
    Task<CompanionDetailInfo?> GetCompanionDetailAsync(int companionId);
    Task<List<GameDetailInfo>> GetGamesAsync();
    Task<PagedResponse<CompanionSimpleInfo>> SearchCompanionsAsync(string keyword, int page, int pageSize);
}
