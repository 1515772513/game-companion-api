using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 陪玩师表
/// </summary>
[Table("companions")]
[Index("Level", Name = "idx_level")]
[Index("Rating", Name = "idx_rating")]
[Index("Status", Name = "idx_status")]
[Index("UserId", Name = "idx_user_id")]
public partial class Companion
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
    /// 陪玩师昵称
    /// </summary>
    [Column("nickname")]
    [StringLength(50)]
    public string Nickname { get; set; } = null!;

    /// <summary>
    /// 真实姓名
    /// </summary>
    [Column("real_name")]
    [StringLength(50)]
    public string? RealName { get; set; }

    /// <summary>
    /// 身份证号
    /// </summary>
    [Column("id_card")]
    [StringLength(18)]
    public string? IdCard { get; set; }

    /// <summary>
    /// 身份证正面照片
    /// </summary>
    [Column("id_card_front")]
    [StringLength(255)]
    public string? IdCardFront { get; set; }

    /// <summary>
    /// 身份证反面照片
    /// </summary>
    [Column("id_card_back")]
    [StringLength(255)]
    public string? IdCardBack { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    [Column("phone")]
    [StringLength(11)]
    public string Phone { get; set; } = null!;

    /// <summary>
    /// 等级
    /// </summary>
    [Column("level")]
    public int? Level { get; set; }

    /// <summary>
    /// 评分(0.00-5.00)
    /// </summary>
    [Column("rating")]
    [Precision(3, 2)]
    public decimal? Rating { get; set; }

    /// <summary>
    /// 总订单数
    /// </summary>
    [Column("total_orders")]
    public int? TotalOrders { get; set; }

    /// <summary>
    /// 好评率
    /// </summary>
    [Column("good_review_rate")]
    [Precision(5, 2)]
    public decimal? GoodReviewRate { get; set; }

    /// <summary>
    /// 个人简介
    /// </summary>
    [Column("bio", TypeName = "text")]
    public string? Bio { get; set; }

    /// <summary>
    /// 标签(多个用逗号分隔)
    /// </summary>
    [Column("tags")]
    [StringLength(500)]
    public string? Tags { get; set; }

    /// <summary>
    /// 0=待审核,1=审核通过,2=审核拒绝
    /// </summary>
    [Column("status")]
    public int? Status { get; set; }

    /// <summary>
    /// 拒绝原因
    /// </summary>
    [Column("reject_reason")]
    [StringLength(255)]
    public string? RejectReason { get; set; }

    /// <summary>
    /// 在线状态
    /// </summary>
    [Column("online_status", TypeName = "enum('online','offline','busy')")]
    public string? OnlineStatus { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    [InverseProperty("Companion")]
    public virtual ICollection<CompanionBackgroundImage> CompanionBackgroundImages { get; set; } = new List<CompanionBackgroundImage>();

    [InverseProperty("Companion")]
    public virtual ICollection<CompanionGame> CompanionGames { get; set; } = new List<CompanionGame>();

    [InverseProperty("Companion")]
    public virtual ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

    [InverseProperty("Companion")]
    public virtual ICollection<OrderReview> OrderReviews { get; set; } = new List<OrderReview>();

    [InverseProperty("Companion")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [ForeignKey("UserId")]
    [InverseProperty("Companions")]
    public virtual User User { get; set; } = null!;
}
