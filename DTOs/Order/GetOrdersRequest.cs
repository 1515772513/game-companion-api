using System.ComponentModel.DataAnnotations;

namespace GameCompanion.Api.DTOs.Order;

/// <summary>
/// 获取订单列表请求
/// </summary>
public class GetOrdersRequest
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 页码，从1开始
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// 每页数量，范围1-50
    /// </summary>
    [Range(1, 50, ErrorMessage = "每页数量必须在1-50之间")]
    public int PageSize { get; set; } = 20;

    /// <summary>
    /// 订单状态筛选：1-待付款，2-待服务，3-服务中，4-待确认，5-已完成，6-已取消，7-退款中
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// 排序字段：created_at-创建时间，service_time-服务时间
    /// </summary>
    public string SortBy { get; set; } = "created_at";

    /// <summary>
    /// 排序方向：asc-升序，desc-降序
    /// </summary>
    public string SortOrder { get; set; } = "desc";
}

/// <summary>
/// 获取分页订单列表请求
/// </summary>
public class GetOrdersPaginationRequest
{
    /// <summary>
    /// 搜索关键词：申请人姓名/昵称
    /// </summary>
    public string? Keyword { get; set; }

    /// <summary>
    /// 游戏ID（或游戏类型，前端下拉选择）
    /// </summary>
    public int? GameId { get; set; }

    /// <summary>
    /// 订单类型：1-技术陪玩，2-娱乐陪玩
    /// </summary>
    public string? OrderType { get; set; }

    /// <summary>
    /// 订单状态筛选：1-待付款，2-待服务，3-服务中，4-待确认，5-已完成，6-已取消，7-退款中
    /// </summary>
    public string? Status { get; set; }

    /// <summary>
    /// 订单时间范围 - 开始时间
    /// </summary>
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 订单时间范围 - 结束时间
    /// </summary>
    public DateTime? EndTime { get; set; }

    /// <summary>
    /// 页码（默认1）
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// 每页条数（默认10）
    /// </summary>
    public int PageSize { get; set; } = 10;
}
