namespace GameCompanion.Api.DTOs;

/// <summary>
/// 统一API响应格式
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// 响应码
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// 响应消息
    /// </summary>
    public string Message { get; set; } = "success";

    /// <summary>
    /// 响应数据
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// 错误信息
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// 时间戳
    /// </summary>
    public long Timestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    /// <summary>
    /// 创建成功响应
    /// </summary>
    public static ApiResponse<T> Success(T? data, string message = "success")
    {
        return new ApiResponse<T>
        {
            Code = 200,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// 创建成功响应（无数据）
    /// </summary>
    public static ApiResponse<T> Success(string message = "success")
    {
        return new ApiResponse<T>
        {
            Code = 200,
            Message = message
        };
    }

    /// <summary>
    /// 创建错误响应
    /// </summary>
    public static ApiResponse<T> Fail(int code, string message, string? error = null)
    {
        return new ApiResponse<T>
        {
            Code = code,
            Message = message,
            Error = error
        };
    }
}

/// <summary>
/// 分页响应数据
/// </summary>
/// <typeparam name="T">列表项类型</typeparam>
public class PagedResponse<T>
{
    /// <summary>
    /// 数据列表
    /// </summary>
    public List<T> Items { get; set; } = new();

    /// <summary>
    /// 分页信息
    /// </summary>
    public PaginationInfo Pagination { get; set; } = new();
}

/// <summary>
/// 分页信息
/// </summary>
public class PaginationInfo
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
}
