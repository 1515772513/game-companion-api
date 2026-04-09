namespace GameCompanion.Api.DTOs.User;

/// <summary>
/// 添加收藏请求DTO
/// </summary>
public class AddFavoriteDto
{
    /// <summary>
    /// 陪玩师ID
    /// </summary>
    public int CompanionId { get; set; }
}

/// <summary>
/// 收藏响应DTO（可扩展）
/// </summary>
public class FavoriteResultDto
{
    /// <summary>
    /// 是否操作成功
    /// </summary>
    public bool Success { get; set; }
    /// <summary>
    /// 提示信息
    /// </summary>
    public string Message { get; set; } = string.Empty;
}