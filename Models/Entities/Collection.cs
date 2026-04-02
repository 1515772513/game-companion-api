using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 收藏表
/// </summary>
[Table("collections")]
[Index("TargetType", "TargetId", Name = "idx_target")]
[Index("UserId", Name = "idx_user_id")]
[Index("UserId", "TargetType", "TargetId", Name = "uk_user_target", IsUnique = true)]
public partial class Collection
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 收藏用户ID
    /// </summary>
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// 收藏类型
    /// </summary>
    [Column("target_type", TypeName = "enum('post','companion')")]
    public string TargetType { get; set; } = null!;

    /// <summary>
    /// 目标ID
    /// </summary>
    [Column("target_id")]
    public int TargetId { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("Collections")]
    public virtual User User { get; set; } = null!;
}
