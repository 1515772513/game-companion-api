namespace GameCompanion.Api.DTOs.User;

/// <summary>
/// 用户收藏响应DTO
/// </summary>
public class CollectionsResponseDto
{
    public List<UserCollectionDto> Collections { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

/// <summary>
/// 关注列表响应DTO
/// </summary>
public class FollowingListResponseDto
{
    public List<UserListDto> Following { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}

/// <summary>
/// 粉丝列表响应DTO
/// </summary>
public class FollowersListResponseDto
{
    public List<UserListDto> Followers { get; set; } = new();
    public int TotalCount { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
}