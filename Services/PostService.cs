using GameCompanion.Api.Data;
using GameCompanion.Api.DTOs.Posts;
using GameCompanion.Api.Models;
using GameCompanion.Api.Models.Entities;
using GameCompanion.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Services;

/// <summary>
/// 动态服务实现
/// </summary>
public class PostService : IPostService
{
    private readonly ApplicationDbContext _context;

    public PostService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 发布动态
    /// </summary>
    public async Task<ApiResponse<CreatePostResponse>> CreatePostAsync(CreatePostRequest request, int userId)
    {
        // 验证内容长度
        if (string.IsNullOrWhiteSpace(request.Content) || request.Content.Length < 10)
        {
            return ApiResponse<CreatePostResponse>.ErrorResponse(400, "动态内容不能为空，至少输入10个字符");
        }

        // 验证图片数量
        if (request.Images != null && request.Images.Count > 9)
        {
            return ApiResponse<CreatePostResponse>.ErrorResponse(400, "最多支持9张图片，当前上传了" + request.Images.Count + "张");
        }

        // 检查发布频率限制
        var recentPosts = await _context.Posts
            .Where(p => p.UserId == userId && p.CreatedAt > DateTime.UtcNow.AddHours(-1))
            .CountAsync();

        if (recentPosts >= 10)
        {
            return ApiResponse<CreatePostResponse>.ErrorResponse(429, "发布过于频繁，请稍后再试");
        }

        // 创建动态实体
        var post = new Post
        {
            UserId = userId,
            Content = request.Content,
            Images = request.Images != null ? string.Join(",", request.Images) : null,
            GameId = request.GameId,
            Visibility = request.Visibility?.ToString(),
            Status = "已发布",
            LikeCount = 0,
            CommentCount = 0,
            ShareCount = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        _context.Posts.Add(post);
        await _context.SaveChangesAsync();

        // 返回响应
        var response = new CreatePostResponse
        {
            PostId = post.Id,
            Status = 1, // 1-已发布
            StatusText = "已发布",
            AuditStatus = 1, // 1-审核通过
            AuditStatusText = "审核通过",
            Content = post.Content,
            Images = post.Images?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
            CreatedAt = post.CreatedAt,
            EstimatedAuditTime = "5-10分钟"
        };

        return ApiResponse<CreatePostResponse>.SuccessResponse(response, "发布成功");
    }

    /// <summary>
    /// 获取动态列表
    /// </summary>
    public async Task<ApiResponse<GetPostsResponse>> GetPostsAsync(GetPostsRequest request, int userId)
    {
        var query = _context.Posts
            .Include(p => p.User)
            .Include(p => p.Comments)
            .Include(p => p.Likes)
            .AsQueryable();

        // 根据feed类型过滤
        if (request.FeedType == "follow")
        {
            // TODO: 实现关注用户的动态
            // 这里简化处理，暂时返回推荐内容
        }

        // 游戏筛选
        if (request.GameId.HasValue)
        {
            query = query.Where(p => p.GameId == request.GameId.Value);
        }

        // 分页查询
        var total = await query.CountAsync();
        var posts = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        // 构建响应
        var response = new GetPostsResponse
        {
            Items = posts.Select(p => new PostListItemDto
            {
                Id = p.Id,
                User = new PostUserDto
                {
                    Id = p.User.Id,
                    Nickname = p.User.Nickname,
                    AvatarUrl = p.User.Avatar,
                    IsVerified = false,
                    Level = null
                },
                Content = p.Content,
                Images = p.Images?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
                Game = p.Game != null ? new PostGameDto
                {
                    Id = p.Game.Id,
                    Name = p.Game.Name,
                    IconUrl = p.Game.IconUrl
                } : null,
                Location = p.Location,
                LikeCount = p.LikeCount ?? 0,
                CommentCount = p.CommentCount ?? 0,
                CollectCount = 0, // TODO: 实现收藏数统计
                ShareCount = p.ShareCount ?? 0,
                IsLiked = p.Likes.Any(l => l.UserId == userId),
                IsCollected = false, // TODO: 实现收藏状态检查
                CreatedAt = p.CreatedAt,
                TimeText = GetTimeText(p.CreatedAt)
            }).ToList(),
            Pagination = new PaginationDto
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Total = total,
                TotalPages = (int)Math.Ceiling((double)total / request.PageSize),
                HasMore = request.Page * request.PageSize < total
            }
        };

        return ApiResponse<GetPostsResponse>.SuccessResponse(response, "获取成功");
    }

