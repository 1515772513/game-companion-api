using System.Diagnostics;

namespace GameCompanion.Api.Middleware;

/// <summary>
/// 请求日志中间件
/// </summary>
public class RequestLoggingMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<RequestLoggingMiddleware> _logger;

    public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var stopwatch = Stopwatch.StartNew();
        var requestId = Guid.NewGuid().ToString();

        // 添加请求ID到响应头
        context.Response.OnStarting(() =>
        {
            context.Response.Headers.Append("X-Request-ID", requestId);
            return Task.CompletedTask;
        });

        try
        {
            await _next(context);
        }
        finally
        {
            stopwatch.Stop();
            var elapsed = stopwatch.ElapsedMilliseconds;

            _logger.LogInformation(
                "请求完成: {Method} {Path} | 状态码: {StatusCode} | 耗时: {Elapsed}ms | 请求ID: {RequestId}",
                context.Request.Method,
                context.Request.Path,
                context.Response.StatusCode,
                elapsed,
                requestId
            );
        }
    }
}
