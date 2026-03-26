namespace GameCompanion.Api.DTOs.Home;

/// <summary>
/// 搜索陪玩师请求参数
/// </summary>
public class SearchCompanionsRequest
{
    /// <summary>
    /// 搜索关键词，至少2个字符
    /// </summary>
    public string Keyword { get; set; } = "";

    /// <summary>
    /// 页码
    /// </summary>
    public int Page { get; set; } = 1;

    /// <summary>
    /// 每页数量
    /// </summary>
    public int PageSize { get; set; } = 20;
}

/// <summary>
/// 搜索陪玩师响应
/// </summary>
public class SearchCompanionsResponse
{
    /// <summary>
    /// 陪玩师列表
    /// </summary>
    public List<CompanionSummaryDto> Items { get; set; } = new();

    /// <summary>
    /// 分页信息
    /// </summary>
    public PaginationDto Pagination { get; set; } = new();
}