    /// <summary>
    /// 获取动态详情
    /// </summary>
    public async Task<ApiResponse<GetPostDetailResponse>> GetPostDetailAsync(int postId, int userId)
    {
        var post = await _context.Posts
            .Include(p => p.User)
            .Include(p => p.Comments)
                .ThenInclude(c => c.User)
            .Include(p => p.Likes)
            .FirstOrDefaultAsync(p => p.Id == postId);

        if (post == null)
        {
            return ApiResponse<GetPostDetailResponse>.ErrorResponse(4001, "指定的动态ID不存在或已删除");
        }

        if (post.Status == "已删除")
        {
            return ApiResponse<GetPostDetailResponse>.ErrorResponse(4002, "该动态已被作者删除");
        }

        // 获取热门评论
        var hotComments = post.Comments
            .Where(c => c.ParentId == null)
            .OrderByDescending(c => c.LikeCount)
            .Take(3)
            .Select(c => new PostCommentDto
            {
                Id = c.Id,
                User = new PostUserDto
                {
                    Id = c.User.Id,
                    Nickname = c.User.Nickname,
                    AvatarUrl = c.User.Avatar
                },
                Content = c.Content,
                LikeCount = c.LikeCount ?? 0,
                IsLiked = c.Likes.Any(l => l.UserId == userId),
                CreatedAt = c.CreatedAt,
                Replies = c.Replies.Select(r => new PostCommentReplyDto
                {
                    Id = r.Id,
                    User = new PostUserDto
                    {
                        Id = r.User.Id,
                        Nickname = r.User.Nickname,
                        AvatarUrl = r.User.Avatar
                    },
                    Content = r.Content,
                    CreatedAt = r.CreatedAt
                }).ToList()
            }).ToList();

        var response = new GetPostDetailResponse
        {
            Id = post.Id,
            User = new PostUserDto
            {
                Id = post.User.Id,
                Nickname = post.User.Nickname,
                AvatarUrl = post.User.Avatar,
                IsVerified = false,
                Bio = post.User.Bio,
                FollowersCount = 0, // TODO: 实现粉丝数统计
                FollowingCount = 0 // TODO: 实现关注数统计
            },
            Content = post.Content,
            Images = post.Images?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
            Game = post.Game != null ? new PostGameDto
            {
                Id = post.Game.Id,
                Name = post.Game.Name,
                IconUrl = post.Game.IconUrl
            } : null,
            Location = post.Location,
            LikeCount = post.LikeCount ?? 0,
            CommentCount = post.CommentCount ?? 0,
            CollectCount = 0, // TODO: 实现收藏数统计
            ShareCount = post.ShareCount ?? 0,
            IsLiked = post.Likes.Any(l => l.UserId == userId),
            IsCollected = false, // TODO: 实现收藏状态检查
            IsAuthor = post.UserId == userId,
            CreatedAt = post.CreatedAt,
            HotComments = hotComments
        };

        return ApiResponse<GetPostDetailResponse>.SuccessResponse(response, "获取成功");
    }

