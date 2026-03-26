namespace GameCompanion.Api.Services;

using GameCompanion.Api.DTOs.Home;
using GameCompanion.Api.Models;

/// <summary>
/// 首页服务接口
/// </summary>
public interface IHomeService
{
    /// <summary>
    /// 获取首页数据
    /// </summary>
    /// <returns>首页数据响应</returns>
    Task<ApiResponse<HomeDataResponse>> GetHomeDataAsync();

    /// <summary>
    /// 获取陪玩师列表
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>陪玩师列表响应</returns>
    Task<ApiResponse<CompanionListResponse>> GetCompanionsAsync(CompanionListRequest request);

    /// <summary>
    /// 获取陪玩师详情
    /// </summary>
    /// <param name="companionId">陪玩师ID</param>
    /// <returns>陪玩师详情响应</returns>
    Task<ApiResponse<CompanionDetailResponse>> GetCompanionDetailAsync(int companionId);

    /// <summary>
    /// 获取游戏列表
    /// </summary>
    /// <returns>游戏列表响应</returns>
    Task<ApiResponse<GameListResponse>> GetGamesAsync();

    /// <summary>
    /// 搜索陪玩师
    /// </summary>
    /// <param name="request">搜索请求参数</param>
    /// <returns>搜索响应</returns>
    Task<ApiResponse<SearchCompanionsResponse>> SearchCompanionsAsync(SearchCompanionsRequest request);

    /// <summary>
    /// 获取游戏圈子列表
    /// </summary>
    /// <param name="request">请求参数</param>
    /// <returns>圈子列表响应</returns>
    Task<ApiResponse<CirclesListResponse>> GetCirclesAsync(CirclesListRequest request);
}
