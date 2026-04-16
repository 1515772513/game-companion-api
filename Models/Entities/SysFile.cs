using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 文件上传记录表
/// </summary>
[Table("sys_file")]
[Index("FileUrl", Name = "idx_file_url")]
[Index("UploadUser", Name = "idx_upload_user")]
public partial class SysFile
{
    /// <summary>
    /// 主键UUID
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// 原始文件名
    /// </summary>
    [Column("file_name")]
    [StringLength(255)]
    public string FileName { get; set; } = null!;

    /// <summary>
    /// 文件访问URL
    /// </summary>
    [Column("file_url")]
    [StringLength(512)]
    public string FileUrl { get; set; } = null!;

    /// <summary>
    /// 文件物理路径
    /// </summary>
    [Column("file_path")]
    [StringLength(512)]
    public string FilePath { get; set; } = null!;

    /// <summary>
    /// 文件大小（字节）
    /// </summary>
    [Column("file_size")]
    public long FileSize { get; set; }

    /// <summary>
    /// 文件后缀
    /// </summary>
    [Column("file_ext")]
    [StringLength(50)]
    public string? FileExt { get; set; }

    /// <summary>
    /// 文件类型
    /// </summary>
    [Column("content_type")]
    [StringLength(100)]
    public string? ContentType { get; set; }

    /// <summary>
    /// 上传人ID
    /// </summary>
    [Column("upload_user")]
    public long? UploadUser { get; set; }

    /// <summary>
    /// 上传平台
    /// </summary>
    [Column("upload_platform")]
    [StringLength(50)]
    public string? UploadPlatform { get; set; }

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

    [InverseProperty("File")]
    public virtual ICollection<CompanionBackgroundImage> CompanionBackgroundImages { get; set; } = new List<CompanionBackgroundImage>();
}
