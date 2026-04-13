namespace GameCompanion.Api.DTOs.User;

/// <summary>
/// 添加收藏请求DTO
/// </summary>
public class AddFavoriteDto
{
    /// <summary>
    /// 关联项目ID
    /// </summary>
    public int ItemId { get; set; }

    /// <summary>
    /// 关联项目类型(companion/post等)
    /// </summary>
    public string ItemType { get; set; } = "companion";
    
}

/// <summary>
/// 取消收藏请求DTO
/// </summary>
public class RemoveFavoriteDto
{
    /// <summary>
    /// 关联项目ID
    /// </summary>
    public int ItemId { get; set; }

    /// <summary>
    /// 关联项目类型(companion/post等)
    /// </summary>
    public string ItemType { get; set; } = "companion";
    
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