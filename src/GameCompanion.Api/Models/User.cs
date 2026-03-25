using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models;

/// <summary>
/// 用户表
/// </summary>
[Table("users")]
public class User : BaseEntity
{
    /// <summary>
    /// 用户名
    /// </summary>
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

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
    /// 性别: 0未知, 1男, 2女
    /// </summary>
    public int? Gender { get; set; }

    /// <summary>
    /// 生日
    /// </summary>
    public DateTime? Birthday { get; set; }

    /// <summary>
    /// 所在地
    /// </summary>
    [MaxLength(100)]
    public string? Location { get; set; }

    /// <summary>
    /// 个性签名
    /// </summary>
    [MaxLength(200)]
    public string? Bio { get; set; }

    /// <summary>
    /// 手机号
    /// </summary>
    [MaxLength(20)]
    public string? Phone { get; set; }

    /// <summary>
    /// 手机认证状态
    /// </summary>
    public int? PhoneVerified { get; set; }

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
    /// 实名认证状态
    /// </summary>
    public int? IdCardVerified { get; set; }

    /// <summary>
    /// 身份证正面照
    /// </summary>
    [MaxLength(255)]
    public string? IdCardFrontUrl { get; set; }

    /// <summary>
    /// 身份证反面照
    /// </summary>
    [MaxLength(255)]
    public string? IdCardBackUrl { get; set; }

    /// <summary>
    /// 账号状态: 0正常, 1禁用, 2封禁
    /// </summary>
    public int AccountStatus { get; set; } = 0;

    /// <summary>
    /// VIP等级: 0普通, 1VIP
    /// </summary>
    public int VipLevel { get; set; } = 0;

    /// <summary>
    /// VIP过期时间
    /// </summary>
    public DateTime? VipExpireTime { get; set; }

    /// <summary>
    /// 积分
    /// </summary>
    public int Points { get; set; } = 0;

    /// <summary>
    /// 余额
    /// </summary>
    public decimal Balance { get; set; } = 0;

    // 导航属性
    public ICollection<Companion> Companions { get; set; } = new List<Companion>();
    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<Post> Posts { get; set; } = new List<Post>();
}
