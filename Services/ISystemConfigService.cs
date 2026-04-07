using GameCompanion.Api.DTOs.Settings;
using GameCompanion.Api.Models;
using GameCompanion.Api.Models.Entities;

namespace GameCompanion.Api.Services
{
    /// <summary>
    /// 系统配置服务接口
    /// </summary>
    public interface ISystemConfigService
    {
        /// <summary>
        /// 获取系统配置列表
        /// </summary>
        Task<List<SystemConfigDto>> GetConfigListAsync(string name = "", string configKey = "");

        /// <summary>
        /// 新增系统配置
        /// </summary>
        Task<ApiResponse<string>> AddConfigAsync(SystemConfigDto dto);

        /// <summary>
        /// 修改系统配置
        /// </summary>
        Task<ApiResponse<string>> UpdateConfigAsync(SystemConfigDto dto);

        /// <summary>
        /// 删除系统配置
        /// </summary>
        Task<ApiResponse<string>> DeleteConfigAsync(int id);

        /// <summary>
        /// 根据Key获取配置（用于首页读取Banner）
        /// </summary>
        Task<SystemConfig?> GetConfigByKeyAsync(string configKey);
    }
}