    /// <summary>
    /// 点赞动态
    /// </summary>
    public async Task<ApiResponse<LikePostResponse>> LikePostAsync(int postId, LikePostRequest request, int userId)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post == null)
        {
            return ApiResponse<LikePostResponse>.ErrorResponse(4001, "指定的动态ID不存在或已删除");
        }

        if (request.Action.ToLower() == "like")
        {
            // 检查是否已经点赞
            var existingLike = await _context.PostLikes
                .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

            if (existingLike != null)
            {
                return ApiResponse<LikePostResponse>.ErrorResponse(4003, "您已经点赞过该动态，请勿重复操作");
            }

            // 创建点赞
            var like = new PostLike
            {
                PostId = postId,
                UserId = userId,
                CreatedAt = DateTime.UtcNow
            };

            _context.PostLikes.Add(like);
            post.LikeCount = (post.LikeCount ?? 0) + 1;
        }
        else if (request.Action.ToLower() == "unlike")
        {
            // 取消点赞
            var like = await _context.PostLikes
                .FirstOrDefaultAsync(l => l.PostId == postId && l.UserId == userId);

            if (like != null)
            {
                _context.PostLikes.Remove(like);
                post.LikeCount = Math.Max(0, (post.LikeCount ?? 0) - 1);
            }
        }
        else
        {
            return ApiResponse<LikePostResponse>.ErrorResponse(400, "无效的操作类型");
        }

        await _context.SaveChangesAsync();

        return ApiResponse<LikePostResponse>.SuccessResponse(new LikePostResponse
        {
            PostId = postId,
            IsLiked = request.Action.ToLower() == "like",
            LikeCount = post.LikeCount ?? 0
        }, "操作成功");
    }

    /// <summary>
    /// 收藏动态
    /// </summary>
    public async Task<ApiResponse<CollectPostResponse>> CollectPostAsync(int postId, CollectPostRequest request, int userId)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post == null)
        {
            return ApiResponse<CollectPostResponse>.ErrorResponse(4001, "指定的动态ID不存在或已删除");
        }

        var isCollecting = request.Action.ToLower() == "collect";

        // TODO: 实现收藏功能
        // 这里简化处理，仅更新标记和计数

        if (isCollecting)
        {
            post.CollectCount = (post.CollectCount ?? 0) + 1;
        }
        else
        {
            post.CollectCount = Math.Max(0, (post.CollectCount ?? 0) - 1);
        }

        await _context.SaveChangesAsync();

        return ApiResponse<CollectPostResponse>.SuccessResponse(new CollectPostResponse
        {
            PostId = postId,
            IsCollected = isCollecting,
            CollectCount = post.CollectCount ?? 0
        }, "操作成功");
    }

    /// <summary>
    /// 评论动态
    /// </summary>
    public async Task<ApiResponse<CommentPostResponse>> CommentPostAsync(int postId, CommentPostRequest request, int userId)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post == null)
        {
            return ApiResponse<CommentPostResponse>.ErrorResponse(4001, "指定的动态ID不存在或已删除");
        }

        // 检查评论频率限制
        var recentComments = await _context.PostComments
            .Where(c => c.UserId == userId && c.CreatedAt > DateTime.UtcNow.AddMinutes(-30))
            .CountAsync();

        if (recentComments >= 10)
        {
            return ApiResponse<CommentPostResponse>.ErrorResponse(429, "评论过于频繁，请稍后再试");
        }

        var comment = new PostComment
        {
            PostId = postId,
            UserId = userId,
            ParentId = request.ParentId,
            Content = request.Content,
            LikeCount = 0,
            CreatedAt = DateTime.UtcNow
        };

        _context.PostComments.Add(comment);
        post.CommentCount = (post.CommentCount ?? 0) + 1;
        await _context.SaveChangesAsync();

        var response = new CommentPostResponse
        {
            CommentId = comment.Id,
            Content = comment.Content,
            User = new PostUserDto
            {
                Id = userId,
                Nickname = _context.Users.Find(userId)?.Nickname ?? "",
                AvatarUrl = _context.Users.Find(userId)?.Avatar
            },
            LikeCount = 0,
            CreatedAt = comment.CreatedAt
        };

        return ApiResponse<CommentPostResponse>.SuccessResponse(response, "评论成功");
    }

    /// <summary>
    /// 获取我的发布
    /// </summary>
    public async Task<ApiResponse<GetMyPostsResponse>> GetMyPostsAsync(GetMyPostsRequest request, int userId)
    {
        var query = _context.Posts
            .Where(p => p.UserId == userId)
            .AsQueryable();

        // 状态筛选
        if (request.Status.HasValue)
        {
            var statusMap = new Dictionary<int, string>
            {
                { 0, "审核中" },
                { 1, "已发布" },
                { 2, "已拒绝" }
            };

            if (statusMap.ContainsKey(request.Status.Value))
            {
                query = query.Where(p => p.Status == statusMap[request.Status.Value]);
            }
        }

        var total = await query.CountAsync();
        var posts = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync();

        var response = new GetMyPostsResponse
        {
            Items = posts.Select(p => new MyPostItemDto
            {
                Id = p.Id,
                Content = p.Content,
                Images = p.Images?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
                Status = GetStatusValue(p.Status),
                StatusText = p.Status,
                LikeCount = p.LikeCount ?? 0,
                CommentCount = p.CommentCount ?? 0,
                CreatedAt = p.CreatedAt
            }).ToList(),
            Pagination = new PaginationDto
            {
                Page = request.Page,
                PageSize = request.PageSize,
                Total = total,
                TotalPages = (int)Math.Ceiling((double)total / request.PageSize),
                HasMore = request.Page * request.PageSize < total
            }
        };

        return ApiResponse<GetMyPostsResponse>.SuccessResponse(response, "获取成功");
    }

    /// <summary>
    /// 删除动态
    /// </summary>
    public async Task<ApiResponse> DeletePostAsync(int postId, int userId)
    {
        var post = await _context.Posts.FirstOrDefaultAsync(p => p.Id == postId && p.UserId == userId);
        if (post == null)
        {
            return ApiResponse.Error(403, "您只能删除自己发布的动态");
        }

        post.Status = "已删除";
        post.UpdatedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();

        return ApiResponse.Success("删除成功");
    }

    /// <summary>
    /// 保存草稿
    /// </summary>
    public async Task<ApiResponse<CreateDraftResponse>> SaveDraftAsync(CreateDraftRequest request, int userId)
    {
        // 验证内容长度
        if (string.IsNullOrWhiteSpace(request.Content) || request.Content.Length < 10)
        {
            return ApiResponse<CreateDraftResponse>.ErrorResponse(400, "动态内容不能为空，至少输入10个字符");
        }

        // 验证图片数量
        if (request.Images != null && request.Images.Count > 9)
        {
            return ApiResponse<CreateDraftResponse>.ErrorResponse(400, "最多支持9张图片，当前上传了" + request.Images.Count + "张");
        }

        Draft draft;
        var now = DateTime.UtcNow;

        if (request.DraftId.HasValue)
        {
            // 编辑现有草稿
            draft = await _context.Drafts.FirstOrDefaultAsync(d => d.Id == request.DraftId.Value && d.UserId == userId);
            if (draft == null)
            {
                return ApiResponse<CreateDraftResponse>.ErrorResponse(400, "草稿不存在");
            }

            draft.Content = request.Content;
            draft.Images = request.Images != null ? string.Join(",", request.Images) : null;
            draft.UpdatedAt = now;
        }
        else
        {
            // 创建新草稿
            draft = new Draft
            {
                UserId = userId,
                Content = request.Content,
                Images = request.Images != null ? string.Join(",", request.Images) : null,
                CreatedAt = now,
                UpdatedAt = now
            };

            _context.Drafts.Add(draft);
        }

        await _context.SaveChangesAsync();

        var response = new CreateDraftResponse
        {
            DraftId = draft.Id,
            Content = draft.Content,
            Images = draft.Images?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
            SavedAt = draft.UpdatedAt
        };

        return ApiResponse<CreateDraftResponse>.SuccessResponse(response, "保存成功");
    }

    /// <summary>
    /// 获取草稿列表
    /// </summary>
    public async Task<ApiResponse<GetDraftsResponse>> GetDraftsAsync(int userId)
    {
        var drafts = await _context.Drafts
            .Where(d => d.UserId == userId)
            .OrderByDescending(d => d.UpdatedAt)
            .ToListAsync();

        var response = new GetDraftsResponse
        {
            Items = drafts.Select(d => new DraftItemDto
            {
                DraftId = d.Id,
                Content = d.Content,
                Images = d.Images?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
                CreatedAt = d.CreatedAt,
                UpdatedAt = d.UpdatedAt
            }).ToList()
        };

        return ApiResponse<GetDraftsResponse>.SuccessResponse(response, "获取成功");
    }

    /// <summary>
    /// 获取时间文本
    /// </summary>
    private string GetTimeText(DateTime dateTime)
    {
        var now = DateTime.UtcNow;
        var diff = now - dateTime;

        if (diff.TotalMinutes < 1)
            return "刚刚";
        else if (diff.TotalMinutes < 60)
            return $"{(int)diff.TotalMinutes}分钟前";
        else if (diff.TotalHours < 24)
            return $"{(int)diff.TotalHours}小时前";
        else if (diff.TotalDays < 7)
            return $"{(int)diff.TotalDays}天前";
        else
            return dateTime.ToString("yyyy-MM-dd");
    }

    /// <summary>
    /// 获取状态值
    /// </summary>
    private int GetStatusValue(string status)
    {
        return status switch
        {
            "审核中" => 0,
            "已发布" => 1,
            "已拒绝" => 2,
            "已删除" => 3,
            _ => 1
        };
    }
}