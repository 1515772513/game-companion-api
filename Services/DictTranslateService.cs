using GameCompanion.Api.DTOs.Dict;
using GameCompanion.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Services.DictTranslate;

public class DictTranslateService : IDictTranslateService
{
    // 🔥 关键点：自己创建上下文，不依赖注入
    private GameCompanionContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<GameCompanionContext>()
            .UseMySql("server=47.98.225.136;port=3306;database=game_companion;user=admin;password=admin123;charset=utf8mb4",
            Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.37-mysql"))
            .Options;

        return new GameCompanionContext(options);
    }

    public async Task<string> TranslateAsync(string dictType, string dictValue, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dictType) || string.IsNullOrWhiteSpace(dictValue))
            return dictValue;

        // 自己创建上下文
        using var context = CreateContext();

        var label = await context.SysDictData
            .Where(d => d.DictType == dictType && d.DictValue == dictValue && d.Status == 0)
            .Select(d => d.DictLabel)
            .FirstOrDefaultAsync(cancellationToken);

        return label ?? dictValue;
    }

    public async Task<Dictionary<string, string>> BatchTranslateAsync(string dictType, List<string> dictValues, CancellationToken cancellationToken = default)
    {
        var result = new Dictionary<string, string>();
        // 打印字典翻译请求
        Console.WriteLine($"BatchTranslateAsync: {dictType}, {string.Join(", ", dictValues)}");

        if (string.IsNullOrWhiteSpace(dictType) || dictValues == null || !dictValues.Any())
            return result;

        using var context = CreateContext();
        var distinctValues = dictValues.Distinct().ToList();

        var dictMap = await context.SysDictData
            .Where(d => d.DictType == dictType && distinctValues.Contains(d.DictValue) && d.Status == 0)
            .ToDictionaryAsync(d => d.DictValue, d => d.DictLabel, cancellationToken);

        foreach (var value in distinctValues)
        {
            result[value] = dictMap.TryGetValue(value, out var label) ? label : value;
        }

        return result;
    }

    public async Task<List<DictItemDto>> GetDictItemsAsync(string dictType, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dictType))
            return new List<DictItemDto>();

        using var context = CreateContext();
        return await context.SysDictData
            .Where(d => d.DictType == dictType && d.Status == 0)
            .OrderBy(d => d.DictSort)
            .Select(d => new DictItemDto
            {
                DictType = d.DictType,
                DictValue = d.DictValue,
                DictLabel = d.DictLabel
            })
            .ToListAsync(cancellationToken);
    }
}