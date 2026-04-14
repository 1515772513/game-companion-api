using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Models.Entities;

/// <summary>
/// 交易记录表
/// </summary>
[Table("transactions")]
public partial class Transaction
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("user_id")]
    public int UserId { get; set; }

    [Column("type")]
    [StringLength(20)]
    public string Type { get; set; } = null!;

    [Column("amount")]
    [Precision(10, 2)]
    public decimal Amount { get; set; }

    [Column("description")]
    [StringLength(255)]
    public string Description { get; set; } = null!;

    [Column("created_at", TypeName = "datetime")]
    public DateTime CreatedAt { get; set; }

    [Column("order_id")]
    [StringLength(50)]
    public string? OrderId { get; set; }

    [Column("status")]
    [StringLength(20)]
    public string Status { get; set; } = null!;
}
