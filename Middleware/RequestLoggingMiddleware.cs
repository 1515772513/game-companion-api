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
        var requestId = context.TraceIdentifier;
        context.Items["RequestId"] = requestId;

        var startTime = DateTime.Now;

        _logger.LogInformation("请求开始: {RequestId} - {Method} {Path}", requestId, context.Request.Method, context.Request.Path);

        await _next(context);

        var duration = (DateTime.Now - startTime).TotalMilliseconds;

        _logger.LogInformation("请求完成: {RequestId} - {StatusCode} - {Duration}ms", requestId, context.Response.StatusCode, duration);
    }
}
