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

    // 关注表
    public DbSet<Follow> Follows { get; set; }

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

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 配置实体关系和约束
        ConfigureUser(modelBuilder);
        ConfigureCompanion(modelBuilder);
        ConfigureOrder(modelBuilder);
        ConfigurePost(modelBuilder);
        ConfigureMessage(modelBuilder);
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
}
