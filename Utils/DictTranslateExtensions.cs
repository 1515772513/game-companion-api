using GameCompanion.Api.Services.DictTranslate;

namespace GameCompanion.Api.Utils;

/// <summary>
/// 字典翻译扩展方法（简化常用场景调用）
/// </summary>
public static class DictTranslateExtensions
{
    /// <summary>
    /// 翻译服务类型（service_type）
    /// </summary>
    public static async Task<string> TranslateServiceTypeAsync(this IDictTranslateService service, string serviceType, CancellationToken cancellationToken = default)
    {
        return await service.TranslateAsync("service_type", serviceType, cancellationToken);
    }

    /// <summary>
    /// 翻译在线状态（online_status）
    /// </summary>
    public static async Task<string> TranslateOnlineStatusAsync(this IDictTranslateService service, string onlineStatus, CancellationToken cancellationToken = default)
    {
        return await service.TranslateAsync("online_status", onlineStatus, cancellationToken);
    }

    /// <summary>
    /// 翻译订单状态（order_status）
    /// </summary>
    public static async Task<string> TranslateOrderStatusAsync(this IDictTranslateService service, string orderStatus, CancellationToken cancellationToken = default)
    {
        return await service.TranslateAsync("order_status", orderStatus, cancellationToken);
    }

    /// <summary>
    /// 通用翻译扩展（任意字典类型）
    /// </summary>
    public static async Task<string> TranslateDictAsync(this IDictTranslateService service, string dictType, string dictValue, CancellationToken cancellationToken = default)
    {
        return await service.TranslateAsync(dictType, dictValue, cancellationToken);
    }
}