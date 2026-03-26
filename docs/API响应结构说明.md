# API响应结构与状态码说明文档

## 📋 目录

1. [统一响应格式](#统一响应格式)
2. [响应字段说明](#响应字段说明)
3. [HTTP状态码](#http状态码)
4. [业务状态码](#业务状态码)
5. [响应示例](#响应示例)
6. [错误处理](#错误处理)

---

## 统一响应格式

### 标准响应结构

所有API接口均使用统一的响应格式 `ApiResponse<T>`，其中 `T` 为返回数据的类型。

```json
{
  "code": 200,
  "message": "success",
  "data": {},
  "error": null,
  "timestamp": 1620000000000
}
```

### 字段说明

| 字段名 | 类型 | 必填 | 说明 |
|--------|------|------|------|
| code | Integer | 是 | 业务状态码（见下方详细说明） |
| message | String | 是 | 响应消息，描述请求结果 |
| data | T | 否 | 响应数据，成功时返回具体数据，失败时可为null |
| error | String | 否 | 错误详细信息，仅在错误时返回 |
| timestamp | Long | 是 | Unix时间戳（毫秒），标识响应时间 |

---

## 响应字段说明

### 1. code（业务状态码）

业务状态码用于标识请求的处理结果，与HTTP状态码配合使用。

- **200**: 请求成功
- **400**: 请求参数错误
- **401**: 未授权，需要登录
- **403**: 无权限访问
- **404**: 资源不存在
- **500**: 服务器内部错误

### 2. message（响应消息）

简短描述请求结果的文字说明。

- 成功时：`"success"` 或具体的成功消息
- 失败时：具体的错误描述

### 3. data（响应数据）

实际的业务数据，类型由 `T` 决定。

- 可以是对象：`{ "id": 1, "name": "测试" }`
- 可以是数组：`[1, 2, 3]`
- 可以是null：无数据返回时
- 可以是复杂结构：嵌套对象或数组

### 4. error（错误详情）

仅在发生错误时提供详细信息的字段。

- 包含具体的错误原因
- 用于调试和问题定位
- 在成功响应中通常为 `null`

### 5. timestamp（时间戳）

Unix时间戳（毫秒级），用于标识响应生成的时间。

- 示例：`1620000000000`
- 对应：`2021-05-03 00:00:00 UTC`

---

## HTTP状态码

### 2xx 成功响应

#### 200 OK
请求成功，服务器返回请求的数据。

**使用场景**:
- GET: 成功获取资源
- POST: 成功创建资源
- PUT: 成功更新资源
- DELETE: 成功删除资源

**示例**:
```json
{
  "code": 200,
  "message": "操作成功",
  "data": { "id": 100, "name": "测试数据" },
  "error": null,
  "timestamp": 1620000000000
}
```

### 4xx 客户端错误

#### 400 Bad Request
请求参数错误或请求格式不正确。

**使用场景**:
- 缺少必填参数
- 参数类型错误
- 参数格式不正确
- 参数验证失败

**示例**:
```json
{
  "code": 400,
  "message": "请求参数错误",
  "error": "用户名不能为空",
  "timestamp": 1620000000000
}
```

#### 401 Unauthorized
未授权，需要身份验证。

**使用场景**:
- Token缺失或无效
- Token已过期
- 未提供认证信息

**示例**:
```json
{
  "code": 401,
  "message": "未授权访问",
  "error": "Invalid or expired token",
  "timestamp": 1620000000000
}
```

#### 403 Forbidden
已授权但无权限访问该资源。

**使用场景**:
- 权限不足
- 角色不允许访问该资源
- 需要更高的权限级别

**示例**:
```json
{
  "code": 403,
  "message": "权限不足",
  "error": "您没有权限执行此操作",
  "timestamp": 1620000000000
}
```

#### 404 Not Found
请求的资源不存在。

**使用场景**:
- 用户不存在
- 订单不存在
- 陪玩师不存在
- API端点错误

**示例**:
```json
{
  "code": 404,
  "message": "资源不存在",
  "error": "用户ID: 10001 不存在",
  "timestamp": 1620000000000
}
```

### 5xx 服务器错误

#### 500 Internal Server Error
服务器内部错误。

**使用场景**:
- 数据库连接失败
- 服务器异常
- 未捕获的异常

**示例**:
```json
{
  "code": 500,
  "message": "服务器内部错误",
  "error": "数据库连接失败",
  "timestamp": 1620000000000
}
```

---

## 业务状态码

除了HTTP状态码外，系统还定义了业务状态码用于更细粒度的错误分类。

### 用户管理相关

| 状态码 | 说明 | HTTP状态码 |
|--------|------|------------|
| 200 | 操作成功 | 200 |
| 400 | 参数错误（用户名为空、手机号格式错误等） | 400 |
| 401 | 未登录或Token无效 | 401 |
| 403 | 无权限（封禁用户需要超级管理员权限） | 403 |
| 404 | 用户不存在 | 404 |

### 陪玩管理相关

| 状态码 | 说明 | HTTP状态码 |
|--------|------|------------|
| 200 | 操作成功 | 200 |
| 400 | 参数错误 | 400 |
| 403 | 无权限 | 403 |
| 404 | 陪玩师不存在或认证申请不存在 | 404 |

### 订单管理相关

| 状态码 | 说明 | HTTP状态码 |
|--------|------|------------|
| 200 | 操作成功 | 200 |
| 400 | 订单状态不允许当前操作 | 400 |
| 403 | 无权限 | 403 |
| 404 | 订单不存在 | 404 |

### 内容管理相关

| 状态码 | 说明 | HTTP状态码 |
|--------|------|------------|
| 200 | 操作成功 | 200 |
| 403 | 无权限 | 403 |
| 404 | 动态不存在 | 404 |

### 系统配置相关

| 状态码 | 说明 | HTTP状态码 |
|--------|------|------------|
| 200 | 操作成功 | 200 |
| 400 | 配置参数错误 | 400 |
| 403 | 无权限 | 403 |
| 404 | 管理员不存在或配置不存在 | 404 |

---

## 响应示例

### 1. 成功响应（有数据）

**场景**: 获取用户详情

```json
{
  "code": 200,
  "message": "success",
  "data": {
    "id": 10001,
    "username": "xiaoyu",
    "nickname": "小雨酱",
    "avatar_url": "https://example.com/avatar.jpg",
    "phone": "138****8888",
    "gender": 2,
    "vip_level": 1
  },
  "error": null,
  "timestamp": 1620000000000
}
```

### 2. 成功响应（无数据）

**场景**: 删除操作成功

```json
{
  "code": 200,
  "message": "删除成功",
  "data": null,
  "error": null,
  "timestamp": 1620000000000
}
```

### 3. 分页响应

**场景**: 获取用户列表

```json
{
  "code": 200,
  "message": "success",
  "data": {
    "items": [
      {
        "id": 10001,
        "username": "xiaoyu",
        "nickname": "小雨酱"
      },
      {
        "id": 10002,
        "username": "ajie",
        "nickname": "阿杰游戏"
      }
    ],
    "pagination": {
      "page": 1,
      "page_size": 20,
      "total": 100,
      "total_pages": 5
    }
  },
  "error": null,
  "timestamp": 1620000000000
}
```

### 4. 参数错误响应

**场景**: 缺少必填参数

```json
{
  "code": 400,
  "message": "请求参数错误",
  "error": "用户名不能为空",
  "timestamp": 1620000000000
}
```

### 5. 未授权响应

**场景**: Token无效或过期

```json
{
  "code": 401,
  "message": "未授权访问",
  "error": "Invalid or expired token",
  "timestamp": 1620000000000
}
```

### 6. 资源不存在响应

**场景**: 用户不存在

```json
{
  "code": 404,
  "message": "资源不存在",
  "error": "用户ID: 99999 不存在",
  "timestamp": 1620000000000
}
```

### 7. 服务器错误响应

**场景**: 数据库连接失败

```json
{
  "code": 500,
  "message": "服务器内部错误",
  "error": "数据库连接失败: Connection timeout",
  "timestamp": 1620000000000
}
```

---

## 错误处理

### 全局异常处理

项目使用全局异常处理中间件 `ExceptionHandlingMiddleware`，自动捕获所有未处理的异常并转换为标准错误响应。

**异常类型映射**:

| 异常类型 | HTTP状态码 | 业务Code | 说明 |
|----------|------------|----------|------|
| ArgumentException | 400 | 400 | 参数验证失败 |
| UnauthorizedAccessException | 401 | 401 | 未授权访问 |
| KeyNotFoundException | 404 | 404 | 资源不存在 |
| 其他异常 | 500 | 500 | 服务器内部错误 |

### 错误响应格式

所有异常都被转换为统一格式：

```json
{
  "code": <错误状态码>,
  "message": "<错误类型描述>",
  "error": "<具体错误信息>",
  "timestamp": <当前时间戳>
}
```

### 使用示例

#### 在控制器中返回成功响应

```csharp
// 返回数据
return Ok(ApiResponse<object>.Success(new { id = 1, name = "测试" }));

// 返回成功消息（无数据）
return Ok(ApiResponse<object>.Success("操作成功"));

// 返回null数据
return Ok(ApiResponse<object>.Success<object?>(null, "删除成功"));
```

#### 在控制器中返回错误响应

```csharp
// 参数错误
return BadRequest(ApiResponse<object>.Fail(400, "参数错误", "用户名不能为空"));

// 未授权
return Unauthorized(ApiResponse<object>.Fail(401, "未授权", "Token已过期"));

// 资源不存在
return NotFound(ApiResponse<object>.Fail(404, "用户不存在", "用户ID: 99999 不存在"));

// 服务器错误
return StatusCode(500, ApiResponse<object>.Fail(500, "服务器错误", ex.Message));
```

#### 在服务层抛出异常

```csharp
// 参数验证失败
throw new ArgumentException("用户名不能为空");

// 权限验证失败
throw new UnauthorizedAccessException("您没有权限执行此操作");

// 资源不存在
throw new KeyNotFoundException("用户不存在");
```

### 最佳实践

1. **统一使用ApiResponse**: 所有接口都应使用 `ApiResponse<T>` 作为返回类型

2. **清晰的错误消息**: `message` 字段应该是用户友好的简短描述

3. **详细的错误信息**: `error` 字段应该包含详细的错误原因，便于调试

4. **正确使用HTTP状态码**:
   - 成功返回 200
   - 客户端错误返回 4xx
   - 服务器错误返回 5xx

5. **异常处理**:
   - 在服务层抛出标准异常类型
   - 由全局异常处理中间件统一处理
   - 避免在控制器中直接 try-catch

6. **时间戳**:
   - 所有响应自动包含时间戳
   - 用于追踪请求顺序和调试

---

## 分页响应说明

### 分页响应结构

列表类接口使用 `PagedResponse<T>` 返回分页数据。

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

### 分页参数

| 参数名 | 类型 | 必填 | 默认值 | 说明 |
|--------|------|------|--------|------|
| page | Integer | 否 | 1 | 当前页码，从1开始 |
| page_size | Integer | 否 | 20 | 每页数量，最大100 |
| order_by | String | 否 | created_at | 排序字段 |
| order | String | 否 | desc | 排序方向：asc升序，desc降序 |

### 分页信息字段

| 字段名 | 类型 | 说明 |
|--------|------|------|
| page | Integer | 当前页码 |
| page_size | Integer | 每页数量 |
| total | Integer | 总记录数 |
| total_pages | Integer | 总页数 |

**计算公式**:
```
total_pages = Math.Ceiling((double)total / page_size)
```

---

## 附录：完整的状态码速查表

### HTTP状态码

| 状态码 | 名称 | 说明 |
|--------|------|------|
| 200 | OK | 请求成功 |
| 400 | Bad Request | 请求参数错误 |
| 401 | Unauthorized | 未授权 |
| 403 | Forbidden | 禁止访问 |
| 404 | Not Found | 资源不存在 |
| 500 | Internal Server Error | 服务器内部错误 |

### 常用业务场景

| 场景 | HTTP状态码 | Code | Message示例 |
|------|------------|------|-------------|
| 登录成功 | 200 | 200 | "登录成功" |
| 登录失败 | 401 | 401 | "账号或密码错误" |
| Token过期 | 401 | 401 | "Token已过期" |
| 参数缺失 | 400 | 400 | "用户名不能为空" |
| 用户不存在 | 404 | 404 | "用户不存在" |
| 陪玩师不存在 | 404 | 404 | "陪玩师不存在" |
| 订单不存在 | 404 | 404 | "订单不存在" |
| 权限不足 | 403 | 403 | "权限不足" |
| 数据库错误 | 500 | 500 | "数据库连接失败" |

---

**文档版本**: v1.0
**最后更新**: 2026-03-25
**维护者**: GameCompanion API Team
