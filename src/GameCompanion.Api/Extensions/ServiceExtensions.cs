using GameCompanion.Api.Configuration;
using GameCompanion.Api.Services;
using GameCompanion.Api.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace GameCompanion.Api.Extensions;

/// <summary>
/// 服务注册扩展
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// 注册自定义服务
    /// </summary>
    public static IServiceCollection RegisterServices(this IServiceCollection services, IConfiguration configuration)
    {
        // 注册数据库上下文
        services.AddDbContext<ApplicationDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString),
                mysqlOptions =>
                {
                    mysqlOptions.EnableRetryOnFailure(
                        maxRetryCount: 3,
                        maxRetryDelay: TimeSpan.FromSeconds(5),
                        errorNumbersToAdd: null);
                });
        });

        // 注册JWT服务
        services.AddScoped<IJwtService, JwtService>();

        // 注册业务服务
        services.AddScoped<IAuthService, AuthService>();
        services.AddScoped<IDashboardService, DashboardService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<ICompanionService, CompanionService>();
        services.AddScoped<IOrderService, OrderService>();
        services.AddScoped<IPostService, PostService>();
        services.AddScoped<IMessageService, MessageService>();
        services.AddScoped<ISettingService, SettingService>();

        return services;
    }

    /// <summary>
    /// 注册AutoMapper
    /// </summary>
    public static IServiceCollection RegisterAutoMapper(this IServiceCollection services)
    {
        // AutoMapper配置将在后续添加
        return services;
    }
}
