using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// VIP会员表
/// </summary>
[Table("vip_memberships")]
[Index("EndDate", Name = "idx_end_date")]
[Index("Status", Name = "idx_status")]
[Index("UserId", Name = "idx_user_id")]
public partial class VipMembership
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
    /// 会员等级
    /// </summary>
    [Column("level", TypeName = "enum('silver','gold','platinum')")]
    public string Level { get; set; } = null!;

    /// <summary>
    /// 开始日期
    /// </summary>
    [Column("start_date")]
    public DateOnly StartDate { get; set; }

    /// <summary>
    /// 结束日期
    /// </summary>
    [Column("end_date")]
    public DateOnly EndDate { get; set; }

    /// <summary>
    /// 状态
    /// </summary>
    [Column("status", TypeName = "enum('active','expired','cancelled')")]
    public string? Status { get; set; }

    /// <summary>
    /// 购买金额
    /// </summary>
    [Column("purchase_amount")]
    [Precision(10, 2)]
    public decimal? PurchaseAmount { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("VipMemberships")]
    public virtual User User { get; set; } = null!;
}
