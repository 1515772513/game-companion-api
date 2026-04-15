namespace GameCompanion.Api.DTOs.Order;


/// <summary>
/// 订单项
/// </summary>
public class OrderItemBase
{
    /// <summary>
    /// 订单ID
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 订单号
    /// </summary>
    public string OrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 订单类型：1-陪玩订单，2-代练订单
    /// </summary>
    public string OrderType { get; set; } = string.Empty;

    /// <summary>
    /// 订单类型文本
    /// </summary>
    public string OrderTypeText { get; set; } = string.Empty;

    /// <summary>
    /// 订单状态
    /// </summary>
    public string Status { get; set; }

    /// <summary>
    /// 订单状态文本
    /// </summary>
    public string StatusText { get; set; } = string.Empty;

    /// <summary>
    /// 支付状态：0-未支付，1-已支付，2-退款中，3-已退款
    /// </summary>
    public int PaymentStatus { get; set; }

    /// <summary>
    /// 支付状态文本
    /// </summary>
    public string PaymentStatusText { get; set; } = string.Empty;

    /// <summary>
    /// 陪玩师信息
    /// </summary>
    public CompanionInfoBase Companion { get; set; } = new();

    /// <summary>
    /// 游戏名称
    /// </summary>
    public string GameName { get; set; } = string.Empty;

    /// <summary>
    /// 用户名
    /// </summary>
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// 游戏段位/等级
    /// </summary>
    public string GameRank { get; set; } = string.Empty;

    /// <summary>
    /// 服务数量
    /// </summary>
    public int ServiceCount { get; set; }

    /// <summary>
    /// 预约服务时间
    /// </summary>
    public string ServiceTime { get; set; } = string.Empty;

    /// <summary>
    /// 订单金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 订单创建时间
    /// </summary>
    public string CreatedAt { get; set; } = string.Empty;

    /// <summary>
    /// 订单金额
    /// </summary>
    public decimal? TotalPrice { get; set; }

    /// <summary>
    /// 优惠金额
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 最终金额
    /// </summary>
    public decimal? FinalPrice { get; set; }

    /// <summary>
    /// 服务类型
    /// </summary>
    public string ServiceType { get; set; } = string.Empty;

    /// <summary>
    /// 服务类型文本
    /// </summary>
    public string ServiceTypeName { get; set; } = string.Empty;

    /// <summary>
    /// 是否失效（超过15分钟 + 状态=0）
    /// </summary>
    public bool IsExpired { get; set; } = false;
}

/// <summary>
/// 陪玩师信息
/// </summary>
public class CompanionInfoBase
{
    /// <summary>
    /// 陪玩师ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 陪玩师昵称
    /// </summary>
    public string Nickname { get; set; } = string.Empty;

    /// <summary>
    /// 陪玩师真实姓名
    /// </summary>
    public string RealName { get; set; } = string.Empty;

    /// <summary>
    /// 陪玩师头像URL
    /// </summary>
    public string AvatarUrl { get; set; } = string.Empty;

    /// <summary>
    /// 陪玩师等级
    /// </summary>
    public string? Level { get; set; } = string.Empty;
}


/// <summary>
/// 获取订单列表响应
/// </summary>
public class GetOrdersResponse
{
    /// <summary>
    /// 订单列表
    /// </summary>
    public List<OrderItem> Items { get; set; } = new();

    /// <summary>
    /// 分页信息
    /// </summary>
    public OrderPagination Pagination { get; set; } = new();

    public class OrderItem : OrderItemBase {}

    public class CompanionInfo : CompanionInfoBase {}

    /// <summary>
    /// 分页信息
    /// </summary>
    public class OrderPagination
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int Page { get; set; }

        /// <summary>
        /// 每页数量
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// 总数量
        /// </summary>
        public long Total { get; set; }

        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages { get; set; }

        /// <summary>
        /// 是否有更多数据
        /// </summary>
        public bool HasMore { get; set; }
    }
}

/// <summary>
/// 订单分页列表响应
/// </summary>
public class GetOrdersPaginationResponse
{
    public int Total { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public List<OrderItem> list { get; set; } = new List<OrderItem>();
    public class OrderItem : OrderItemBase {}

    public class CompanionInfo : CompanionInfoBase {}
}
