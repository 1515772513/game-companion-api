using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 订单评价表
/// </summary>
[Table("order_reviews")]
[Index("CompanionId", Name = "idx_companion_id")]
[Index("Rating", Name = "idx_rating")]
[Index("OrderId", Name = "uk_order_id", IsUnique = true)]
[Index("UserId", Name = "user_id")]
public partial class OrderReview
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 订单ID
    /// </summary>
    [Column("order_id")]
    public int OrderId { get; set; }

    /// <summary>
    /// 评价用户ID
    /// </summary>
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// 陪玩师ID
    /// </summary>
    [Column("companion_id")]
    public int CompanionId { get; set; }

    /// <summary>
    /// 评分1-5
    /// </summary>
    [Column("rating")]
    public int Rating { get; set; }

    /// <summary>
    /// 评价内容
    /// </summary>
    [Column("content", TypeName = "text")]
    public string? Content { get; set; }

    /// <summary>
    /// 评价标签
    /// </summary>
    [Column("tags")]
    [StringLength(255)]
    public string? Tags { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("CompanionId")]
    [InverseProperty("OrderReviews")]
    public virtual Companion Companion { get; set; } = null!;

    [ForeignKey("OrderId")]
    [InverseProperty("OrderReview")]
    public virtual Order Order { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("OrderReviews")]
    public virtual User User { get; set; } = null!;
}
