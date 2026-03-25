using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace GameCompanion.Api.Models;

/// <summary>
/// 系统配置表
/// </summary>
[Table("system_configs")]
public class SystemConfig : BaseEntity
{
    /// <summary>
    /// 配置键
    /// </summary>
    [MaxLength(100)]
    public string ConfigKey { get; set; } = string.Empty;

    /// <summary>
    /// 配置值
    /// </summary>
    public string? ConfigValue { get; set; }

    /// <summary>
    /// 配置类型: string字符串, number数字, boolean布尔, json JSON
    /// </summary>
    [MaxLength(20)]
    public string? ConfigType { get; set; }

    /// <summary>
    /// 配置分组: basic基础, payment支付, sms短信, storage存储
    /// </summary>
    [MaxLength(50)]
    public string? ConfigGroup { get; set; }

    /// <summary>
    /// 配置说明
    /// </summary>
    [MaxLength(200)]
    public string? Description { get; set; }
}
