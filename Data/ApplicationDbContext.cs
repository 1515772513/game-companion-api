using GameCompanion.Api.Models.Entities;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Data;

/// <summary>
/// 应用程序数据库上下文
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // 用户表
    public DbSet<User> Users { get; set; }

    // 陪玩师表
    public DbSet<Companion> Companions { get; set; }

    // 订单表
    public DbSet<Order> Orders { get; set; }

    // 动态表
    public DbSet<Post> Posts { get; set; }

    // 消息表
    public DbSet<Message> Messages { get; set; }

    // 会话表
    public DbSet<Conversation> Conversations { get; set; }

    // 通知表
    public DbSet<Notification> Notifications { get; set; }

    // 游戏表
    public DbSet<Game> Games { get; set; }

    // 游戏圈子表
    public DbSet<GameCircle> GameCircles { get; set; }

    // 陪玩师游戏技能表
    public DbSet<CompanionGame> CompanionGames { get; set; }

    // 陪玩需求表
    public DbSet<CompanionRequest> CompanionRequests { get; set; }

    // 代练服务表
    public DbSet<PowerLeveling> PowerLevelings { get; set; }

    // 订单评价表
    public DbSet<OrderReview> OrderReviews { get; set; }

    // 动态评论表
    public DbSet<PostComment> PostComments { get; set; }

    // 动态点赞表
    public DbSet<PostLike> PostLikes { get; set; }

    // 收藏表
    public DbSet<Collection> Collections { get; set; }

    // 用户收藏表
    public DbSet<UserCollection> UserCollections { get; set; }

    // 关注表
    public DbSet<Follow> Follows { get; set; }

    // 交易记录表
    public DbSet<Transaction> Transactions { get; set; }

    // 陪玩师申请表
    public DbSet<CompanionApplication> CompanionApplications { get; set; }

    // 优惠券表
    public DbSet<Coupon> Coupons { get; set; }

    // VIP会员表
    public DbSet<VipMembership> VipMemberships { get; set; }

    // 用户设置表
    public DbSet<UserSetting> UserSettings { get; set; }

    // 意见反馈表
    public DbSet<Feedback> Feedbacks { get; set; }

    // 草稿表
    public DbSet<Draft> Drafts { get; set; }

    // 文件表
    public DbSet<SysFile> SysFiles { get; set; }

    // 系统配置表（新增）
    public DbSet<SystemConfig> SystemConfigs { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Companion>()
        .HasMany(x => x.CompanionGames)
        .WithOne()
        .HasForeignKey(x => x.CompanionId)
        .OnDelete(DeleteBehavior.Cascade);

        // 配置实体关系和约束
        ConfigureUser(modelBuilder);
        ConfigureCompanion(modelBuilder);
        ConfigureOrder(modelBuilder);
        ConfigurePost(modelBuilder);
        ConfigureMessage(modelBuilder);
        ConfigureSystemConfig(modelBuilder);
    }

    private void ConfigureUser(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.Phone).IsUnique();
            entity.HasIndex(e => e.Username).IsUnique();
            entity.Property(e => e.Balance).HasPrecision(10, 2);
        });

        // 配置关注关系
        modelBuilder.Entity<Follow>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => new { e.FollowerId, e.FollowingId }).IsUnique();
            entity.HasOne(e => e.Follower)
                  .WithMany()
                  .HasForeignKey(e => e.FollowerId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Following)
                  .WithMany()
                  .HasForeignKey(e => e.FollowingId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 配置用户收藏
        modelBuilder.Entity<UserCollection>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => new { e.UserId, e.ItemType, e.ItemId }).IsUnique();
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 配置交易记录
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.OrderId);
            entity.Property(e => e.Amount).HasPrecision(10, 2);
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 配置陪玩师申请
        modelBuilder.Entity<CompanionApplication>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Status);
            entity.Property(e => e.HourlyRate).HasPrecision(10, 2);
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // 配置意见反馈
        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.Status);
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }

    private void ConfigureCompanion(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Companion>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                  .WithOne()
                  .HasForeignKey<Companion>(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.PricePerGame).HasPrecision(10, 2);
            entity.Property(e => e.PricePerHour).HasPrecision(10, 2);
            entity.Property(e => e.Rating).HasPrecision(3, 2);
            entity.Property(e => e.GoodReviewRate).HasPrecision(5, 2);
        });
    }

    private void ConfigureOrder(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.OrderNo).IsUnique();
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Companion)
                  .WithMany()
                  .HasForeignKey(e => e.CompanionId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.Property(e => e.UnitPrice).HasPrecision(10, 2);
            entity.Property(e => e.TotalPrice).HasPrecision(10, 2);
            entity.Property(e => e.DiscountAmount).HasPrecision(10, 2);
            entity.Property(e => e.FinalPrice).HasPrecision(10, 2);
        });
    }

    private void ConfigurePost(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<PostLike>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Post)
                  .WithMany()
                  .HasForeignKey(e => e.PostId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => e.PostId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => new { e.PostId, e.UserId }).IsUnique();
        });

        modelBuilder.Entity<PostComment>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Post)
                  .WithMany()
                  .HasForeignKey(e => e.PostId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.User)
                  .WithMany()
                  .HasForeignKey(e => e.UserId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Parent)
                  .WithMany()
                  .HasForeignKey(e => e.ParentId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasIndex(e => e.PostId);
            entity.HasIndex(e => e.UserId);
            entity.HasIndex(e => e.ParentId);
        });
    }

    private void ConfigureMessage(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasOne(e => e.Sender)
                  .WithMany()
                  .HasForeignKey(e => e.SenderId)
                  .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(e => e.Receiver)
                  .WithMany()
                  .HasForeignKey(e => e.ReceiverId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }

    /// <summary>
    /// 系统配置表约束（新增）
    /// </summary>
    private void ConfigureSystemConfig(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SystemConfig>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.HasIndex(e => e.ConfigKey).IsUnique();
            entity.Property(e => e.ConfigValue).HasColumnType("text");
        });
    }
}