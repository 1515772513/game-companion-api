using GameCompanion.Api.DTOs;
using GameCompanion.Api.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameCompanion.Api.Controllers;

/// <summary>
/// 数据概览控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class DashboardController : ControllerBase
{
    private readonly IDashboardService _dashboardService;

    public DashboardController(IDashboardService dashboardService)
    {
        _dashboardService = dashboardService;
    }

    [HttpGet("stats")]
    public async Task<ActionResult<ApiResponse<object>>> GetStats()
    {
        var data = await _dashboardService.GetStatsAsync();
        return Ok(ApiResponse<object>.Success(data));
    }

    [HttpGet("revenue-trend")]
    public async Task<ActionResult<ApiResponse<object>>> GetRevenueTrend([FromQuery] int days = 30)
    {
        var data = await _dashboardService.GetRevenueTrendAsync(days);
        return Ok(ApiResponse<object>.Success(data));
    }

    [HttpGet("order-distribution")]
    public async Task<ActionResult<ApiResponse<object>>> GetOrderDistribution()
    {
        var data = await _dashboardService.GetOrderDistributionAsync();
        return Ok(ApiResponse<object>.Success(data));
    }

    [HttpGet("latest-orders")]
    public async Task<ActionResult<ApiResponse<object>>> GetLatestOrders([FromQuery] int limit = 10)
    {
        var data = await _dashboardService.GetLatestOrdersAsync(limit);
        return Ok(ApiResponse<object>.Success(data));
    }

    [HttpGet("quick-actions")]
    public async Task<ActionResult<ApiResponse<object>>> GetQuickActions()
    {
        var data = await _dashboardService.GetQuickActionsAsync();
        return Ok(ApiResponse<object>.Success(data));
    }
}

