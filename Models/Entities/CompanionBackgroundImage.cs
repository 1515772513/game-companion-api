using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 陪玩师背景墙轮播图关联表
/// </summary>
[Table("companion_background_images")]
[Index("CompanionId", Name = "idx_companion_id")]
[Index("FileId", Name = "idx_file_id")]
public partial class CompanionBackgroundImage
{
    /// <summary>
    /// 主键UUID
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// 陪玩师ID（关联companions表id）
    /// </summary>
    [Column("companion_id")]
    public int CompanionId { get; set; }

    /// <summary>
    /// 文件ID（关联sys_file表id）
    /// </summary>
    [Column("file_id")]
    public Guid FileId { get; set; }

    /// <summary>
    /// 排序权重（越小越靠前，轮播顺序）
    /// </summary>
    [Column("sort")]
    public int Sort { get; set; }

    /// <summary>
    /// 创建时间
    /// </summary>
    [Column("create_time", TypeName = "datetime")]
    public DateTime CreateTime { get; set; }

    /// <summary>
    /// 更新时间
    /// </summary>
    [Column("update_time", TypeName = "datetime")]
    public DateTime UpdateTime { get; set; }

    /// <summary>
    /// 是否删除 0=否 1=是
    /// </summary>
    [Column("is_deleted")]
    public sbyte IsDeleted { get; set; }

    [ForeignKey("CompanionId")]
    [InverseProperty("CompanionBackgroundImages")]
    public virtual Companion Companion { get; set; } = null!;

    [ForeignKey("FileId")]
    [InverseProperty("CompanionBackgroundImages")]
    public virtual SysFile File { get; set; } = null!;
}
