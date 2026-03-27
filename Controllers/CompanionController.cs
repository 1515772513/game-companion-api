using GameCompanion.Api.DTOs.Companion;
using GameCompanion.Api.Models;
using GameCompanion.Api.Services;
using Microsoft.AspNetCore.Mvc;
using GameCompanion.Api.Data;

namespace GameCompanion.Api.Controllers;

/// <summary>
/// 陪玩师认证接口
/// </summary>
[ApiController]
[Route("api/companion")]
[Produces("application/json")]
public class CompanionController : ControllerBase
{
    private readonly ICompanionService _companionService;
    private readonly ILogger<CompanionController> _logger;
    private readonly ApplicationDbContext _context;

    public CompanionController(ICompanionService companionService, ILogger<CompanionController> logger, ApplicationDbContext context)
    {
        _companionService = companionService;
        _logger = logger;
        _context = context;
    }

    /// <summary>
    /// 申请成为陪玩师
    /// </summary>
    /// <param name="request">申请请求</param>
    /// <returns>申请结果</returns>
    [HttpPost("apply")]
    [ProducesResponseType(typeof(ApiResponse<ApplicationStatusResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<ApplicationStatusResponse>>> Apply([FromBody] ApplyCompanionRequest request)
    {
        _logger.LogInformation("用户申请成为陪玩师");
        var result = await _companionService.ApplyCompanionAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// 获取认证申请状态
    /// </summary>
    /// <returns>申请状态</returns>
    [HttpGet("application-status")]
    [ProducesResponseType(typeof(ApiResponse<ApplicationStatusResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<ApplicationStatusResponse>>> GetApplicationStatus()
    {
        _logger.LogInformation("获取认证申请状态");
        var result = await _companionService.GetApplicationStatusAsync();
        return Ok(result);
    }

    /// <summary>
    /// 获取我的陪玩师信息
    /// </summary>
    /// <returns>陪玩师信息</returns>
    [HttpGet("my-info")]
    [ProducesResponseType(typeof(ApiResponse<CompanionInfoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<CompanionInfoResponse>>> GetMyCompanionInfo()
    {
        _logger.LogInformation("获取陪玩师信息");
        var result = await _companionService.GetMyCompanionInfoAsync();
        return Ok(result);
    }

    /// <summary>
    /// 更新陪玩师信息
    /// </summary>
    /// <param name="request">更新请求</param>
    /// <returns>更新结果</returns>
    [HttpPut("my-info")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> UpdateCompanionInfo([FromBody] UpdateCompanionInfoRequest request)
    {
        _logger.LogInformation("更新陪玩师信息");
        var result = await _companionService.UpdateCompanionInfoAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// 切换在线状态
    /// </summary>
    /// <param name="request">状态更新请求</param>
    /// <returns>更新结果</returns>
    [HttpPut("online-status")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> UpdateOnlineStatus([FromBody] UpdateOnlineStatusRequest request)
    {
        _logger.LogInformation("切换陪玩师在线状态");
        var result = await _companionService.UpdateOnlineStatusAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// 获取接单列表
    /// </summary>
    /// <param name="page">页码</param>
    /// <param name="pageSize">每页数量</param>
    /// <param name="status">订单状态筛选</param>
    /// <returns>接单列表</returns>
    [HttpGet("orders")]
    [ProducesResponseType(typeof(ApiResponse<CompanionOrdersResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<CompanionOrdersResponse>>> GetCompanionOrders(
        [FromQuery] int? page = 1,
        [FromQuery] int? pageSize = 20,
        [FromQuery] int? status = null)
    {
        _logger.LogInformation("获取接单列表，页码：{Page}，状态：{Status}", page, status);
        var result = await _companionService.GetCompanionOrdersAsync(page, pageSize, status);
        return Ok(result);
    }

    /// <summary>
    /// 接受订单
    /// </summary>
    /// <param name="orderId">订单ID</param>
    /// <returns>接受结果</returns>
    [HttpPost("orders/{orderId}/accept")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> AcceptOrder(int orderId)
    {
        _logger.LogInformation("陪玩师接受订单：{OrderId}", orderId);
        var result = await _companionService.AcceptOrderAsync(orderId);
        return Ok(result);
    }

    /// <summary>
    /// 拒绝订单
    /// </summary>
    /// <param name="orderId">订单ID</param>
    /// <param name="rejectReason">拒绝原因</param>
    /// <returns>拒绝结果</returns>
    [HttpPost("orders/{orderId}/reject")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> RejectOrder(int orderId, [FromBody] string rejectReason)
    {
        _logger.LogInformation("陪玩师拒绝订单：{OrderId}，原因：{RejectReason}", orderId, rejectReason);
        var result = await _companionService.RejectOrderAsync(orderId, rejectReason);
        return Ok(result);
    }

    /// <summary>
    /// 开始服务
    /// </summary>
    /// <param name="orderId">订单ID</param>
    /// <returns>开始结果</returns>
    [HttpPost("orders/{orderId}/start")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> StartOrder(int orderId)
    {
        _logger.LogInformation("陪玩师开始服务：{OrderId}", orderId);
        var result = await _companionService.StartOrderAsync(orderId);
        return Ok(result);
    }

    /// <summary>
    /// 完成服务
    /// </summary>
    /// <param name="orderId">订单ID</param>
    /// <param name="serviceSummary">服务总结</param>
    /// <returns>完成结果</returns>
    [HttpPost("orders/{orderId}/complete")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> CompleteOrder(int orderId, [FromBody] string? serviceSummary = null)
    {
        _logger.LogInformation("陪玩师完成服务：{OrderId}", orderId);
        var result = await _companionService.CompleteOrderAsync(orderId, serviceSummary);
        return Ok(result);
    }

    /// <summary>
    /// 获取收益统计
    /// </summary>
    /// <returns>收益统计</returns>
    [HttpGet("earnings")]
    [ProducesResponseType(typeof(ApiResponse<EarningsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<EarningsResponse>>> GetEarnings()
    {
        _logger.LogInformation("获取收益统计");
        var result = await _companionService.GetEarningsAsync();
        return Ok(result);
    }

    /// <summary>
    /// 申请提现
    /// </summary>
    /// <param name="request">提现请求</param>
    /// <returns>申请结果</returns>
    [HttpPost("withdraw")]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse>> ApplyWithdraw([FromBody] WithdrawRequest request)
    {
        _logger.LogInformation("申请提现，金额：{Amount}，方式：{Method}", request.Amount, request.WithdrawMethod);
        var result = await _companionService.ApplyWithdrawAsync(request);
        return Ok(result);
    }

    /// <summary>
    /// 获取提现记录
    /// </summary>
    /// <param name="page">页码</param>
    /// <param name="pageSize">每页数量</param>
    /// <returns>提现记录</returns>
    [HttpGet("withdraw-records")]
    [ProducesResponseType(typeof(ApiResponse<WithdrawRecordsResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ApiResponse<WithdrawRecordsResponse>>> GetWithdrawRecords(
        [FromQuery] int? page = 1,
        [FromQuery] int? pageSize = 20)
    {
        _logger.LogInformation("获取提现记录，页码：{Page}", page);
        var result = await _companionService.GetWithdrawRecordsAsync(page, pageSize);
        return Ok(result);
    }


    /// <summary>
    /// 获取陪玩师列表()
    /// </summary>
    /// <returns>陪玩师列表</returns>
    [HttpPost("list")]
    [ProducesResponseType(typeof(ApiResponse<CompanionListResponse>), 200)]
    [ProducesResponseType(typeof(ApiResponse<>), 404)]
    [ProducesResponseType(typeof(ApiResponse<>), 500)]
    public async Task<ActionResult<ApiResponse<CompanionListResponse>>> GetList([FromBody] CompanionListRequest request)
    {
        var result = await _companionService.GetCompanionListAsync(request);
        return Ok(result);
    }


}