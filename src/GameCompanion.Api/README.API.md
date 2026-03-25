# 陪玩平台后台管理 API

这是陪玩平台后台管理系统的 .NET 8 Web API 项目。

## 项目结构

```
src/GameCompanion.Api/
├── Controllers/          # API控制器
│   ├── AuthController.cs          # 认证授权
│   └── AllControllers.cs          # 其他所有控制器
├── Models/               # 数据模型
│   ├── Admin.cs
│   ├── User.cs
│   ├── Companion.cs
│   ├── Order.cs
│   ├── Post.cs
│   └── ...
├── DTOs/                 # 数据传输对象
│   └── ApiResponse.cs    # 统一响应格式
├── Services/             # 业务服务
│   ├── ServiceInterfaces.cs       # 服务接口
│   └── Implementations/           # 服务实现
├── Configuration/        # 配置
│   └── ApplicationDbContext.cs    # 数据库上下文
├── Middleware/           # 中间件
│   ├── ExceptionHandlingMiddleware.cs  # 异常处理
│   └── RequestLoggingMiddleware.cs     # 请求日志
├── Extensions/           # 扩展
│   └── ServiceExtensions.cs
├── Helpers/              # 辅助类
│   └── JwtService.cs     # JWT服务
├── Program.cs            # 主程序入口
├── appsettings.json      # 配置文件
└── GameCompanion.Api.csproj
```

## 技术栈

- .NET 8.0
- ASP.NET Core Web API
- Entity Framework Core 8
- MySQL 8.0 (Pomelo.EntityFrameworkCore.MySql)
- JWT Authentication
- Serilog (日志)
- AutoMapper (对象映射)

## API 模块

### 1. 认证授权接口 (Auth)
- POST /api/v1/auth/login - 管理员登录
- POST /api/v1/auth/logout - 管理员登出
- POST /api/v1/auth/refresh - 刷新Token
- GET /api/v1/auth/me - 获取当前管理员信息

### 2. 数据概览接口 (Dashboard)
- GET /api/v1/dashboard/stats - 获取统计数据
- GET /api/v1/dashboard/revenue-trend - 获取收入趋势
- GET /api/v1/dashboard/order-distribution - 获取订单分布
- GET /api/v1/dashboard/latest-orders - 获取最新订单
- GET /api/v1/dashboard/quick-actions - 获取快捷操作数据

### 3. 用户管理接口 (Users)
- GET /api/v1/users - 获取用户列表
- GET /api/v1/users/{userId} - 获取用户详情
- PUT /api/v1/users/{userId}/status - 封禁/解封用户
- PUT /api/v1/users/{userId} - 编辑用户信息
- GET /api/v1/users/{userId}/orders - 获取用户订单列表
- GET /api/v1/users/stats - 获取用户统计
- POST /api/v1/users/export - 导出用户数据
- POST /api/v1/users/{userId}/reset-password - 重置用户密码

### 4. 陪玩管理接口 (Companions)
- GET /api/v1/companions/applications - 获取认证申请列表
- GET /api/v1/companions/applications/{applicationId} - 获取认证申请详情
- POST /api/v1/companions/applications/{applicationId}/audit - 审核认证申请
- GET /api/v1/companions - 获取陪玩师列表
- PUT /api/v1/companions/{companionId} - 编辑陪玩师信息
- GET /api/v1/companions/stats - 获取陪玩统计

### 5. 订单管理接口 (Orders)
- GET /api/v1/orders - 获取订单列表
- GET /api/v1/orders/{orderId} - 获取订单详情
- PUT /api/v1/orders/{orderId} - 编辑订单
- POST /api/v1/orders/{orderId}/refund - 处理退款申请
- POST /api/v1/orders/{orderId}/cancel - 取消订单
- GET /api/v1/orders/stats - 获取订单统计
- POST /api/v1/orders/export - 导出订单数据

### 6. 内容管理接口 (Posts)
- GET /api/v1/posts - 获取动态列表
- GET /api/v1/posts/{postId} - 获取动态详情
- POST /api/v1/posts/{postId}/audit - 审核动态
- PUT /api/v1/posts/{postId}/visibility - 隐藏/恢复动态
- DELETE /api/v1/posts/{postId} - 删除动态

### 7. 消息管理接口 (Messages)
- POST /api/v1/messages/send - 创建消息推送
- GET /api/v1/messages/records - 获取推送记录列表
- GET /api/v1/messages/stats - 获取推送统计
- POST /api/v1/messages/{pushId}/cancel - 取消推送

### 8. 系统配置接口 (Settings)
- GET /api/v1/settings - 获取系统配置
- PUT /api/v1/settings - 更新系统配置
- GET /api/v1/settings/logs - 获取操作日志列表
- POST /api/v1/settings/reset - 重置系统配置
- GET /api/v1/settings/permissions - 获取权限列表
- GET /api/v1/settings/admins - 获取管理员列表
- POST /api/v1/settings - 创建管理员
- PUT /api/v1/settings/admins/{adminId} - 编辑管理员

## 配置说明

### 数据库连接字符串
在 `appsettings.json` 中配置:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=game_companion;Uid=root;Pwd=your_password;Charset=utf8mb4;"
  }
}
```

### JWT配置
```json
{
  "JwtSettings": {
    "SecretKey": "YourVeryLongSecretKeyForJWTTokenGenerationMustBeAtLeast32Characters!",
    "Issuer": "GameCompanion.Api",
    "Audience": "GameCompanion.Admin",
    "AccessTokenExpirationMinutes": 120,
    "RefreshTokenExpirationDays": 7
  }
}
```

## 运行项目

### 前置要求
- .NET 8 SDK
- MySQL 8.0
- (可选) Redis

### 步骤

1. 恢复NuGet包
```bash
dotnet restore
```

2. 更新数据库（需要先安装dotnet-ef）
```bash
dotnet ef database update
```

3. 运行项目
```bash
dotnet run
```

4. 访问Swagger文档
```
https://localhost:5001/swagger
```

## API响应格式

### 成功响应
```json
{
  "code": 200,
  "message": "success",
  "data": {},
  "timestamp": 1620000000000
}
```

### 错误响应
```json
{
  "code": 400,
  "message": "参数错误",
  "error": "具体错误信息",
  "timestamp": 1620000000000
}
```

### 分页响应
```json
{
  "code": 200,
  "message": "success",
  "data": {
    "items": [],
    "pagination": {
      "page": 1,
      "page_size": 20,
      "total": 100,
      "total_pages": 5
    }
  },
  "timestamp": 1620000000000
}
```

## 注意事项

1. 本项目实现了完整的API框架，包括：
   - 统一的响应格式
   - 全局异常处理
   - JWT认证授权
   - 请求日志记录
   - 完整的业务接口

2. 部分服务实现为简化版本，实际使用时需要：
   - 完善业务逻辑
   - 添加数据验证
   - 实现密码哈希（当前是简化实现）
   - 完善权限验证
   - 添加单元测试

3. 数据库迁移文件需要通过EF Core生成

## 开发建议

- 使用Swagger进行API测试
- 添加更多数据验证
- 实现更细粒度的权限控制
- 添加API限流功能
- 完善日志记录
- 添加性能监控
- 实现缓存机制

## 联系方式

- 技术支持邮箱：tech@example.com
