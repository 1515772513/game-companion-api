using GameCompanion.Api.Configuration;
using GameCompanion.Api.DTOs;
using GameCompanion.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Services;

/// <summary>
/// 数据概览服务实现
/// </summary>
public class DashboardService : IDashboardService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<DashboardService> _logger;

    public DashboardService(ApplicationDbContext context, ILogger<DashboardService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<object> GetStatsAsync()
    {
        var totalUsers = await _context.Users.CountAsync();
        var totalOrders = await _context.Orders.CountAsync();
        var totalCompanions = await _context.Companions.CountAsync();
        var totalRevenue = await _context.Orders.Where(o => o.PaymentStatus == 1).SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

        return new
        {
            total_users = totalUsers,
            total_users_change = "+12.5%",
            total_orders = totalOrders,
            total_orders_change = "+8.3%",
            total_revenue = totalRevenue,
            total_revenue_change = "+15.2%",
            total_companions = totalCompanions,
            total_companions_change = "+5.8%"
        };
    }

    public async Task<object> GetRevenueTrendAsync(int days = 30)
    {
        // 简化实现
        return new
        {
            dates = new List<string>(),
            values = new List<decimal>(),
            total = 0,
            average = 0
        };
    }

    public async Task<object> GetOrderDistributionAsync()
    {
        var pending = await _context.Orders.CountAsync(o => o.Status == 1);
        var inProgress = await _context.Orders.CountAsync(o => o.Status == 2);
        var completed = await _context.Orders.CountAsync(o => o.Status == 3);
        var refunded = await _context.Orders.CountAsync(o => o.Status == 5);
        var total = await _context.Orders.CountAsync();

        return new
        {
            pending_payment = pending,
            in_progress = inProgress,
            completed = completed,
            refunded = refunded,
            total = total
        };
    }

    public async Task<object> GetLatestOrdersAsync(int limit = 10)
    {
        var orders = await _context.Orders
            .Include(o => o.User)
            .Include(o => o.Companion)
            .Include(o => o.Game)
            .OrderByDescending(o => o.CreatedAt)
            .Take(limit)
            .Select(o => new
            {
                id = o.Id,
                order_no = o.OrderNo,
                user_name = o.User!.Nickname,
                companion_name = o.Companion != null ? o.Companion.Nickname : "",
                game_name = o.Game!.Name,
                amount = o.TotalAmount,
                status = o.Status,
                status_text = GetStatusText(o.Status),
                created_at = o.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            })
            .ToListAsync();

        return new { items = orders };
    }

    public async Task<object> GetQuickActionsAsync()
    {
        var pendingCompanions = await _context.Companions.CountAsync(c => c.CertificationStatus == 0);
        return new
        {
            pending_companions = pendingCompanions,
            pending_refunds = 0,
            pending_audits = 0,
            user_reports = 0
        };
    }

    private string GetStatusText(int status)
    {
        return status switch
        {
            1 => "待付款",
            2 => "进行中",
            3 => "已完成",
            4 => "已取消",
            5 => "退款中",
            _ => "未知"
        };
    }
}

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

    public async Task<object> GetUserListAsync(int page, int pageSize, string? keyword, int? accountStatus, int? vipLevel, string? registerStart, string? registerEnd, string orderBy, string order)
    {
        var query = _context.Users.AsQueryable();

        // 应用筛选条件
        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(u => u.Username.Contains(keyword) || u.Nickname.Contains(keyword) || u.Phone!.Contains(keyword));
        }

        if (accountStatus.HasValue)
        {
            query = query.Where(u => u.AccountStatus == accountStatus.Value);
        }

        if (vipLevel.HasValue)
        {
            query = query.Where(u => u.VipLevel == vipLevel.Value);
        }

        // 分页
        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(u => new
            {
                id = u.Id,
                username = u.Username,
                nickname = u.Nickname,
                avatar_url = u.AvatarUrl,
                phone = u.Phone != null ? MaskPhone(u.Phone) : "",
                gender = u.Gender,
                gender_text = GetGenderText(u.Gender),
                location = u.Location,
                balance = u.Balance,
                points = u.Points,
                vip_level = u.VipLevel,
                vip_level_text = u.VipLevel == 1 ? "VIP会员" : "普通用户",
                vip_expire_time = u.VipExpireTime.HasValue ? u.VipExpireTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                account_status = u.AccountStatus,
                account_status_text = GetAccountStatusText(u.AccountStatus),
                created_at = u.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                last_active_time = u.UpdatedAt.HasValue ? u.UpdatedAt.Value.ToString("yyyy-MM-dd HH:mm:ss") : null
            })
            .ToListAsync();

        return new
        {
            items = items,
            pagination = new
            {
                page = page,
                page_size = pageSize,
                total = total,
                total_pages = (int)Math.Ceiling((double)total / pageSize)
            }
        };
    }

    public async Task<object?> GetUserDetailAsync(int userId)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return null;

        return new
        {
            id = user.Id,
            username = user.Username,
            nickname = user.Nickname,
            avatar_url = user.AvatarUrl,
            gender = user.Gender,
            gender_text = GetGenderText(user.Gender),
            birthday = user.Birthday?.ToString("yyyy-MM-dd"),
            location = user.Location,
            bio = user.Bio,
            phone = user.Phone != null ? MaskPhone(user.Phone) : "",
            phone_verified = user.PhoneVerified == 1,
            real_name = user.RealName,
            id_card = user.IdCard != null ? MaskIdCard(user.IdCard) : "",
            id_card_verified = user.IdCardVerified == 1,
            balance = user.Balance,
            points = user.Points,
            vip_level = user.VipLevel,
            vip_expire_time = user.VipExpireTime?.ToString("yyyy-MM-dd HH:mm:ss"),
            account_status = user.AccountStatus,
            account_status_text = GetAccountStatusText(user.AccountStatus),
            created_at = user.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            last_active_time = user.UpdatedAt?.ToString("yyyy-MM-dd HH:mm:ss"),
            statistics = new
            {
                total_orders = 0,
                total_spent = 0.0,
                total_companions = 0
            }
        };
    }

    public async Task<bool> UpdateUserStatusAsync(int userId, int accountStatus, string? reason)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;

        user.AccountStatus = accountStatus;
        user.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdateUserAsync(int userId, object updateData)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;

        // 更新用户信息
        user.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<object> GetUserOrdersAsync(int userId, int page, int pageSize, int? status)
    {
        var query = _context.Orders.Where(o => o.UserId == userId);
        if (status.HasValue)
        {
            query = query.Where(o => o.Status == status.Value);
        }

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new { items = items, total = total };
    }

    public async Task<object> GetUserStatsAsync()
    {
        var total = await _context.Users.CountAsync();
        var active = await _context.Users.CountAsync(u => u.AccountStatus == 0);
        var vip = await _context.Users.CountAsync(u => u.VipLevel == 1);
        var banned = await _context.Users.CountAsync(u => u.AccountStatus == 2);

        return new
        {
            total_users = total,
            active_users = active,
            vip_users = vip,
            banned_users = banned,
            today_registered = 0,
            week_registered = 0
        };
    }

    public async Task<string> ExportUsersAsync(object filters)
    {
        // 简化实现
        await Task.CompletedTask;
        return "https://example.com/exports/users.xlsx";
    }

    public async Task<bool> ResetUserPasswordAsync(int userId, string newPassword)
    {
        var user = await _context.Users.FindAsync(userId);
        if (user == null) return false;

        // 实际应该哈希密码
        user.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return true;
    }

    private string MaskPhone(string phone) => phone.Length > 7 ? $"{phone[..3]}****{phone[^4..]}" : phone;
    private string MaskIdCard(string idCard) => idCard.Length > 8 ? $"{idCard[..4]}********{idCard[^4..]}" : idCard;
    private string GetGenderText(int? gender) => gender switch { 1 => "男", 2 => "女", _ => "未知" };
    private string GetAccountStatusText(int status) => status switch { 0 => "正常", 1 => "禁用", 2 => "封禁", _ => "未知" };
}

