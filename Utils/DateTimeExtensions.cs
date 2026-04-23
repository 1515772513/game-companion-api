using System;

namespace GameCompanion.Api.Utils;

/// <summary>
/// DateTime扩展方法
/// </summary>
public static class DateTimeExtensions
{
    /// <summary>
    /// 格式化为友好的时间显示
    /// </summary>
    /// <param name="dateTime">时间</param>
    /// <returns>友好时间字符串</returns>
    public static string ToFriendlyTimeString(this DateTime? dateTime)
    {
        if (!dateTime.HasValue)
        {
            return string.Empty;
        }
        var now = DateTime.Now;
        var diff = now - dateTime.Value;

        if (diff.TotalMinutes < 1)
        {
            return "刚刚";
        }
        else if (diff.TotalMinutes < 60)
        {
            return $"{(int)diff.TotalMinutes}分钟前";
        }
        else if (diff.TotalHours < 24)
        {
            return $"{(int)diff.TotalHours}小时前";
        }
        else if (diff.TotalDays < 7)
        {
            return $"{(int)diff.TotalDays}天前";
        }
        else if (dateTime.Value.Year == now.Year)
        {
            return dateTime.Value.ToDateTimeString();
        }
        else
        {
            return dateTime.Value.ToDateTimeString();
        }
    }

    /// <summary>
    /// 转换为Unix时间戳（秒）
    /// </summary>
    public static long ToUnixTimestamp(this DateTime dateTime)
    {
        return new DateTimeOffset(dateTime).ToUnixTimeSeconds();
    }

    /// <summary>
    /// 从Unix时间戳转换为DateTime
    /// </summary>
    public static DateTime FromUnixTimestamp(this long timestamp)
    {
        return DateTimeOffset.FromUnixTimeSeconds(timestamp).DateTime;
    }

    /// <summary>
    /// 格式化为标准格式
    /// </summary>
    public static string ToDateTimeString(this DateTime dateTime, string format = "yyyy-MM-dd HH:mm:ss")
    {
        return dateTime.ToString(format);
    }

    /// <summary>
    /// 字符串格式时间转标准格式
    /// </summary>
    public static string ToDateTimeString(this string dateTimeStr, string format = "yyyy-MM-dd HH:mm:ss")
    {
        // 兼容多种输入格式，可根据实际场景调整
        if (DateTime.TryParse(dateTimeStr, out DateTime dt))
        {
            return dt.ToString(format);
        }
        // 解析失败返回原字符串或空，按需处理
        return dateTimeStr;
    }
    /// <summary>
    /// **新增：处理 DateTime? 的核心方法**
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string ToDateTimeString(this DateTime? dateTime, string format = "yyyy-MM-dd HH:mm:ss")
    {
        // 如果有值，调用非可空方法；否则返回空字符串
        return dateTime?.ToString(format) ?? string.Empty;
    }

    /// <summary>
    /// **新增：处理 DateTime? 的核心方法**
    /// </summary>
    /// <param name="dateTime"></param>
    /// <returns></returns>
    public static string ToDateTimeString(this DateTime? dateTime)
    {
        // 如果有值，调用非可空方法；否则返回空字符串
        return dateTime?.ToString("yyyy-MM-dd HH:mm:ss") ?? string.Empty;
    }

    /// <summary>
    /// 格式化为标准格式（支持 DateOnly）
    /// </summary>
    public static string ToDateTimeString(this DateOnly dateOnly, string format = "yyyy-MM-dd")
    {
        return dateOnly.ToString(format);
    }

    /// <summary>
    /// 格式化为标准格式（支持 DateOnly）
    /// </summary>
    public static string ToDateTimeString(this DateOnly? dateOnly, string format = "yyyy-MM-dd")
    {
        return dateOnly?.ToString(format) ?? string.Empty;
    }
    
    /// <summary>
    /// 字符串转换为可空 DateTime，兼容空字符串、null、格式错误
    /// </summary>
    public static DateTime? ToNullableDateTime(this string? dateTimeStr)
    {
        if (string.IsNullOrWhiteSpace(dateTimeStr))
        {
            return null;
        }

        if (DateTime.TryParse(dateTimeStr, out DateTime dt))
        {
            return dt;
        }

        return null;
    }
}