namespace GameCompanion.Api.DTOs.Posts;

/// <summary>
/// 分页信息DTO
/// </summary>
public class PaginationDto
{
    /// <summary>
    /// 当前页码
    /// </summary>
    public int Page { get; set; }

    /// <summary>
    /// 每页数量
    /// </summary>
    public int PageSize { get; set; }

    /// <summary>
    /// 总记录数
    /// </summary>
    public int Total { get; set; }

    /// <summary>
    /// 总页数
    /// </summary>
    public int TotalPages { get; set; }

    /// <summary>
    /// 是否有更多数据
    /// </summary>
    public bool HasMore { get; set; }
}