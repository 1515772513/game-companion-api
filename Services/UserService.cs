using GameCompanion.Api.Data;
using GameCompanion.Api.DTOs.User;
using GameCompanion.Api.Models;
using GameCompanion.Api.Models.Entities;
using GameCompanion.Api.Services;
using GameCompanion.Api.Utils;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Services;

/// <summary>
/// 用户服务实现
/// </summary>
public class UserService : IUserService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<UserService> _logger;

    public UserService(ApplicationDbContext context, ILogger<UserService> logger)
    {
        _context = context;
        _logger = logger;
    }

    /// <summary>
    /// 获取用户个人信息
    /// </summary>
    public async Task<ApiResponse<UserProfileDto>> GetProfileAsync(int userId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return ApiResponse<UserProfileDto>.Fail(404, "用户不存在");
            }

            var profileDto = new UserProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                Nickname = user.Nickname,
                RealName = user.RealName,
                IdCard = user.IdCard,
                Phone = user.Phone,
                Avatar = user.Avatar,
                Gender = user.Gender,
                Age = user.Age,
                Name = user.Name,
                Bio = user.Bio,
                VipLevel = user.VipLevel,
                VipExpireDate = user.VipExpireDate,
                Points = user.Points,
                Balance = user.Balance,
                Status = user.Status.GetSafeInt(),
                StatusCn = user.Status.GetStatusCn(), // 状态:1=禁用,0=正常
                LastLoginTime = user.LastLoginTime,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return ApiResponse<UserProfileDto>.Success(profileDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取用户个人信息失败");
            return ApiResponse<UserProfileDto>.Fail(500, "获取用户个人信息失败");
        }
    }

    /// <summary>
    /// 更新用户个人资料
    /// </summary>
    public async Task<ApiResponse<UserProfileDto>> UpdateProfileAsync(int userId, UpdateProfileDto updateDto)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return ApiResponse<UserProfileDto>.Fail(404, "用户不存在");
            }

            // 更新用户信息
            user.Nickname = updateDto.Nickname;
            user.RealName = updateDto.RealName;
            user.IdCard = updateDto.IdCard;
            user.Phone = updateDto.Phone;
            user.Gender = updateDto.Gender;
            user.Age = updateDto.Age;
            user.Name = updateDto.Name;
            user.Bio = updateDto.Bio;
            user.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var profileDto = new UserProfileDto
            {
                Id = user.Id,
                Username = user.Username,
                Nickname = user.Nickname,
                RealName = user.RealName,
                IdCard = user.IdCard,
                Phone = user.Phone,
                Avatar = user.Avatar,
                Gender = user.Gender,
                Age = user.Age,
                Name = user.Name,
                Bio = user.Bio,
                VipLevel = user.VipLevel,
                VipExpireDate = user.VipExpireDate,
                Points = user.Points,
                Balance = user.Balance,
                Status = user.Status.GetSafeInt(),
                StatusCn = user.Status.GetStatusCn(), // 状态:1=禁用,0=正常
                LastLoginTime = user.LastLoginTime,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt
            };

            return ApiResponse<UserProfileDto>.Success(profileDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "更新用户个人资料失败");
            return ApiResponse<UserProfileDto>.Fail(500, "更新用户个人资料失败");
        }
    }

    /// <summary>
    /// 上传用户头像
    /// </summary>
    public async Task<ApiResponse<string>> UploadAvatarAsync(int userId, string avatarUrl)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return ApiResponse<string>.Fail(404, "用户不存在");
            }

            user.Avatar = avatarUrl;
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return ApiResponse<string>.Success(avatarUrl);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "上传用户头像失败");
            return ApiResponse<string>.Fail(500, "上传用户头像失败");
        }
    }

    /// <summary>
    /// 实名认证
    /// </summary>
    public async Task<ApiResponse<bool>> VerifyRealNameAsync(int userId, VerifyRealNameDto verifyDto)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return ApiResponse<bool>.Fail(404, "用户不存在");
            }

            // 简单的实名认证，实际项目中应该调用第三方API进行验证
            user.RealName = verifyDto.RealName;
            user.IdCard = verifyDto.IdCard;
            user.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();

            return ApiResponse<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "实名认证失败");
            return ApiResponse<bool>.Fail(500, "实名认证失败");
        }
    }

    /// <summary>
    /// 获取用户收藏列表
    /// </summary>
    public async Task<ApiResponse<CollectionsResponseDto>> GetCollectionsAsync(int userId, int page = 1, int pageSize = 10)
    {
        try
        {
            var skip = (page - 1) * pageSize;
            var collections = await _context.UserCollections
                .Where(c => c.UserId == userId)
                .OrderByDescending(c => c.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await _context.UserCollections
                .Where(c => c.UserId == userId)
                .CountAsync();

            var collectionDtos = collections.Select(c => new UserCollectionDto
            {
                Id = c.Id,
                Title = c.Title,
                Description = c.Description,
                Category = c.Category,
                CreatedAt = c.CreatedAt
            }).ToList();

            var responseDto = new CollectionsResponseDto
            {
                Collections = collectionDtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return ApiResponse<CollectionsResponseDto>.Success(responseDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取用户收藏列表失败");
            return ApiResponse<CollectionsResponseDto>.Fail(500, "获取用户收藏列表失败");
        }
    }

    /// <summary>
    /// 获取关注列表
    /// </summary>
    public async Task<ApiResponse<FollowingListResponseDto>> GetFollowingAsync(int userId, int page = 1, int pageSize = 10)
    {
        try
        {
            var skip = (page - 1) * pageSize;
            var follows = await _context.Follows
                .Where(f => f.FollowerId == userId)
                .Include(f => f.Following)
                .OrderByDescending(f => f.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await _context.Follows
                .Where(f => f.FollowerId == userId)
                .CountAsync();

            var followingDtos = follows.Select(f => new UserListDto
            {
                Id = f.Following!.Id,
                Username = f.Following.Username,
                Nickname = f.Following.Nickname,
                Avatar = f.Following.Avatar,
                Bio = f.Following.Bio,
                VipLevel = f.Following.VipLevel,
                Points = f.Following.Points,
                Status = f.Following.Status.GetSafeInt(),
                StatusCn = f.Following.Status.GetStatusCn(), // 状态:1=禁用,0=正常
                // 格式化时间格式为yyyy-MM-dd HH:mm:ss
                CreatedAt = f.Following.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList();

            var responseDto = new FollowingListResponseDto
            {
                Following = followingDtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return ApiResponse<FollowingListResponseDto>.Success(responseDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取关注列表失败");
            return ApiResponse<FollowingListResponseDto>.Fail(500, "获取关注列表失败");
        }
    }

    /// <summary>
    /// 获取粉丝列表
    /// </summary>
    public async Task<ApiResponse<FollowersListResponseDto>> GetFollowersAsync(int userId, int page = 1, int pageSize = 10)
    {
        try
        {
            var skip = (page - 1) * pageSize;
            var follows = await _context.Follows
                .Where(f => f.FollowingId == userId)
                .Include(f => f.Follower)
                .OrderByDescending(f => f.CreatedAt)
                .Skip(skip)
                .Take(pageSize)
                .ToListAsync();

            var totalCount = await _context.Follows
                .Where(f => f.FollowingId == userId)
                .CountAsync();

            var followerDtos = follows.Select(f => new UserListDto
            {
                Id = f.Follower!.Id,
                Username = f.Follower.Username,
                Nickname = f.Follower.Nickname,
                Avatar = f.Follower.Avatar,
                Bio = f.Follower.Bio,
                VipLevel = f.Follower.VipLevel,
                Points = f.Follower.Points,
                Status = f.Follower.Status.GetSafeInt(),
                StatusCn = f.Follower.Status.GetStatusCn(), // 状态:1=正常,0=禁用
                // 格式化时间格式为yyyy-MM-dd HH:mm:ss
                CreatedAt = f.Follower.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToList();

            var responseDto = new FollowersListResponseDto
            {
                Followers = followerDtos,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };

            return ApiResponse<FollowersListResponseDto>.Success(responseDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取粉丝列表失败");
            return ApiResponse<FollowersListResponseDto>.Fail(500, "获取粉丝列表失败");
        }
    }

    /// <summary>
    /// 关注/取消关注用户
    /// </summary>
    public async Task<ApiResponse<bool>> FollowUserAsync(int currentUserId, FollowUserDto followDto)
    {
        try
        {
            var targetUser = await _context.Users.FindAsync(followDto.UserId);
            if (targetUser == null)
            {
                return ApiResponse<bool>.Fail(404, "目标用户不存在");
            }

            // 检查是否已经关注
            var existingFollow = await _context.Follows
                .FirstOrDefaultAsync(f => f.FollowerId == currentUserId && f.FollowingId == followDto.UserId);

            if (existingFollow != null)
            {
                // 已关注，取消关注
                _context.Follows.Remove(existingFollow);
            }
            else
            {
                // 未关注，添加关注
                var follow = new Follow
                {
                    FollowerId = currentUserId,
                    FollowingId = followDto.UserId,
                    CreatedAt = DateTime.UtcNow
                };
                _context.Follows.Add(follow);
            }

            await _context.SaveChangesAsync();
            return ApiResponse<bool>.Success(true);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "关注/取消关注用户失败");
            return ApiResponse<bool>.Fail(500, "关注/取消关注用户失败");
        }
    }

    /// <summary>
    /// 申请成为陪玩师
    /// </summary>
    public async Task<ApiResponse<CompanionApplication>> ApplyCompanionAsync(int userId, ApplyCompanionDto applyDto)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return ApiResponse<CompanionApplication>.Fail(404, "用户不存在");
            }

            // 检查是否已经申请过
            var existingApplication = await _context.CompanionApplications
                .FirstOrDefaultAsync(a => a.UserId == userId && a.Status == "待审核");

            if (existingApplication != null)
            {
                return ApiResponse<CompanionApplication>.Fail(400, "您已经提交过申请，请等待审核结果");
            }

            var application = new CompanionApplication
            {
                UserId = userId,
                GameCategory = applyDto.GameCategory,
                SkillLevel = applyDto.SkillLevel,
                SelfIntroduction = applyDto.SelfIntroduction,
                HourlyRate = applyDto.HourlyRate,
                AvailableTime = applyDto.AvailableTime,
                Status = "待审核",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };

            _context.CompanionApplications.Add(application);
            await _context.SaveChangesAsync();

            return ApiResponse<CompanionApplication>.Success(application);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "申请成为陪玩师失败");
            return ApiResponse<CompanionApplication>.Fail(500, "申请成为陪玩师失败");
        }
    }

    /// <summary>
    /// 获取钱包信息
    /// </summary>
    public async Task<ApiResponse<WalletDto>> GetWalletAsync(int userId)
    {
        try
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
            {
                return ApiResponse<WalletDto>.Fail(404, "用户不存在");
            }

            var transactions = await _context.Transactions
                .Where(t => t.UserId == userId)
                .OrderByDescending(t => t.CreatedAt)
                .Take(20)
                .ToListAsync();

            var transactionDtos = transactions.Select(t => new TransactionDto
            {
                Id = t.Id,
                Type = t.Type,
                Amount = t.Amount,
                Description = t.Description,
                CreatedAt = t.CreatedAt,
                OrderId = t.OrderId,
                Status = t.Status
            }).ToList();

            var walletDto = new WalletDto
            {
                UserId = user.Id,
                Balance = user.Balance,
                Points = user.Points,
                VipLevel = user.VipLevel,
                VipExpireDate = user.VipExpireDate,
                Transactions = transactionDtos
            };

            return ApiResponse<WalletDto>.Success(walletDto);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取钱包信息失败");
            return ApiResponse<WalletDto>.Fail(500, "获取钱包信息失败");
        }
    }

    /// <summary>
    /// 获取用户列表 (分页)
    /// <param name="page">页码</param>
    /// <param name="pageSize">每页数量</param>
    /// <returns>用户列表</returns> 
    /// </summary>
    public async Task<ApiResponse<UserListListDto>> GetListAsync(int page = 1, int pageSize = 10)
    {
        try
        {
            var total = await _context.Users.CountAsync();
            var users = await _context.Users
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            // 打印信息
            _logger.LogInformation($"获取用户列表，总用户数: {total}，当前页码: {page}，每页数量: {pageSize}");

            var userDtos = users.Select(u => new UserListDto
            {
                Id = u.Id,
                Username = u.Username,
                Nickname = u.Nickname,
                Avatar = u.Avatar,
                Bio = u.Bio,
                VipLevel = u.VipLevel,
                Points = u.Points,
                Status = u.Status.GetSafeInt(),
                StatusCn = u.Status.GetStatusCn(), // 状态:1=禁用,0=正常
                // 格式化时间格式为yyyy-MM-dd HH:mm:ss
                CreatedAt = u.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            }).ToArray();

            return ApiResponse<UserListListDto>.Success(new UserListListDto
            {
                Total = total,
                Page = page,
                PageSize = pageSize,
                list = userDtos
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "获取用户列表失败");
            return ApiResponse<UserListListDto>.Fail(500, "获取用户列表失败");
        }
    }
}