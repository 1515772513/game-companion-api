namespace GameCompanion.Api.Dtos;

/// <summary>
/// 订单统计响应DTO
/// </summary>
public class OrderStatisticsDto
{
    /// <summary>
    /// 总订单数
    /// </summary>
    public long TotalOrders { get; set; }
    
    /// <summary>
    /// 待付款
    /// </summary>
    public long PendingPayment { get; set; }
    
    /// <summary>
    /// 进行中
    /// </summary>
    public long InProgress { get; set; }
    
    /// <summary>
    /// 已完成
    /// </summary>
    public long Completed { get; set; }
    
    /// <summary>
    /// 退款/售后
    /// </summary>
    public long RefundAfterSale { get; set; }
}

/// <summary>
/// 订单状态统计响应DTO
/// </summary>
public class OrderStatusDto
{
    /// <summary>
    /// 待付款
    /// </summary>
    public long PendingPayment { get; set; }
    
    /// <summary>
    /// 进行中
    /// </summary>
    public long InProgress { get; set; }
    
    /// <summary>
    /// 已完成
    /// </summary>
    public long Completed { get; set; }
    
    /// <summary>
    /// 退款/售后
    /// </summary>
    public long RefundAfterSale { get; set; }
}
