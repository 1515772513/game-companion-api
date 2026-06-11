using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace GameCompanion.Api.Models.Entities;

public partial class GameCompanionContext : DbContext
{
    public GameCompanionContext()
    {
    }

    public GameCompanionContext(DbContextOptions<GameCompanionContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Collection> Collections { get; set; }

    public virtual DbSet<Companion> Companions { get; set; }

    public virtual DbSet<CompanionApplication> CompanionApplications { get; set; }

    public virtual DbSet<CompanionBackgroundImage> CompanionBackgroundImages { get; set; }

    public virtual DbSet<CompanionGame> CompanionGames { get; set; }

    public virtual DbSet<CompanionRequest> CompanionRequests { get; set; }

    public virtual DbSet<Conversation> Conversations { get; set; }

    public virtual DbSet<Coupon> Coupons { get; set; }

    public virtual DbSet<Draft> Drafts { get; set; }

    public virtual DbSet<Feedback> Feedbacks { get; set; }

    public virtual DbSet<Follow> Follows { get; set; }

    public virtual DbSet<Game> Games { get; set; }

    public virtual DbSet<GameCircle> GameCircles { get; set; }

    public virtual DbSet<Message> Messages { get; set; }

    public virtual DbSet<Notification> Notifications { get; set; }

    public virtual DbSet<Order> Orders { get; set; }

    public virtual DbSet<OrderReview> OrderReviews { get; set; }

    public virtual DbSet<Post> Posts { get; set; }

    public virtual DbSet<PostComment> PostComments { get; set; }

    public virtual DbSet<PostLike> PostLikes { get; set; }

    public virtual DbSet<PowerLeveling> PowerLevelings { get; set; }

    public virtual DbSet<RecoverYourDataInfo> RecoverYourDataInfos { get; set; }

    public virtual DbSet<SysDictDatum> SysDictData { get; set; }

    public virtual DbSet<SysDictType> SysDictTypes { get; set; }

    public virtual DbSet<SysFile> SysFiles { get; set; }

    public virtual DbSet<SystemConfig> SystemConfigs { get; set; }

    public virtual DbSet<Transaction> Transactions { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserCollection> UserCollections { get; set; }

    public virtual DbSet<UserSetting> UserSettings { get; set; }

    public virtual DbSet<VipMembership> VipMemberships { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // 运行时连接由 Program.cs 的 AddDbContext 注入（读取 appsettings 的 DefaultConnection）。
        // 仅当外部未配置时（例如设计时 dotnet ef 命令）才回退到下面的本地连接串。
        if (!optionsBuilder.IsConfigured)
        {
            optionsBuilder.UseMySql("server=localhost;port=3306;database=game_companion;user=root;password=123456;charset=utf8mb4", Microsoft.EntityFrameworkCore.ServerVersion.Parse("8.0.37-mysql"));
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Collection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("collections", tb => tb.HasComment("收藏表"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.TargetId).HasComment("目标ID");
            entity.Property(e => e.TargetType).HasComment("收藏类型");
            entity.Property(e => e.UserId).HasComment("收藏用户ID");

            entity.HasOne(d => d.User).WithMany(p => p.Collections).HasConstraintName("collections_ibfk_1");
        });

        modelBuilder.Entity<Companion>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("companions", tb => tb.HasComment("陪玩师表"));

            entity.Property(e => e.Bio).HasComment("个人简介");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.GoodReviewRate)
                .HasDefaultValueSql("'0.00'")
                .HasComment("好评率");
            entity.Property(e => e.IdCard).HasComment("身份证号");
            entity.Property(e => e.IdCardBack).HasComment("身份证反面照片");
            entity.Property(e => e.IdCardFront).HasComment("身份证正面照片");
            entity.Property(e => e.Level)
                .HasDefaultValueSql("'1'")
                .HasComment("等级");
            entity.Property(e => e.Nickname).HasComment("陪玩师昵称");
            entity.Property(e => e.OnlineStatus)
                .HasDefaultValueSql("'offline'")
                .HasComment("在线状态");
            entity.Property(e => e.Phone).HasComment("联系电话");
            entity.Property(e => e.Rating)
                .HasDefaultValueSql("'0.00'")
                .HasComment("评分(0.00-5.00)");
            entity.Property(e => e.RealName).HasComment("真实姓名");
            entity.Property(e => e.RejectReason).HasComment("拒绝原因");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("0=待审核,1=审核通过,2=审核拒绝");
            entity.Property(e => e.Tags).HasComment("标签(多个用逗号分隔)");
            entity.Property(e => e.TotalOrders)
                .HasDefaultValueSql("'0'")
                .HasComment("总订单数");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UserId).HasComment("用户ID");

            entity.HasOne(d => d.User).WithMany(p => p.Companions).HasConstraintName("companions_ibfk_1");
        });

        modelBuilder.Entity<CompanionApplication>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("companion_applications", tb => tb.HasComment("陪玩师申请表"));

            entity.Property(e => e.Id).HasComment("主键ID");
            entity.Property(e => e.AdminNotes).HasComment("管理员备注");
            entity.Property(e => e.AvailableTime).HasComment("可接单时间");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("创建时间");
            entity.Property(e => e.GameCategory).HasComment("游戏类型");
            entity.Property(e => e.HourlyRate).HasComment("时薪");
            entity.Property(e => e.SelfIntroduction).HasComment("自我介绍");
            entity.Property(e => e.SkillLevel).HasComment("技能等级");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'待审核'")
                .HasComment("状态：待审核/已通过/已拒绝");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("更新时间");
            entity.Property(e => e.UserId).HasComment("用户ID");
        });

        modelBuilder.Entity<CompanionBackgroundImage>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("companion_background_images", tb => tb.HasComment("陪玩师背景墙轮播图关联表"));

            entity.Property(e => e.Id).HasComment("主键UUID");
            entity.Property(e => e.CompanionId).HasComment("陪玩师ID（关联companions表id）");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("创建时间");
            entity.Property(e => e.FileId).HasComment("文件ID（关联sys_file表id）");
            entity.Property(e => e.IsDeleted).HasComment("是否删除 0=否 1=是");
            entity.Property(e => e.Sort).HasComment("排序权重（越小越靠前，轮播顺序）");
            entity.Property(e => e.UpdateTime)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("更新时间");

            entity.HasOne(d => d.Companion).WithMany(p => p.CompanionBackgroundImages).HasConstraintName("fk_companion_background_companion");

            entity.HasOne(d => d.File).WithMany(p => p.CompanionBackgroundImages).HasConstraintName("fk_companion_background_file");
        });

        modelBuilder.Entity<CompanionGame>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("companion_games", tb => tb.HasComment("陪玩师游戏技能表"));

            entity.Property(e => e.CompanionId).HasComment("陪玩师ID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.GameId).HasComment("游戏ID");
            entity.Property(e => e.GameLevel).HasComment("游戏段位/等级");
            entity.Property(e => e.PricePerGame).HasComment("单价(元/局)");
            entity.Property(e => e.PricePerHour).HasComment("单价(元/小时)");
            entity.Property(e => e.ServiceType)
                .IsFixedLength()
                .HasComment("服务类型");

            entity.HasOne(d => d.Companion).WithMany(p => p.CompanionGames).HasConstraintName("companion_games_ibfk_1");
        });

        modelBuilder.Entity<CompanionRequest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("companion_requests", tb => tb.HasComment("陪玩需求表"));

            entity.Property(e => e.Budget).HasComment("预算价格");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Description).HasComment("需求描述");
            entity.Property(e => e.DurationType)
                .HasDefaultValueSql("'game'")
                .HasComment("时长类型:局/小时");
            entity.Property(e => e.DurationValue).HasComment("时长数量");
            entity.Property(e => e.GameId).HasComment("游戏ID");
            entity.Property(e => e.GameLevel).HasComment("游戏段位/等级");
            entity.Property(e => e.MatchedCompanionId).HasComment("匹配的陪玩师ID");
            entity.Property(e => e.PlayTime).HasComment("期望陪玩时间");
            entity.Property(e => e.Requirements).HasComment("陪玩要求(多个标签用逗号分隔)");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'pending'")
                .HasComment("状态");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UserId).HasComment("发布用户ID");

            entity.HasOne(d => d.User).WithMany(p => p.CompanionRequests).HasConstraintName("companion_requests_ibfk_1");
        });

        modelBuilder.Entity<Conversation>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("conversations", tb => tb.HasComment("会话表"));

            entity.Property(e => e.CompanionId).HasComment("陪玩师ID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.LastMessage).HasComment("最后一条消息");
            entity.Property(e => e.LastMessageTime).HasComment("最后消息时间");
            entity.Property(e => e.UnreadCount)
                .HasDefaultValueSql("'0'")
                .HasComment("未读数");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UserId).HasComment("用户ID");

            entity.HasOne(d => d.Companion).WithMany(p => p.Conversations).HasConstraintName("conversations_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Conversations).HasConstraintName("conversations_ibfk_1");
        });

        modelBuilder.Entity<Coupon>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("coupons", tb => tb.HasComment("优惠券表"));

            entity.Property(e => e.Amount).HasComment("金额");
            entity.Property(e => e.Code).HasComment("优惠券码");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Discount).HasComment("折扣");
            entity.Property(e => e.ExpireDate).HasComment("过期日期");
            entity.Property(e => e.MaxDiscount).HasComment("最大优惠金额");
            entity.Property(e => e.MinAmount).HasComment("最小使用金额");
            entity.Property(e => e.OrderId).HasComment("使用的订单ID");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'unused'")
                .HasComment("状态");
            entity.Property(e => e.Type).HasComment("类型");
            entity.Property(e => e.UsedTime).HasComment("使用时间");
            entity.Property(e => e.UserId).HasComment("用户ID");

            entity.HasOne(d => d.User).WithMany(p => p.Coupons).HasConstraintName("coupons_ibfk_1");
        });

        modelBuilder.Entity<Draft>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("drafts", tb => tb.HasComment("草稿表"));

            entity.Property(e => e.Content).HasComment("内容");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.DraftData).HasComment("草稿数据(JSON格式)");
            entity.Property(e => e.Images).HasComment("图片URL");
            entity.Property(e => e.Title).HasComment("标题");
            entity.Property(e => e.Type).HasComment("草稿类型");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UserId).HasComment("用户ID");

            entity.HasOne(d => d.User).WithMany(p => p.Drafts).HasConstraintName("drafts_ibfk_1");
        });

        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("feedbacks", tb => tb.HasComment("意见反馈表"));

            entity.Property(e => e.Contact).HasComment("联系方式");
            entity.Property(e => e.Content).HasComment("反馈内容");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Images).HasComment("截图URL");
            entity.Property(e => e.Reply).HasComment("回复内容");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'pending'")
                .HasComment("处理状态");
            entity.Property(e => e.Type).HasComment("反馈类型");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UserId).HasComment("用户ID");

            entity.HasOne(d => d.User).WithMany(p => p.Feedbacks).HasConstraintName("feedbacks_ibfk_1");
        });

        modelBuilder.Entity<Follow>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("follows", tb => tb.HasComment("关注表"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.FollowerId).HasComment("关注者ID");
            entity.Property(e => e.FollowingId).HasComment("被关注者ID");

            entity.HasOne(d => d.Follower).WithMany(p => p.FollowFollowers).HasConstraintName("follows_ibfk_1");

            entity.HasOne(d => d.Following).WithMany(p => p.FollowFollowings).HasConstraintName("follows_ibfk_2");
        });

        modelBuilder.Entity<Game>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("games", tb => tb.HasComment("游戏表"));

            entity.Property(e => e.CoverImage).HasComment("封面图片");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Description).HasComment("游戏描述");
            entity.Property(e => e.Icon).HasComment("游戏图标");
            entity.Property(e => e.IsActive)
                .HasDefaultValueSql("'1'")
                .HasComment("是否启用");
            entity.Property(e => e.Name).HasComment("游戏名称");
            entity.Property(e => e.NameEn).HasComment("英文名");
            entity.Property(e => e.SortOrder)
                .HasDefaultValueSql("'0'")
                .HasComment("排序");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'1'")
                .IsFixedLength()
                .HasComment("状态");
            entity.Property(e => e.Type).HasComment("游戏类型");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<GameCircle>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("game_circles", tb => tb.HasComment("游戏圈子表"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Description).HasComment("圈子描述");
            entity.Property(e => e.GameId).HasComment("游戏ID");
            entity.Property(e => e.Icon).HasComment("圈子图标");
            entity.Property(e => e.MemberCount)
                .HasDefaultValueSql("'0'")
                .HasComment("成员数");
            entity.Property(e => e.Name).HasComment("圈子名称");
            entity.Property(e => e.PostCount)
                .HasDefaultValueSql("'0'")
                .HasComment("动态数");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'active'")
                .HasComment("状态");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(d => d.Game).WithMany(p => p.GameCircles).HasConstraintName("game_circles_ibfk_1");
        });

        modelBuilder.Entity<Message>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("messages", tb => tb.HasComment("消息表"));

            entity.Property(e => e.Content).HasComment("消息内容");
            entity.Property(e => e.ConversationId).HasComment("会话ID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsRead)
                .HasDefaultValueSql("'0'")
                .HasComment("是否已读");
            entity.Property(e => e.MessageType)
                .HasDefaultValueSql("'text'")
                .HasComment("消息类型");
            entity.Property(e => e.ReceiverId).HasComment("接收者ID");
            entity.Property(e => e.SenderId).HasComment("发送者ID");

            entity.HasOne(d => d.Conversation).WithMany(p => p.Messages).HasConstraintName("messages_ibfk_1");

            entity.HasOne(d => d.Receiver).WithMany(p => p.MessageReceivers).HasConstraintName("messages_ibfk_3");

            entity.HasOne(d => d.Sender).WithMany(p => p.MessageSenders).HasConstraintName("messages_ibfk_2");
        });

        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("notifications", tb => tb.HasComment("系统通知表"));

            entity.Property(e => e.ActionUrl).HasComment("跳转链接");
            entity.Property(e => e.Content).HasComment("通知内容");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.IsRead)
                .HasDefaultValueSql("'0'")
                .HasComment("是否已读");
            entity.Property(e => e.Tag).HasComment("标签");
            entity.Property(e => e.Title).HasComment("通知标题");
            entity.Property(e => e.Type).HasComment("通知类型");
            entity.Property(e => e.UserId).HasComment("用户ID");

            entity.HasOne(d => d.User).WithMany(p => p.Notifications).HasConstraintName("notifications_ibfk_1");
        });

        modelBuilder.Entity<Order>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("orders", tb => tb.HasComment("订单表"));

            entity.Property(e => e.CompanionId).HasComment("陪玩师ID");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.DiscountAmount)
                .HasDefaultValueSql("'0.00'")
                .HasComment("优惠金额");
            entity.Property(e => e.DurationType)
                .HasDefaultValueSql("'game'")
                .HasComment("时长类型");
            entity.Property(e => e.DurationValue).HasComment("时长数量");
            entity.Property(e => e.EndTime).HasComment("服务结束时间");
            entity.Property(e => e.FinalPrice).HasComment("实付金额");
            entity.Property(e => e.GameId).HasComment("游戏ID");
            entity.Property(e => e.OrderNo).HasComment("订单号");
            entity.Property(e => e.PayTime).HasComment("支付时间");
            entity.Property(e => e.PlayTime).HasComment("预约时间");
            entity.Property(e => e.Remark).HasComment("备注信息");
            entity.Property(e => e.ServiceType)
                .HasDefaultValueSql("'1'")
                .HasComment("服务类型");
            entity.Property(e => e.StartTime).HasComment("服务开始时间");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("''")
                .HasComment("订单状态 空=全部 0=待付款 1=进行中 2=已完成 4=退款/售后");
            entity.Property(e => e.TotalPrice).HasComment("总价");
            entity.Property(e => e.UnitPrice).HasComment("单价");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UserId).HasComment("下单用户ID");

            entity.HasOne(d => d.Companion).WithMany(p => p.Orders).HasConstraintName("orders_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Orders).HasConstraintName("orders_ibfk_1");
        });

        modelBuilder.Entity<OrderReview>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("order_reviews", tb => tb.HasComment("订单评价表"));

            entity.Property(e => e.CompanionId).HasComment("陪玩师ID");
            entity.Property(e => e.Content).HasComment("评价内容");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.OrderId).HasComment("订单ID");
            entity.Property(e => e.Rating).HasComment("评分1-5");
            entity.Property(e => e.Tags).HasComment("评价标签");
            entity.Property(e => e.UserId).HasComment("评价用户ID");

            entity.HasOne(d => d.Companion).WithMany(p => p.OrderReviews).HasConstraintName("order_reviews_ibfk_3");

            entity.HasOne(d => d.Order).WithOne(p => p.OrderReview).HasConstraintName("order_reviews_ibfk_1");

            entity.HasOne(d => d.User).WithMany(p => p.OrderReviews).HasConstraintName("order_reviews_ibfk_2");
        });

        modelBuilder.Entity<Post>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("posts", tb => tb.HasComment("动态表"));

            entity.Property(e => e.CircleId).HasComment("圈子ID");
            entity.Property(e => e.CollectCount)
                .HasDefaultValueSql("'0'")
                .HasComment("收藏数");
            entity.Property(e => e.CommentCount)
                .HasDefaultValueSql("'0'")
                .HasComment("评论数");
            entity.Property(e => e.Content).HasComment("动态内容");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Images).HasComment("图片URL(多个用逗号分隔)");
            entity.Property(e => e.LikeCount)
                .HasDefaultValueSql("'0'")
                .HasComment("点赞数");
            entity.Property(e => e.Location).HasComment("位置");
            entity.Property(e => e.MentionUsers).HasComment("提醒的用户ID");
            entity.Property(e => e.ShareCount)
                .HasDefaultValueSql("'0'")
                .HasComment("分享数");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'published'")
                .HasComment("状态");
            entity.Property(e => e.Tags).HasComment("话题标签");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UserId).HasComment("发布用户ID");
            entity.Property(e => e.Visibility)
                .HasDefaultValueSql("'public'")
                .HasComment("可见性");

            entity.HasOne(d => d.User).WithMany(p => p.Posts).HasConstraintName("posts_ibfk_1");
        });

        modelBuilder.Entity<PostComment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("post_comments", tb => tb.HasComment("动态评论表"));

            entity.Property(e => e.Content).HasComment("评论内容");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.LikeCount)
                .HasDefaultValueSql("'0'")
                .HasComment("点赞数");
            entity.Property(e => e.ParentId)
                .HasDefaultValueSql("'0'")
                .HasComment("父评论ID(0为一级评论)");
            entity.Property(e => e.PostId).HasComment("动态ID");
            entity.Property(e => e.ReplyToUserId).HasComment("回复给的用户ID");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'normal'")
                .HasComment("状态");
            entity.Property(e => e.UserId).HasComment("评论用户ID");

            entity.HasOne(d => d.Post).WithMany(p => p.PostComments).HasConstraintName("post_comments_ibfk_1");

            entity.HasOne(d => d.User).WithMany(p => p.PostComments).HasConstraintName("post_comments_ibfk_2");
        });

        modelBuilder.Entity<PostLike>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("post_likes", tb => tb.HasComment("动态点赞表"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.PostId).HasComment("动态ID");
            entity.Property(e => e.UserId).HasComment("点赞用户ID");

            entity.HasOne(d => d.Post).WithMany(p => p.PostLikes).HasConstraintName("post_likes_ibfk_1");

            entity.HasOne(d => d.User).WithMany(p => p.PostLikes).HasConstraintName("post_likes_ibfk_2");
        });

        modelBuilder.Entity<PowerLeveling>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("power_leveling", tb => tb.HasComment("代练服务表"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.CurrentRank).HasComment("当前段位");
            entity.Property(e => e.EstimatedDays).HasComment("预计完成天数");
            entity.Property(e => e.GameId).HasComment("游戏ID");
            entity.Property(e => e.Price).HasComment("报价");
            entity.Property(e => e.ServiceType).HasComment("代练项目");
            entity.Property(e => e.SpecialRequirements).HasComment("特殊要求");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'active'")
                .HasComment("状态");
            entity.Property(e => e.TargetRank).HasComment("目标段位");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UserId).HasComment("发布用户ID(陪玩师)");

            entity.HasOne(d => d.User).WithMany(p => p.PowerLevelings).HasConstraintName("power_leveling_ibfk_1");
        });

        modelBuilder.Entity<SysDictDatum>(entity =>
        {
            entity.HasKey(e => e.DictCode).HasName("PRIMARY");

            entity.ToTable("sys_dict_data", tb => tb.HasComment("字典数据表"));

            entity.Property(e => e.DictCode).HasComment("字典编码");
            entity.Property(e => e.CreateBy)
                .HasDefaultValueSql("''")
                .HasComment("创建者");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("创建时间");
            entity.Property(e => e.DictLabel)
                .HasDefaultValueSql("''")
                .HasComment("字典标签");
            entity.Property(e => e.DictSort)
                .HasDefaultValueSql("'0'")
                .HasComment("字典排序");
            entity.Property(e => e.DictType)
                .HasDefaultValueSql("''")
                .HasComment("字典类型");
            entity.Property(e => e.DictValue)
                .HasDefaultValueSql("''")
                .HasComment("字典键值");
            entity.Property(e => e.Remark).HasComment("备注");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("状态（0正常 1停用）");
            entity.Property(e => e.UpdateBy)
                .HasDefaultValueSql("''")
                .HasComment("更新者");
            entity.Property(e => e.UpdateTime)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("更新时间");
        });

        modelBuilder.Entity<SysDictType>(entity =>
        {
            entity.HasKey(e => e.DictId).HasName("PRIMARY");

            entity.ToTable("sys_dict_type", tb => tb.HasComment("字典类型表"));

            entity.Property(e => e.DictId).HasComment("字典主键");
            entity.Property(e => e.CreateBy)
                .HasDefaultValueSql("''")
                .HasComment("创建者");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("创建时间");
            entity.Property(e => e.DictName)
                .HasDefaultValueSql("''")
                .HasComment("字典名称");
            entity.Property(e => e.DictType)
                .HasDefaultValueSql("''")
                .HasComment("字典类型（唯一）");
            entity.Property(e => e.Remark).HasComment("备注");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("状态（0正常 1停用）");
            entity.Property(e => e.UpdateBy)
                .HasDefaultValueSql("''")
                .HasComment("更新者");
            entity.Property(e => e.UpdateTime)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("更新时间");
        });

        modelBuilder.Entity<SysFile>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("sys_file", tb => tb.HasComment("文件上传记录表"));

            entity.Property(e => e.Id).HasComment("主键UUID");
            entity.Property(e => e.ContentType)
                .HasDefaultValueSql("''")
                .HasComment("文件类型");
            entity.Property(e => e.CreateTime)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("创建时间");
            entity.Property(e => e.FileExt)
                .HasDefaultValueSql("''")
                .HasComment("文件后缀");
            entity.Property(e => e.FileName).HasComment("原始文件名");
            entity.Property(e => e.FilePath).HasComment("文件物理路径");
            entity.Property(e => e.FileSize).HasComment("文件大小（字节）");
            entity.Property(e => e.FileUrl).HasComment("文件访问URL");
            entity.Property(e => e.IsDeleted).HasComment("是否删除 0=否 1=是");
            entity.Property(e => e.UpdateTime)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("更新时间");
            entity.Property(e => e.UploadPlatform)
                .HasDefaultValueSql("'web'")
                .HasComment("上传平台");
            entity.Property(e => e.UploadUser).HasComment("上传人ID");
        });

        modelBuilder.Entity<SystemConfig>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("system_config", tb => tb.HasComment("系统全局配置表"));

            entity.Property(e => e.Id).HasComment("主键");
            entity.Property(e => e.ConfigKey).HasComment("配置键（唯一）");
            entity.Property(e => e.ConfigType)
                .HasDefaultValueSql("'string'")
                .HasComment("类型：string/json/banner/number");
            entity.Property(e => e.ConfigValue).HasComment("配置值");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Name)
                .HasDefaultValueSql("''")
                .HasComment("配置名称");
            entity.Property(e => e.Remark)
                .HasDefaultValueSql("''")
                .HasComment("备注");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("transactions", tb => tb.HasComment("交易记录表"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("users", tb => tb.HasComment("用户表"));

            entity.Property(e => e.Age).HasComment("年龄");
            entity.Property(e => e.Avatar).HasComment("头像URL");
            entity.Property(e => e.Balance)
                .HasDefaultValueSql("'0.00'")
                .HasComment("余额");
            entity.Property(e => e.Bio).HasComment("个人简介");
            entity.Property(e => e.Birthday).HasComment("生日");
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Gender)
                .HasDefaultValueSql("'0'")
                .HasComment("性别 0=未知 1=男 2=女");
            entity.Property(e => e.IdCard).HasComment("身份证号");
            entity.Property(e => e.IsAdmin)
                .HasDefaultValueSql("'0'")
                .HasComment("0=否,1=是");
            entity.Property(e => e.IsBlocked)
                .HasDefaultValueSql("'0'")
                .HasComment("0=正常,1=封禁");
            entity.Property(e => e.LastLoginTime).HasComment("最后登录时间");
            entity.Property(e => e.Name).HasComment("姓名");
            entity.Property(e => e.Nickname).HasComment("昵称");
            entity.Property(e => e.Openid)
                .HasDefaultValueSql("''")
                .HasComment("微信openid");
            entity.Property(e => e.Password).HasComment("密码");
            entity.Property(e => e.Phone).HasComment("手机号");
            entity.Property(e => e.Points)
                .HasDefaultValueSql("'0'")
                .HasComment("积分");
            entity.Property(e => e.RealName).HasComment("真实姓名");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'0'")
                .HasComment("状态:1=正常,0=禁用");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UserId).HasComment("用户ID(如10086888)");
            entity.Property(e => e.Username).HasComment("用户名");
            entity.Property(e => e.VipExpireDate).HasComment("VIP过期时间");
            entity.Property(e => e.VipLevel)
                .HasDefaultValueSql("'0'")
                .HasComment("VIP等级 0=普通 1=普通会员 2=VIP会员 3=SVIP会员");
        });

        modelBuilder.Entity<UserCollection>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user_collections", tb => tb.HasComment("用户收藏表"));

            entity.Property(e => e.Id).HasComment("主键ID");
            entity.Property(e => e.Category).HasComment("收藏分类");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasComment("创建时间");
            entity.Property(e => e.Description).HasComment("收藏描述");
            entity.Property(e => e.ItemId).HasComment("关联项目ID");
            entity.Property(e => e.ItemType).HasComment("关联项目类型(companion/post等)");
            entity.Property(e => e.Title).HasComment("收藏标题");
            entity.Property(e => e.UserId).HasComment("用户ID");

            entity.HasOne(d => d.User).WithMany(p => p.UserCollections).HasConstraintName("user_collections_ibfk_1");
        });

        modelBuilder.Entity<UserSetting>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("user_settings", tb => tb.HasComment("用户设置表"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.SettingKey).HasComment("设置键");
            entity.Property(e => e.SettingValue).HasComment("设置值");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UserId).HasComment("用户ID");

            entity.HasOne(d => d.User).WithMany(p => p.UserSettings).HasConstraintName("user_settings_ibfk_1");
        });

        modelBuilder.Entity<VipMembership>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PRIMARY");

            entity.ToTable("vip_memberships", tb => tb.HasComment("VIP会员表"));

            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.EndDate).HasComment("结束日期");
            entity.Property(e => e.Level).HasComment("会员等级");
            entity.Property(e => e.PurchaseAmount).HasComment("购买金额");
            entity.Property(e => e.StartDate).HasComment("开始日期");
            entity.Property(e => e.Status)
                .HasDefaultValueSql("'active'")
                .HasComment("状态");
            entity.Property(e => e.UpdatedAt)
                .ValueGeneratedOnAddOrUpdate()
                .HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.UserId).HasComment("用户ID");

            entity.HasOne(d => d.User).WithMany(p => p.VipMemberships).HasConstraintName("vip_memberships_ibfk_1");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
