using GameCompanion.Api.Data;
using GameCompanion.Api.DTOs.Home;
using GameCompanion.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Services;

/// <summary>
/// 首页服务实现
/// </summary>
public class HomeService : IHomeService
{
    private readonly ApplicationDbContext _context;

    public HomeService(ApplicationDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// 获取首页数据
    /// </summary>
    public async Task<ApiResponse<HomeDataResponse>> GetHomeDataAsync()
    {
        var response = new HomeDataResponse();

        try
        {
            // 获取轮播图（目前数据为空，可根据需要添加）
            response.Banners = new List<BannerDto>();

            // 获取热门陪玩师
            response.HotCompanions = await _context.Companions
                .Where(c => c.OnlineStatus == "online" && c.Rating >= 4.5)
                .OrderByDescending(c => c.Rating)
                .Take(6)
                .Select(c => new CompanionSummaryDto
                {
                    Id = c.Id,
                    Nickname = c.Nickname,
                    AvatarUrl = c.User?.Avatar ?? "",
                    Level = c.Level ?? "银牌",
                    ServiceType = c.ServiceType ?? "娱乐陪玩",
                    Price = c.PricePerGame,
                    PriceUnit = "局",
                    Rating = c.Rating ?? 0,
                    OnlineStatus = c.OnlineStatus == "online" ? 1 : 0,
                    OnlineStatusText = c.OnlineStatus == "online" ? "在线接单" : "离线",
                    Tags = c.Tags?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>()
                })
                .ToListAsync();

            // 获取热门游戏
            response.HotGames = await _context.Games
                .Where(g => g.Status == "active")
                .OrderByDescending(g => g.CompanionGames.Count)
                .Take(8)
                .Select(g => new GameDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    IconUrl = g.Icon ?? "",
                    CompanionCount = g.CompanionGames.Count,
                    Description = g.Description ?? ""
                })
                .ToListAsync();

            // 获取热门动态
            response.HotPosts = await _context.Posts
                .Where(p => p.Status == "published")
                .OrderByDescending(p => p.LikeCount + p.CommentCount)
                .Take(5)
                .Select(p => new PostDto
                {
                    Id = p.Id,
                    UserId = p.UserId,
                    UserName = p.User?.Nickname ?? "",
                    UserAvatar = p.User?.Avatar ?? "",
                    Content = p.Content,
                    Images = p.Images?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(),
                    LikeCount = p.LikeCount ?? 0,
                    CommentCount = p.CommentCount ?? 0,
                    CreatedAt = GetRelativeTime(p.CreatedAt)
                })
                .ToListAsync();

            return ApiResponse<HomeDataResponse>.SuccessResponse(response, "获取成功");
        }
        catch (Exception ex)
        {
            return ApiResponse<HomeDataResponse>.ErrorResponse(500, ex.Message, "系统错误");
        }
    }

