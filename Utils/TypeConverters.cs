namespace GameCompanion.Api.Utils;

/// <summary>
/// 类型转换器
/// </summary>
public static class TypeConverters
{
    /// <summary>
    /// 消息类型转换（数据库类型到数字类型）
    /// </summary>
    public static int ConvertMessageType(string? dbType)
    {
        return dbType switch
        {
            "文本" => 0,
            "图片" => 1,
            "语音" => 2,
            _ => 0
        };
    }

    /// <summary>
    /// 消息类型转换（数字类型到数据库类型）
    /// </summary>
    public static string ConvertMessageTypeToDb(int messageType)
    {
        return messageType switch
        {
            0 => "文本",
            1 => "图片",
            2 => "语音",
            _ => "文本"
        };
    }

    /// <summary>
    /// 通知类型转换（数据库类型到数字类型）
    /// </summary>
    public static int ConvertNotificationType(string? dbType)
    {
        return dbType switch
        {
            "系统通知" => 0,
            "活动通知" => 1,
            "订单通知" => 2,
            _ => 0
        };
    }

    /// <summary>
    /// 获取通知类型文本
    /// </summary>
    public static string GetNotificationTypeText(string? dbType)
    {
        return dbType switch
        {
            "系统通知" => "系统通知",
            "活动通知" => "活动通知",
            "订单通知" => "订单通知",
            _ => "系统通知"
        };
    }

    /// <summary>
    /// 获取消息类型文本
    /// </summary>
    public static string GetMessageTypeText(int messageType)
    {
        return messageType switch
        {
            0 => "文本",
            1 => "图片",
            2 => "语音",
            _ => "文本"
        };
    }

    /// <summary>
    /// 获取优先级文本
    /// </summary>
    public static string GetPriorityText(int priority)
    {
        return priority switch
        {
            1 => "重要",
            2 => "普通",
            _ => "普通"
        };
    }
}