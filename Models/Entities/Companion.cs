using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 陪玩师实体
/// </summary>
[Table("companions")]
public class Companion
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("nickname")]
    [MaxLength(50)]
    public string Nickname { get; set; } = string.Empty;

    [Column("avatar")]
    [MaxLength(255)]
    public string? Avatar { get; set; }

    [Column("real_name")]
    [MaxLength(50)]
    public string? RealName { get; set; }

    [Column("id_card")]
    [MaxLength(18)]
    public string? IdCard { get; set; }

    [Column("id_card_front")]
    [MaxLength(255)]
    public string? IdCardFront { get; set; }

    [Column("id_card_back")]
    [MaxLength(255)]
    public string? IdCardBack { get; set; }

    [Column("phone")]
    [MaxLength(11)]
    public string Phone { get; set; } = string.Empty;

    [Column("level")]
    [MaxLength(20)]
    public string? Level { get; set; } = "银牌"; // 银牌、金牌、钻石、王者

    [Column("service_type")]
    [MaxLength(20)]
    public string? ServiceType { get; set; } = "技术陪玩"; // 技术陪玩、娱乐陪玩

    [Column("price_per_game", TypeName = "decimal(10,2)")]
    public decimal PricePerGame { get; set; } = 0;

    [Column("price_per_hour", TypeName = "decimal(10,2)")]
    public decimal? PricePerHour { get; set; }

    [Column("rating", TypeName = "decimal(3,2)")]
    public decimal? Rating { get; set; }

    [Column("total_orders")]
    public int? TotalOrders { get; set; }

    [Column("good_review_rate", TypeName = "decimal(5,2)")]
    public decimal? GoodReviewRate { get; set; }

    [Column("bio")]
    public string? Bio { get; set; }

    [Column("tags")]
    [MaxLength(500)]
    public string? Tags { get; set; }

    [Column("status")]
    [MaxLength(20)]
    public string? Status { get; set; } = "审核中"; // 审核中、已认证、已拒绝

    [Column("reject_reason")]
    [MaxLength(255)]
    public string? RejectReason { get; set; }

    [Column("online_status")]
    [MaxLength(20)]
    public string? OnlineStatus { get; set; } = "离线"; // 在线、离线、忙碌

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<CompanionGame> Games { get; set; } = new List<CompanionGame>();
    public virtual ICollection<CompanionGame> CompanionGames { get; set; } = new List<CompanionGame>();
    public virtual ICollection<OrderReview> OrderReviews { get; set; } = new List<OrderReview>();
}
