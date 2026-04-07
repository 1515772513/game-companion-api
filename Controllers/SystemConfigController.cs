using GameCompanion.Api.DTOs.Settings;
using GameCompanion.Api.Models;
using GameCompanion.Api.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GameCompanion.Api.Controllers
{
    /// <summary>
    /// 系统配置控制器
    /// </summary>
    [ApiController]
    [Route("api/system-config")]
    public class SystemConfigController : ControllerBase
    {
        private readonly ISystemConfigService _settingsService;
        private readonly ILogger<SystemConfigController> _logger;

        public SystemConfigController(ISystemConfigService settingsService, ILogger<SystemConfigController> logger)
        {
            _settingsService = settingsService;
            _logger = logger;
        }

        /// <summary>
        /// 分页获取系统配置列表
        /// </summary>
        [HttpGet("page")]
        [ProducesResponseType(typeof(ApiResponse<List<SystemConfigDto>>), 200)]
        [ProducesResponseType(typeof(ApiResponse<>), 500)]
        public async Task<ActionResult<ApiResponse<List<SystemConfigDto>>>> GetPage(string name = "", string configKey = "")
        {
            try
            {
                var list = await _settingsService.GetConfigListAsync(name, configKey);
                return ApiResponse<List<SystemConfigDto>>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取系统配置列表接口异常");
                return ApiResponse<List<SystemConfigDto>>.Fail(500, "服务器异常");
            }
        }

        /// <summary>
        /// 新增系统配置
        /// </summary>
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        [ProducesResponseType(typeof(ApiResponse<>), 400)]
        [ProducesResponseType(typeof(ApiResponse<>), 500)]
        public async Task<ActionResult<ApiResponse<string>>> Add([FromBody] SystemConfigDto dto)
        {
            var result = await _settingsService.AddConfigAsync(dto);
            return result;
        }

        /// <summary>
        /// 修改系统配置
        /// </summary>
        [HttpPut]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        [ProducesResponseType(typeof(ApiResponse<>), 400)]
        [ProducesResponseType(typeof(ApiResponse<>), 404)]
        [ProducesResponseType(typeof(ApiResponse<>), 500)]
        public async Task<ActionResult<ApiResponse<string>>> Update([FromBody] SystemConfigDto dto)
        {
            var result = await _settingsService.UpdateConfigAsync(dto);
            return result;
        }

        /// <summary>
        /// 删除系统配置
        /// </summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(typeof(ApiResponse<string>), 200)]
        [ProducesResponseType(typeof(ApiResponse<>), 404)]
        [ProducesResponseType(typeof(ApiResponse<>), 500)]
        public async Task<ActionResult<ApiResponse<string>>> Delete(int id)
        {
            var result = await _settingsService.DeleteConfigAsync(id);
            return result;
        }
    }
}