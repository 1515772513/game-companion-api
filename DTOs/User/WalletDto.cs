using System.ComponentModel.DataAnnotations;

namespace GameCompanion.Api.DTOs.User;

/// <summary>
/// 钱包信息DTO
/// </summary>
public class WalletDto
{
    public int UserId { get; set; }
    public decimal Balance { get; set; }
    public int Points { get; set; }
    public int? VipLevel { get; set; }
    public DateTime? VipExpireDate { get; set; }

    public List<TransactionDto> Transactions { get; set; } = new();
}

/// <summary>
/// 交易记录DTO
/// </summary>
public class TransactionDto
{
    public int Id { get; set; }
    public string Type { get; set; } = string.Empty; // 收入、支出、退款
    public decimal Amount { get; set; }
    public string Description { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public string? OrderId { get; set; }
    public string? Status { get; set; }
}