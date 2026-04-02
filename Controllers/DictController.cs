using System.ComponentModel.DataAnnotations;
using GameCompanion.Api.DTOs.Dict;
using GameCompanion.Api.Services.DictTranslate;
using Microsoft.AspNetCore.Mvc;

namespace GameCompanion.Api.Controllers
{
    /// <summary>
    /// 字典接口（前端下拉框专用）
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class DictController : ControllerBase
    {
        private readonly IDictTranslateService _dictTranslateService;

        public DictController(IDictTranslateService dictTranslateService)
        {
            _dictTranslateService = dictTranslateService;
        }

        /// <summary>
        /// 获取字典下拉列表（前端通用：service_type / review_status 等）
        /// </summary>
        /// <param name="dictType">字典类型：service_type / review_status</param>
        [HttpGet("list")]
        public async Task<IActionResult> GetDictList([Required] string dictType)
        {
            var list = await _dictTranslateService.GetDictItemsAsync(dictType);
            return Ok(new { code = 200, data = list });
        }

        /// <summary>
        /// 获取陪玩师服务类型下拉框
        /// </summary>
        [HttpGet("service-type")]
        public async Task<IActionResult> GetServiceTypeList()
        {
            var list = await _dictTranslateService.GetDictItemsAsync("service_type");
            return Ok(new { code = 200, data = list });
        }

        /// <summary>
        /// 获取审核状态下拉框（reviewStatus）
        /// </summary>
        [HttpGet("review-status")]
        public async Task<IActionResult> GetReviewStatusList()
        {
            var list = await _dictTranslateService.GetDictItemsAsync("review_status");
            return Ok(new { code = 200, data = list });
        }
    }
}