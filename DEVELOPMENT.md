# 陪玩平台 API 开发文档

## 项目概述

本项目是基于 .NET 8 Web API 的陪玩平台后端服务，为微信小程序和PC管理端提供统一的接口服务。

## 技术栈

- **框架**: ASP.NET Core 8 Web API
- **ORM**: Entity Framework Core 8
- **数据库**: MySQL 8.0
- **缓存**: Redis
- **认证**: JWT Bearer Token
- **日志**: Serilog
- **API文档**: Swagger/OpenAPI
- **对象映射**: AutoMapper

## 项目结构

```
GameCompanion.Api/
├── Controllers/          # API控制器
│   ├── AuthController.cs         # 用户认证接口（已完成）
│   ├── HomeController.cs         # 首页服务接口（待实现）
│   ├── OrdersController.cs       # 订单管理接口（待实现）
│   ├── PostsController.cs        # 动态社区接口（待实现）
│   ├── ConversationsController.cs # 消息聊天接口（待实现）
│   ├── NotificationsController.cs # 系统通知接口（待实现）
│   ├── UserController.cs         # 个人中心接口（待实现）
│   ├── SettingsController.cs     # 设置相关接口（待实现）
│   ├── FeedbackController.cs     # 意见反馈接口（待实现）
│   ├── CompanionController.cs    # 陪玩师认证接口（待实现）
│   └── PowerLevelingController.cs # 代练服务接口（待实现）
├── Data/                 # 数据访问层
│   └── ApplicationDbContext.cs    # 数据库上下文
├── DTOs/                 # 数据传输对象
│   ├── Auth/             # 认证相关DTO（已完成）
│   ├── Home/             # 首页相关DTO（待创建）
│   ├── Order/            # 订单相关DTO（待创建）
│   ├── Post/             # 动态相关DTO（待创建）
│   ├── Message/          # 消息相关DTO（待创建）
│   ├── User/             # 用户相关DTO（待创建）
│   └── Companion/        # 陪玩师相关DTO（待创建）
├── Models/               # 数据模型
│   ├── Entities/         # 实体模型
│   │   ├── User.cs       # 用户实体（已完成）
│   │   ├── Companion.cs  # 陪玩师实体（已完成）
│   │   └── Order.cs      # 订单实体（已完成）
│   ├── ApiResponse.cs    # 统一响应格式（已完成）
│   └── PaginatedList.cs  # 分页列表（已完成）
├── Services/             # 业务逻辑层
│   ├── IAuthService.cs   # 认证服务接口（已完成）
│   ├── AuthService.cs    # 认证服务实现（已完成）
│   └── 其他服务接口和实现（待完成）
├── Middleware/           # 中间件
│   ├── ExceptionMiddleware.cs        # 全局异常处理（已完成）
│   └── RequestLoggingMiddleware.cs   # 请求日志（已完成）
├── Helpers/              # 辅助类（待创建）
├── Program.cs            # 程序入口（已完成）
├── appsettings.json      # 配置文件（已完成）
└── appsettings.Development.json # 开发环境配置（已完成）
```

## 接口文档路由

所有接口都使用 `api/` 前缀，具体路由如下：

### 3. 用户认证接口
- POST `/api/auth/login` - 用户登录
- POST `/api/auth/register` - 用户注册
- POST `/api/auth/send-code` - 发送验证码
- POST `/api/auth/reset-password` - 重置密码
- POST `/api/auth/refresh` - 刷新Token
- POST `/api/auth/logout` - 退出登录

### 4. 首页服务接口
- GET `/api/home` - 获取首页数据
- GET `/api/companions` - 获取陪玩师列表
- GET `/api/companions/{id}` - 获取陪玩师详情
- GET `/api/games` - 获取游戏列表
- GET `/api/search/companions` - 搜索陪玩师
- GET `/api/circles` - 获取游戏圈子列表

### 5. 订单管理接口
- POST `/api/orders` - 创建订单
- GET `/api/orders` - 获取订单列表
- GET `/api/orders/{id}` - 获取订单详情
- POST `/api/orders/{id}/cancel` - 取消订单
- POST `/api/orders/{id}/refund` - 申请退款
- POST `/api/orders/{id}/confirm` - 确认完成
- POST `/api/orders/{id}/review` - 订单评价

