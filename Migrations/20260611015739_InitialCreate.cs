using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GameCompanion.Api.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "games",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "游戏名称", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name_en = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "英文名", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    icon = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "游戏图标", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    cover_image = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "封面图片", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "游戏类型", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "text", nullable: true, comment: "游戏描述", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "char(1)", fixedLength: true, maxLength: 1, nullable: true, defaultValueSql: "'1'", comment: "状态", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    sort_order = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'0'", comment: "排序"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    IsActive = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'1'", comment: "是否启用")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                },
                comment: "游戏表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "recover_your_data_info",
                columns: table => new
                {
                    READ_ME = table.Column<string>(type: "text", nullable: true, collation: "utf8mb3_general_ci")
                        .Annotation("MySql:CharSet", "utf8mb3")
                },
                constraints: table =>
                {
                })
                .Annotation("MySql:CharSet", "utf8mb3")
                .Annotation("Relational:Collation", "utf8mb3_general_ci");

            migrationBuilder.CreateTable(
                name: "sys_dict_data",
                columns: table => new
                {
                    dict_code = table.Column<long>(type: "bigint", nullable: false, comment: "字典编码")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    dict_sort = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'0'", comment: "字典排序"),
                    dict_label = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, defaultValueSql: "''", comment: "字典标签", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dict_value = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, defaultValueSql: "''", comment: "字典键值", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dict_type = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, defaultValueSql: "''", comment: "字典类型", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<sbyte>(type: "tinyint", nullable: true, defaultValueSql: "'0'", comment: "状态（0正常 1停用）"),
                    create_by = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true, defaultValueSql: "''", comment: "创建者", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_time = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP", comment: "创建时间"),
                    update_by = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true, defaultValueSql: "''", comment: "更新者", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    update_time = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP", comment: "更新时间"),
                    remark = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "备注", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.dict_code);
                },
                comment: "字典数据表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "sys_dict_type",
                columns: table => new
                {
                    dict_id = table.Column<long>(type: "bigint", nullable: false, comment: "字典主键")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    dict_name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, defaultValueSql: "''", comment: "字典名称", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    dict_type = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, defaultValueSql: "''", comment: "字典类型（唯一）", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<sbyte>(type: "tinyint", nullable: true, defaultValueSql: "'0'", comment: "状态（0正常 1停用）"),
                    create_by = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true, defaultValueSql: "''", comment: "创建者", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_time = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP", comment: "创建时间"),
                    update_by = table.Column<string>(type: "varchar(64)", maxLength: 64, nullable: true, defaultValueSql: "''", comment: "更新者", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    update_time = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP", comment: "更新时间"),
                    remark = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "备注", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.dict_id);
                },
                comment: "字典类型表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "sys_file",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, comment: "主键UUID", collation: "ascii_general_ci"),
                    file_name = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "原始文件名", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    file_url = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: false, comment: "文件访问URL", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    file_path = table.Column<string>(type: "varchar(512)", maxLength: 512, nullable: false, comment: "文件物理路径", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    file_size = table.Column<long>(type: "bigint", nullable: false, comment: "文件大小（字节）"),
                    file_ext = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, defaultValueSql: "''", comment: "文件后缀", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    content_type = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, defaultValueSql: "''", comment: "文件类型", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    upload_user = table.Column<long>(type: "bigint", nullable: true, comment: "上传人ID"),
                    upload_platform = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, defaultValueSql: "'web'", comment: "上传平台", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    create_time = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "创建时间"),
                    update_time = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "更新时间")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    is_deleted = table.Column<sbyte>(type: "tinyint", nullable: false, comment: "是否删除 0=否 1=是")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                },
                comment: "文件上传记录表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "system_config",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, comment: "主键")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    config_key = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "配置键（唯一）", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    config_value = table.Column<string>(type: "text", nullable: true, comment: "配置值", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    config_type = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, defaultValueSql: "'string'", comment: "类型：string/json/banner/number", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, defaultValueSql: "''", comment: "配置名称", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    remark = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, defaultValueSql: "''", comment: "备注", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                },
                comment: "系统全局配置表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "users",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    username = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "用户名", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    password = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "密码", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    nickname = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "昵称", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    real_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "真实姓名", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_card = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: true, comment: "身份证号", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: true, comment: "手机号", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    avatar = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "头像URL", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    gender = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'0'", comment: "性别 0=未知 1=男 2=女"),
                    age = table.Column<int>(type: "int", nullable: false, comment: "年龄"),
                    name = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "姓名", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_id = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: true, comment: "用户ID(如10086888)", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    bio = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "个人简介", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    vip_level = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'0'", comment: "VIP等级 0=普通 1=普通会员 2=VIP会员 3=SVIP会员"),
                    vip_expire_date = table.Column<DateOnly>(type: "date", nullable: true, comment: "VIP过期时间"),
                    points = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'0'", comment: "积分"),
                    balance = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true, defaultValueSql: "'0.00'", comment: "余额"),
                    status = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'", comment: "状态:1=正常,0=禁用"),
                    last_login_time = table.Column<DateTime>(type: "datetime", nullable: true, comment: "最后登录时间"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    is_blocked = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'", comment: "0=正常,1=封禁"),
                    is_admin = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'", comment: "0=否,1=是"),
                    openid = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, defaultValueSql: "''", comment: "微信openid", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    Birthday = table.Column<DateTime>(type: "datetime", nullable: true, comment: "生日")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                },
                comment: "用户表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "game_circles",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    game_id = table.Column<int>(type: "int", nullable: false, comment: "游戏ID"),
                    name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "圈子名称", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "text", nullable: true, comment: "圈子描述", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    icon = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "圈子图标", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    member_count = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'0'", comment: "成员数"),
                    post_count = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'0'", comment: "动态数"),
                    status = table.Column<string>(type: "enum('active','inactive')", nullable: true, defaultValueSql: "'active'", comment: "状态", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "game_circles_ibfk_1",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "游戏圈子表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "collections",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "收藏用户ID"),
                    target_type = table.Column<string>(type: "enum('post','companion')", nullable: false, comment: "收藏类型", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    target_id = table.Column<int>(type: "int", nullable: false, comment: "目标ID"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "collections_ibfk_1",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "收藏表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "companion_applications",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, comment: "主键ID")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "用户ID"),
                    game_category = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "游戏类型", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    skill_level = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "技能等级", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    self_introduction = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: false, comment: "自我介绍", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    hourly_rate = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false, comment: "时薪"),
                    available_time = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: false, comment: "可接单时间", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, defaultValueSql: "'待审核'", comment: "状态：待审核/已通过/已拒绝", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    admin_notes = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "管理员备注", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "创建时间"),
                    updated_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "更新时间")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "FK_companion_applications_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "陪玩师申请表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "companion_requests",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "发布用户ID"),
                    game_id = table.Column<int>(type: "int", nullable: false, comment: "游戏ID"),
                    game_level = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "游戏段位/等级", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    play_time = table.Column<DateTime>(type: "datetime", nullable: false, comment: "期望陪玩时间"),
                    duration_type = table.Column<string>(type: "enum('game','hour')", nullable: true, defaultValueSql: "'game'", comment: "时长类型:局/小时", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    duration_value = table.Column<int>(type: "int", nullable: false, comment: "时长数量"),
                    budget = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true, comment: "预算价格"),
                    requirements = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "陪玩要求(多个标签用逗号分隔)", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "text", nullable: true, comment: "需求描述", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "enum('pending','matched','completed','cancelled')", nullable: true, defaultValueSql: "'pending'", comment: "状态", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    matched_companion_id = table.Column<int>(type: "int", nullable: true, comment: "匹配的陪玩师ID"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "companion_requests_ibfk_1",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "陪玩需求表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "companions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "用户ID"),
                    nickname = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "陪玩师昵称", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    real_name = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "真实姓名", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_card = table.Column<string>(type: "varchar(18)", maxLength: 18, nullable: true, comment: "身份证号", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_card_front = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "身份证正面照片", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    id_card_back = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "身份证反面照片", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    phone = table.Column<string>(type: "varchar(11)", maxLength: 11, nullable: false, comment: "联系电话", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    level = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'1'", comment: "等级"),
                    rating = table.Column<decimal>(type: "decimal(3,2)", precision: 3, scale: 2, nullable: true, defaultValueSql: "'0.00'", comment: "评分(0.00-5.00)"),
                    total_orders = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'0'", comment: "总订单数"),
                    good_review_rate = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true, defaultValueSql: "'0.00'", comment: "好评率"),
                    bio = table.Column<string>(type: "text", nullable: true, comment: "个人简介", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tags = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "标签(多个用逗号分隔)", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'0'", comment: "0=待审核,1=审核通过,2=审核拒绝"),
                    reject_reason = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "拒绝原因", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    online_status = table.Column<string>(type: "enum('online','offline','busy')", nullable: true, defaultValueSql: "'offline'", comment: "在线状态", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "companions_ibfk_1",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "陪玩师表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "coupons",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "用户ID"),
                    code = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "优惠券码", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    type = table.Column<string>(type: "enum('discount','cash','vip')", nullable: false, comment: "类型", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true, comment: "金额"),
                    discount = table.Column<decimal>(type: "decimal(5,2)", precision: 5, scale: 2, nullable: true, comment: "折扣"),
                    min_amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true, comment: "最小使用金额"),
                    max_discount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true, comment: "最大优惠金额"),
                    expire_date = table.Column<DateOnly>(type: "date", nullable: false, comment: "过期日期"),
                    status = table.Column<string>(type: "enum('unused','used','expired')", nullable: true, defaultValueSql: "'unused'", comment: "状态", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    used_time = table.Column<DateTime>(type: "datetime", nullable: true, comment: "使用时间"),
                    order_id = table.Column<int>(type: "int", nullable: true, comment: "使用的订单ID"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "coupons_ibfk_1",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "优惠券表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "drafts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "用户ID"),
                    type = table.Column<string>(type: "enum('post','request')", nullable: false, comment: "草稿类型", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "标题", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    content = table.Column<string>(type: "text", nullable: true, comment: "内容", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    images = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "图片URL", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    draft_data = table.Column<string>(type: "json", nullable: true, comment: "草稿数据(JSON格式)", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "drafts_ibfk_1",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "草稿表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "feedbacks",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "用户ID"),
                    type = table.Column<string>(type: "enum('feature','bug','order','other')", nullable: false, comment: "反馈类型", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    content = table.Column<string>(type: "text", nullable: false, comment: "反馈内容", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    images = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "截图URL", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    contact = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "联系方式", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "enum('pending','processing','resolved','closed')", nullable: true, defaultValueSql: "'pending'", comment: "处理状态", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    reply = table.Column<string>(type: "text", nullable: true, comment: "回复内容", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "feedbacks_ibfk_1",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "意见反馈表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "follows",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    follower_id = table.Column<int>(type: "int", nullable: false, comment: "关注者ID"),
                    following_id = table.Column<int>(type: "int", nullable: false, comment: "被关注者ID"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "follows_ibfk_1",
                        column: x => x.follower_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "follows_ibfk_2",
                        column: x => x.following_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "关注表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "用户ID"),
                    type = table.Column<string>(type: "enum('system','activity','order','important','warning','success')", nullable: false, comment: "通知类型", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    title = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, comment: "通知标题", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    content = table.Column<string>(type: "text", nullable: false, comment: "通知内容", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tag = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "标签", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_read = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'", comment: "是否已读"),
                    action_url = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "跳转链接", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "notifications_ibfk_1",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "系统通知表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "posts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "发布用户ID"),
                    circle_id = table.Column<int>(type: "int", nullable: true, comment: "圈子ID"),
                    content = table.Column<string>(type: "text", nullable: false, comment: "动态内容", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    images = table.Column<string>(type: "varchar(1000)", maxLength: 1000, nullable: true, comment: "图片URL(多个用逗号分隔)", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    location = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "位置", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tags = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "话题标签", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    mention_users = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "提醒的用户ID", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    visibility = table.Column<string>(type: "enum('public','followers','private')", nullable: true, defaultValueSql: "'public'", comment: "可见性", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "enum('draft','pending','approved','rejected','published')", nullable: true, defaultValueSql: "'published'", comment: "状态", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    like_count = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'0'", comment: "点赞数"),
                    comment_count = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'0'", comment: "评论数"),
                    share_count = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'0'", comment: "分享数"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    CollectCount = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'0'", comment: "收藏数")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "posts_ibfk_1",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "动态表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "power_leveling",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "发布用户ID(陪玩师)"),
                    game_id = table.Column<int>(type: "int", nullable: false, comment: "游戏ID"),
                    service_type = table.Column<string>(type: "enum('rank','star','achievement','hero','custom')", nullable: false, comment: "代练项目", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    current_rank = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "当前段位", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    target_rank = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "目标段位", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    estimated_days = table.Column<int>(type: "int", nullable: true, comment: "预计完成天数"),
                    price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false, comment: "报价"),
                    special_requirements = table.Column<string>(type: "text", nullable: true, comment: "特殊要求", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "enum('active','inactive','completed')", nullable: true, defaultValueSql: "'active'", comment: "状态", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "power_leveling_ibfk_1",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "代练服务表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "transactions",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false),
                    type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false),
                    description = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    order_id = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "FK_transactions_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "交易记录表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "user_collections",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false, comment: "主键ID")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "用户ID"),
                    title = table.Column<string>(type: "varchar(100)", maxLength: 100, nullable: true, comment: "收藏标题", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    description = table.Column<string>(type: "varchar(500)", maxLength: 500, nullable: true, comment: "收藏描述", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    category = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "收藏分类", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    item_id = table.Column<int>(type: "int", nullable: false, comment: "关联项目ID"),
                    item_type = table.Column<string>(type: "varchar(20)", maxLength: 20, nullable: false, comment: "关联项目类型(companion/post等)", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "创建时间")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "user_collections_ibfk_1",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "用户收藏表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "user_settings",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "用户ID"),
                    setting_key = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "设置键", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    setting_value = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "设置值", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "user_settings_ibfk_1",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "用户设置表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "vip_memberships",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "用户ID"),
                    level = table.Column<string>(type: "enum('silver','gold','platinum')", nullable: false, comment: "会员等级", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    start_date = table.Column<DateOnly>(type: "date", nullable: false, comment: "开始日期"),
                    end_date = table.Column<DateOnly>(type: "date", nullable: false, comment: "结束日期"),
                    status = table.Column<string>(type: "enum('active','expired','cancelled')", nullable: true, defaultValueSql: "'active'", comment: "状态", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    purchase_amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true, comment: "购买金额"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "vip_memberships_ibfk_1",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "VIP会员表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "companion_background_images",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "char(36)", nullable: false, comment: "主键UUID", collation: "ascii_general_ci"),
                    companion_id = table.Column<int>(type: "int", nullable: false, comment: "陪玩师ID（关联companions表id）"),
                    file_id = table.Column<Guid>(type: "char(36)", nullable: false, comment: "文件ID（关联sys_file表id）", collation: "ascii_general_ci"),
                    sort = table.Column<int>(type: "int", nullable: false, comment: "排序权重（越小越靠前，轮播顺序）"),
                    create_time = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "创建时间"),
                    update_time = table.Column<DateTime>(type: "datetime", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP", comment: "更新时间")
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    is_deleted = table.Column<sbyte>(type: "tinyint", nullable: false, comment: "是否删除 0=否 1=是")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "fk_companion_background_companion",
                        column: x => x.companion_id,
                        principalTable: "companions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_companion_background_file",
                        column: x => x.file_id,
                        principalTable: "sys_file",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "陪玩师背景墙轮播图关联表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "companion_games",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    companion_id = table.Column<int>(type: "int", nullable: false, comment: "陪玩师ID"),
                    game_id = table.Column<int>(type: "int", nullable: false, comment: "游戏ID"),
                    game_level = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: true, comment: "游戏段位/等级", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    service_type = table.Column<string>(type: "char(1)", fixedLength: true, maxLength: 1, nullable: true, comment: "服务类型", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    price_per_game = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true, comment: "单价(元/局)"),
                    price_per_hour = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true, comment: "单价(元/小时)"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "FK_companion_games_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "companion_games_ibfk_1",
                        column: x => x.companion_id,
                        principalTable: "companions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "陪玩师游戏技能表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "conversations",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "用户ID"),
                    companion_id = table.Column<int>(type: "int", nullable: false, comment: "陪玩师ID"),
                    last_message = table.Column<string>(type: "text", nullable: true, comment: "最后一条消息", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    last_message_time = table.Column<DateTime>(type: "datetime", nullable: true, comment: "最后消息时间"),
                    unread_count = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'0'", comment: "未读数"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "conversations_ibfk_1",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "conversations_ibfk_2",
                        column: x => x.companion_id,
                        principalTable: "companions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "会话表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "orders",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    order_no = table.Column<string>(type: "varchar(50)", maxLength: 50, nullable: false, comment: "订单号", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "下单用户ID"),
                    companion_id = table.Column<int>(type: "int", nullable: false, comment: "陪玩师ID"),
                    game_id = table.Column<int>(type: "int", nullable: false, comment: "游戏ID"),
                    service_type = table.Column<string>(type: "enum('1','2','3')", nullable: true, defaultValueSql: "'1'", comment: "服务类型", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    play_time = table.Column<DateTime>(type: "datetime", nullable: true, comment: "预约时间"),
                    duration_type = table.Column<string>(type: "enum('game','hour')", nullable: true, defaultValueSql: "'game'", comment: "时长类型", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    duration_value = table.Column<int>(type: "int", nullable: false, comment: "时长数量"),
                    unit_price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false, comment: "单价"),
                    total_price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false, comment: "总价"),
                    discount_amount = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: true, defaultValueSql: "'0.00'", comment: "优惠金额"),
                    final_price = table.Column<decimal>(type: "decimal(10,2)", precision: 10, scale: 2, nullable: false, comment: "实付金额"),
                    remark = table.Column<string>(type: "text", nullable: true, comment: "备注信息", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "varchar(8)", maxLength: 8, nullable: true, defaultValueSql: "''", comment: "订单状态 空=全部 0=待付款 1=进行中 2=已完成 4=退款/售后", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    pay_time = table.Column<DateTime>(type: "datetime", nullable: true, comment: "支付时间"),
                    start_time = table.Column<DateTime>(type: "datetime", nullable: true, comment: "服务开始时间"),
                    end_time = table.Column<DateTime>(type: "datetime", nullable: true, comment: "服务结束时间"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    updated_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "FK_orders_games_game_id",
                        column: x => x.game_id,
                        principalTable: "games",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "orders_ibfk_1",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "orders_ibfk_2",
                        column: x => x.companion_id,
                        principalTable: "companions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "订单表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "post_comments",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    post_id = table.Column<int>(type: "int", nullable: false, comment: "动态ID"),
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "评论用户ID"),
                    parent_id = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'0'", comment: "父评论ID(0为一级评论)"),
                    reply_to_user_id = table.Column<int>(type: "int", nullable: true, comment: "回复给的用户ID"),
                    content = table.Column<string>(type: "text", nullable: false, comment: "评论内容", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    status = table.Column<string>(type: "enum('normal','hidden','deleted')", nullable: true, defaultValueSql: "'normal'", comment: "状态", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP"),
                    LikeCount = table.Column<int>(type: "int", nullable: true, defaultValueSql: "'0'", comment: "点赞数")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "post_comments_ibfk_1",
                        column: x => x.post_id,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "post_comments_ibfk_2",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "动态评论表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "post_likes",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    post_id = table.Column<int>(type: "int", nullable: false, comment: "动态ID"),
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "点赞用户ID"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "post_likes_ibfk_1",
                        column: x => x.post_id,
                        principalTable: "posts",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "post_likes_ibfk_2",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "动态点赞表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "messages",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    conversation_id = table.Column<int>(type: "int", nullable: false, comment: "会话ID"),
                    sender_id = table.Column<int>(type: "int", nullable: false, comment: "发送者ID"),
                    receiver_id = table.Column<int>(type: "int", nullable: false, comment: "接收者ID"),
                    content = table.Column<string>(type: "text", nullable: false, comment: "消息内容", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    message_type = table.Column<string>(type: "enum('text','image','voice','system')", nullable: true, defaultValueSql: "'text'", comment: "消息类型", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    is_read = table.Column<bool>(type: "tinyint(1)", nullable: true, defaultValueSql: "'0'", comment: "是否已读"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "messages_ibfk_1",
                        column: x => x.conversation_id,
                        principalTable: "conversations",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "messages_ibfk_2",
                        column: x => x.sender_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "messages_ibfk_3",
                        column: x => x.receiver_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "消息表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateTable(
                name: "order_reviews",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    order_id = table.Column<int>(type: "int", nullable: false, comment: "订单ID"),
                    user_id = table.Column<int>(type: "int", nullable: false, comment: "评价用户ID"),
                    companion_id = table.Column<int>(type: "int", nullable: false, comment: "陪玩师ID"),
                    rating = table.Column<int>(type: "int", nullable: false, comment: "评分1-5"),
                    content = table.Column<string>(type: "text", nullable: true, comment: "评价内容", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    tags = table.Column<string>(type: "varchar(255)", maxLength: 255, nullable: true, comment: "评价标签", collation: "utf8mb4_0900_ai_ci")
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    created_at = table.Column<DateTime>(type: "timestamp", nullable: true, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PRIMARY", x => x.id);
                    table.ForeignKey(
                        name: "order_reviews_ibfk_1",
                        column: x => x.order_id,
                        principalTable: "orders",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "order_reviews_ibfk_2",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "order_reviews_ibfk_3",
                        column: x => x.companion_id,
                        principalTable: "companions",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                },
                comment: "订单评价表")
                .Annotation("MySql:CharSet", "utf8mb4")
                .Annotation("Relational:Collation", "utf8mb4_0900_ai_ci");

            migrationBuilder.CreateIndex(
                name: "idx_target",
                table: "collections",
                columns: new[] { "target_type", "target_id" });

            migrationBuilder.CreateIndex(
                name: "idx_user_id",
                table: "collections",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "uk_user_target",
                table: "collections",
                columns: new[] { "user_id", "target_type", "target_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_companion_applications_user_id",
                table: "companion_applications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_companion_id",
                table: "companion_background_images",
                column: "companion_id");

            migrationBuilder.CreateIndex(
                name: "idx_file_id",
                table: "companion_background_images",
                column: "file_id");

            migrationBuilder.CreateIndex(
                name: "idx_game_id",
                table: "companion_games",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "uk_companion_game",
                table: "companion_games",
                columns: new[] { "companion_id", "game_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_game_id1",
                table: "companion_requests",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "idx_play_time",
                table: "companion_requests",
                column: "play_time");

            migrationBuilder.CreateIndex(
                name: "idx_status1",
                table: "companion_requests",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "idx_user_id2",
                table: "companion_requests",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_level",
                table: "companions",
                column: "level");

            migrationBuilder.CreateIndex(
                name: "idx_rating",
                table: "companions",
                column: "rating");

            migrationBuilder.CreateIndex(
                name: "idx_status",
                table: "companions",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "idx_user_id1",
                table: "companions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_companion_id1",
                table: "conversations",
                column: "companion_id");

            migrationBuilder.CreateIndex(
                name: "idx_user_id3",
                table: "conversations",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "uk_user_companion",
                table: "conversations",
                columns: new[] { "user_id", "companion_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "code",
                table: "coupons",
                column: "code",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_expire_date",
                table: "coupons",
                column: "expire_date");

            migrationBuilder.CreateIndex(
                name: "idx_status2",
                table: "coupons",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "idx_user_id4",
                table: "coupons",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_type",
                table: "drafts",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "idx_user_id5",
                table: "drafts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_status3",
                table: "feedbacks",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "idx_type1",
                table: "feedbacks",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "idx_user_id6",
                table: "feedbacks",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_follower_id",
                table: "follows",
                column: "follower_id");

            migrationBuilder.CreateIndex(
                name: "idx_following_id",
                table: "follows",
                column: "following_id");

            migrationBuilder.CreateIndex(
                name: "uk_follower_following",
                table: "follows",
                columns: new[] { "follower_id", "following_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_game_id2",
                table: "game_circles",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "idx_status5",
                table: "game_circles",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "idx_sort_order",
                table: "games",
                column: "sort_order");

            migrationBuilder.CreateIndex(
                name: "idx_status4",
                table: "games",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "name",
                table: "games",
                column: "name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_conversation_id",
                table: "messages",
                column: "conversation_id");

            migrationBuilder.CreateIndex(
                name: "idx_created_at",
                table: "messages",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "idx_receiver_id",
                table: "messages",
                column: "receiver_id");

            migrationBuilder.CreateIndex(
                name: "idx_sender_id",
                table: "messages",
                column: "sender_id");

            migrationBuilder.CreateIndex(
                name: "idx_created_at1",
                table: "notifications",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "idx_is_read",
                table: "notifications",
                column: "is_read");

            migrationBuilder.CreateIndex(
                name: "idx_type2",
                table: "notifications",
                column: "type");

            migrationBuilder.CreateIndex(
                name: "idx_user_id7",
                table: "notifications",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_companion_id3",
                table: "order_reviews",
                column: "companion_id");

            migrationBuilder.CreateIndex(
                name: "idx_rating1",
                table: "order_reviews",
                column: "rating");

            migrationBuilder.CreateIndex(
                name: "uk_order_id",
                table: "order_reviews",
                column: "order_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "user_id",
                table: "order_reviews",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_companion_id2",
                table: "orders",
                column: "companion_id");

            migrationBuilder.CreateIndex(
                name: "idx_user_id8",
                table: "orders",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "IX_orders_game_id",
                table: "orders",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "order_no",
                table: "orders",
                column: "order_no",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_parent_id",
                table: "post_comments",
                column: "parent_id");

            migrationBuilder.CreateIndex(
                name: "idx_post_id",
                table: "post_comments",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "idx_user_id10",
                table: "post_comments",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_post_id1",
                table: "post_likes",
                column: "post_id");

            migrationBuilder.CreateIndex(
                name: "uk_post_user",
                table: "post_likes",
                columns: new[] { "post_id", "user_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "user_id1",
                table: "post_likes",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_circle_id",
                table: "posts",
                column: "circle_id");

            migrationBuilder.CreateIndex(
                name: "idx_created_at2",
                table: "posts",
                column: "created_at");

            migrationBuilder.CreateIndex(
                name: "idx_status6",
                table: "posts",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "idx_user_id9",
                table: "posts",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_game_id3",
                table: "power_leveling",
                column: "game_id");

            migrationBuilder.CreateIndex(
                name: "idx_status7",
                table: "power_leveling",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "idx_user_id11",
                table: "power_leveling",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_dict_type",
                table: "sys_dict_data",
                column: "dict_type");

            migrationBuilder.CreateIndex(
                name: "idx_dict_type1",
                table: "sys_dict_type",
                column: "dict_type",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_file_url",
                table: "sys_file",
                column: "file_url");

            migrationBuilder.CreateIndex(
                name: "idx_upload_user",
                table: "sys_file",
                column: "upload_user");

            migrationBuilder.CreateIndex(
                name: "uk_config_key",
                table: "system_config",
                column: "config_key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_transactions_user_id",
                table: "transactions",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "idx_user_id12",
                table: "user_collections",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "uk_user_item",
                table: "user_collections",
                columns: new[] { "user_id", "item_type", "item_id" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_user_id13",
                table: "user_settings",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "uk_user_key",
                table: "user_settings",
                columns: new[] { "user_id", "setting_key" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "idx_end_date",
                table: "vip_memberships",
                column: "end_date");

            migrationBuilder.CreateIndex(
                name: "idx_status8",
                table: "vip_memberships",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "idx_user_id14",
                table: "vip_memberships",
                column: "user_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "collections");

            migrationBuilder.DropTable(
                name: "companion_applications");

            migrationBuilder.DropTable(
                name: "companion_background_images");

            migrationBuilder.DropTable(
                name: "companion_games");

            migrationBuilder.DropTable(
                name: "companion_requests");

            migrationBuilder.DropTable(
                name: "coupons");

            migrationBuilder.DropTable(
                name: "drafts");

            migrationBuilder.DropTable(
                name: "feedbacks");

            migrationBuilder.DropTable(
                name: "follows");

            migrationBuilder.DropTable(
                name: "game_circles");

            migrationBuilder.DropTable(
                name: "messages");

            migrationBuilder.DropTable(
                name: "notifications");

            migrationBuilder.DropTable(
                name: "order_reviews");

            migrationBuilder.DropTable(
                name: "post_comments");

            migrationBuilder.DropTable(
                name: "post_likes");

            migrationBuilder.DropTable(
                name: "power_leveling");

            migrationBuilder.DropTable(
                name: "recover_your_data_info");

            migrationBuilder.DropTable(
                name: "sys_dict_data");

            migrationBuilder.DropTable(
                name: "sys_dict_type");

            migrationBuilder.DropTable(
                name: "system_config");

            migrationBuilder.DropTable(
                name: "transactions");

            migrationBuilder.DropTable(
                name: "user_collections");

            migrationBuilder.DropTable(
                name: "user_settings");

            migrationBuilder.DropTable(
                name: "vip_memberships");

            migrationBuilder.DropTable(
                name: "sys_file");

            migrationBuilder.DropTable(
                name: "conversations");

            migrationBuilder.DropTable(
                name: "orders");

            migrationBuilder.DropTable(
                name: "posts");

            migrationBuilder.DropTable(
                name: "games");

            migrationBuilder.DropTable(
                name: "companions");

            migrationBuilder.DropTable(
                name: "users");
        }
    }
}
