namespace GameCompanion.Api.Models;

/// <summary>
/// 分页列表
/// </summary>
/// <typeparam name="T">列表项类型</typeparam>
public class PaginatedList<T>
{
    /// <summary>
    /// 列表数据
    /// </summary>
    public List<T> Items { get; set; } = new();

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
    public int TotalPages => (int)Math.Ceiling((double)Total / PageSize);

    /// <summary>
    /// 是否有更多数据
    /// </summary>
    public bool HasMore => Page < TotalPages;

    /// <summary>
    /// 创建分页列表
    /// </summary>
    public static PaginatedList<T> Create(List<T> items, int page, int pageSize, int total)
    {
        return new PaginatedList<T>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            Total = total
        };
    }
}
