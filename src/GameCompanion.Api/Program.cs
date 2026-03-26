using GameCompanion.Api.Extensions;
using GameCompanion.Api.Middleware;
using Serilog;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;

// 打印启动信息
Console.WriteLine("========================================");
Console.WriteLine("陪玩平台后台管理 API 启动中...");
Console.WriteLine("========================================");
Console.WriteLine($"启动时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
// ✅ 修复：条件表达式用括号包裹
Console.WriteLine($"环境: {(args.Length > 0 ? string.Join(", ", args) : "默认")}");
Console.WriteLine();

var builder = WebApplication.CreateBuilder(args);

// 配置Serilog
builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

Console.WriteLine("✓ Serilog 日志系统配置完成");

// 添加服务
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
Console.WriteLine("✓ 控制器和API端点配置完成");

// 配置Swagger
builder.Services.AddSwaggerGen(options =>
{
    Console.WriteLine("开始配置Swagger...");
    options.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "陪玩平台后台管理 API",
        Version = "v1.1.0",
        Description = "陪玩平台后台管理系统API接口文档",
        Contact = new OpenApiContact
        {
            Name = "技术支持",
            Email = "tech@example.com"
        }
    });

    // 配置JWT认证
    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "JWT授权，请输入 Bearer {token}",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" }
            },
            Array.Empty<string>()
        }
    });
});
Console.WriteLine("✓ Swagger配置完成");

// 配置JWT认证
Console.WriteLine("开始配置JWT认证...");
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = builder.Configuration["JwtSettings:Issuer"],
            ValidAudience = builder.Configuration["JwtSettings:Audience"],
            IssuerSigningKey = new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(
                System.Text.Encoding.UTF8.GetBytes(builder.Configuration["JwtSettings:SecretKey"] ?? "YourDefaultSecretKey!"))
        };
    });

builder.Services.AddAuthorization();
Console.WriteLine("✓ JWT认证配置完成");

// 注册自定义服务
Console.WriteLine("开始注册自定义服务...");
builder.Services.RegisterServices(builder.Configuration);
builder.Services.RegisterAutoMapper();
Console.WriteLine("✓ 自定义服务注册完成");

// 配置CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
Console.WriteLine("✓ CORS配置完成");

Console.WriteLine();
Console.WriteLine("开始构建应用程序...");
var app = builder.Build();
Console.WriteLine("✓ 应用程序构建完成");
Console.WriteLine();

// 配置请求管道
Console.WriteLine("配置请求管道...");
if (app.Environment.IsDevelopment() || true)
{
    Console.WriteLine("  - 环境: 开发模式");
    Console.WriteLine("  - 启用Swagger文档");
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "陪玩平台后台管理 API v1.1.0");
        options.RoutePrefix = "swagger";
    });
}
else
{
    Console.WriteLine("  - 环境: 生产模式");
}

try
{
    Console.WriteLine("  - 注册中间件...");
    app.UseMiddleware<ExceptionHandlingMiddleware>();
    app.UseMiddleware<RequestLoggingMiddleware>();

    // app.UseHttpsRedirection(); // 注释掉
    app.UseCors("AllowAll");
    app.UseAuthentication();
    app.UseAuthorization();

    Console.WriteLine("  - 映射控制器...");
    app.MapControllers();

    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine("🎉 应用程序启动成功！");
    Console.WriteLine("========================================");
    Console.WriteLine($"监听地址: http://localhost:5000");
    Console.WriteLine($"Swagger文档: http://localhost:5000/swagger");
    Console.WriteLine($"API端点: http://localhost:5000/api/v1");
    Console.WriteLine();
    Console.WriteLine("按 Ctrl+C 停止服务器");
    Console.WriteLine("========================================");
    Console.WriteLine();

    app.Run("http://localhost:5000"); // 强制指定端口
}
catch (Exception ex)
{
    Console.WriteLine();
    Console.WriteLine("========================================");
    Console.WriteLine("❌ 启动失败！");
    Console.WriteLine("========================================");
    Console.WriteLine($"错误类型: {ex.GetType().Name}");
    Console.WriteLine($"错误消息: {ex.Message}");
    Console.WriteLine();
    Console.WriteLine("堆栈跟踪:");
    Console.WriteLine(ex.StackTrace);
    Console.WriteLine("========================================");
    Console.WriteLine();
    throw;
}