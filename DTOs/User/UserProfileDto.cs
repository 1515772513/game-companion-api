using System.ComponentModel.DataAnnotations;

namespace GameCompanion.Api.DTOs.User;

/// <summary>
/// 用户个人信息DTO
/// </summary>
public class UserProfileDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string? RealName { get; set; }
    public string? IdCard { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string? Gender { get; set; }
    public int Age { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Bio { get; set; }
    public string? VipLevel { get; set; }
    public DateTime? VipExpireDate { get; set; }
    public int Points { get; set; }
    public decimal Balance { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime? LastLoginTime { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }
}

/// <summary>
/// 更新用户资料DTO
/// </summary>
public class UpdateProfileDto
{
    [Required]
    [MaxLength(50)]
    public string Nickname { get; set; } = string.Empty;

    [MaxLength(50)]
    public string? RealName { get; set; }

    [MaxLength(18)]
    public string? IdCard { get; set; }

    [Required]
    [MaxLength(11)]
    [RegularExpression(@"^1[3-9]\d{9}$", ErrorMessage = "手机号格式不正确")]
    public string Phone { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Gender { get; set; }

    public int Age { get; set; }

    [MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Bio { get; set; }
}

/// <summary>
/// 上传头像DTO
/// </summary>
public class UploadAvatarDto
{
    public string AvatarUrl { get; set; } = string.Empty;
}

/// <summary>
/// 实名认证DTO
/// </summary>
public class VerifyRealNameDto
{
    [Required]
    [MaxLength(50)]
    public string RealName { get; set; } = string.Empty;

    [Required]
    [MaxLength(18)]
    [RegularExpression(@"^[1-9]\d{5}(18|19|20)\d{2}(0[1-9]|1[0-2])(0[1-9]|[12]\d|3[01])\d{3}[\dXx]$", ErrorMessage = "身份证号格式不正确")]
    public string IdCard { get; set; } = string.Empty;
}

/// <summary>
/// 关注用户DTO
/// </summary>
public class FollowUserDto
{
    [Required]
    public int UserId { get; set; }
}

/// <summary>
/// 申请陪玩师DTO
/// </summary>
public class ApplyCompanionDto
{
    [Required]
    [MaxLength(100)]
    public string GameCategory { get; set; } = string.Empty;

    [Required]
    [MaxLength(100)]
    public string SkillLevel { get; set; } = string.Empty;

    [Required]
    [MaxLength(500)]
    public string SelfIntroduction { get; set; } = string.Empty;

    [Required]
    public decimal HourlyRate { get; set; }

    [Required]
    [MaxLength(100)]
    public string AvailableTime { get; set; } = string.Empty;
}

/// <summary>
/// 用户收藏DTO
/// </summary>
public class UserCollectionDto
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Category { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// 关注关系DTO
/// </summary>
public class FollowRelationshipDto
{
    public int Id { get; set; }
    public int FollowerId { get; set; }
    public int FollowingId { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// 用户列表DTO
/// </summary>
public class UserListDto
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Nickname { get; set; } = string.Empty;
    public string? Avatar { get; set; }
    public string? Bio { get; set; }
    public string? VipLevel { get; set; }
    public int Points { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
}