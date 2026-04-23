using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 用户表
/// </summary>
[Table("users")]
public partial class User
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 用户名
    /// </summary>
    [Column("username")]
    [StringLength(50)]
    public string? Username { get; set; }

    /// <summary>
    /// 密码
    /// </summary>
    [Column("password")]
    [StringLength(255)]
    public string? Password { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    [Column("nickname")]
    [StringLength(50)]
    public string? Nickname { get; set; }

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
    /// 手机号
    /// </summary>
    [Column("phone")]
    [StringLength(11)]
    public string? Phone { get; set; }

    /// <summary>
    /// 头像URL
    /// </summary>
    [Column("avatar")]
    [StringLength(255)]
    public string? Avatar { get; set; }

    /// <summary>
    /// 性别 0=未知 1=男 2=女
    /// </summary>
    [Column("gender")]
    public int? Gender { get; set; }

    /// <summary>
    /// 年龄
    /// </summary>
    [Column("age")]
    public int Age { get; set; }

    /// <summary>
    /// 姓名
    /// </summary>
    [Column("name")]
    [StringLength(100)]
    public string? Name { get; set; }

    /// <summary>
    /// 用户ID(如10086888)
    /// </summary>
    [Column("user_id")]
    [StringLength(20)]
    public string? UserId { get; set; }

    /// <summary>
    /// 个人简介
    /// </summary>
    [Column("bio")]
    [StringLength(500)]
    public string? Bio { get; set; }

    /// <summary>
    /// VIP等级 0=普通 1=普通会员 2=VIP会员 3=SVIP会员
    /// </summary>
    [Column("vip_level")]
    public int? VipLevel { get; set; }

    /// <summary>
    /// VIP过期时间
    /// </summary>
    [Column("vip_expire_date")]
    public DateOnly? VipExpireDate { get; set; }

    /// <summary>
    /// 积分
    /// </summary>
    [Column("points")]
    public int? Points { get; set; }

    /// <summary>
    /// 余额
    /// </summary>
    [Column("balance")]
    [Precision(10, 2)]
    public decimal? Balance { get; set; }

    /// <summary>
    /// 状态:1=正常,0=禁用
    /// </summary>
    [Column("status")]
    public bool? Status { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    [Column("last_login_time", TypeName = "datetime")]
    public DateTime? LastLoginTime { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// 0=正常,1=封禁
    /// </summary>
    [Column("is_blocked")]
    public bool? IsBlocked { get; set; }

    /// <summary>
    /// 0=否,1=是
    /// </summary>
    [Column("is_admin")]
    public bool? IsAdmin { get; set; }

    /// <summary>
    /// 微信openid
    /// </summary>
    [Column("openid")]
    [StringLength(100)]
    public string? Openid { get; set; }

    /// <summary>
    /// 生日
    /// </summary>
    [Column(TypeName = "datetime")]
    public DateTime? Birthday { get; set; }

    [InverseProperty("User")]
    public virtual ICollection<Collection> Collections { get; set; } = new List<Collection>();

    [InverseProperty("User")]
    public virtual ICollection<CompanionRequest> CompanionRequests { get; set; } = new List<CompanionRequest>();

    [InverseProperty("User")]
    public virtual ICollection<Companion> Companions { get; set; } = new List<Companion>();

    [InverseProperty("User")]
    public virtual ICollection<Conversation> Conversations { get; set; } = new List<Conversation>();

    [InverseProperty("User")]
    public virtual ICollection<Coupon> Coupons { get; set; } = new List<Coupon>();

    [InverseProperty("User")]
    public virtual ICollection<Draft> Drafts { get; set; } = new List<Draft>();

    [InverseProperty("User")]
    public virtual ICollection<Feedback> Feedbacks { get; set; } = new List<Feedback>();

    [InverseProperty("Follower")]
    public virtual ICollection<Follow> FollowFollowers { get; set; } = new List<Follow>();

    [InverseProperty("Following")]
    public virtual ICollection<Follow> FollowFollowings { get; set; } = new List<Follow>();

    [InverseProperty("Receiver")]
    public virtual ICollection<Message> MessageReceivers { get; set; } = new List<Message>();

    [InverseProperty("Sender")]
    public virtual ICollection<Message> MessageSenders { get; set; } = new List<Message>();

    [InverseProperty("User")]
    public virtual ICollection<Notification> Notifications { get; set; } = new List<Notification>();

    [InverseProperty("User")]
    public virtual ICollection<OrderReview> OrderReviews { get; set; } = new List<OrderReview>();

    [InverseProperty("User")]
    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    [InverseProperty("User")]
    public virtual ICollection<PostComment> PostComments { get; set; } = new List<PostComment>();

    [InverseProperty("User")]
    public virtual ICollection<PostLike> PostLikes { get; set; } = new List<PostLike>();

    [InverseProperty("User")]
    public virtual ICollection<Post> Posts { get; set; } = new List<Post>();

    [InverseProperty("User")]
    public virtual ICollection<PowerLeveling> PowerLevelings { get; set; } = new List<PowerLeveling>();

    [InverseProperty("User")]
    public virtual ICollection<UserCollection> UserCollections { get; set; } = new List<UserCollection>();

    [InverseProperty("User")]
    public virtual ICollection<UserSetting> UserSettings { get; set; } = new List<UserSetting>();

    [InverseProperty("User")]
    public virtual ICollection<VipMembership> VipMemberships { get; set; } = new List<VipMembership>();
}
