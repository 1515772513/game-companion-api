using GameCompanion.Api.Data;
using GameCompanion.Api.DTO.Game;
using GameCompanion.Api.Models;
using GameCompanion.Api.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace GameCompanion.Api.Services
{
    public class GameService : IGameService
    {
        private readonly GameCompanionContext _dbContext;
        private readonly ILogger<GameService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public GameService(
            GameCompanionContext dbContext,
            ILogger<GameService> logger,
            IHttpContextAccessor httpContextAccessor)
        {
            _dbContext = dbContext;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// 获取游戏下拉选项
        /// </summary>
        public async Task<ApiResponse<List<GameSelectOptionDto>>> GetGameSelectOptionsAsync()
        {
            try
            {
                var list = await _dbContext.Set<Game>()
                    .Where(g => g.Status == "1")
                    .OrderBy(g => g.SortOrder)
                    .Select(g => new GameSelectOptionDto
                    {
                        Value = g.Id,
                        Label = g.Name
                    })
                    .ToListAsync();

                _logger.LogInformation("获取游戏下拉选项成功，数量：{Count}", list.Count);
                return ApiResponse<List<GameSelectOptionDto>>.Success(list);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取游戏下拉选项异常");
                return ApiResponse<List<GameSelectOptionDto>>.Fail(500, "获取游戏列表失败");
            }
        }
    }
}