using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models;

/// <summary>
/// 陪玩师表
/// </summary>
[Table("companions")]
public class Companion : BaseEntity
{
    /// <summary>
    /// 用户ID
    /// </summary>
    public int UserId { get; set; }

    /// <summary>
    /// 关联用户
    /// </summary>
    [ForeignKey(nameof(UserId))]
    public User? User { get; set; }

    /// <summary>
    /// 真实姓名
    /// </summary>
    [MaxLength(50)]
    public string? RealName { get; set; }

    /// <summary>
    /// 身份证号
    /// </summary>
    [MaxLength(18)]
    public string? IdCard { get; set; }

    /// <summary>
    /// 身份证正面照URL
    /// </summary>
    [MaxLength(255)]
    public string? IdCardFrontUrl { get; set; }

    /// <summary>
    /// 身份证反面照URL
    /// </summary>
    [MaxLength(255)]
    public string? IdCardBackUrl { get; set; }

    /// <summary>
    /// 联系电话
    /// </summary>
    [MaxLength(20)]
    public string? Phone { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [MaxLength(100)]
    public string Nickname { get; set; } = string.Empty;

    /// <summary>
    /// 头像URL
    /// </summary>
    [MaxLength(255)]
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// 等级: 银牌/金牌/钻石/王者
    /// </summary>
    [MaxLength(20)]
    public string? Level { get; set; }

    /// <summary>
    /// 服务类型
    /// </summary>
    [MaxLength(50)]
    public string? ServiceType { get; set; }

    /// <summary>
    /// 价格
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// 评分
    /// </summary>
    public decimal? Rating { get; set; }

    /// <summary>
    /// 接单数
    /// </summary>
    public int? OrderCount { get; set; }

    /// <summary>
    /// 评价数
    /// </summary>
    public int? RatingCount { get; set; }

    /// <summary>
    /// 好评率
    /// </summary>
    public decimal? PositiveRate { get; set; }

    /// <summary>
    /// 个人简介
    /// </summary>
    public string? Bio { get; set; }

    /// <summary>
    /// 标签 (JSON)
    /// </summary>
    public string? Tags { get; set; }

    /// <summary>
    /// 擅长游戏 (JSON)
    /// </summary>
    public string? Games { get; set; }

    /// <summary>
    /// 游戏段位/等级
    /// </summary>
    [MaxLength(100)]
    public string? GameRank { get; set; }

    /// <summary>
    /// 认证状态: 0待审核, 1已通过, 2已拒绝
    /// </summary>
    public int CertificationStatus { get; set; } = 0;

    /// <summary>
    /// 认证时间
    /// </summary>
    public DateTime? CertificationTime { get; set; }

    /// <summary>
    /// 认证申请时间
    /// </summary>
    public DateTime? CertificationApplyTime { get; set; }

    /// <summary>
    /// 认证审核管理员ID
    /// </summary>
    public int? CertificationAuditAdminId { get; set; }

    /// <summary>
    /// 认证拒绝原因
    /// </summary>
    [MaxLength(200)]
    public string? CertificationRejectReason { get; set; }

    /// <summary>
    /// 在线状态: 0离线, 1在线, 2忙碌
    /// </summary>
    public int? OnlineStatus { get; set; }

    /// <summary>
    /// 是否认证
    /// </summary>
    public int IsVerified { get; set; } = 0;

    // 导航属性
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
