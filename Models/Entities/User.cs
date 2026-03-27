using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 用户实体
/// </summary>
[Table("users")]
public class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("username")]
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    [Column("password")]
    [MaxLength(255)]
    public string Password { get; set; } = string.Empty;

    [Column("nickname")]
    [MaxLength(50)]
    public string Nickname { get; set; } = string.Empty;

    [Column("real_name")]
    [MaxLength(50)]
    public string? RealName { get; set; }

    [Column("id_card")]
    [MaxLength(18)]
    public string? IdCard { get; set; }

    [Column("phone")]
    [MaxLength(11)]
    public string Phone { get; set; } = string.Empty;

    [Column("avatar")]
    [MaxLength(255)]
    public string? Avatar { get; set; }

    [Column("gender")]
    [MaxLength(20)]
    public string? Gender { get; set; } = "未知"; // 未知、男、女

    [Column("age")]
    public int Age { get; set; } = 0;

    [Column("name")]
    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Column("user_id")]
    [MaxLength(20)]
    public string? UserIdStr { get; set; }

    [Column("bio")]
    [MaxLength(500)]
    public string? Bio { get; set; }

    [Column("vip_level")]
    [MaxLength(20)]
    public string? VipLevel { get; set; } = "普通"; // 普通、VIP1、VIP2、VIP3

    [Column("vip_expire_date")]
    public DateTime? VipExpireDate { get; set; }

    [Column("points")]
    public int Points { get; set; } = 0;

    [Column("balance", TypeName = "decimal(10,2)")]
    public decimal Balance { get; set; } = 0;

    [Column("status")]
    [MaxLength(20)]
    public string? Status { get; set; } = "正常"; // 正常、禁用

    [Column("is_blocked")]
    public int? IsBlocked { get; set; } = 0;

    [Column("last_login_time")]
    public DateTime? LastLoginTime { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    [Column("is_admin")]
    public int? IsAdmin { get; set; } = 0;

    // 导航属性
    public virtual Companion? Companion { get; set; }
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();
    public virtual ICollection<Message> SentMessages { get; set; } = new List<Message>();
    public virtual ICollection<Message> ReceivedMessages { get; set; } = new List<Message>();
}
