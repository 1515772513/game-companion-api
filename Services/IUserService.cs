using GameCompanion.Api.DTOs.User;
using GameCompanion.Api.Models;
using GameCompanion.Api.Models.Entities;

namespace GameCompanion.Api.Services;

/// <summary>
/// 用户服务接口
/// </summary>
public interface IUserService
{
    /// <summary>
    /// 获取用户个人信息
    /// </summary>
    Task<ApiResponse<UserProfileDto>> GetProfileAsync(int userId);

    /// <summary>
    /// 更新用户个人资料
    /// </summary>
    Task<ApiResponse<UserProfileDto>> UpdateProfileAsync(int userId, UpdateProfileDto updateDto);

    /// <summary>
    /// 上传用户头像
    /// </summary>
    Task<ApiResponse<string>> UploadAvatarAsync(int userId, string avatarUrl);

    /// <summary>
    /// 实名认证
    /// </summary>
    Task<ApiResponse<bool>> VerifyRealNameAsync(int userId, VerifyRealNameDto verifyDto);

    /// <summary>
    /// 获取用户收藏列表
    /// </summary>
    Task<ApiResponse<CollectionsResponseDto>> GetCollectionsAsync(int userId, int page = 1, int pageSize = 10);

    /// <summary>
    /// 获取关注列表
    /// </summary>
    Task<ApiResponse<FollowingListResponseDto>> GetFollowingAsync(int userId, int page = 1, int pageSize = 10);

    /// <summary>
    /// 获取粉丝列表
    /// </summary>
    Task<ApiResponse<FollowersListResponseDto>> GetFollowersAsync(int userId, int page = 1, int pageSize = 10);

    /// <summary>
    /// 关注/取消关注用户
    /// </summary>
    Task<ApiResponse<bool>> FollowUserAsync(int currentUserId, FollowUserDto followDto);

    /// <summary>
    /// 申请成为陪玩师
    /// </summary>
    Task<ApiResponse<CompanionApplication>> ApplyCompanionAsync(int userId, ApplyCompanionDto applyDto);

    /// <summary>
    /// 获取钱包信息
    /// </summary>
    Task<ApiResponse<WalletDto>> GetWalletAsync(int userId);

    /// <summary>
    /// 获取用户列表
    /// </summary>
    Task<ApiResponse<UserListListDto>> GetListAsync(GetUserListDto request);
}