### 6. 动态社区接口
- POST `/api/posts` - 发布动态
- GET `/api/posts` - 获取动态列表
- GET `/api/posts/{id}` - 获取动态详情
- POST `/api/posts/{id}/like` - 点赞动态
- POST `/api/posts/{id}/collect` - 收藏动态
- POST `/api/posts/{id}/comments` - 评论动态
- GET `/api/posts/my` - 获取我的发布
- DELETE `/api/posts/{id}` - 删除动态
- POST `/api/posts/draft` - 保存草稿
- GET `/api/posts/drafts` - 获取草稿列表

### 7. 消息聊天接口
- GET `/api/conversations` - 获取会话列表
- GET `/api/conversations/{id}/messages` - 获取聊天详情
- POST `/api/conversations/{id}/messages` - 发送消息
- POST `/api/conversations/upload-image` - 上传聊天图片
- GET `/api/notifications` - 获取官方通知
- POST `/api/notifications/{id}/read` - 标记通知已读

### 8. 个人中心接口
- GET `/api/user/profile` - 获取个人信息
- PUT `/api/user/profile` - 更新个人资料
- POST `/api/user/upload-avatar` - 上传头像
- POST `/api/user/verify-real-name` - 实名认证
- GET `/api/user/collections` - 获取我的收藏
- GET `/api/user/following` - 获取关注列表
- GET `/api/user/followers` - 获取粉丝列表
- POST `/api/user/follow` - 关注/取消关注用户
- POST `/api/user/apply-companion` - 申请成为陪玩师
- GET `/api/user/wallet` - 获取钱包信息

### 9. 设置相关接口
- GET `/api/settings/account` - 获取账号设置
- POST `/api/settings/change-phone` - 修改手机号
- POST `/api/settings/change-password` - 修改密码
- PUT `/api/settings/privacy` - 更新隐私设置
- PUT `/api/settings/notification` - 更新通知设置
- POST `/api/feedback` - 意见反馈

### 10. 陪玩师认证接口
- POST `/api/companion/apply` - 申请成为陪玩师
- GET `/api/companion/application-status` - 获取认证申请状态
- GET `/api/companion/my-info` - 获取我的陪玩师信息
- PUT `/api/companion/my-info` - 更新陪玩师信息
- PUT `/api/companion/online-status` - 切换在线状态
- GET `/api/companion/orders` - 获取接单列表
- POST `/api/companion/orders/{id}/accept` - 接受订单
- POST `/api/companion/orders/{id}/reject` - 拒绝订单
- POST `/api/companion/orders/{id}/start` - 开始服务
- POST `/api/companion/orders/{id}/complete` - 完成服务
- GET `/api/companion/earnings` - 获取收益统计
- POST `/api/companion/withdraw` - 申请提现

### 11. 代练服务接口
- GET `/api/power-leveling/services` - 获取代练服务列表
- GET `/api/power-leveling/services/{id}` - 获取代练服务详情
- POST `/api/power-leveling/orders` - 创建代练订单
- GET `/api/power-leveling/orders` - 获取代练订单列表
- GET `/api/power-leveling/orders/{id}` - 获取代练订单详情
- POST `/api/power-leveling/orders/{id}/cancel` - 取消代练订单
- POST `/api/power-leveling/orders/{id}/refund` - 申请代练退款
- POST `/api/power-leveling/orders/{id}/review` - 代练订单评价

## 开发指南

### 1. 实现新的接口

按照以下步骤实现新的接口：

1. **创建DTO类** (如果需要)
   - 在 `DTOs/` 相应目录下创建请求和响应DTO
   - 参考 `DTOs/Auth/LoginRequest.cs`

2. **创建/扩展服务接口**
   - 在 `Services/` 目录下的接口文件中添加方法定义
   - 或创建新的服务接口文件

3. **实现服务逻辑**
   - 在对应的服务实现类中实现业务逻辑
   - 注入 `ApplicationDbContext` 进行数据库操作
   - 使用 `ApiResponse` 包装返回结果

4. **创建/扩展控制器**
   - 在 `Controllers/` 目录下的控制器中添加Action方法
   - 或创建新的控制器
   - 使用路由特性标记API路径
   - 添加Swagger注释

### 2. 示例：实现陪玩师列表接口

