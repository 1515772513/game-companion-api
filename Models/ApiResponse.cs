using System.Runtime.Serialization;
using GameCompanion.Api.Helpers;

namespace GameCompanion.Api.Models;

/// <summary>
/// 统一API响应格式
/// </summary>
/// <typeparam name="T">数据类型</typeparam>
[DataContract]
public class ApiResponse<T>
{
    /// <summary>
    /// 状态码（200表示成功，其他表示失败）
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// 响应消息
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 响应数据
    /// </summary>
    public T? Data { get; set; }

    /// <summary>
    /// 时间戳
    /// </summary>
    public long Timestamp { get; set; }

    /// <summary>
    /// 请求ID
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// 错误信息（失败时返回）
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// 无参构造函数
    /// </summary>
    public ApiResponse()
    {
        Code = 200;
        Message = "success";
        Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

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

    /// <summary>
    /// 链式设置错误信息（修复原代码缺失的方法）
    /// </summary>
    public ApiResponse<T> WithError(string error)
    {
        Error = error;
        return this;
    }
}

/// <summary>
/// 无数据类型的API响应（不继承泛型类，彻底解决Swagger解析问题）
/// </summary>
[DataContract]
public class ApiResponse
{
    /// <summary>
    /// 状态码（200表示成功，其他表示失败）
    /// </summary>
    public int Code { get; set; }

    /// <summary>
    /// 响应消息
    /// </summary>
    public string Message { get; set; } = string.Empty;

    /// <summary>
    /// 响应数据
    /// </summary>
    public object? Data { get; set; }

    /// <summary>
    /// 时间戳
    /// </summary>
    public long Timestamp { get; set; }

    /// <summary>
    /// 请求ID
    /// </summary>
    public string? RequestId { get; set; }

    /// <summary>
    /// 错误信息（失败时返回）
    /// </summary>
    public string? Error { get; set; }

    /// <summary>
    /// 无参构造函数
    /// </summary>
    public ApiResponse()
    {
        Code = 200;
        Message = "success";
        Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
    }

    /// <summary>
    /// 无数据成功响应
    /// </summary>
    public static ApiResponse Success(string message = "操作成功")
    {
        return new ApiResponse
        {
            Code = 200,
            Message = message,
            Data = null
        };
    }

    /// <summary>
    /// 无数据错误响应
    /// </summary>
    public static ApiResponse Fail(int code, string message, string error = "")
    {
        return new ApiResponse
        {
            Code = code,
            Message = message,
            Error = error
        };
    }

    /// <summary>
    /// 无数据错误响应
    /// </summary>
    public static ApiResponse ErrorResponse(int code, string error, string message = "操作失败")
    {
        return new ApiResponse
        {
            Code = code,
            Message = message,
            Error = error
        };
    }

    /// <summary>
    /// 创建成功响应(带数据)
    /// </summary>
    public static ApiResponse SuccessResponse(object? data, string message = "操作成功")
    {
        return new ApiResponse
        {
            Code = 200,
            Message = message,
            Data = data
        };
    }

    /// <summary>
    /// 链式设置错误信息
    /// </summary>
    public ApiResponse WithError(string error)
    {
        Error = error;
        return this;
    }
}