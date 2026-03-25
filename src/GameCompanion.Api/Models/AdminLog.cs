using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models;

/// <summary>
/// 管理员操作日志表
/// </summary>
[Table("admin_logs")]
public class AdminLog : BaseEntity
{
    /// <summary>
    /// 管理员ID
    /// </summary>
    public int AdminId { get; set; }

    /// <summary>
    /// 操作动作
    /// </summary>
    [MaxLength(100)]
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// 模块名称
    /// </summary>
    [MaxLength(50)]
    public string? Module { get; set; }

    /// <summary>
    /// 操作描述
    /// </summary>
    [MaxLength(500)]
    public string? Description { get; set; }

    /// <summary>
    /// 请求方法: GET/POST/PUT/DELETE
    /// </summary>
    [MaxLength(10)]
    public string? RequestMethod { get; set; }

    /// <summary>
    /// 请求URL
    /// </summary>
    [MaxLength(255)]
    public string? RequestUrl { get; set; }

    /// <summary>
    /// 请求参数 (JSON)
    /// </summary>
    public string? RequestParams { get; set; }

    /// <summary>
    /// 响应状态码
    /// </summary>
    public int? ResponseStatus { get; set; }

    /// <summary>
    /// IP地址
    /// </summary>
    [MaxLength(50)]
    public string? IpAddress { get; set; }

    /// <summary>
    /// 用户代理
    /// </summary>
    [MaxLength(500)]
    public string? UserAgent { get; set; }
}
