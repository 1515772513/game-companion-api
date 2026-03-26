namespace GameCompanion.Api.DTOs.Order;

/// <summary>
/// 获取订单列表请求
/// </summary>
public class GetOrdersRequest
{
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
    public int? Status { get; set; }

    /// <summary>
    /// 排序字段：created_at-创建时间，service_time-服务时间
    /// </summary>
    public string SortBy { get; set; } = "created_at";

    /// <summary>
    /// 排序方向：asc-升序，desc-降序
    /// </summary>
    public string SortOrder { get; set; } = "desc";
}