using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 字典数据表
/// </summary>
[Table("sys_dict_data")]
[Index("DictType", Name = "idx_dict_type")]
public partial class SysDictDatum
{
    /// <summary>
    /// 字典编码
    /// </summary>
    [Key]
    [Column("dict_code")]
    public long DictCode { get; set; }

    /// <summary>
    /// 字典排序
    /// </summary>
    [Column("dict_sort")]
    public int? DictSort { get; set; }

    /// <summary>
    /// 字典标签
    /// </summary>
    [Column("dict_label")]
    [StringLength(100)]
    public string? DictLabel { get; set; }

    /// <summary>
    /// 字典键值
    /// </summary>
    [Column("dict_value")]
    [StringLength(100)]
    public string? DictValue { get; set; }

    /// <summary>
    /// 字典类型
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
