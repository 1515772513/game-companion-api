using GameCompanion.Api.DTO.Game;
using GameCompanion.Api.Models;

namespace GameCompanion.Api.Services
{
    public interface IGameService
    {
        Task<ApiResponse<List<GameSelectOptionDto>>> GetGameSelectOptionsAsync();
    }
}