using System;

namespace GameCompanion.Api.Utils;

/// <summary>
/// DateTime扩展方法
/// </summary>
public static class UserStatusExtensions
{
  /// <summary>
    /// 获取用户状态文字（正常/禁用）
    /// </summary>
    public static string GetStatusCn(this int? status)
    {
        return status == 1 ? "禁用" : "正常";
    }

    /// <summary>
    /// 数字int判空（空值返回0）
    /// </summary>
    public static int GetSafeInt(this int? value)
    {
        return value ?? 0;
    }

    /// <summary>
    /// 数字decimal判空（空值返回0）
    /// </summary>
    public static decimal GetSafeDecimal(this decimal? value)
    {
        return value ?? 0;
    }
}