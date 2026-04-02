namespace GameCompanion.Api.DTOs.Dict;

/// <summary>
/// 通用字典项DTO
/// </summary>
public class DictItemDto
{
    /// <summary>
    /// 字典类型
    /// </summary>
    public string DictType { get; set; } = string.Empty;

    /// <summary>
    /// 字典键值（如：entertainment）
    /// </summary>
    public string DictValue { get; set; } = string.Empty;

    /// <summary>
    /// 字典标签（翻译值，如：娱乐陪玩）
    /// </summary>
    public string DictLabel { get; set; } = string.Empty;
}