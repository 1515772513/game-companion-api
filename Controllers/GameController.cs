using GameCompanion.Api.DTO.Game;
using GameCompanion.Api.Models;
using GameCompanion.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameCompanion.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IGameService _gameService;

        public GameController(IGameService gameService)
        {
            _gameService = gameService;
        }

        /// <summary>
        /// 获取游戏下拉选项
        /// </summary>
        [HttpGet("select-options")]
        public async Task<ApiResponse<List<GameSelectOptionDto>>> GetSelectOptions()
        {
            return await _gameService.GetGameSelectOptionsAsync();
        }
    }
}