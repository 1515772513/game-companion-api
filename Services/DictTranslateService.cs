using GameCompanion.Api.DTOs.Dict;
using GameCompanion.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Services.DictTranslate;

public class DictTranslateService : IDictTranslateService
{
    // 每次调用使用独立的短生命周期上下文：
    // 连接串从配置(DefaultConnection)读取（开发环境=本地库），同时保证可被并发调用
    // （GetCompanionDetailAsync 会 Task.WhenAll 并发翻译多个游戏段位，不能共用同一个 DbContext）。
    private readonly IConfiguration _configuration;

    public DictTranslateService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    private GameCompanionContext CreateContext()
    {
        var conn = _configuration.GetConnectionString("DefaultConnection");
        var options = new DbContextOptionsBuilder<GameCompanionContext>()
            .UseMySql(conn, Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.37-mysql"))
            .Options;
        return new GameCompanionContext(options);
    }

    public async Task<string> TranslateAsync(string dictType, string dictValue, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(dictType) || string.IsNullOrWhiteSpace(dictValue))
            return dictValue;

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