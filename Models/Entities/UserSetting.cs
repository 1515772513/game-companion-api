using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 用户设置实体
/// </summary>
[Table("user_settings")]
public class UserSetting
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("show_online_status")]
    public int? ShowOnlineStatus { get; set; } = 1;

    [Column("allow_stranger_message")]
    public int? AllowStrangerMessage { get; set; } = 1;

    [Column("show_game_activity")]
    public int? ShowGameActivity { get; set; } = 1;

    [Column("order_notification")]
    public int? OrderNotification { get; set; } = 1;

    [Column("message_notification")]
    public int? MessageNotification { get; set; } = 1;

    [Column("promotion_notification")]
    public int? PromotionNotification { get; set; } = 0;

    [Column("system_notification")]
    public int? SystemNotification { get; set; } = 1;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}
