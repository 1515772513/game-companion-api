using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 字典类型表
/// </summary>
[Table("sys_dict_type")]
[Index("DictType", Name = "idx_dict_type", IsUnique = true)]
public partial class SysDictType
{
    /// <summary>
    /// 字典主键
    /// </summary>
    [Key]
    [Column("dict_id")]
    public long DictId { get; set; }

    /// <summary>
    /// 字典名称
    /// </summary>
    [Column("dict_name")]
    [StringLength(100)]
    public string? DictName { get; set; }

    /// <summary>
    /// 字典类型（唯一）
    /// </summary>
    [Column("dict_type")]
    [StringLength(100)]
    public string? DictType { get; set; }

    /// <summary>
    /// 状态（0正常 1停用）
    /// </summary>
    [Column("status")]
    public sbyte? Status { get; set; }

    /// <summary>
    /// 创建者
    /// </summary>
    [Column("create_by")]
    [StringLength(64)]
    public string? CreateBy { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Column("create_time", TypeName = "datetime")]
    public DateTime? CreateTime { get; set; }

    /// <summary>
    /// 更新者
    /// </summary>
    [Column("update_by")]
    [StringLength(64)]
    public string? UpdateBy { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [Column("update_time", TypeName = "datetime")]
    public DateTime? UpdateTime { get; set; }

    /// <summary>
    /// 备注
    /// </summary>
    [Column("remark")]
    [StringLength(500)]
    public string? Remark { get; set; }
}
