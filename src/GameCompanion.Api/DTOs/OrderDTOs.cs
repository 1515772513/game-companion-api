namespace GameCompanion.Api.DTOs;

/// <summary>
/// 创建订单请求
/// </summary>
public class CreateOrderRequest
{
    public int Companion_id { get; set; }
    public int Game_id { get; set; }
    public string? Game_rank { get; set; }
    public int Service_count { get; set; }
    public string Service_time { get; set; } = string.Empty; // 格式：YYYY-MM-DD HH:mm:ss
    public string? Special_requirements { get; set; }
}

/// <summary>
/// 订单响应数据
/// </summary>
public class OrderResponse
{
    public long Order_id { get; set; }
    public string Order_no { get; set; } = string.Empty;
    public int Companion_id { get; set; }
    public string Companion_name { get; set; } = string.Empty;
    public string Game_name { get; set; } = string.Empty;
    public int Service_count { get; set; }
    public string Service_time { get; set; } = string.Empty;
    public decimal Unit_price { get; set; }
    public decimal Total_amount { get; set; }
    public decimal Service_fee { get; set; }
    public decimal Discount_amount { get; set; }
    public decimal Final_amount { get; set; }
    public int Status { get; set; }
    public string Status_text { get; set; } = string.Empty;
    public int Payment_timeout { get; set; }
    public string Payment_url { get; set; } = string.Empty;
    public string Created_at { get; set; } = string.Empty;
}

/// <summary>
/// 订单详细信息
/// </summary>
public class OrderDetailInfo
{
    public long Id { get; set; }
    public string Order_no { get; set; } = string.Empty;
    public int Order_type { get; set; } // 1-陪玩订单，2-代练订单
    public string Order_type_text { get; set; } = string.Empty;
    public int Status { get; set; }
    public string Status_text { get; set; } = string.Empty;
    public int Payment_status { get; set; }
    public string Payment_status_text { get; set; } = string.Empty;
    public CompanionSimpleInfo Companion { get; set; } = new();
    public GameSimpleInfo Game { get; set; } = new();
    public string? Game_rank { get; set; }
    public int Service_count { get; set; }
    public string Service_unit { get; set; } = string.Empty; // 局、小时
    public string Service_time { get; set; } = string.Empty;
    public string? Special_requirements { get; set; }
    public decimal Unit_price { get; set; }
    public decimal Service_fee { get; set; }
    public decimal Discount_amount { get; set; }
    public decimal Total_amount { get; set; }
    public decimal Final_amount { get; set; }
    public string Payment_method { get; set; } = string.Empty; // balance, wechat, alipay
    public string Payment_method_text { get; set; } = string.Empty;
    public string? Payment_time { get; set; }
    public string Created_at { get; set; } = string.Empty;
    public string? Updated_at { get; set; }
    public OrderCountdown Countdown { get; set; } = new();
    public OrderActions Actions { get; set; } = new();
    public List<OrderTimelineItem> Timeline { get; set; } = new();
}

/// <summary>
/// 订单倒计时信息
/// </summary>
public class OrderCountdown
{
    public int Service_start_in { get; set; }
    public int Auto_confirm_in { get; set; }
}

/// <summary>
/// 订单可执行操作
/// </summary>
public class OrderActions
{
    public bool Can_cancel { get; set; }
    public bool Can_refund { get; set; }
    public bool Can_confirm { get; set; }
    public bool Can_review { get; set; }
}

/// <summary>
/// 订单时间线项
/// </summary>
public class OrderTimelineItem
{
    public string Status { get; set; } = string.Empty;
    public string Time { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}

/// <summary>
/// 取消订单请求
/// </summary>
public class CancelOrderRequest
{
    public string Cancel_reason { get; set; } = string.Empty;
}

/// <summary>
/// 申请退款请求
/// </summary>
public class RefundOrderRequest
{
    public string Refund_reason { get; set; } = string.Empty;
    public decimal? Refund_amount { get; set; }
    public int Refund_type { get; set; } // 1-全额退款，2-部分退款
}

/// <summary>
/// 订单评价请求
/// </summary>
public class ReviewOrderRequest
{
    public int Rating { get; set; } // 1-5星
    public string Comment { get; set; } = string.Empty;
    public List<string>? Images { get; set; }
    public List<string>? Tags { get; set; }
}
