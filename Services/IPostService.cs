using GameCompanion.Api.DTOs.Posts;

namespace GameCompanion.Api.Services;

/// <summary>
/// 动态服务接口
/// </summary>
public interface IPostService
{
    /// <summary>
    /// 发布动态
    /// </summary>
    /// <param name="request">发布动态请求</param>
    /// <param name="userId">用户ID</param>
    /// <returns>发布动态响应</returns>
    Task<ApiResponse<CreatePostResponse>> CreatePostAsync(CreatePostRequest request, int userId);

    /// <summary>
    /// 获取动态列表
    /// </summary>
    /// <param name="request">获取动态列表请求</param>
    /// <param name="userId">用户ID</param>
    /// <returns>动态列表响应</returns>
    Task<ApiResponse<GetPostsResponse>> GetPostsAsync(GetPostsRequest request, int userId);

    /// <summary>
    /// 获取动态详情
    /// </summary>
    /// <param name="postId">动态ID</param>
    /// <param name="userId">用户ID</param>
    /// <returns>动态详情响应</returns>
    Task<ApiResponse<GetPostDetailResponse>> GetPostDetailAsync(long postId, int userId);

    /// <summary>
    /// 点赞动态
    /// </summary>
    /// <param name="postId">动态ID</param>
    /// <param name="request">点赞请求</param>
    /// <param name="userId">用户ID</param>
    /// <returns>点赞响应</returns>
    Task<ApiResponse<LikePostResponse>> LikePostAsync(long postId, LikePostRequest request, int userId);

    /// <summary>
    /// 收藏动态
    /// </summary>
    /// <param name="postId">动态ID</param>
    /// <param name="request">收藏请求</param>
    /// <param name="userId">用户ID</param>
    /// <returns>收藏响应</returns>
    Task<ApiResponse<CollectPostResponse>> CollectPostAsync(long postId, CollectPostRequest request, int userId);

    /// <summary>
    /// 评论动态
    /// </summary>
    /// <param name="postId">动态ID</param>
    /// <param name="request">评论请求</param>
    /// <param name="userId">用户ID</param>
    /// <returns>评论响应</returns>
    Task<ApiResponse<CommentPostResponse>> CommentPostAsync(long postId, CommentPostRequest request, int userId);

    /// <summary>
    /// 获取我的发布
    /// </summary>
    /// <param name="request">获取我的发布请求</param>
    /// <param name="userId">用户ID</param>
    /// <returns>我的发布响应</returns>
    Task<ApiResponse<GetMyPostsResponse>> GetMyPostsAsync(GetMyPostsRequest request, int userId);

    /// <summary>
    /// 删除动态
    /// </summary>
    /// <param name="postId">动态ID</param>
    /// <param name="userId">用户ID</param>
    /// <returns>删除响应</returns>
    Task<ApiResponse> DeletePostAsync(long postId, int userId);

    /// <summary>
    /// 保存草稿
    /// </summary>
    /// <param name="request">保存草稿请求</param>
    /// <param name="userId">用户ID</param>
    /// <returns>保存草稿响应</returns>
    Task<ApiResponse<CreateDraftResponse>> SaveDraftAsync(CreateDraftRequest request, int userId);

    /// <summary>
    /// 获取草稿列表
    /// </summary>
    /// <param name="userId">用户ID</param>
    /// <returns>草稿列表响应</returns>
    Task<ApiResponse<GetDraftsResponse>> GetDraftsAsync(int userId);
}