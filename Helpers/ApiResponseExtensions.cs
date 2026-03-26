using GameCompanion.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameCompanion.Api.Helpers;

/// <summary>
/// ApiResponse扩展方法
/// </summary>
public static class ApiResponseExtensions
{
    /// <summary>
    /// 设置Error属性
    /// </summary>
    public static T WithError<T>(this T response, string error) where T : ApiResponse
    {
        var errorProperty = typeof(T).GetProperty("Error");
        errorProperty?.SetValue(response, error);
        return response;
    }

    /// <summary>
    /// 将ApiResponse转换为ActionResult
    /// </summary>
    public static IActionResult ToActionResult<T>(this ApiResponse<T> response)
    {
        return response.Code switch
        {
            200 => new OkObjectResult(response),
            400 => new BadRequestObjectResult(response),
            401 => new UnauthorizedObjectResult(response),
            403 => new ObjectResult(response) { StatusCode = 403 },
            404 => new NotFoundObjectResult(response),
            500 => new ObjectResult(response)
            {
                StatusCode = 500
            },
            _ => new ObjectResult(response)
            {
                StatusCode = response.Code
            }
        };
    }

    /// <summary>
    /// 将ApiResponse转换为ActionResult（无数据类型）
    /// </summary>
    public static IActionResult ToActionResult(this ApiResponse response)
    {
        return response.Code switch
        {
            200 => new OkObjectResult(response),
            400 => new BadRequestObjectResult(response),
            401 => new UnauthorizedObjectResult(response),
            403 => new ObjectResult(response) { StatusCode = 403 },
            404 => new NotFoundObjectResult(response),
            500 => new ObjectResult(response)
            {
                StatusCode = 500
            },
            _ => new ObjectResult(response)
            {
                StatusCode = response.Code
            }
        };
    }
}