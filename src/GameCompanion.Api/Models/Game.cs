using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models;

/// <summary>
/// 游戏表
/// </summary>
[Table("games")]
public class Game : BaseEntity
{
    /// <summary>
    /// 游戏名称
    /// </summary>
    [MaxLength(50)]
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 图标URL
    /// </summary>
    [MaxLength(255)]
    public string? IconUrl { get; set; }

    /// <summary>
    /// 封面URL
    /// </summary>
    [MaxLength(255)]
    public string? CoverUrl { get; set; }

    /// <summary>
    /// 描述
    /// </summary>
    [MaxLength(200)]
    public string? Description { get; set; }

    /// <summary>
    /// 排序
    /// </summary>
    public int? SortOrder { get; set; }

    /// <summary>
    /// 是否热门
    /// </summary>
    public int? IsHot { get; set; }

    // 导航属性
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
    public ICollection<CompanionGame> CompanionGames { get; set; } = new List<CompanionGame>();
}
