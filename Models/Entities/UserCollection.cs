using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models.Entities;

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
