namespace GameCompanion.Api.DTOs.Order;

/// <summary>
/// 获取订单详情响应
/// </summary>
public class GetOrderResponse
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
    public int OrderType { get; set; }

    /// <summary>
    /// 订单类型文本
    /// </summary>
    public string OrderTypeText { get; set; } = string.Empty;

    /// <summary>
    /// 订单状态
    /// </summary>
    public int Status { get; set; }

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
    /// 陪玩师详细信息
    /// </summary>
    public CompanionDetail Companion { get; set; } = new();

    /// <summary>
    /// 游戏信息
    /// </summary>
    public GameInfo Game { get; set; } = new();

    /// <summary>
    /// 游戏段位/等级
    /// </summary>
    public string GameRank { get; set; } = string.Empty;

    /// <summary>
    /// 服务类型
    /// </summary>
    public string ServiceType { get; set; } = string.Empty;

    /// <summary>
    /// 服务名称
    /// </summary>
    public string ServiceTypeName { get; set; } = string.Empty;

    /// <summary>
    /// 服务数量
    /// </summary>
    public int ServiceCount { get; set; }

    /// <summary>
    /// 服务单位：局、小时
    /// </summary>
    public string ServiceUnit { get; set; } = string.Empty;

    /// <summary>
    /// 预约服务时间
    /// </summary>
    public string ServiceTime { get; set; } = string.Empty;

    /// <summary>
    /// 特殊要求
    /// </summary>
    public string SpecialRequirements { get; set; } = string.Empty;

    /// <summary>
    /// 单价
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 服务手续费
    /// </summary>
    public decimal ServiceFee { get; set; }

    /// <summary>
    /// 优惠金额
    /// </summary>
    public decimal DiscountAmount { get; set; }

    /// <summary>
    /// 订单总额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 实付金额
    /// </summary>
    public decimal FinalAmount { get; set; }

    /// <summary>
    /// 支付方式：balance-余额，wechat-微信，alipay-支付宝
    /// </summary>
    public string PaymentMethod { get; set; } = string.Empty;

    /// <summary>
    /// 支付方式文本
    /// </summary>
    public string PaymentMethodText { get; set; } = string.Empty;

    /// <summary>
    /// 支付时间
    /// </summary>
    public string PaymentTime { get; set; } = string.Empty;

    /// <summary>
    /// 订单创建时间
    /// </summary>
    public string CreatedAt { get; set; } = string.Empty;

    /// <summary>
    /// 订单更新时间
    /// </summary>
    public string UpdatedAt { get; set; } = string.Empty;

    /// <summary>
    /// 倒计时信息
    /// </summary>
    public CountdownInfo Countdown { get; set; } = new();

    /// <summary>
    /// 可执行操作
    /// </summary>
    public OrderActions Actions { get; set; } = new();

    /// <summary>
    /// 订单时间线
    /// </summary>
    public List<OrderTimeline> Timeline { get; set; } = new();

    /// <summary>
    /// 陪玩师详细信息
    /// </summary>
    public class CompanionDetail
    {
        /// <summary>
        /// 陪玩师ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 用户ID
        /// </summary>
        public int UserId { get; set; }

        /// <summary>
        /// 陪玩师昵称
        /// </summary>
        public string Nickname { get; set; } = string.Empty;

        /// <summary>
        /// 陪玩师头像URL
        /// </summary>
        public string AvatarUrl { get; set; } = string.Empty;

        /// <summary>
        /// 陪玩师等级
        /// </summary>
        public int? Level { get; set; } = null;

        /// <summary>
        /// 陪玩师联系电话（脱敏）
        /// </summary>
        public string Phone { get; set; } = string.Empty;

        /// <summary>
        /// 陪玩师微信号（脱敏）
        /// </summary>
        public string Wechat { get; set; } = string.Empty;
    }

    /// <summary>
    /// 游戏信息
    /// </summary>
    public class GameInfo
    {
        /// <summary>
        /// 游戏ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 游戏名称
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }

    /// <summary>
    /// 倒计时信息
    /// </summary>
    public class CountdownInfo
    {
        /// <summary>
        /// 距离服务开始剩余秒数
        /// </summary>
        public int ServiceStartIn { get; set; }

        /// <summary>
        /// 距离自动确认剩余秒数
        /// </summary>
        public int AutoConfirmIn { get; set; }
    }

    /// <summary>
    /// 可执行操作
    /// </summary>
    public class OrderActions
    {
        /// <summary>
        /// 是否可以取消
        /// </summary>
        public bool CanCancel { get; set; }

        /// <summary>
        /// 是否可以申请退款
        /// </summary>
        public bool CanRefund { get; set; }

        /// <summary>
        /// 是否可以确认完成
        /// </summary>
        public bool CanConfirm { get; set; }

        /// <summary>
        /// 是否可以评价
        /// </summary>
        public bool CanReview { get; set; }
    }

    /// <summary>
    /// 订单时间线
    /// </summary>
    public class OrderTimeline
    {
        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; } = string.Empty;

        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; } = string.Empty;

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}