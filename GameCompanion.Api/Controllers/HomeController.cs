using GameCompanion.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace GameCompanion.Api.Controllers;

/// <summary>
/// 首页服务接口
/// </summary>
[ApiController]
[Route("api/home")]
public class HomeController : ControllerBase
{
    // TODO: 实现首页相关接口
    // 参考接口文档：
    // - GET /api/home - 获取首页数据
    // - GET /api/companions - 获取陪玩师列表
    // - GET /api/companions/{id} - 获取陪玩师详情
    // - GET /api/games - 获取游戏列表
    // - GET /api/search/companions - 搜索陪玩师
    // - GET /api/circles - 获取游戏圈子列表
}

/// <summary>
/// 订单管理接口
/// </summary>
[ApiController]
[Route("api/orders")]
public class OrdersController : ControllerBase
{
    // TODO: 实现订单相关接口
    // 参考接口文档：
    // - POST /api/orders - 创建订单
    // - GET /api/orders - 获取订单列表
    // - GET /api/orders/{id} - 获取订单详情
    // - POST /api/orders/{id}/cancel - 取消订单
    // - POST /api/orders/{id}/refund - 申请退款
    // - POST /api/orders/{id}/confirm - 确认完成
    // - POST /api/orders/{id}/review - 订单评价
}

/// <summary>
/// 动态社区接口
/// </summary>
[ApiController]
[Route("api/posts")]
public class PostsController : ControllerBase
{
    // TODO: 实现动态相关接口
    // 参考接口文档：
    // - POST /api/posts - 发布动态
    // - GET /api/posts - 获取动态列表
    // - GET /api/posts/{id} - 获取动态详情
    // - POST /api/posts/{id}/like - 点赞动态
    // - POST /api/posts/{id}/collect - 收藏动态
    // - POST /api/posts/{id}/comments - 评论动态
    // - GET /api/posts/my - 获取我的发布
    // - DELETE /api/posts/{id} - 删除动态
    // - POST /api/posts/draft - 保存草稿
    // - GET /api/posts/drafts - 获取草稿列表
}

/// <summary>
/// 消息聊天接口
/// </summary>
[ApiController]
[Route("api/conversations")]
public class ConversationsController : ControllerBase
{
    // TODO: 实现消息相关接口
    // 参考接口文档：
    // - GET /api/conversations - 获取会话列表
    // - GET /api/conversations/{id}/messages - 获取聊天详情
    // - POST /api/conversations/{id}/messages - 发送消息
    // - POST /api/conversations/upload-image - 上传聊天图片
}

/// <summary>
/// 系统通知接口
/// </summary>
[ApiController]
[Route("api/notifications")]
public class NotificationsController : ControllerBase
{
    // TODO: 实现通知相关接口
    // 参考接口文档：
    // - GET /api/notifications - 获取官方通知
    // - POST /api/notifications/{id}/read - 标记通知已读
}

/// <summary>
/// 个人中心接口
/// </summary>
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
    // TODO: 实现个人中心相关接口
    // 参考接口文档：
    // - GET /api/user/profile - 获取个人信息
    // - PUT /api/user/profile - 更新个人资料
    // - POST /api/user/upload-avatar - 上传头像
    // - POST /api/user/verify-real-name - 实名认证
    // - GET /api/user/collections - 获取我的收藏
    // - GET /api/user/following - 获取关注列表
    // - GET /api/user/followers - 获取粉丝列表
    // - POST /api/user/follow - 关注/取消关注用户
    // - POST /api/user/apply-companion - 申请成为陪玩师
    // - GET /api/user/wallet - 获取钱包信息
}

/// <summary>
/// 设置相关接口
/// </summary>
[ApiController]
[Route("api/settings")]
public class SettingsController : ControllerBase
{
    // TODO: 实现设置相关接口
    // 参考接口文档：
    // - GET /api/settings/account - 获取账号设置
    // - POST /api/settings/change-phone - 修改手机号
    // - POST /api/settings/change-password - 修改密码
    // - PUT /api/settings/privacy - 更新隐私设置
    // - PUT /api/settings/notification - 更新通知设置
}

/// <summary>
/// 意见反馈接口
/// </summary>
[ApiController]
[Route("api/feedback")]
public class FeedbackController : ControllerBase
{
    // TODO: 实现反馈相关接口
}

/// <summary>
/// 陪玩师认证接口
/// </summary>
[ApiController]
[Route("api/companion")]
public class CompanionController : ControllerBase
{
    // TODO: 实现陪玩师相关接口
    // 参考接口文档中的陪玩师认证部分
}

/// <summary>
/// 代练服务接口
/// </summary>
[ApiController]
[Route("api/power-leveling")]
public class PowerLevelingController : ControllerBase
{
    // TODO: 实现代练服务相关接口
    // 参考接口文档中的代练服务部分
}
