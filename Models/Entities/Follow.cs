using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 关注实体
/// </summary>
[Table("follows")]
public class Follow
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("follower_id")]
    public int FollowerId { get; set; }

    [Column("following_id")]
    public int FollowingId { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    [ForeignKey("FollowerId")]
    public virtual User? Follower { get; set; }

    [ForeignKey("FollowingId")]
    public virtual User? Following { get; set; }
}

/// <summary>
/// 用户收藏实体
/// </summary>
[Table("user_collections")]
public class UserCollection
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("title")]
    [MaxLength(100)]
    public string Title { get; set; } = string.Empty;

    [Column("description")]
    [MaxLength(500)]
    public string? Description { get; set; }

    [Column("category")]
    [MaxLength(50)]
    public string Category { get; set; } = string.Empty;

    [Column("item_id")]
    public int ItemId { get; set; }

    [Column("item_type")]
    [MaxLength(20)]
    public string ItemType { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    [ForeignKey("UserId")]
    public virtual User? User { get; set; }
}

/// <summary>
/// 交易记录实体
/// </summary>
[Table("transactions")]
public class Transaction
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("type")]
    [MaxLength(20)]
    public string Type { get; set; } = string.Empty; // 收入、支出、退款

    [Column("amount", TypeName = "decimal(10,2)")]
    public decimal Amount { get; set; }

    [Column("description")]
    [MaxLength(255)]
    public string Description { get; set; } = string.Empty;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("order_id")]
    [MaxLength(50)]
    public string? OrderId { get; set; }

    [Column("status")]
    [MaxLength(20)]
    public string Status { get; set; } = string.Empty;

    // 导航属性
    [ForeignKey("UserId")]
    public virtual User? User { get; set; }
}

/// <summary>
/// 意见反馈实体
/// </summary>
[Table("feedbacks")]
public class Feedback
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("content")]
    [MaxLength(1000)]
    public string Content { get; set; } = string.Empty;

    [Column("contact_info")]
    [MaxLength(100)]
    public string? ContactInfo { get; set; }

    [Column("type")]
    [MaxLength(20)]
    public string Type { get; set; } = "建议";

    [Column("status")]
    [MaxLength(20)]
    public string Status { get; set; } = "待处理";

    [Column("response")]
    [MaxLength(1000)]
    public string? Response { get; set; }

    [Column("response_at")]
    public DateTime? ResponseAt { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    [ForeignKey("UserId")]
    public virtual User? User { get; set; }
}

/// <summary>
/// 陪玩师申请实体
/// </summary>
[Table("companion_applications")]
public class CompanionApplication
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("game_category")]
    [MaxLength(100)]
    public string GameCategory { get; set; } = string.Empty;

    [Column("skill_level")]
    [MaxLength(100)]
    public string SkillLevel { get; set; } = string.Empty;

    [Column("self_introduction")]
    [MaxLength(500)]
    public string SelfIntroduction { get; set; } = string.Empty;

    [Column("hourly_rate")]
    public decimal HourlyRate { get; set; }

    [Column("available_time")]
    [MaxLength(100)]
    public string AvailableTime { get; set; } = string.Empty;

    [Column("status")]
    [MaxLength(20)]
    public string Status { get; set; } = "待审核";

    [Column("admin_notes")]
    [MaxLength(500)]
    public string? AdminNotes { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    [ForeignKey("UserId")]
    public virtual User? User { get; set; }
}
