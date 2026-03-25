using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models;

/// <summary>
/// 管理员表
/// </summary>
[Table("admins")]
public class Admin : BaseEntity
{
    /// <summary>
    /// 管理员账号
    /// </summary>
    [MaxLength(50)]
    public string Username { get; set; } = string.Empty;

    /// <summary>
    /// 密码(加密)
    /// </summary>
    [MaxLength(255)]
    public string Password { get; set; } = string.Empty;

    /// <summary>
    /// 真实姓名
    /// </summary>
    [MaxLength(50)]
    public string? RealName { get; set; }

    /// <summary>
    /// 头像URL
    /// </summary>
    [MaxLength(255)]
    public string? AvatarUrl { get; set; }

    /// <summary>
    /// 角色: super_admin超级管理员, admin管理员, editor审核员
    /// </summary>
    [MaxLength(50)]
    public string? Role { get; set; }

    /// <summary>
    /// 权限列表 (JSON)
    /// </summary>
    public string? Permissions { get; set; }

    /// <summary>
    /// 最后登录时间
    /// </summary>
    public DateTime? LastLoginTime { get; set; }

    /// <summary>
    /// 最后登录IP
    /// </summary>
    [MaxLength(50)]
    public string? LastLoginIp { get; set; }

    /// <summary>
    /// 账号状态: 0正常, 1禁用
    /// </summary>
    public int AccountStatus { get; set; } = 0;
}
