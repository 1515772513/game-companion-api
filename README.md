# gameCompanion
游戏陪玩
陪玩平台后端服务 .NET 8 Web API 项目说明
这是陪玩小程序 + PC 管理端的统一后端服务，采用 ASP.NET Core 8 构建，支持高并发、模块化、前后端分离架构，为全平台提供稳定可靠的接口服务。
项目简介
面向：微信小程序、PC 管理端
架构：RESTful API + 原生 WebSocket 实时通信
环境：Windows / Linux 跨平台部署
部署：阿里云 ECS + IIS / Docker
数据库：MySQL 8.0
缓存：Redis
存储：阿里云 OSS
支付：微信支付 V3
实时消息：原生 WebSocket（订单、接单、派单、提醒全实时）
核心技术栈
开发框架：ASP.NET Core 8 Web API
ORM 框架：EF Core 8（数据库操作）
身份鉴权：JWT 身份验证、Token 授权
缓存服务：Redis（高频数据、登录状态、排行榜、限流）
数据库：MySQL 8.0（主业务数据存储）
文件存储：阿里云 OSS / 腾讯云 COS（头像、认证资料、音频、图片）
实时通信：原生 WebSocket（订单通知、新消息、派单提醒、状态同步）
支付接入：微信支付 V3 SDK
日志系统：Serilog（结构化日志）+ ELK（可选）
API 文档：Swagger / Knife4j 自动生成接口文档
定时任务：Hangfire（超时订单自动取消、自动结算、数据统计）

环境要求
.NET 8 SDK
MySQL 8.0
Redis
阿里云 OSS（可选）
微信支付商户号
微信小程序 AppID / AppSecret
快速启动
配置 appsettings.json
数据库连接字符串
Redis 连接
JWT 密钥
阿里云 OSS 配置
微信支付 V3 配置
WebSocket 配置
执行 EF Core 迁移生成数据库
bash
运行
Update-Database
启动项目
自动打开 Swagger 文档：https://localhost:5001/swagger
运行 Hangfire 面板
查看定时任务、超时订单、自动结算
接口规范
遵循 RESTful 风格
统一返回格式（Code + Msg + Data）
JWT 鉴权统一放入请求头
支持全局异常处理
支持接口限流、日志记录
WebSocket 实时通信说明
采用 ASP.NET Core 原生 WebSocket 实现，轻量、稳定、兼容性强
支持小程序、管理端双端实时消息推送
支持用户连接管理、消息广播、点对点推送
支持心跳检测、自动重连、消息队列
典型场景：
用户下单 → 陪玩师实时收到提醒
陪玩接单 → 用户实时收到通知
订单状态变更 → 双方同步更新
部署方式
Windows Server（IIS）
发布项目为文件夹
IIS 添加网站
安装 .NET 8 托管捆绑包
配置应用程序池
安全组开放端口：80/443/5000
开启 IIS WebSocket 支持
Linux（Docker）
编写 Dockerfile
构建镜像
运行容器
Nginx 反向代理（支持 WebSocket）
核心能力
微信小程序一键登录
JWT 无状态鉴权
原生 WebSocket 高兼容实时消息
微信支付 V3 安全对接
超时订单自动关闭
自动结算、自动统计
文件云存储，不占用服务器空间
高并发、可横向扩展
常见问题
无法连接 MySQL：检查端口 3306、权限、连接字符串
无法连接 Redis：检查 6379 端口、密码
小程序无法访问：ECS 安全组开放端口
文件上传失败：检查 OSS 密钥、Bucket 权限
实时消息不通：检查 WebSocket 配置、端口、Nginx 代理、心跳机制

# 数据库连接字符串
server=47.98.225.136;port=3306;database=game_companion;user=admin;password=admin123;charset=utf8mb4
# 实体类更新
Scaffold-DbContext "server=47.98.225.136;port=3306;database=game_companion;user=admin;password=admin123;charset=utf8mb4;" Pomelo.EntityFrameworkCore.MySql -OutputDir Models/Entities -Force
# vscode里面执行 模型会生成注解
dotnet ef dbcontext scaffold "server=47.98.225.136;port=3306;database=game_companion;user=admin;password=admin123;charset=utf8mb4" Pomelo.EntityFrameworkCore.MySql -o Models/Entities -f --no-build --project GameCompanion.Api.csproj --data-annotations
# 部署IIS 打包命令
dotnet publish GameCompanion.Api.csproj --configuration Release --output ./publish --self-contained false