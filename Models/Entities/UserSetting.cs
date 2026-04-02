using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 用户设置表
/// </summary>
[Table("user_settings")]
[Index("UserId", Name = "idx_user_id")]
[Index("UserId", "SettingKey", Name = "uk_user_key", IsUnique = true)]
public partial class UserSetting
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    /// <summary>
    /// 用户ID
    /// </summary>
    [Column("user_id")]
    public int UserId { get; set; }

    /// <summary>
    /// 设置键
    /// </summary>
    [Column("setting_key")]
    [StringLength(50)]
    public string SettingKey { get; set; } = null!;

    /// <summary>
    /// 设置值
    /// </summary>
    [Column("setting_value")]
    [StringLength(255)]
    public string? SettingValue { get; set; }

    [Column("created_at", TypeName = "timestamp")]
    public DateTime? CreatedAt { get; set; }

    [Column("updated_at", TypeName = "timestamp")]
    public DateTime? UpdatedAt { get; set; }

    [ForeignKey("UserId")]
    [InverseProperty("UserSettings")]
    public virtual User User { get; set; } = null!;
}
