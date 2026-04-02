using GameCompanion.Api.DTOs.Dict;

namespace GameCompanion.Api.Services.DictTranslate;

/// <summary>
/// 通用字典翻译服务接口
/// </summary>
public interface IDictTranslateService
{
    /// <summary>
    /// 单个字典值翻译
    /// </summary>
    /// <param name="dictType">字典类型（如：service_type）</param>
    /// <param name="dictValue">字典值（如：entertainment）</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>翻译后的标签，无匹配返回原value</returns>
    Task<string> TranslateAsync(string dictType, string dictValue, CancellationToken cancellationToken = default);

    /// <summary>
    /// 批量字典值翻译（列表场景用，一次查询）
    /// </summary>
    /// <param name="dictType">字典类型</param>
    /// <param name="dictValues">字典值列表</param>
    /// <param name="cancellationToken">取消令牌</param>
    /// <returns>字典值→标签的映射</returns>
    Task<Dictionary<string, string>> BatchTranslateAsync(string dictType, List<string> dictValues, CancellationToken cancellationToken = default);

    /// <summary>
    /// 获取指定字典类型的所有有效项
    /// </summary>
    /// <param name="dictType">字典类型</param>
    /// <param name="cancellationToken">取消令牌</param>
    Task<List<DictItemDto>> GetDictItemsAsync(string dictType, CancellationToken cancellationToken = default);
}