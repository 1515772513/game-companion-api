# API响应使用指南与最佳实践

## 📚 目录

1. [快速开始](#快速开始)
2. [响应类型](#响应类型)
3. [控制器使用示例](#控制器使用示例)
4. [服务层使用示例](#服务层使用示例)
5. [常见场景](#常见场景)
6. [注意事项](#注意事项)

---

## 快速开始

### 基本响应

```csharp
// 1. 成功响应（带数据）
var data = new { id = 1, name = "测试" };
return Ok(ApiResponse<object>.Success(data));

// 响应：
{
//   "code": 200,
//   "message": "success",
//   "data": { "id": 1, "name": "测试" }
// }

// 2. 成功响应（无数据）
return Ok(ApiResponse<object>.Success("操作成功"));

// 响应：
{
//   "code": 200,
//   "message": "操作成功",
//   "data": null
// }

// 3. 错误响应
return BadRequest(ApiResponse<object>.Fail(400, "参数错误", "用户名不能为空"));

// 响应：
{
//   "code": 400,
//   "message": "参数错误",
//   "error": "用户名不能为空"
// }
```

---

## 响应类型

### 1. ApiResponse<T>

通用响应类型，适用于大多数场景。

```csharp
// 返回单个对象
public async Task<ActionResult<ApiResponse<object>>> GetUser(int id)
{
    var user = await _userService.GetUserById(id);
    if (user == null)
    {
        return NotFound(ApiResponse<object>.Fail(404, "用户不存在"));
    }
    return Ok(ApiResponse<object>.Success(user));
}

// 返回列表
public async Task<ActionResult<ApiResponse<object>>> GetUsers()
{
    var users = await _userService.GetAllUsers();
    return Ok(ApiResponse<object>.Success(users));
}

// 返回分页数据
public async Task<ActionResult<ApiResponse<object>>> GetUsersPaged(int page, int pageSize)
{
    var result = await _userService.GetUsersPaged(page, pageSize);
    return Ok(ApiResponse<object>.Success(result));
}
```

### 2. PagedResponse<T>

分页响应专用类型。

```csharp
public class PagedUserList
{
    public List<UserDto> Items { get; set; }
    public PaginationInfo Pagination { get; set; }
}

public async Task<ActionResult<ApiResponse<object>>> GetUsers(int page, int pageSize)
{
    var users = await _context.Users
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    var total = await _context.Users.CountAsync();

    return Ok(ApiResponse<object>.Success(new
    {
        items = users,
        pagination = new
        {
            page = page,
            page_size = pageSize,
            total = total,
            total_pages = (int)Math.Ceiling((double)total / pageSize)
        }
    }));
}
```

---

## 控制器使用示例

### GET请求示例

#### 获取单个资源

```csharp
[HttpGet("{userId}")]
public async Task<ActionResult<ApiResponse<object>>> GetUserDetail(int userId)
{
    try
    {
        var user = await _userService.GetUserDetailAsync(userId);

        if (user == null)
        {
            return NotFound(ApiResponse<object>.Fail(
                404,
                "用户不存在",
                $"用户ID: {userId} 不存在"
            ));
        }

        return Ok(ApiResponse<object>.Success(user));
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "获取用户详情失败: {UserId}", userId);
        return StatusCode(500, ApiResponse<object>.Fail(
            500,
            "服务器内部错误",
            ex.Message
        ));
    }
}
```

#### 获取列表资源

```csharp
[HttpGet]
public async Task<ActionResult<ApiResponse<object>>> GetUsers(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20,
    [FromQuery] string? keyword = null)
{
    try
    {
        var result = await _userService.GetUserListAsync(
            page, pageSize, keyword
        );

        return Ok(ApiResponse<object>.Success(result));
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "获取用户列表失败");
        return BadRequest(ApiResponse<object>.Fail(
            400,
            "获取用户列表失败",
            ex.Message
        ));
    }
}
```

### POST请求示例

#### 创建资源

```csharp
[HttpPost]
public async Task<ActionResult<ApiResponse<object>>> CreateUser(
    [FromBody] CreateUserRequest request)
{
    try
    {
        // 参数验证
        if (string.IsNullOrEmpty(request.Username))
        {
            return BadRequest(ApiResponse<object>.Fail(
                400,
                "参数错误",
                "用户名不能为空"
            ));
        }

        // 创建用户
        var userId = await _userService.CreateUserAsync(request);

        return Ok(ApiResponse<object>.Success(
            new { id = userId, message = "用户创建成功" },
            "用户创建成功"
        ));
    }
    catch (ArgumentException ex)
    {
        return BadRequest(ApiResponse<object>.Fail(
            400,
            "参数错误",
            ex.Message
        ));
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "创建用户失败");
        return StatusCode(500, ApiResponse<object>.Fail(
            500,
            "创建用户失败",
            ex.Message
        ));
    }
}
```

### PUT请求示例

#### 更新资源

```csharp
[HttpPut("{userId}")]
public async Task<ActionResult<ApiResponse<object>>> UpdateUser(
    int userId,
    [FromBody] UpdateUserRequest request)
{
    try
    {
        var exists = await _userService.UserExistsAsync(userId);
        if (!exists)
        {
            return NotFound(ApiResponse<object>.Fail(
                404,
                "用户不存在"
            ));
        }

        await _userService.UpdateUserAsync(userId, request);

        return Ok(ApiResponse<object>.Success(null, "更新成功"));
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "更新用户失败: {UserId}", userId);
        return BadRequest(ApiResponse<object>.Fail(
            400,
            "更新失败",
            ex.Message
        ));
    }
}
```

### DELETE请求示例

#### 删除资源

```csharp
[HttpDelete("{userId}")]
public async Task<ActionResult<ApiResponse<object>>> DeleteUser(int userId)
{
    try
    {
        var success = await _userService.DeleteUserAsync(userId);

        if (!success)
        {
            return NotFound(ApiResponse<object>.Fail(
                404,
                "用户不存在"
            ));
        }

        return Ok(ApiResponse<object>.Success(null, "删除成功"));
    }
    catch (Exception ex)
    {
        _logger.LogError(ex, "删除用户失败: {UserId}", userId);
        return StatusCode(500, ApiResponse<object>.Fail(
            500,
            "删除失败",
            ex.Message
        ));
    }
}
```

---

## 服务层使用示例

### 在服务层返回数据

```csharp
public class UserService : IUserService
{
    public async Task<object?> GetUserDetailAsync(int userId)
    {
        var user = await _context.Users
            .Where(u => u.Id == userId)
            .Select(u => new
            {
                u.Id,
                u.Username,
                u.Nickname,
                u.AvatarUrl,
                u.Phone,
                u.Gender,
                u.VipLevel
            })
            .FirstOrDefaultAsync();

        return user; // 如果不存在返回null，由控制器处理404
    }

    public async Task<object> GetUserListAsync(
        int page,
        int pageSize,
        string? keyword)
    {
        var query = _context.Users.AsQueryable();

        if (!string.IsNullOrEmpty(keyword))
        {
            query = query.Where(u =>
                u.Username.Contains(keyword) ||
                u.Nickname.Contains(keyword)
            );
        }

        var total = await query.CountAsync();
        var items = await query
            .OrderByDescending(u => u.CreatedAt)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
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
}
```

### 在服务层验证并抛出异常

```csharp
public class AuthService : IAuthService
{
    public async Task<object> LoginAsync(string username, string password)
    {
        // 验证参数
        if (string.IsNullOrEmpty(username))
        {
            throw new ArgumentException("用户名不能为空", nameof(username));
        }

        if (string.IsNullOrEmpty(password))
        {
            throw new ArgumentException("密码不能为空", nameof(password));
        }

        // 查找用户
        var admin = await _context.Admins
            .FirstOrDefaultAsync(a => a.Username == username);

        if (admin == null)
        {
            throw new KeyNotFoundException("管理员不存在");
        }

        // 验证密码
        if (!VerifyPassword(password, admin.Password))
        {
            throw new UnauthorizedAccessException("账号或密码错误");
        }

        // 检查账号状态
        if (admin.AccountStatus != 0)
        {
            throw new UnauthorizedAccessException("账号已被禁用");
        }

        // 返回登录信息
        return new
        {
            access_token = GenerateToken(admin),
            admin_info = new { /* ... */ }
        };
    }
}
```

---

## 常见场景

### 场景1：参数验证失败

```csharp
[HttpPost]
public async Task<ActionResult<ApiResponse<object>>> CreateOrder(
    [FromBody] CreateOrderRequest request)
{
    // 方式1：直接返回错误响应
    if (request.CompanionId <= 0)
    {
        return BadRequest(ApiResponse<object>.Fail(
            400,
            "参数错误",
            "陪玩师ID必须大于0"
        ));
    }

    // 方式2：使用Data Annotations
    if (!ModelState.IsValid)
    {
        return BadRequest(ApiResponse<object>.Fail(
            400,
            "参数验证失败",
            ModelState.Values.First().Errors.First().ErrorMessage
        ));
    }

    var orderId = await _orderService.CreateOrderAsync(request);
    return Ok(ApiResponse<object>.Success(new { id = orderId }));
}
```

### 场景2：资源不存在

```csharp
[HttpGet("{orderId}")]
public async Task<ActionResult<ApiResponse<object>>> GetOrder(int orderId)
{
    var order = await _orderService.GetOrderAsync(orderId);

    if (order == null)
    {
        return NotFound(ApiResponse<object>.Fail(
            404,
            "订单不存在",
            $"订单ID: {orderId} 不存在"
        ));
    }

    return Ok(ApiResponse<object>.Success(order));
}
```

### 场景3：权限验证

```csharp
[HttpDelete("{userId}")]
[Authorize(Roles = "Admin")]
public async Task<ActionResult<ApiResponse<object>>> DeleteUser(int userId)
{
    // 检查是否尝试删除超级管理员
    var user = await _userService.GetUserById(userId);
    if (user != null && user.Role == "super_admin")
    {
        return Forbid(ApiResponse<object>.Fail(
            403,
            "权限不足",
            "不能删除超级管理员"
        ));
    }

    await _userService.DeleteUserAsync(userId);
    return Ok(ApiResponse<object>.Success(null, "删除成功"));
}
```

### 场景4：业务逻辑错误

```csharp
[HttpPost("{orderId}/refund")]
public async Task<ActionResult<ApiResponse<object>>> ProcessRefund(
    int orderId,
    [FromBody] RefundRequest request)
{
    var order = await _orderService.GetOrderAsync(orderId);

    if (order == null)
    {
        return NotFound(ApiResponse<object>.Fail(404, "订单不存在"));
    }

    // 检查订单状态
    if (order.Status != 3) // 3 = 已完成
    {
        return BadRequest(ApiResponse<object>.Fail(
            400,
            "订单状态不允许退款",
            "只有已完成的订单才能申请退款"
        ));
    }

    // 检查退款时限
    if ((DateTime.Now - order.CompletedTime).Value.Days > 7)
    {
        return BadRequest(ApiResponse<object>.Fail(
            400,
            "超过退款期限",
            "完成7天后不可申请退款"
        ));
    }

    await _orderService.ProcessRefundAsync(orderId, request.RefundAmount);
    return Ok(ApiResponse<object>.Success(null, "退款处理成功"));
}
```

### 场景5：分页查询

```csharp
[HttpGet]
public async Task<ActionResult<ApiResponse<object>>> GetUsers(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 20,
    [FromQuery] string? orderBy = "created_at",
    [FromQuery] string order = "desc")
{
    // 验证分页参数
    if (page < 1)
    {
        return BadRequest(ApiResponse<object>.Fail(
            400,
            "参数错误",
            "页码必须大于0"
        ));
    }

    if (pageSize < 1 || pageSize > 100)
    {
        return BadRequest(ApiResponse<object>.Fail(
            400,
            "参数错误",
            "每页数量必须在1-100之间"
        ));
    }

    // 验证排序参数
    var allowedOrderFields = new[] { "created_at", "username", "last_active_time" };
    if (!allowedOrderFields.Contains(orderBy))
    {
        return BadRequest(ApiResponse<object>.Fail(
            400,
            "参数错误",
            $"排序字段必须是: {string.Join(", ", allowedOrderFields)}"
        ));
    }

    if (order != "asc" && order != "desc")
    {
        return BadRequest(ApiResponse<object>.Fail(
            400,
            "参数错误",
            "排序方向必须是 asc 或 desc"
        ));
    }

    var result = await _userService.GetUsersAsync(
        page, pageSize, orderBy, order
    );

    return Ok(ApiResponse<object>.Success(result));
}
```

---

## 注意事项

### 1. 时间戳使用

```csharp
// ✅ 正确：时间戳自动生成
public ApiResponse<T> Response => new()
{
    Code = 200,
    Message = "success",
    Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds() // 自动设置
};

// ❌ 错误：手动设置时间戳（容易遗漏）
public ApiResponse<T> CreateResponse()
{
    return new ApiResponse<T>
    {
        Code = 200,
        Timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds()
    };
}
```

### 2. 可空类型处理

```csharp
// ✅ 正确：使用 T? 支持null
public static ApiResponse<T> Success(T? data, string message = "success")

// 调用时可以传递null
ApiResponse<object>.Success<object?>(null, "删除成功")

// ❌ 错误：不使用可空类型会导致警告
public static ApiResponse<T> Success(T data, string message = "success")
ApiResponse<object>.Success(null) // 编译警告
```

### 3. 错误字段使用

```csharp
// ✅ 正确：只在错误时提供详细信息
return BadRequest(ApiResponse<object>.Fail(
    400,
    "参数错误",  // 用户友好的简短消息
    "用户名不能为空，且长度必须在3-20个字符之间"  // 详细错误信息
));

// ❌ 错误：不提供详细错误信息
return BadRequest(ApiResponse<object>.Fail(400, "参数错误"));
```

### 4. HTTP状态码一致性

```csharp
// ✅ 正确：HTTP状态码与业务Code一致
return NotFound(ApiResponse<object>.Fail(404, "用户不存在"));
// HTTP: 404, Code: 404

return Unauthorized(ApiResponse<object>.Fail(401, "未授权"));
// HTTP: 401, Code: 401

// ❌ 错误：HTTP状态码与业务Code不一致
return Ok(ApiResponse<object>.Fail(404, "用户不存在"));
// HTTP: 200, Code: 404 （不推荐）
```

### 5. 异常处理最佳实践

```csharp
// ✅ 推荐：在服务层抛出标准异常
public async Task<User> GetUserAsync(int id)
{
    var user = await _context.Users.FindAsync(id);
    if (user == null)
    {
        throw new KeyNotFoundException($"用户ID: {id} 不存在");
    }
    return user;
}

// 在控制器中让全局异常处理中间件处理
[HttpGet("{id}")]
public async Task<ActionResult<ApiResponse<object>>> GetUser(int id)
{
    // 不需要try-catch，让异常传播到全局中间件
    var user = await _userService.GetUserAsync(id);
    return Ok(ApiResponse<object>.Success(user));
}

// ❌ 不推荐：在控制器中过度try-catch
[HttpGet("{id}")]
public async Task<ActionResult<ApiResponse<object>>> GetUser(int id)
{
    try
    {
        var user = await _userService.GetUserAsync(id);
        return Ok(ApiResponse<object>.Success(user));
    }
    catch (KeyNotFoundException ex)
    {
        // 这部分逻辑应该由全局中间件处理
        return NotFound(ApiResponse<object>.Fail(404, ex.Message));
    }
    // ... 其他异常捕获
}
```

### 6. 分页数据格式

```csharp
// ✅ 推荐：使用统一格式
return Ok(ApiResponse<object>.Success(new
{
    items = userList,
    pagination = new
    {
        page = page,
        page_size = pageSize,
        total = totalCount,
        total_pages = totalPages
    }
}));

// ❌ 不推荐：使用非标准字段名
return Ok(ApiResponse<object>.Success(new
{
    data = userList,
    pageInfo = new { /* ... */ }
}));
```

---

## 调试技巧

### 1. 查看完整响应

使用Swagger UI可以查看完整的响应JSON：

```
http://localhost:5000/swagger
```

### 2. 检查时间戳

```javascript
// JavaScript转换时间戳
const timestamp = 1620000000000;
const date = new Date(timestamp);
console.log(date.toISOString()); // 2021-05-03T00:00:00.000Z
```

### 3. 验证响应结构

```csharp
// 在客户端验证响应
if (response.code === 200) {
    console.log("成功:", response.message);
    console.log("数据:", response.data);
} else {
    console.error("失败:", response.message);
    console.error("错误:", response.error);
}
```

---

**文档版本**: v1.0
**最后更新**: 2026-03-25
**相关文档**: [API响应结构说明.md](./API响应结构说明.md)
