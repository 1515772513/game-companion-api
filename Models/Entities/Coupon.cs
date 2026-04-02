using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 优惠券表
/// </summary>
[Table("coupons")]
[Index("Code", Name = "code", IsUnique = true)]
[Index("ExpireDate", Name = "idx_expire_date")]
[Index("Status", Name = "idx_status")]
[Index("UserId", Name = "idx_user_id")]
public partial class Coupon
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// 优惠券码
    /// </summary>
    [Column("code")]
    [StringLength(50)]
    public string Code { get; set; } = null!;

    /// <summary>
    /// 类型
    /// </summary>
    [Column("type", TypeName = "enum('discount','cash','vip')")]
    public string Type { get; set; } = null!;

    /// <summary>
    /// 金额
    /// </summary>
    [Column("amount")]
    [Precision(10, 2)]
    public decimal? Amount { get; set; }

    /// <summary>
    /// 折扣
    /// </summary>
    [Column("discount")]
    [Precision(5, 2)]
    public decimal? Discount { get; set; }

    /// <summary>
    /// 最小使用金额
    /// </summary>
    [Column("min_amount")]
    [Precision(10, 2)]
    public decimal? MinAmount { get; set; }

    /// <summary>
    /// 最大优惠金额
    /// </summary>
    [Column("max_discount")]
    [Precision(10, 2)]
    public decimal? MaxDiscount { get; set; }

    /// <summary>
    /// 过期日期
    /// </summary>
    [Column("expire_date")]
    public DateOnly ExpireDate { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [Column("status", TypeName = "enum('unused','used','expired')")]
    public string? Status { get; set; }

    /// <summary>
    /// 使用时间
    /// </summary>
    [Column("used_time", TypeName = "datetime")]
    public DateTime? UsedTime { get; set; }

    /// <summary>
    /// 使用的订单ID
    /// </summary>
    [Column("order_id")]
    public int? OrderId { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Coupons")]
    public virtual User User { get; set; } = null!;
}