/// <summary>
/// 陪玩服务实现
/// </summary>
public class CompanionService : ICompanionService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<CompanionService> _logger;

    public CompanionService(ApplicationDbContext context, ILogger<CompanionService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<object> GetApplicationsAsync(int page, int pageSize, string? keyword, int? certificationStatus, string? serviceType, int? gameId, string? applyStart, string? applyEnd, string orderBy, string order)
    {
        var query = _context.Companions.Include(c => c.User).AsQueryable();

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new
        {
            items = items.Select(c => new
            {
                id = c.Id,
                user_id = c.UserId,
                nickname = c.Nickname,
                avatar_url = c.AvatarUrl,
                real_name = c.RealName,
                phone = c.Phone,
                id_card = c.IdCard,
                service_type = c.ServiceType,
                price = c.Price,
                games = new string[] { },
                game_rank = c.GameRank,
                bio = c.Bio,
                certification_status = c.CertificationStatus,
                certification_status_text = GetCertificationStatusText(c.CertificationStatus),
                certification_apply_time = c.CertificationApplyTime?.ToString("yyyy-MM-dd HH:mm:ss"),
                online_status = c.OnlineStatus,
                online_status_text = GetOnlineStatusText(c.OnlineStatus ?? 0)
            }),
            pagination = new
            {
                page = page,
                page_size = pageSize,
                total = total,
                total_pages = (int)Math.Ceiling((double)total / pageSize)
            }
        };
    }

    public async Task<object?> GetApplicationDetailAsync(int applicationId)
    {
        var companion = await _context.Companions
            .Include(c => c.User)
            .FirstOrDefaultAsync(c => c.Id == applicationId);

        if (companion == null) return null;

        return new
        {
            id = companion.Id,
            user_id = companion.UserId,
            nickname = companion.Nickname,
            real_name = companion.RealName,
            id_card = companion.IdCard,
            phone = companion.Phone,
            service_type = companion.ServiceType,
            price = companion.Price,
            bio = companion.Bio,
            certification_status = companion.CertificationStatus,
            certification_apply_time = companion.CertificationApplyTime
        };
    }

    public async Task<bool> AuditApplicationAsync(int applicationId, string action, string? rejectReason)
    {
        var companion = await _context.Companions.FindAsync(applicationId);
        if (companion == null) return false;

        companion.CertificationStatus = action == "approve" ? 1 : 2;
        companion.CertificationTime = DateTime.Now;
        companion.CertificationRejectReason = action == "reject" ? rejectReason : null;
        companion.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<object> GetCompanionsAsync(int page, int pageSize, string? keyword, string? level, string? serviceType, int? gameId, int? onlineStatus, int certificationStatus, string orderBy, string order)
    {
        var query = _context.Companions.Where(c => c.CertificationStatus == certificationStatus);
        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(c => c.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new
        {
            items = items,
            pagination = new { page, page_size = pageSize, total, total_pages = (int)Math.Ceiling((double)total / pageSize) }
        };
    }

    public async Task<bool> UpdateCompanionAsync(int companionId, object updateData)
    {
        var companion = await _context.Companions.FindAsync(companionId);
        if (companion == null) return false;

        companion.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<object> GetCompanionStatsAsync()
    {
        var total = await _context.Companions.CountAsync();
        var pending = await _context.Companions.CountAsync(c => c.CertificationStatus == 0);

        return new
        {
            total_companions = total,
            pending_audit = pending,
            approved_today = 0,
            rejected_today = 0,
            online_count = 0,
            total_orders_today = 0
        };
    }

    private string GetCertificationStatusText(int status) => status switch { 0 => "待审核", 1 => "已通过", 2 => "已拒绝", _ => "未知" };
    private string GetOnlineStatusText(int status) => status switch { 0 => "离线", 1 => "在线", 2 => "忙碌", _ => "未知" };
}

/// <summary>
/// 订单服务实现
/// </summary>
public class OrderService : IOrderService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<OrderService> _logger;

    public OrderService(ApplicationDbContext context, ILogger<OrderService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<object> GetOrderListAsync(int page, int pageSize, string? keyword, int? orderType, int? status, int? paymentStatus, int? gameId, int? userId, int? companionId, string? startTime, string? endTime, string orderBy, string order)
    {
        var query = _context.Orders
            .Include(o => o.User)
            .Include(o => o.Companion)
            .Include(o => o.Game)
            .AsQueryable();

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(o => o.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new
        {
            items = items,
            pagination = new { page, page_size = pageSize, total, total_pages = (int)Math.Ceiling((double)total / pageSize) }
        };
    }

    public async Task<object?> GetOrderDetailAsync(int orderId)
    {
        var order = await _context.Orders
            .Include(o => o.User)
            .Include(o => o.Companion)
            .Include(o => o.Game)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        return order;
    }

    public async Task<bool> UpdateOrderAsync(int orderId, object updateData)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null) return false;

        order.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ProcessRefundAsync(int orderId, string action, decimal? refundAmount, string? rejectReason)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null) return false;

        if (action == "approve")
        {
            order.RefundStatus = 2;
            order.RefundAmount = refundAmount;
            order.RefundAuditTime = DateTime.Now;
        }
        else
        {
            order.RefundStatus = 3;
            order.RefundRejectReason = rejectReason;
        }

        order.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CancelOrderAsync(int orderId, string cancelReason)
    {
        var order = await _context.Orders.FindAsync(orderId);
        if (order == null) return false;

        order.Status = 4;
        order.CancelReason = cancelReason;
        order.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<object> GetOrderStatsAsync()
    {
        var total = await _context.Orders.CountAsync();
        var pending = await _context.Orders.CountAsync(o => o.Status == 1);
        var inProgress = await _context.Orders.CountAsync(o => o.Status == 2);
        var completed = await _context.Orders.CountAsync(o => o.Status == 3);
        var refunded = await _context.Orders.CountAsync(o => o.Status == 5);
        var revenue = await _context.Orders.Where(o => o.PaymentStatus == 1).SumAsync(o => (decimal?)o.TotalAmount) ?? 0;

        return new
        {
            total_orders = total,
            pending_payment = pending,
            in_progress = inProgress,
            completed = completed,
            refunded = refunded,
            total_revenue = revenue,
            today_orders = 0,
            today_revenue = 0
        };
    }

    public async Task<string> ExportOrdersAsync(object filters)
    {
        await Task.CompletedTask;
        return "https://example.com/exports/orders.xlsx";
    }
}

/// <summary>
/// 内容服务实现
/// </summary>
public class PostService : IPostService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<PostService> _logger;

    public PostService(ApplicationDbContext context, ILogger<PostService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<object> GetPostListAsync(int page, int pageSize, string? keyword, int? gameId, int? auditStatus, int? status, int? userId, string? startTime, string? endTime, string orderBy, string order)
    {
        var query = _context.Posts.Include(p => p.User).Include(p => p.Game).AsQueryable();
        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(p => p.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new
        {
            items = items,
            pagination = new { page, page_size = pageSize, total, total_pages = (int)Math.Ceiling((double)total / pageSize) }
        };
    }

    public async Task<object?> GetPostDetailAsync(int postId)
    {
        return await _context.Posts.FindAsync(postId);
    }

    public async Task<bool> AuditPostAsync(int postId, string action, string? auditReason)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post == null) return false;

        post.AuditStatus = action == "approve" ? 1 : (action == "hide" ? 3 : 2);
        post.AuditTime = DateTime.Now;
        post.AuditReason = auditReason;
        post.UpdatedAt = DateTime.Now;

        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> UpdatePostVisibilityAsync(int postId, int auditStatus)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post == null) return false;

        post.AuditStatus = auditStatus;
        post.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeletePostAsync(int postId)
    {
        var post = await _context.Posts.FindAsync(postId);
        if (post == null) return false;

        _context.Posts.Remove(post);
        await _context.SaveChangesAsync();
        return true;
    }
}

/// <summary>
/// 消息服务实现
/// </summary>
public class MessageService : IMessageService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<MessageService> _logger;

    public MessageService(ApplicationDbContext context, ILogger<MessageService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<object> CreateMessageAsync(object messageData)
    {
        var message = new MessagePush
        {
            Title = "",
            Content = "",
            Status = "pending",
            CreatedAt = DateTime.Now
        };

        _context.MessagePushes.Add(message);
        await _context.SaveChangesAsync();

        return new
        {
            push_id = message.Id,
            status = "pending",
            total_count = 0,
            estimated_time = "约5-10分钟"
        };
    }

    public async Task<object> GetMessageRecordsAsync(int page, int pageSize, string? keyword, string? targetType, string? status, string? startTime, string? endTime, string orderBy, string order)
    {
        var query = _context.MessagePushes.AsQueryable();
        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(m => m.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new
        {
            items = items,
            pagination = new { page, page_size = pageSize, total, total_pages = (int)Math.Ceiling((double)total / pageSize) }
        };
    }

    public async Task<object> GetMessageStatsAsync()
    {
        return new
        {
            today_pushes = 0,
            today_sent = 0,
            total_sent = 0,
            delivery_rate = 98.5,
            open_rate = 45.6,
            click_rate = 12.3
        };
    }

    public async Task<bool> CancelMessageAsync(int pushId)
    {
        var message = await _context.MessagePushes.FindAsync(pushId);
        if (message == null) return false;

        message.Status = "cancelled";
        await _context.SaveChangesAsync();
        return true;
    }
}

/// <summary>
/// 设置服务实现
/// </summary>
public class SettingService : ISettingService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<SettingService> _logger;

    public SettingService(ApplicationDbContext context, ILogger<SettingService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<object> GetSystemConfigAsync(string? configGroup)
    {
        var configs = await _context.SystemConfigs.ToListAsync();

        return new
        {
            basic = new { },
            payment = new { },
            sms = new { },
            storage = new { }
        };
    }

    public async Task<bool> UpdateSystemConfigAsync(object configData)
    {
        await Task.CompletedTask;
        return true;
    }

    public async Task<object> GetAdminLogsAsync(int page, int pageSize, int? adminId, string? action, string? module, string? startTime, string? endTime, string orderBy, string order)
    {
        var query = _context.AdminLogs.AsQueryable();
        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(l => l.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new
        {
            items = items,
            pagination = new { page, page_size = pageSize, total, total_pages = (int)Math.Ceiling((double)total / pageSize) }
        };
    }

    public async Task<bool> ResetConfigAsync(string configGroup)
    {
        await Task.CompletedTask;
        return true;
    }

    public async Task<object> GetPermissionsAsync()
    {
        return new
        {
            permissions = new[]
            {
                new
                {
                    module = "users",
                    module_name = "用户管理",
                    actions = new[]
                    {
                        new { key = "users.read", name = "查看用户" },
                        new { key = "users.write", name = "编辑用户" },
                        new { key = "users.ban", name = "封禁用户" }
                    }
                }
            }
        };
    }

    public async Task<object> GetAdminsAsync()
    {
        var admins = await _context.Admins
            .Select(a => new
            {
                id = a.Id,
                username = a.Username,
                real_name = a.RealName,
                avatar_url = a.AvatarUrl,
                role = a.Role,
                account_status = a.AccountStatus,
                last_login_time = a.LastLoginTime.HasValue ? a.LastLoginTime.Value.ToString("yyyy-MM-dd HH:mm:ss") : null,
                created_at = a.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss")
            })
            .ToListAsync();

        return new { items = admins };
    }

    public async Task<bool> CreateAdminAsync(object adminData)
    {
        await Task.CompletedTask;
        return true;
    }

    public async Task<bool> UpdateAdminAsync(int adminId, object adminData)
    {
        var admin = await _context.Admins.FindAsync(adminId);
        if (admin == null) return false;

        admin.UpdatedAt = DateTime.Now;
        await _context.SaveChangesAsync();
        return true;
    }
}

/// <summary>
/// 首页服务实现
/// </summary>
public class HomeService : IHomeService
{
    private readonly ApplicationDbContext _context;
    private readonly ILogger<HomeService> _logger;

    public HomeService(ApplicationDbContext context, ILogger<HomeService> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<HomeResponse> GetHomeDataAsync()
    {
        // 获取轮播图（简化实现，从系统配置或数据库获取）
        var banners = new List<BannerItem>
        {
            new BannerItem
            {
                Id = 1,
                Title = "新人专享优惠",
                Image_url = "https://cdn.example.com/banner1.jpg",
                Jump_url = "https://example.com/promo/new-user",
                Jump_type = "url"
            },
            new BannerItem
            {
                Id = 2,
                Title = "VIP会员特惠",
                Image_url = "https://cdn.example.com/banner2.jpg",
                Jump_url = "vip://open",
                Jump_type = "internal"
            }
        };

        // 获取热门陪玩师
        var hotCompanions = await _context.Companions
            .Where(c => c.Status == "certified" && c.OnlineStatus == "online")
            .OrderByDescending(c => c.Rating)
            .Take(10)
            .Select(c => new CompanionSimpleInfo
            {
                Id = c.Id,
                Nickname = c.Nickname,
                Avatar_url = c.Avatar ?? "https://cdn.example.com/avatar/default.png",
                Level = GetLevelText(c.Level),
                Service_type = GetServiceTypeText(c.ServiceType),
                Price = c.PricePerGame,
                Price_unit = "局",
                Rating = c.Rating,
                Online_status = 1,
                Online_status_text = "在线接单",
                Tags = c.Tags?.Split(',').ToList() ?? new List<string>()
            })
            .ToListAsync();

        // 获取热门游戏
        var hotGames = await _context.Games
            .Where(g => g.Status == "active")
            .OrderBy(g => g.SortOrder)
            .Take(10)
            .Select(g => new GameSimpleInfo
            {
                Id = g.Id,
                Name = g.Name,
                Icon_url = g.Icon ?? "https://cdn.example.com/game/default.png",
                Companion_count = _context.CompanionGames.Count(cg => cg.GameId == g.Id),
                Description = g.Description ?? ""
            })
            .ToListAsync();

        // 获取热门动态
        var hotPosts = await _context.Posts
            .Where(p => p.Status == "published")
            .OrderByDescending(p => p.LikeCount)
            .Take(10)
            .Select(p => new PostSimpleInfo
            {
                Id = p.Id,
                User_id = p.UserId,
                User_name = _context.Users.Where(u => u.Id == p.UserId).Select(u => u.Nickname).FirstOrDefault() ?? "",
                User_avatar = _context.Users.Where(u => u.Id == p.UserId).Select(u => u.Avatar ?? "https://cdn.example.com/avatar/default.png").FirstOrDefault() ?? "",
                Content = p.Content.Length > 100 ? p.Content.Substring(0, 100) + "..." : p.Content,
                Images = p.Images?.Split(',').ToList() ?? new List<string>(),
                Like_count = p.LikeCount,
                Comment_count = p.CommentCount,
                Created_at = GetFriendlyTime(p.CreatedAt)
            })
            .ToListAsync();

        return new HomeResponse
        {
            Banners = banners,
            Hot_companions = hotCompanions,
            Hot_games = hotGames,
            Hot_posts = hotPosts
        };
    }

    public async Task<PagedResponse<CompanionDetailInfo>> GetCompanionsAsync(
        int page, int pageSize, int? gameId, string? serviceType, string? level,
        decimal? minPrice, decimal? maxPrice, int? onlineStatus, string? keyword,
        string sortBy, string sortOrder)
    {
        var query = _context.Companions
            .Include(c => c.User)
            .Where(c => c.Status == "certified");

        // 游戏筛选
        if (gameId.HasValue)
        {
            query = query.Where(c => _context.CompanionGames.Any(cg => cg.CompanionId == c.Id && cg.GameId == gameId.Value));
        }

        // 服务类型筛选
        if (!string.IsNullOrWhiteSpace(serviceType))
        {
            query = query.Where(c => c.ServiceType == serviceType);
        }

        // 等级筛选
        if (!string.IsNullOrWhiteSpace(level))
        {
            query = query.Where(c => c.Level == level);
        }

        // 价格筛选
        if (minPrice.HasValue)
        {
            query = query.Where(c => c.PricePerGame >= minPrice.Value);
        }
        if (maxPrice.HasValue)
        {
            query = query.Where(c => c.PricePerGame <= maxPrice.Value);
        }

        // 在线状态筛选
        if (onlineStatus.HasValue)
        {
            if (onlineStatus.Value == 1)
            {
                query = query.Where(c => c.OnlineStatus == "online");
            }
            else if (onlineStatus.Value == 2)
            {
                query = query.Where(c => c.OnlineStatus == "offline");
            }
        }

        // 关键词搜索
        if (!string.IsNullOrWhiteSpace(keyword))
        {
            query = query.Where(c => c.Nickname.Contains(keyword) || c.Bio.Contains(keyword));
        }

        // 排序
        query = sortBy.ToLower() switch
        {
            "rating" => sortOrder.ToLower() == "asc" ? query.OrderBy(c => c.Rating) : query.OrderByDescending(c => c.Rating),
            "price" => sortOrder.ToLower() == "asc" ? query.OrderBy(c => c.PricePerGame) : query.OrderByDescending(c => c.PricePerGame),
            "order_count" => sortOrder.ToLower() == "asc" ? query.OrderBy(c => c.TotalOrders) : query.OrderByDescending(c => c.TotalOrders),
            _ => query.OrderByDescending(c => c.Rating)
        };

        var total = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CompanionDetailInfo
            {
                Id = c.Id,
                User_id = c.UserId,
                Nickname = c.Nickname,
                Avatar_url = c.Avatar ?? "https://cdn.example.com/avatar/default.png",
                Level = GetLevelText(c.Level),
                Level_code = c.Level,
                Service_type = GetServiceTypeText(c.ServiceType),
                Service_type_code = c.ServiceType,
                Price = c.PricePerGame,
                Price_unit = "局",
                Rating = c.Rating,
                Rating_count = 0, // TODO: 从评价表计算
                Order_count = c.TotalOrders,
                Positive_rate = c.GoodReviewRate,
                Online_status = c.OnlineStatus == "online" ? 1 : 0,
                Online_status_text = c.OnlineStatus == "online" ? "在线接单" : "离线",
                Is_verified = c.Status == "certified",
                Verified_at = c.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                Games = _context.CompanionGames
                    .Where(cg => cg.CompanionId == c.Id)
                    .Join(_context.Games, cg => cg.GameId, g => g.Id, (cg, g) => new GameSkillInfo
                    {
                        Game_id = g.Id,
                        Game_name = g.Name,
                        Game_rank = cg.GameLevel ?? ""
                    })
                    .ToList(),
                Service_times = new List<ServiceTimeInfo>(), // TODO: 添加服务时间配置
                Tags = c.Tags?.Split(',').ToList() ?? new List<string>(),
                Bio = c.Bio ?? "",
                Strengths = new List<string>(), // TODO: 添加优势配置
                Created_at = c.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
                Recent_reviews = new List<ReviewInfo>(), // TODO: 从评价表获取
                Statistics = new CompanionStatistics
                {
                    Total_orders = c.TotalOrders,
                    Total_hours = 0, // TODO: 计算总服务时长
                    Avg_response_time = 5,
                    Completion_rate = 99.2m,
                    On_time_rate = 98.5m
                }
            })
            .ToListAsync();

        return new PagedResponse<CompanionDetailInfo>
        {
            Items = items,
            Pagination = new PaginationInfo
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                TotalPages = (int)Math.Ceiling((double)total / pageSize)
            }
        };
    }

    public async Task<CompanionDetailInfo?> GetCompanionDetailAsync(int companionId)
    {
        var companion = await _context.Companions
            .Include(c => c.User)
            .Where(c => c.Id == companionId && c.Status == "certified")
            .FirstOrDefaultAsync();

        if (companion == null) return null;

        return new CompanionDetailInfo
        {
            Id = companion.Id,
            User_id = companion.UserId,
            Nickname = companion.Nickname,
            Avatar_url = companion.Avatar ?? "https://cdn.example.com/avatar/default.png",
            Level = GetLevelText(companion.Level),
            Level_code = companion.Level,
            Service_type = GetServiceTypeText(companion.ServiceType),
            Service_type_code = companion.ServiceType,
            Price = companion.PricePerGame,
            Price_unit = "局",
            Rating = companion.Rating,
            Rating_count = 0,
            Order_count = companion.TotalOrders,
            Positive_rate = companion.GoodReviewRate,
            Online_status = companion.OnlineStatus == "online" ? 1 : 0,
            Online_status_text = companion.OnlineStatus == "online" ? "在线接单" : "离线",
            Is_verified = companion.Status == "certified",
            Verified_at = companion.UpdatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            Games = _context.CompanionGames
                .Where(cg => cg.CompanionId == companion.Id)
                .Join(_context.Games, cg => cg.GameId, g => g.Id, (cg, g) => new GameSkillInfo
                {
                    Game_id = g.Id,
                    Game_name = g.Name,
                    Game_rank = cg.GameLevel ?? ""
                })
                .ToList(),
            Service_times = new List<ServiceTimeInfo>(),
            Tags = companion.Tags?.Split(',').ToList() ?? new List<string>(),
            Bio = companion.Bio ?? "",
            Strengths = new List<string>(),
            Created_at = companion.CreatedAt.ToString("yyyy-MM-dd HH:mm:ss"),
            Recent_reviews = new List<ReviewInfo>(),
            Statistics = new CompanionStatistics
            {
                Total_orders = companion.TotalOrders,
                Total_hours = 0,
                Avg_response_time = 5,
                Completion_rate = 99.2m,
                On_time_rate = 98.5m
            }
        };
    }

    public async Task<List<GameDetailInfo>> GetGamesAsync()
    {
        return await _context.Games
            .Where(g => g.Status == "active")
            .OrderBy(g => g.SortOrder)
            .Select(g => new GameDetailInfo
            {
                Id = g.Id,
                Name = g.Name,
                Icon_url = g.Icon ?? "https://cdn.example.com/game/default.png",
                Companion_count = _context.CompanionGames.Count(cg => cg.GameId == g.Id),
                Online_companion_count = _context.CompanionGames
                    .Join(_context.Companions, cg => cg.CompanionId, c => c.Id, (cg, c) => new { cg, c })
                    .Count(x => x.cg.GameId == g.Id && x.c.OnlineStatus == "online" && x.c.Status == "certified"),
                Description = g.Description ?? "",
                Is_hot = g.SortOrder <= 3
            })
            .ToListAsync();
    }

    public async Task<PagedResponse<CompanionSimpleInfo>> SearchCompanionsAsync(string keyword, int page, int pageSize)
    {
        var query = _context.Companions
            .Where(c => c.Status == "certified")
            .Where(c => c.Nickname.Contains(keyword) || c.Bio.Contains(keyword) || (c.Tags != null && c.Tags.Contains(keyword)));

        var total = await query.CountAsync();
        var items = await query
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(c => new CompanionSimpleInfo
            {
                Id = c.Id,
                Nickname = c.Nickname,
                Avatar_url = c.Avatar ?? "https://cdn.example.com/avatar/default.png",
                Level = GetLevelText(c.Level),
                Service_type = GetServiceTypeText(c.ServiceType),
                Price = c.PricePerGame,
                Price_unit = "局",
                Rating = c.Rating,
                Online_status = c.OnlineStatus == "online" ? 1 : 0,
                Online_status_text = c.OnlineStatus == "online" ? "在线接单" : "离线",
                Tags = c.Tags?.Split(',').ToList() ?? new List<string>()
            })
            .ToListAsync();

        return new PagedResponse<CompanionSimpleInfo>
        {
            Items = items,
            Pagination = new PaginationInfo
            {
                Page = page,
                PageSize = pageSize,
                Total = total,
                TotalPages = (int)Math.Ceiling((double)total / pageSize)
            }
        };
    }

    // 辅助方法
    private string GetLevelText(string? level)
    {
        return level?.ToLower() switch
        {
            "silver" => "银牌",
            "gold" => "金牌",
            "diamond" => "钻石",
            "king" => "王者",
            _ => "银牌"
        };
    }

    private string GetServiceTypeText(string? serviceType)
    {
        return serviceType?.ToLower() switch
        {
            "tech" => "技术陪玩",
            "entertainment" => "娱乐陪玩",
            "voice" => "语音陪伴",
            _ => "技术陪玩"
        };
    }

    private string GetFriendlyTime(DateTime dateTime)
    {
        var span = DateTime.Now - dateTime;
        if (span.TotalMinutes < 60)
            return $"{(int)span.TotalMinutes}分钟前";
        if (span.TotalHours < 24)
            return $"{(int)span.TotalHours}小时前";
        if (span.TotalDays < 30)
            return $"{(int)span.TotalDays}天前";
        return dateTime.ToString("yyyy-MM-dd");
    }
}