    /// <summary>
    /// 获取陪玩师列表
    /// </summary>
    public async Task<ApiResponse<CompanionListResponse>> GetCompanionsAsync(CompanionListRequest request)
    {
        try
        {
            var query = _context.Companions
                .Include(c => c.User)
                .Include(c => c.CompanionGames)
                    .ThenInclude(cg => cg.Game)
                .Where(c => c.Status == "approved");

            // 游戏筛选
            if (request.GameId.HasValue)
            {
                query = query.Where(c => c.CompanionGames.Any(cg => cg.GameId == request.GameId.Value));
            }

            // 服务类型筛选
            if (!string.IsNullOrEmpty(request.ServiceType))
            {
                query = query.Where(c => c.ServiceType == request.ServiceType);
            }

            // 等级筛选
            if (!string.IsNullOrEmpty(request.Level))
            {
                query = query.Where(c => c.Level == request.Level);
            }

            // 价格区间筛选
            if (request.MinPrice.HasValue)
            {
                query = query.Where(c => c.PricePerGame >= request.MinPrice.Value);
            }

            if (request.MaxPrice.HasValue)
            {
                query = query.Where(c => c.PricePerGame <= request.MaxPrice.Value);
            }

            // 在线状态筛选
            if (request.OnlineStatus.HasValue)
            {
                switch (request.OnlineStatus.Value)
                {
                    case 1: // 仅在线
                        query = query.Where(c => c.OnlineStatus == "online");
                        break;
                    case 2: // 仅离线
                        query = query.Where(c => c.OnlineStatus != "online");
                        break;
                }
            }

            // 关键词搜索
            if (!string.IsNullOrEmpty(request.Keyword))
            {
                query = query.Where(c => c.Nickname.Contains(request.Keyword) ||
                                       c.Tags.Contains(request.Keyword));
            }

            // 排序
            switch (request.SortBy.ToLower())
            {
                case "rating":
                    query = request.SortOrder.ToLower() == "asc"
                        ? query.OrderBy(c => c.Rating)
                        : query.OrderByDescending(c => c.Rating);
                    break;
                case "price":
                    query = request.SortOrder.ToLower() == "asc"
                        ? query.OrderBy(c => c.PricePerGame)
                        : query.OrderByDescending(c => c.PricePerGame);
                    break;
                case "order_count":
                    query = request.SortOrder.ToLower() == "asc"
                        ? query.OrderBy(c => c.TotalOrders)
                        : query.OrderByDescending(c => c.TotalOrders);
                    break;
                default:
                    query = query.OrderByDescending(c => c.Rating);
                    break;
            }

            // 分页
            var total = await query.CountAsync();
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(c => new CompanionDetailDto
                {
                    Id = c.Id,
                    UserId = c.UserId,
                    Nickname = c.Nickname,
                    AvatarUrl = c.User?.Avatar ?? "",
                    Level = c.Level ?? "银牌",
                    LevelCode = c.Level ?? "silver",
                    ServiceType = c.ServiceType == "tech" ? "技术陪玩" : "娱乐陪玩",
                    ServiceTypeCode = c.ServiceType ?? "entertainment",
                    Price = c.PricePerGame,
                    PriceUnit = "局",
                    Rating = c.Rating ?? 0,
                    RatingCount = c.OrderReviews?.Count ?? 0,
                    OrderCount = c.TotalOrders ?? 0,
                    PositiveRate = c.GoodReviewRate ?? 0,
                    OnlineStatus = c.OnlineStatus == "online" ? 1 : 0,
                    OnlineStatusText = c.OnlineStatus == "online" ? "在线接单" : "离线",
                    Games = c.CompanionGames.Select(cg => cg.Game?.Name ?? "").ToList(),
                    GameRank = c.CompanionGames.FirstOrDefault()?.GameLevel ?? "",
                    Tags = c.Tags?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(),
                    Bio = c.Bio ?? ""
                })
                .ToListAsync();

            return ApiResponse<CompanionListResponse>.SuccessResponse(new CompanionListResponse
            {
                Items = items,
                Pagination = new PaginationDto
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    Total = total,
                    TotalPages = (int)Math.Ceiling((double)total / request.PageSize),
                    HasMore = request.Page * request.PageSize < total
                }
            }, "获取成功");
        }
        catch (Exception ex)
        {
            return ApiResponse<CompanionListResponse>.ErrorResponse(500, ex.Message, "系统错误");
        }
    }

    /// <summary>
    /// 获取陪玩师详情
    /// </summary>
    public async Task<ApiResponse<CompanionDetailResponse>> GetCompanionDetailAsync(int companionId)
    {
        try
        {
            var companion = await _context.Companions
                .Include(c => c.User)
                .Include(c => c.CompanionGames)
                    .ThenInclude(cg => cg.Game)
                .Include(c => c.OrderReviews)
                    .ThenInclude(r => r.User)
                .FirstOrDefaultAsync(c => c.Id == companionId);

            if (companion == null)
            {
                return ApiResponse<CompanionDetailResponse>.ErrorResponse(2001, "指定的陪玩师ID不存在或已被删除", "陪玩师不存在");
            }

            if (companion.Status != "approved")
            {
                return ApiResponse<CompanionDetailResponse>.ErrorResponse(2002, "该陪玩师尚未通过平台认证", "陪玩师未认证");
            }

            var response = new CompanionDetailResponse
            {
                Id = companion.Id,
                UserId = companion.UserId,
                Nickname = companion.Nickname,
                AvatarUrl = companion.User?.Avatar ?? "",
                Level = companion.Level ?? "银牌",
                LevelCode = companion.Level ?? "silver",
                ServiceType = companion.ServiceType == "tech" ? "技术陪玩" : "娱乐陪玩",
                ServiceTypeCode = companion.ServiceType ?? "entertainment",
                Price = companion.PricePerGame,
                PriceUnit = "局",
                Rating = companion.Rating ?? 0,
                RatingCount = companion.OrderReviews?.Count ?? 0,
                OrderCount = companion.TotalOrders ?? 0,
                PositiveRate = companion.GoodReviewRate ?? 0,
                OnlineStatus = companion.OnlineStatus == "online" ? 1 : 0,
                OnlineStatusText = companion.OnlineStatus == "online" ? "在线接单" : "离线",
                IsVerified = companion.Status == "approved",
                VerifiedAt = companion.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss") ?? "",
                Games = companion.CompanionGames.Select(cg => new GameSkillDto
                {
                    GameId = cg.GameId,
                    GameName = cg.Game?.Name ?? "",
                    GameRank = cg.GameLevel ?? ""
                }).ToList(),
                ServiceTimes = new List<ServiceTimeDto>(), // 服务时间需要额外配置
                Tags = companion.Tags?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>(),
                Bio = companion.Bio ?? "",
                Strengths = new List<string>(), // 优势需要额外配置
                RecentReviews = companion.OrderReviews
                    .OrderByDescending(r => r.CreatedAt)
                    .Take(5)
                    .Select(r => new CompanionReviewDto
                    {
                        Id = r.Id,
                        OrderId = r.OrderId,
                        UserName = r.User?.Nickname ?? "",
                        UserAvatar = r.User?.Avatar ?? "",
                        Rating = r.Rating,
                        Comment = r.Content ?? "",
                        ServiceDate = r.CreatedAt.ToString("yyyy-MM-dd HH:mm"),
                        CreatedAt = GetRelativeTime(r.CreatedAt)
                    }).ToList(),
                Statistics = new CompanionStatisticsDto
                {
                    TotalOrders = companion.TotalOrders ?? 0,
                    TotalHours = companion.TotalOrders ?? 0, // 需要根据订单计算
                    AvgResponseTime = 5, // 平均响应时间
                    CompletionRate = companion.GoodReviewRate ?? 99,
                    OnTimeRate = 98.5m
                }
            };

            return ApiResponse<CompanionDetailResponse>.SuccessResponse(response, "获取成功");
        }
        catch (Exception ex)
        {
            return ApiResponse<CompanionDetailResponse>.ErrorResponse(500, ex.Message, "系统错误");
        }
    }

    /// <summary>
    /// 获取游戏列表
    /// </summary>
    public async Task<ApiResponse<GameListResponse>> GetGamesAsync()
    {
        try
        {
            var games = await _context.Games
                .Where(g => g.Status == "active")
                .Select(g => new GameDetailDto
                {
                    Id = g.Id,
                    Name = g.Name,
                    IconUrl = g.Icon ?? "",
                    CompanionCount = g.CompanionGames.Count,
                    OnlineCompanionCount = g.CompanionGames.Count(cg => cg.Companion.OnlineStatus == "online"),
                    Description = g.Description ?? "",
                    IsHot = g.CompanionGames.Count > 100 // 陪玩师数量大于100为热门
                })
                .OrderBy(g => g.SortOrder)
                .ToListAsync();

            return ApiResponse<GameListResponse>.SuccessResponse(new GameListResponse
            {
                Items = games
            }, "获取成功");
        }
        catch (Exception ex)
        {
            return ApiResponse<GameListResponse>.ErrorResponse(500, ex.Message, "系统错误");
        }
    }

    /// <summary>
    /// 搜索陪玩师
    /// </summary>
    public async Task<ApiResponse<SearchCompanionsResponse>> SearchCompanionsAsync(SearchCompanionsRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Keyword) || request.Keyword.Length < 2)
            {
                return ApiResponse<SearchCompanionsResponse>.ErrorResponse(400, "搜索关键词至少需要2个字符", "请求参数错误");
            }

            var query = _context.Companions
                .Include(c => c.User)
                .Where(c => c.Status == "approved" &&
                           (c.Nickname.Contains(request.Keyword) ||
                            (c.Tags != null && c.Tags.Contains(request.Keyword)) ||
                            (c.Bio != null && c.Bio.Contains(request.Keyword))));

            var total = await query.CountAsync();
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(c => new CompanionSummaryDto
                {
                    Id = c.Id,
                    Nickname = c.Nickname,
                    AvatarUrl = c.User?.Avatar ?? "",
                    Level = c.Level ?? "银牌",
                    ServiceType = c.ServiceType == "tech" ? "技术陪玩" : "娱乐陪玩",
                    Price = c.PricePerGame,
                    PriceUnit = "局",
                    Rating = c.Rating ?? 0,
                    OnlineStatus = c.OnlineStatus == "online" ? 1 : 0,
                    OnlineStatusText = c.OnlineStatus == "online" ? "在线接单" : "离线",
                    Tags = c.Tags?.Split(',', StringSplitOptions.RemoveEmptyEntries).ToList() ?? new List<string>()
                })
                .ToListAsync();

            return ApiResponse<SearchCompanionsResponse>.SuccessResponse(new SearchCompanionsResponse
            {
                Items = items,
                Pagination = new PaginationDto
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    Total = total,
                    TotalPages = (int)Math.Ceiling((double)total / request.PageSize),
                    HasMore = request.Page * request.PageSize < total
                }
            }, "搜索成功");
        }
        catch (Exception ex)
        {
            return ApiResponse<SearchCompanionsResponse>.ErrorResponse(500, ex.Message, "系统错误");
        }
    }

    /// <summary>
    /// 获取游戏圈子列表
    /// </summary>
    public async Task<ApiResponse<CirclesListResponse>> GetCirclesAsync(CirclesListRequest request)
    {
        try
        {
            var query = _context.GameCircles
                .Include(c => c.Game)
                .Where(c => c.Status == "active");

            // 游戏筛选
            if (request.GameId.HasValue)
            {
                query = query.Where(c => c.GameId == request.GameId.Value);
            }

            var total = await query.CountAsync();
            var items = await query
                .Skip((request.Page - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(c => new CircleDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    GameId = c.GameId,
                    GameName = c.Game?.Name ?? "",
                    AvatarUrl = c.Icon ?? "",
                    MemberCount = c.MemberCount ?? 0,
                    PostCount = c.PostCount ?? 0,
                    OnlineCount = 0, // 在线人数需要额外计算
                    Description = c.Description ?? "",
                    IsOfficial = c.Name.Contains("官方") || c.Name.Contains("官方群"),
                    CreatedAt = c.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
                })
                .ToListAsync();

            return ApiResponse<CirclesListResponse>.SuccessResponse(new CirclesListResponse
            {
                Items = items,
                Pagination = new PaginationDto
                {
                    Page = request.Page,
                    PageSize = request.PageSize,
                    Total = total,
                    TotalPages = (int)Math.Ceiling((double)total / request.PageSize),
                    HasMore = request.Page * request.PageSize < total
                }
            }, "获取成功");
        }
        catch (Exception ex)
        {
            return ApiResponse<CirclesListResponse>.ErrorResponse(500, ex.Message, "系统错误");
        }
    }

    /// <summary>
    /// 获取相对时间
    /// </summary>
    private string GetRelativeTime(DateTimeOffset dateTime)
    {
        var now = DateTimeOffset.Now;
        var diff = now - dateTime;

        if (diff.TotalMinutes < 60)
        {
            return $"{(int)diff.TotalMinutes}分钟前";
        }
        else if (diff.TotalHours < 24)
        {
            return $"{(int)diff.TotalHours}小时前";
        }
        else if (diff.TotalDays < 30)
        {
            return $"{(int)diff.TotalDays}天前";
        }
        else
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm");
        }
    }
}
