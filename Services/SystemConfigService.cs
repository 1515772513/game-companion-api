using GameCompanion.Api.Data;
using GameCompanion.Api.DTOs.Settings;
using GameCompanion.Api.Models;
using GameCompanion.Api.Models.Entities;
using GameCompanion.Api.Utils;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCompanion.Api.Services
{
    /// <summary>
    /// 系统配置服务实现
    /// </summary>
    public class SystemConfigService : ISystemConfigService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<SystemConfigService> _logger;

        public SystemConfigService(ApplicationDbContext context, ILogger<SystemConfigService> logger)
        {
            _context = context;
            _logger = logger;
        }

        /// <summary>
        /// 获取系统配置列表
        /// </summary>
        public async Task<List<SystemConfigDto>> GetConfigListAsync(string name = "", string configKey = "")
        {
            try
            {
                var query = _context.SystemConfigs.AsNoTracking();

                if (!string.IsNullOrEmpty(name))
                    query = query.Where(c => c.Name.Contains(name));

                if (!string.IsNullOrEmpty(configKey))
                    query = query.Where(c => c.ConfigKey.Contains(configKey));

                return await query
                    .OrderByDescending(c => c.Id)
                    .Select(c => new SystemConfigDto
                    {
                        Id = c.Id,
                        ConfigKey = c.ConfigKey,
                        Name = c.Name ?? string.Empty,
                        ConfigValue = c.ConfigValue ?? string.Empty,
                        ConfigType = c.ConfigType ?? string.Empty,
                        Remark = c.Remark ?? string.Empty,
                        CreatedAt = c.CreatedAt.ToDateTimeString(),
                        UpdatedAt = c.UpdatedAt.ToDateTimeString()
                    })
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取系统配置列表异常");
                throw;
            }
        }

        /// <summary>
        /// 新增系统配置
        /// </summary>
        public async Task<ApiResponse<string>> AddConfigAsync(SystemConfigDto dto)
        {
            try
            {
                if (string.IsNullOrEmpty(dto.ConfigKey))
                    return ApiResponse<string>.Fail(400, "配置键不能为空");

                var exists = await _context.SystemConfigs
                    .AnyAsync(c => c.ConfigKey == dto.ConfigKey);

                if (exists)
                    return ApiResponse<string>.Fail(400, "配置键已存在");

                var config = new SystemConfig
                {
                    ConfigKey = dto.ConfigKey,
                    Name = dto.Name,
                    ConfigValue = dto.ConfigValue,
                    ConfigType = dto.ConfigType,
                    Remark = dto.Remark,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                _context.SystemConfigs.Add(config);
                await _context.SaveChangesAsync();

                return ApiResponse<string>.Success("新增成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "新增系统配置异常");
                return ApiResponse<string>.Fail(500, "服务器异常");
            }
        }

        /// <summary>
        /// 修改系统配置
        /// </summary>
        public async Task<ApiResponse<string>> UpdateConfigAsync(SystemConfigDto dto)
        {
            try
            {
                var config = await _context.SystemConfigs.FindAsync(dto.Id);
                if (config == null)
                    return ApiResponse<string>.Fail(404, "配置不存在");

                var keyExists = await _context.SystemConfigs
                    .AnyAsync(c => c.ConfigKey == dto.ConfigKey && c.Id != dto.Id);

                if (keyExists)
                    return ApiResponse<string>.Fail(400, "配置键已被占用");

                config.ConfigKey = dto.ConfigKey;
                config.Name = dto.Name;
                config.ConfigValue = dto.ConfigValue;
                config.ConfigType = dto.ConfigType;
                config.Remark = dto.Remark;
                config.UpdatedAt = DateTime.Now;

                await _context.SaveChangesAsync();

                return ApiResponse<string>.Success("修改成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "修改系统配置异常");
                return ApiResponse<string>.Fail(500, "服务器异常");
            }
        }

        /// <summary>
        /// 删除系统配置
        /// </summary>
        public async Task<ApiResponse<string>> DeleteConfigAsync(int id)
        {
            try
            {
                var config = await _context.SystemConfigs.FindAsync(id);
                if (config == null)
                    return ApiResponse<string>.Fail(404, "配置不存在");

                _context.SystemConfigs.Remove(config);
                await _context.SaveChangesAsync();

                return ApiResponse<string>.Success("删除成功");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "删除系统配置异常");
                return ApiResponse<string>.Fail(500, "服务器异常");
            }
        }

        /// <summary>
        /// 根据Key获取配置
        /// </summary>
        public async Task<SystemConfig?> GetConfigByKeyAsync(string configKey)
        {
            return await _context.SystemConfigs
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.ConfigKey == configKey);
        }
    }
}