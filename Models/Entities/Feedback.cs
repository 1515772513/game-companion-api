using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

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

    [Column("feedback_type")]
    [MaxLength(20)]
    public string? FeedbackType { get; set; }

    [Column("content")]
    public string Content { get; set; } = string.Empty;

    [Column("contact")]
    [MaxLength(100)]
    public string? Contact { get; set; }

    [Column("images")]
    [MaxLength(1000)]
    public string? Images { get; set; }

    [Column("status")]
    [MaxLength(20)]
    public string? Status { get; set; } = "待处理";

    [Column("reply")]
    public string? Reply { get; set; }

    [Column("created_at")]
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

    // 导航属性
    [ForeignKey("UserId")]
    public virtual User User { get; set; } = null!;
}
