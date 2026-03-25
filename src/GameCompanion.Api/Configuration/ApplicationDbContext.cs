using GameCompanion.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace GameCompanion.Api.Configuration;

/// <summary>
/// 应用程序数据库上下文
/// </summary>
public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    // DbSets
    public DbSet<Admin> Admins { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Companion> Companions { get; set; }
    public DbSet<Game> Games { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<Post> Posts { get; set; }
    public DbSet<AdminLog> AdminLogs { get; set; }
    public DbSet<SystemConfig> SystemConfigs { get; set; }
    public DbSet<MessagePush> MessagePushes { get; set; }
    public DbSet<Notification> Notifications { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 配置Admin实体
        modelBuilder.Entity<Admin>(entity =>
        {
            entity.HasIndex(e => e.Username).IsUnique();
            entity.Property(e => e.Permissions).HasColumnType("json");
        });

        // 配置User实体
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasIndex(e => e.Phone).IsUnique();
            entity.HasIndex(e => e.Username).IsUnique();
        });

        // 配置Companion实体
        modelBuilder.Entity<Companion>(entity =>
        {
            entity.HasOne(e => e.User)
                .WithMany(u => u.Companions)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.Tags).HasColumnType("json");
            entity.Property(e => e.Games).HasColumnType("json");
        });

        // 配置Game实体
        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasIndex(e => e.Name).IsUnique();
        });

        // 配置Order实体
        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasIndex(e => e.OrderNo).IsUnique();

            entity.HasOne(e => e.User)
                .WithMany(u => u.Orders)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Companion)
                .WithMany(c => c.Orders)
                .HasForeignKey(e => e.CompanionId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.HasOne(e => e.Game)
                .WithMany(g => g.Orders)
                .HasForeignKey(e => e.GameId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // 配置Post实体
        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasOne(e => e.User)
                .WithMany(u => u.Posts)
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(e => e.Game)
                .WithMany(g => g.Posts)
                .HasForeignKey(e => e.GameId)
                .OnDelete(DeleteBehavior.SetNull);

            entity.Property(e => e.Images).HasColumnType("json");
            entity.Property(e => e.MentionedUsers).HasColumnType("json");
        });

        // 配置AdminLog实体
        modelBuilder.Entity<AdminLog>(entity =>
        {
            entity.HasIndex(e => e.AdminId);
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.Module);
            entity.Property(e => e.RequestParams).HasColumnType("json");
        });

        // 配置SystemConfig实体
        modelBuilder.Entity<SystemConfig>(entity =>
        {
            entity.HasIndex(e => e.ConfigKey).IsUnique();
        });

        // 配置MessagePush实体
        modelBuilder.Entity<MessagePush>(entity =>
        {
            entity.HasIndex(e => e.CreatedAt);
            entity.HasIndex(e => e.Status);
            entity.Property(e => e.TargetCriteria).HasColumnType("json");
        });

        // 配置Notification实体
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasOne(e => e.User)
                .WithMany()
                .HasForeignKey(e => e.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.Property(e => e.ExtraData).HasColumnType("json");
        });
    }
}