/// <summary>
/// 用户管理控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetUserList(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? keyword = null,
        [FromQuery] int? accountStatus = null,
        [FromQuery] int? vipLevel = null,
        [FromQuery] string? registerStart = null,
        [FromQuery] string? registerEnd = null,
        [FromQuery] string orderBy = "created_at",
        [FromQuery] string order = "desc")
    {
        var data = await _userService.GetUserListAsync(page, pageSize, keyword, accountStatus, vipLevel, registerStart, registerEnd, orderBy, order);
        return Ok(ApiResponse<object>.Success(data));
    }

    [HttpGet("{userId}")]
    public async Task<ActionResult<ApiResponse<object>>> GetUserDetail(int userId)
    {
        var data = await _userService.GetUserDetailAsync(userId);
        if (data == null)
        {
            return NotFound(ApiResponse<object>.Fail(404, "用户不存在", "User not found"));
        }
        return Ok(ApiResponse<object>.Success(data));
    }

    [HttpPut("{userId}/status")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateUserStatus(int userId, [FromBody] UpdateStatusRequest request)
    {
        var success = await _userService.UpdateUserStatusAsync(userId, request.AccountStatus, request.Reason);
        if (!success)
        {
            return NotFound(ApiResponse<object>.Fail(404, "用户不存在", "User not found"));
        }
        return Ok(ApiResponse<object>.Success(new { user_id = userId, account_status = request.AccountStatus }));
    }

    [HttpPut("{userId}")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateUser(int userId, [FromBody] object updateData)
    {
        var success = await _userService.UpdateUserAsync(userId, updateData);
        if (!success)
        {
            return NotFound(ApiResponse<object>.Fail(404, "用户不存在", "User not found"));
        }
        return Ok(ApiResponse<object>.Success(null, "更新成功"));
    }

    [HttpGet("{userId}/orders")]
    public async Task<ActionResult<ApiResponse<object>>> GetUserOrders(
        int userId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] int? status = null)
    {
        var data = await _userService.GetUserOrdersAsync(userId, page, pageSize, status);
        return Ok(ApiResponse<object>.Success(data));
    }

    [HttpGet("stats")]
    public async Task<ActionResult<ApiResponse<object>>> GetUserStats()
    {
        var data = await _userService.GetUserStatsAsync();
        return Ok(ApiResponse<object>.Success(data));
    }

    [HttpPost("export")]
    public async Task<ActionResult<ApiResponse<object>>> ExportUsers([FromBody] object filters)
    {
        var url = await _userService.ExportUsersAsync(filters);
        return Ok(ApiResponse<object>.Success(new { download_url = url, expires_at = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") }));
    }

    [HttpPost("{userId}/reset-password")]
    public async Task<ActionResult<ApiResponse<object>>> ResetUserPassword(int userId, [FromBody] PasswordResetRequest request)
    {
        var success = await _userService.ResetUserPasswordAsync(userId, request.NewPassword);
        if (!success)
        {
            return NotFound(ApiResponse<object>.Fail(404, "用户不存在", "User not found"));
        }
        return Ok(ApiResponse<object>.Success(null, "密码重置成功"));
    }
}

/// <summary>
/// 陪玩管理控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CompanionsController : ControllerBase
{
    private readonly ICompanionService _companionService;

    public CompanionsController(ICompanionService companionService)
    {
        _companionService = companionService;
    }

    [HttpGet("applications")]
    public async Task<ActionResult<ApiResponse<object>>> GetApplications(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? keyword = null,
        [FromQuery] int? certificationStatus = null,
        [FromQuery] string? serviceType = null,
        [FromQuery] int? gameId = null,
        [FromQuery] string? applyStart = null,
        [FromQuery] string? applyEnd = null,
        [FromQuery] string orderBy = "apply_time",
        [FromQuery] string order = "desc")
    {
        var data = await _companionService.GetApplicationsAsync(page, pageSize, keyword, certificationStatus, serviceType, gameId, applyStart, applyEnd, orderBy, order);
        return Ok(ApiResponse<object>.Success(data));
    }

    [HttpGet("applications/{applicationId}")]
    public async Task<ActionResult<ApiResponse<object>>> GetApplicationDetail(int applicationId)
    {
        var data = await _companionService.GetApplicationDetailAsync(applicationId);
        if (data == null)
        {
            return NotFound(ApiResponse<object>.Fail(404, "认证申请不存在", "Application not found"));
        }
        return Ok(ApiResponse<object>.Success(data));
    }

    [HttpPost("applications/{applicationId}/audit")]
    public async Task<ActionResult<ApiResponse<object>>> AuditApplication(int applicationId, [FromBody] AuditRequest request)
    {
        var success = await _companionService.AuditApplicationAsync(applicationId, request.Action, request.RejectReason);
        if (!success)
        {
            return NotFound(ApiResponse<object>.Fail(404, "认证申请不存在", "Application not found"));
        }
        return Ok(ApiResponse<object>.Success(new { application_id = applicationId, certification_status = request.Action == "approve" ? 1 : 2 }));
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetCompanions(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? keyword = null,
        [FromQuery] string? level = null,
        [FromQuery] string? serviceType = null,
        [FromQuery] int? gameId = null,
        [FromQuery] int? onlineStatus = null,
        [FromQuery] int certificationStatus = 1,
        [FromQuery] string orderBy = "created_at",
        [FromQuery] string order = "desc")
    {
        var data = await _companionService.GetCompanionsAsync(page, pageSize, keyword, level, serviceType, gameId, onlineStatus, certificationStatus, orderBy, order);
        return Ok(ApiResponse<object>.Success(data));
    }

    [HttpPut("{companionId}")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateCompanion(int companionId, [FromBody] object updateData)
    {
        var success = await _companionService.UpdateCompanionAsync(companionId, updateData);
        if (!success)
        {
            return NotFound(ApiResponse<object>.Fail(404, "陪玩师不存在", "Companion not found"));
        }
        return Ok(ApiResponse<object>.Success(null, "更新成功"));
    }

    [HttpGet("stats")]
    public async Task<ActionResult<ApiResponse<object>>> GetCompanionStats()
    {
        var data = await _companionService.GetCompanionStatsAsync();
        return Ok(ApiResponse<object>.Success(data));
    }
}

/// <summary>
/// 订单管理控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetOrderList(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? keyword = null,
        [FromQuery] int? orderType = null,
        [FromQuery] int? status = null,
        [FromQuery] int? paymentStatus = null,
        [FromQuery] int? gameId = null,
        [FromQuery] int? userId = null,
        [FromQuery] int? companionId = null,
        [FromQuery] string? startTime = null,
        [FromQuery] string? endTime = null,
        [FromQuery] string orderBy = "created_at",
        [FromQuery] string order = "desc")
    {
        var data = await _orderService.GetOrderListAsync(page, pageSize, keyword, orderType, status, paymentStatus, gameId, userId, companionId, startTime, endTime, orderBy, order);
        return Ok(ApiResponse<object>.Success(data));
    }

    [HttpGet("{orderId}")]
    public async Task<ActionResult<ApiResponse<object>>> GetOrderDetail(int orderId)
    {
        var data = await _orderService.GetOrderDetailAsync(orderId);
        if (data == null)
        {
            return NotFound(ApiResponse<object>.Fail(404, "订单不存在", "Order not found"));
        }
        return Ok(ApiResponse<object>.Success(data));
    }

    [HttpPut("{orderId}")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateOrder(int orderId, [FromBody] object updateData)
    {
        var success = await _orderService.UpdateOrderAsync(orderId, updateData);
        if (!success)
        {
            return NotFound(ApiResponse<object>.Fail(404, "订单不存在", "Order not found"));
        }
        return Ok(ApiResponse<object>.Success(null, "更新成功"));
    }

    [HttpPost("{orderId}/refund")]
    public async Task<ActionResult<ApiResponse<object>>> ProcessRefund(int orderId, [FromBody] RefundRequest request)
    {
        var success = await _orderService.ProcessRefundAsync(orderId, request.Action, request.RefundAmount, request.RejectReason);
        if (!success)
        {
            return NotFound(ApiResponse<object>.Fail(404, "订单不存在", "Order not found"));
        }
        return Ok(ApiResponse<object>.Success(new { order_id = orderId, refund_status = request.Action == "approve" ? 2 : 3 }));
    }

    [HttpPost("{orderId}/cancel")]
    public async Task<ActionResult<ApiResponse<object>>> CancelOrder(int orderId, [FromBody] CancelOrderRequest request)
    {
        var success = await _orderService.CancelOrderAsync(orderId, request.CancelReason);
        if (!success)
        {
            return NotFound(ApiResponse<object>.Fail(404, "订单不存在", "Order not found"));
        }
        return Ok(ApiResponse<object>.Success(null, "订单已取消"));
    }

    [HttpGet("stats")]
    public async Task<ActionResult<ApiResponse<object>>> GetOrderStats()
    {
        var data = await _orderService.GetOrderStatsAsync();
        return Ok(ApiResponse<object>.Success(data));
    }

    [HttpPost("export")]
    public async Task<ActionResult<ApiResponse<object>>> ExportOrders([FromBody] object filters)
    {
        var url = await _orderService.ExportOrdersAsync(filters);
        return Ok(ApiResponse<object>.Success(new { download_url = url, expires_at = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") }));
    }
}

/// <summary>
/// 内容管理控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly IPostService _postService;

    public PostsController(IPostService postService)
    {
        _postService = postService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetPostList(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? keyword = null,
        [FromQuery] int? gameId = null,
        [FromQuery] int? auditStatus = null,
        [FromQuery] int? status = null,
        [FromQuery] int? userId = null,
        [FromQuery] string? startTime = null,
        [FromQuery] string? endTime = null,
        [FromQuery] string orderBy = "created_at",
        [FromQuery] string order = "desc")
    {
        var data = await _postService.GetPostListAsync(page, pageSize, keyword, gameId, auditStatus, status, userId, startTime, endTime, orderBy, order);
        return Ok(ApiResponse<object>.Success(data));
    }

    [HttpGet("{postId}")]
    public async Task<ActionResult<ApiResponse<object>>> GetPostDetail(int postId)
    {
        var data = await _postService.GetPostDetailAsync(postId);
        if (data == null)
        {
            return NotFound(ApiResponse<object>.Fail(404, "动态不存在", "Post not found"));
        }
        return Ok(ApiResponse<object>.Success(data));
    }

    [HttpPost("{postId}/audit")]
    public async Task<ActionResult<ApiResponse<object>>> AuditPost(int postId, [FromBody] AuditPostRequest request)
    {
        var success = await _postService.AuditPostAsync(postId, request.Action, request.AuditReason);
        if (!success)
        {
            return NotFound(ApiResponse<object>.Fail(404, "动态不存在", "Post not found"));
        }
        return Ok(ApiResponse<object>.Success(null, "审核成功"));
    }

    [HttpPut("{postId}/visibility")]
    public async Task<ActionResult<ApiResponse<object>>> UpdatePostVisibility(int postId, [FromBody] VisibilityRequest request)
    {
        var success = await _postService.UpdatePostVisibilityAsync(postId, request.AuditStatus);
        if (!success)
        {
            return NotFound(ApiResponse<object>.Fail(404, "动态不存在", "Post not found"));
        }
        return Ok(ApiResponse<object>.Success(null, "更新成功"));
    }

    [HttpDelete("{postId}")]
    public async Task<ActionResult<ApiResponse<object>>> DeletePost(int postId)
    {
        var success = await _postService.DeletePostAsync(postId);
        if (!success)
        {
            return NotFound(ApiResponse<object>.Fail(404, "动态不存在", "Post not found"));
        }
        return Ok(ApiResponse<object>.Success(null, "删除成功"));
    }
}

/// <summary>
/// 消息管理控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class MessagesController : ControllerBase
{
    private readonly IMessageService _messageService;

    public MessagesController(IMessageService messageService)
    {
        _messageService = messageService;
    }

    [HttpPost("send")]
    public async Task<ActionResult<ApiResponse<object>>> CreateMessage([FromBody] object messageData)
    {
        var data = await _messageService.CreateMessageAsync(messageData);
        return Ok(ApiResponse<object>.Success(data, "推送创建成功"));
    }

    [HttpGet("records")]
    public async Task<ActionResult<ApiResponse<object>>> GetMessageRecords(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] string? keyword = null,
        [FromQuery] string? targetType = null,
        [FromQuery] string? status = null,
        [FromQuery] string? startTime = null,
        [FromQuery] string? endTime = null,
        [FromQuery] string orderBy = "created_at",
        [FromQuery] string order = "desc")
    {
        var data = await _messageService.GetMessageRecordsAsync(page, pageSize, keyword, targetType, status, startTime, endTime, orderBy, order);
        return Ok(ApiResponse<object>.Success(data));
    }

    [HttpGet("stats")]
    public async Task<ActionResult<ApiResponse<object>>> GetMessageStats()
    {
        var data = await _messageService.GetMessageStatsAsync();
        return Ok(ApiResponse<object>.Success(data));
    }

    [HttpPost("{pushId}/cancel")]
    public async Task<ActionResult<ApiResponse<object>>> CancelMessage(int pushId)
    {
        var success = await _messageService.CancelMessageAsync(pushId);
        if (!success)
        {
            return NotFound(ApiResponse<object>.Fail(404, "推送记录不存在", "Message not found"));
        }
        return Ok(ApiResponse<object>.Success(new { push_id = pushId, status = "cancelled" }));
    }
}

/// <summary>
/// 系统配置控制器
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class SettingsController : ControllerBase
{
    private readonly ISettingService _settingService;

    public SettingsController(ISettingService settingService)
    {
        _settingService = settingService;
    }

    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> GetSystemConfig([FromQuery] string? configGroup = null)
    {
        var data = await _settingService.GetSystemConfigAsync(configGroup);
        return Ok(ApiResponse<object>.Success(data));
    }

    [HttpPut]
    public async Task<ActionResult<ApiResponse<object>>> UpdateSystemConfig([FromBody] object configData)
    {
        var success = await _settingService.UpdateSystemConfigAsync(configData);
        return Ok(ApiResponse<object>.Success(new { updated_count = 1, updated_at = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") }, "配置保存成功"));
    }

    [HttpGet("logs")]
    public async Task<ActionResult<ApiResponse<object>>> GetAdminLogs(
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20,
        [FromQuery] int? adminId = null,
        [FromQuery] string? action = null,
        [FromQuery] string? module = null,
        [FromQuery] string? startTime = null,
        [FromQuery] string? endTime = null,
        [FromQuery] string orderBy = "created_at",
        [FromQuery] string order = "desc")
    {
        var data = await _settingService.GetAdminLogsAsync(page, pageSize, adminId, action, module, startTime, endTime, orderBy, order);
        return Ok(ApiResponse<object>.Success(data));
    }

    [HttpPost("reset")]
    public async Task<ActionResult<ApiResponse<object>>> ResetConfig([FromBody] ResetConfigRequest request)
    {
        var success = await _settingService.ResetConfigAsync(request.ConfigGroup);
        return Ok(ApiResponse<object>.Success(null, "配置重置成功"));
    }

    [HttpGet("permissions")]
    public async Task<ActionResult<ApiResponse<object>>> GetPermissions()
    {
        var data = await _settingService.GetPermissionsAsync();
        return Ok(ApiResponse<object>.Success(data));
    }

    [HttpGet("admins")]
    public async Task<ActionResult<ApiResponse<object>>> GetAdmins()
    {
        var data = await _settingService.GetAdminsAsync();
        return Ok(ApiResponse<object>.Success(data));
    }

    [HttpPost]
    public async Task<ActionResult<ApiResponse<object>>> CreateAdmin([FromBody] object adminData)
    {
        var success = await _settingService.CreateAdminAsync(adminData);
        return Ok(ApiResponse<object>.Success(null, "管理员创建成功"));
    }

    [HttpPut("admins/{adminId}")]
    public async Task<ActionResult<ApiResponse<object>>> UpdateAdmin(int adminId, [FromBody] object adminData)
    {
        var success = await _settingService.UpdateAdminAsync(adminId, adminData);
        if (!success)
        {
            return NotFound(ApiResponse<object>.Fail(404, "管理员不存在", "Admin not found"));
        }
        return Ok(ApiResponse<object>.Success(null, "管理员更新成功"));
    }
}

// 请求模型类
public class UpdateStatusRequest
{
    public int AccountStatus { get; set; }
    public string? Reason { get; set; }
}

public class PasswordResetRequest
{
    public string NewPassword { get; set; } = string.Empty;
}

public class AuditRequest
{
    public string Action { get; set; } = string.Empty;
    public string? RejectReason { get; set; }
}

public class RefundRequest
{
    public string Action { get; set; } = string.Empty;
    public decimal? RefundAmount { get; set; }
    public string? RejectReason { get; set; }
}

public class CancelOrderRequest
{
    public string CancelReason { get; set; } = string.Empty;
}

public class AuditPostRequest
{
    public string Action { get; set; } = string.Empty;
    public string? AuditReason { get; set; }
}

public class VisibilityRequest
{
    public int AuditStatus { get; set; }
}

public class ResetConfigRequest
{
    public string ConfigGroup { get; set; } = string.Empty;
}
