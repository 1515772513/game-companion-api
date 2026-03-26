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
    public static string ToFriendlyTimeString(this DateTime dateTime)
    {
        var now = DateTime.Now;
        var diff = now - dateTime;

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
        else if (dateTime.Year == now.Year)
        {
            return dateTime.ToString("MM-dd");
        }
        else
        {
            return dateTime.ToString("yyyy-MM-dd");
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
    public static string ToStandardString(this DateTime dateTime)
    {
        return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
    }
}