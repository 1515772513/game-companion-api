using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 订单表
/// </summary>
[Table("orders")]
[Index("CompanionId", Name = "idx_companion_id")]
[Index("OrderNo", Name = "idx_order_no", IsUnique = true)]
[Index("PlayTime", Name = "idx_play_time")]
[Index("Status", Name = "idx_status")]
[Index("UserId", Name = "idx_user_id")]
public partial class Order
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 订单号
    /// </summary>
    [Column("order_no")]
    [StringLength(50)]
    public string OrderNo { get; set; } = null!;

    /// <summary>
    /// 下单用户ID
    /// </summary>
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// 陪玩师ID
    /// </summary>
    [Column("companion_id")]
    public int CompanionId { get; set; }

    /// <summary>
    /// 游戏ID
    /// </summary>
    [Column("game_id")]
    public int GameId { get; set; }

    /// <summary>
    /// 服务类型
    /// </summary>
    [Column("service_type", TypeName = "enum('companion','power_leveling')")]
    public string? ServiceType { get; set; }

    /// <summary>
    /// 预约时间
    /// </summary>
    [Column("play_time", TypeName = "datetime")]
    public DateTime? PlayTime { get; set; }

    /// <summary>
    /// 时长类型
    /// </summary>
    [Column("duration_type", TypeName = "enum('game','hour')")]
    public string? DurationType { get; set; }

    /// <summary>
    /// 时长数量
    /// </summary>
    [Column("duration_value")]
    public int DurationValue { get; set; }

    /// <summary>
    /// 单价
    /// </summary>
    [Column("unit_price")]
    [Precision(10, 2)]
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 总价
    /// </summary>
    [Column("total_price")]
    [Precision(10, 2)]
    public decimal TotalPrice { get; set; }

    /// <summary>
    /// 优惠金额
    /// </summary>
    [Column("discount_amount")]
    [Precision(10, 2)]
    public decimal? DiscountAmount { get; set; }

    /// <summary>
    /// 实付金额
    /// </summary>
    [Column("final_price")]
    [Precision(10, 2)]
    public decimal FinalPrice { get; set; }

    /// <summary>
    /// 备注信息
    /// </summary>
    [Column("remark", TypeName = "text")]
    public string? Remark { get; set; }

    /// <summary>
    /// 订单状态
    /// </summary>
    [Column("status", TypeName = "enum('pending','paid','in_progress','completed','cancelled','refunded')")]
    public string? Status { get; set; }

    /// <summary>
    /// 支付时间
    /// </summary>
    [Column("pay_time", TypeName = "datetime")]
    public DateTime? PayTime { get; set; }

    /// <summary>
    /// 服务开始时间
    /// </summary>
    [Column("start_time", TypeName = "datetime")]
    public DateTime? StartTime { get; set; }

    /// <summary>
    /// 服务结束时间
    /// </summary>
    [Column("end_time", TypeName = "datetime")]
    public DateTime? EndTime { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("CompanionId")]
    [InverseProperty("Orders")]
    public virtual Companion Companion { get; set; } = null!;

    [InverseProperty("Order")]
    public virtual OrderReview? OrderReview { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Orders")]
    public virtual User User { get; set; } = null!;
}
