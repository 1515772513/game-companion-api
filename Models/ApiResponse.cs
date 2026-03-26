namespace GameCompanion.Api.Models;

/// <summary>
/// 统一API响应格式
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
public class ApiResponse<T>
{
    /// <summary>
    /// 状态码（200表示成功，其他表示失败）
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
    /// 时间戳
    /// </summary>
    public long Timestamp { get; set; } = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();

    /// <summary>
    /// 请求ID
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// 错误信息（失败时返回）
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// 创建成功响应
    /// </summary>
    public static ApiResponse<T> Success(T data, string message = "操作成功")
    {
        return new ApiResponse<T>
        {
            Code = 200,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// 创建失败响应
    /// </summary>
    public static ApiResponse<T> Fail(int code, string message, string error = "")
    {
        return new ApiResponse<T>
        {
            Code = code,
            Message = message,
            Error = error
        };
    }

    /// <summary>
    /// 创建成功响应(带消息)
    /// </summary>
    public static ApiResponse<T> SuccessResponse(T data, string message = "操作成功")
    {
        return new ApiResponse<T>
        {
            Code = 200,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// 创建错误响应(带错误消息)
    /// </summary>
    public static ApiResponse<T> ErrorResponse(int code, string error, string message = "操作失败")
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
/// 无数据类型的API响应
/// </summary>
public class ApiResponse : ApiResponse<object>
{
    public static ApiResponse Success(string message = "操作成功")
    {
        return new ApiResponse
        {
            Code = 200,
            Message = message,
            Data = null
        };
    }

    public new static ApiResponse Error(int code, string error, string message = "操作失败")
    {
        return new ApiResponse()
        {
            Code = code,
            Message = message
        }.WithError(error);
    }

    /// <summary>
    /// 创建成功响应(带数据)
    /// </summary>
    public new static ApiResponse SuccessResponse(object data, string message = "操作成功")
    {
        return new ApiResponse
        {
            Code = 200,
            Message = message,
            Data = data
        };
    }
}

