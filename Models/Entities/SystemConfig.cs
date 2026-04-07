using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 系统全局配置表
/// </summary>
[Table("system_config")]
[Index("ConfigKey", Name = "uk_config_key", IsUnique = true)]
public partial class SystemConfig
{
    /// <summary>
    /// 主键
    /// </summary>
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 配置键（唯一）
    /// </summary>
    [Column("config_key")]
    [StringLength(100)]
    public string ConfigKey { get; set; } = null!;

    /// <summary>
    /// 配置值
    /// </summary>
    [Column("config_value", TypeName = "text")]
    public string? ConfigValue { get; set; }

    /// <summary>
    /// 类型：string/json/banner/number
    /// </summary>
    [Column("config_type")]
    [StringLength(50)]
    public string? ConfigType { get; set; }

    /// <summary>
    /// 配置名称
    /// </summary>
    [Column("name")]
    [StringLength(100)]
    public string? Name { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Column("remark")]
    [StringLength(255)]
    public string? Remark { get; set; }

    [Column("created_at", TypeName = "datetime")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "datetime")]
    public DateTime? UpdatedAt { get; set; }
}