```csharp
// 1. 创建DTO (DTOs/Home/CompanionListResponse.cs)
public class CompanionListResponse
{
    public List<CompanionItem> Items { get; set; }
    public PaginationInfo Pagination { get; set; }
}

// 2. 在IHomeService中添加接口
Task<ApiResponse<CompanionListResponse>> GetCompanionsAsync(int page, int pageSize, int? gameId);

// 3. 在HomeService中实现
public async Task<ApiResponse<CompanionListResponse>> GetCompanionsAsync(int page, int pageSize, int? gameId)
{
    var query = _context.Companions
        .Include(c => c.User)
        .Where(c => c.Status == "已认证");

    if (gameId.HasValue)
    {
        query = query.Where(c => c.Games.Any(g => g.GameId == gameId.Value));
    }

    var total = await query.CountAsync();
    var items = await query
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    // 使用AutoMapper映射到DTO
    // ...

    return ApiResponse<CompanionListResponse>.SuccessResponse(result);
}

// 4. 在HomeController中添加Action
[HttpGet("companions")]
public async Task<ActionResult<ApiResponse<CompanionListResponse>>> GetCompanions(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20,
    [FromQuery] int? gameId = null)
{
    var result = await _homeService.GetCompanionsAsync(page, pageSize, gameId);
    return Ok(result);
}
```

### 3. 数据库操作

使用Entity Framework Core进行数据库操作：

```csharp
// 查询
var user = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

// 添加
_context.Users.Add(newUser);
await _context.SaveChangesAsync();

// 更新
user.Balance += amount;
await _context.SaveChangesAsync();

// 删除
_context.Users.Remove(user);
await _context.SaveChangesAsync();
```

### 4. 认证和授权

使用JWT进行身份验证：

```csharp
[Authorize]  // 需要登录
[HttpPost("secure-endpoint")]
public async Task<ActionResult> SecureEndpoint()
{
    // 从JWT Token中获取用户ID
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    var userId = int.Parse(userIdClaim);

    // 业务逻辑...
}
```

### 5. 错误处理

使用统一的响应格式返回错误：

```csharp
return ApiResponse<UserInfo>.ErrorResponse(1001, "用户不存在");
```

常见错误码：
- 1001: 用户不存在
- 1002: 密码错误
- 1003: 验证码错误
- 2001: 陪玩师不存在
- 3001: 订单不存在
- 4001: 动态不存在
- 5001: 消息发送失败

## 配置说明

### 数据库连接字符串

在 `appsettings.json` 中配置：

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=game_companion;Uid=root;Pwd=your_password;CharSet=utf8mb4;"
  }
}
```

### Redis配置

```json
{
  "ConnectionStrings": {
    "Redis": "localhost:6379,password=your_password,defaultDatabase=0"
  }
}
```

### JWT配置

```json
{
  "JwtSettings": {
    "SecretKey": "YourSuperSecretKeyHereMustBeAtLeast32CharactersLong!",
    "Issuer": "GameCompanion.Api",
    "Audience": "GameCompanion.Client",
    "ExpirationMinutes": 120,
    "RefreshExpirationDays": 7
  }
}
```

## 运行项目

### 开发环境

1. 确保已安装 .NET 8 SDK
2. 配置好数据库连接字符串
3. 运行项目：

```bash
cd GameCompanion.Api
dotnet run
```

4. 访问Swagger文档：`https://localhost:5001/swagger`

### 生产环境

1. 发布项目：

```bash
dotnet publish -c Release
```

2. 部署到服务器（IIS/Docker）

## 数据库迁移

创建数据库迁移：

```bash
dotnet ef migrations add InitialCreate
dotnet ef database update
```

## 注意事项

1. **密码加密**: 当前示例使用简化的Base64编码，实际项目应使用BCrypt等安全哈希算法
2. **验证码**: 需要集成短信服务商API（阿里云、腾讯云等）
3. **文件上传**: 需要集成云存储服务（阿里云OSS、腾讯云COS等）
4. **支付**: 需要集成微信支付V3 SDK
5. **WebSocket**: 实时消息功能需要配置WebSocket支持
6. **日志**: 生产环境建议使用ELK（Elasticsearch + Logstash + Kibana）

## 待完成功能

- [ ] 实现所有剩余的接口
- [ ] 集成短信验证码服务
- [ ] 集成云存储服务
- [ ] 集成微信支付
- [ ] 实现WebSocket实时通信
- [ ] 添加单元测试
- [ ] 添加集成测试
- [ ] 优化数据库查询性能
- [ ] 添加Redis缓存
- [ ] 实现Hangfire定时任务
- [ ] 完善错误处理
- [ ] 添加API限流

## 联系方式

- 技术支持: tech@example.com
- 产品经理: product@example.com
