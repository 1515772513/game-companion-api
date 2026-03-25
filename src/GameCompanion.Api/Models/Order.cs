using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models;

/// <summary>
/// 订单表
/// </summary>
[Table("orders")]
public class Order : BaseEntity
{
    /// <summary>
    /// 订单号
    /// </summary>
    [MaxLength(32)]
    public string OrderNo { get; set; } = string.Empty;

    /// <summary>
    /// 用户ID
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 关联用户
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    /// <summary>
    /// 陪玩师ID
    /// </summary>
    public int? CompanionId { get; set; }

    /// <summary>
    /// 关联陪玩师
    /// </summary>
    [ForeignKey(nameof(CompanionId))]
    public Companion? Companion { get; set; }

    /// <summary>
    /// 订单类型: 1陪玩, 2代练
    /// </summary>
    public int? OrderType { get; set; }

    /// <summary>
    /// 游戏ID
    /// </summary>
    public int GameId { get; set; }

    /// <summary>
    /// 关联游戏
    /// </summary>
    [ForeignKey(nameof(GameId))]
    public Game? Game { get; set; }

    /// <summary>
    /// 游戏段位/等级
    /// </summary>
    [MaxLength(100)]
    public string? GameRank { get; set; }

    /// <summary>
    /// 服务时间
    /// </summary>
    public DateTime? ServiceTime { get; set; }

    /// <summary>
    /// 服务时长
    /// </summary>
    [MaxLength(50)]
    public string? Duration { get; set; }

    /// <summary>
    /// 特殊要求
    /// </summary>
    public string? SpecialRequirements { get; set; }

    /// <summary>
    /// 单价
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 平台服务费
    /// </summary>
    public decimal? ServiceFee { get; set; }

    /// <summary>
    /// 总金额
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 订单状态: 1待付款, 2进行中, 3已完成, 4已取消, 5退款中
    /// </summary>
    public int Status { get; set; } = 1;

    /// <summary>
    /// 支付状态: 0未支付, 1已支付, 2退款中, 3已退款
    /// </summary>
    public int PaymentStatus { get; set; } = 0;

    /// <summary>
    /// 退款状态: 0无退款, 1申请中, 2已同意, 3已拒绝
    /// </summary>
    public int RefundStatus { get; set; } = 0;

    /// <summary>
    /// 退款申请时间
    /// </summary>
    public DateTime? RefundApplyTime { get; set; }

    /// <summary>
    /// 退款金额
    /// </summary>
    public decimal? RefundAmount { get; set; }

    /// <summary>
    /// 退款审核时间
    /// </summary>
    public DateTime? RefundAuditTime { get; set; }

    /// <summary>
    /// 退款审核管理员ID
    /// </summary>
    public int? RefundAuditAdminId { get; set; }

    /// <summary>
    /// 退款拒绝原因
    /// </summary>
    [MaxLength(200)]
    public string? RefundRejectReason { get; set; }

    /// <summary>
    /// 支付时间
    /// </summary>
    public DateTime? PaymentTime { get; set; }

    /// <summary>
    /// 完成时间
    /// </summary>
    public DateTime? CompletedTime { get; set; }

    /// <summary>
    /// 取消原因
    /// </summary>
    [MaxLength(200)]
    public string? CancelReason { get; set; }

    /// <summary>
    /// 退款原因
    /// </summary>
    public string? RefundReason { get; set; }
}
