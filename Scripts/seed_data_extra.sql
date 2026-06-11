-- ============================================================
-- 补充测试数据：其余空表
-- 目标库 game_companion ; utf8mb4
-- 注意：recover_your_data_info 疑似勒索/恢复提示表，不写入
-- ============================================================
SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

TRUNCATE TABLE companion_background_images;
TRUNCATE TABLE sys_file;
TRUNCATE TABLE collections;
TRUNCATE TABLE companion_requests;
TRUNCATE TABLE drafts;
TRUNCATE TABLE feedbacks;
TRUNCATE TABLE power_leveling;
TRUNCATE TABLE sys_dict_data;
TRUNCATE TABLE sys_dict_type;
TRUNCATE TABLE system_config;
TRUNCATE TABLE user_collections;
TRUNCATE TABLE user_settings;
TRUNCATE TABLE vip_memberships;

-- ===================== 文件 sys_file =====================
INSERT INTO sys_file (id, file_name, file_url, file_path, file_size, file_ext, content_type, upload_user, upload_platform, is_deleted) VALUES
('11111111-1111-1111-1111-111111111111', 'avatar1.jpg',   'https://cdn.example.com/files/avatar1.jpg',   '/uploads/2026/06/avatar1.jpg',   102400,  'jpg', 'image/jpeg', 4, 'web', 0),
('22222222-2222-2222-2222-222222222222', 'bg_cover1.jpg', 'https://cdn.example.com/files/bg_cover1.jpg', '/uploads/2026/06/bg_cover1.jpg', 358400,  'jpg', 'image/jpeg', 4, 'app', 0),
('33333333-3333-3333-3333-333333333333', 'bg_cover2.png', 'https://cdn.example.com/files/bg_cover2.png', '/uploads/2026/06/bg_cover2.png', 512000,  'png', 'image/png',  5, 'app', 0);

-- ===================== 陪玩背景图 companion_background_images =====================
INSERT INTO companion_background_images (id, companion_id, file_id, sort, is_deleted) VALUES
('aaaaaaaa-0000-0000-0000-000000000001', 1, '22222222-2222-2222-2222-222222222222', 1, 0),
('aaaaaaaa-0000-0000-0000-000000000002', 2, '33333333-3333-3333-3333-333333333333', 1, 0);

-- ===================== 收藏 collections =====================
INSERT INTO collections (user_id, target_type, target_id) VALUES
(2, 'post', 2),
(2, 'companion', 2),
(3, 'companion', 4),
(7, 'post', 4);

-- ===================== 找陪玩需求 companion_requests =====================
INSERT INTO companion_requests (user_id, game_id, game_level, play_time, duration_type, duration_value, budget, requirements, description, status, matched_companion_id) VALUES
(2, 1, '星耀', '2026-06-15 20:00:00', 'game', 5, 100.00, '声音甜美,有耐心', '想找个温柔的小姐姐带上分', 'pending', NULL),
(3, 2, '钻石', '2026-06-14 21:00:00', 'hour', 2, 250.00, '技术好,会教学', '求大神教学打野', 'matched', 4),
(7, 3, '黄金', '2026-06-13 22:00:00', 'hour', 1, 50.00,  '能开麦', '一起吃鸡组排', 'completed', 3);

-- ===================== 草稿 drafts =====================
INSERT INTO drafts (user_id, type, title, content, images, draft_data) VALUES
(2, 'post', '我的上分日记', '今天又赢了三把，心情不错...', 'https://picsum.photos/seed/d1/400/300', JSON_OBJECT('circle_id', 1, 'tags', JSON_ARRAY('上分','日常'))),
(3, 'request', NULL, '想找代练冲个王者', NULL, JSON_OBJECT('game_id', 1, 'target_rank', '王者'));

-- ===================== 反馈 feedbacks =====================
INSERT INTO feedbacks (user_id, type, content, contact, status, reply) VALUES
(2, 'bug',     '订单页面偶尔加载不出来', 'qq:123456', 'resolved', '已修复，感谢反馈'),
(3, 'feature', '希望增加语音试听功能',   '13800000003', 'processing', NULL),
(7, 'other',   '客服在哪里联系？',       NULL, 'pending', NULL);

-- ===================== 代练 power_leveling =====================
INSERT INTO power_leveling (user_id, game_id, service_type, current_rank, target_rank, estimated_days, price, special_requirements, status) VALUES
(2, 1, 'rank', '钻石',   '星耀',   3, 120.00, '不秒退,稳定上分', 'active'),
(3, 2, 'rank', '黄金',   '铂金',   5, 200.00, '纯手动', 'active'),
(7, 1, 'star', '星耀30', '星耀50', 7, 350.00, NULL, 'completed');

-- ===================== 字典类型 sys_dict_type =====================
INSERT INTO sys_dict_type (dict_id, dict_name, dict_type, status, create_by, remark) VALUES
(1, '订单状态', 'order_status',      0, 'admin', '订单状态列表'),
(2, '陪玩审核状态', 'companion_status', 0, 'admin', '陪玩审核状态'),
(3, '服务类型', 'service_type',      0, 'admin', '陪玩服务计价方式'),
(4, '在线状态', 'online_status',     0, 'admin', '陪玩在线状态');

-- ===================== 字典数据 sys_dict_data =====================
INSERT INTO sys_dict_data (dict_code, dict_sort, dict_label, dict_value, dict_type, status, create_by) VALUES
(1, 1, '待付款', '0', 'order_status', 0, 'admin'),
(2, 2, '进行中', '1', 'order_status', 0, 'admin'),
(3, 3, '已完成', '2', 'order_status', 0, 'admin'),
(4, 4, '售后', '4', 'order_status', 0, 'admin'),
(5, 1, '待审核', '0', 'companion_status', 0, 'admin'),
(6, 2, '审核通过', '1', 'companion_status', 0, 'admin'),
(7, 3, '审核拒绝', '2', 'companion_status', 0, 'admin'),
(8, 1, '按局', '1', 'service_type', 0, 'admin'),
(9, 2, '按小时', '2', 'service_type', 0, 'admin'),
(10, 1, '在线', 'online', 'online_status', 0, 'admin'),
(11, 2, '离线', 'offline', 'online_status', 0, 'admin'),
(12, 3, '忙碌', 'busy', 'online_status', 0, 'admin');

-- ===================== 系统配置 system_config =====================
INSERT INTO system_config (id, config_key, config_value, config_type, name, remark) VALUES
(1, 'site_name',      '游玩陪玩平台', 'string', '站点名称', '应用标题'),
(2, 'service_phone',  '400-888-8888', 'string', '客服电话', '客服联系方式'),
(3, 'min_withdraw',   '10',           'number', '最低提现金额', '单位:元'),
(4, 'commission_rate','0.15',         'number', '平台抽成比例', '0~1'),
(5, 'home_banner',    '[{"img":"https://picsum.photos/seed/b1/750/300","link":"/activity/1"},{"img":"https://picsum.photos/seed/b2/750/300","link":"/activity/2"}]', 'banner', '首页轮播图', '首页banner配置'),
(6, 'about_us',       '游玩是一个专业的游戏陪玩社交平台。', 'string', '关于我们', '关于页面文案');

-- ===================== 用户收藏夹 user_collections =====================
INSERT INTO user_collections (user_id, title, description, category, item_id, item_type) VALUES
(2, '心仪陪玩', '收藏的优质陪玩', 'companion', 1, 'companion'),
(2, '攻略合集', '好用的上分攻略', 'post', 2, 'post'),
(3, '大神列表', '技术流陪玩', 'companion', 4, 'companion');

-- ===================== 用户设置 user_settings =====================
INSERT INTO user_settings (user_id, setting_key, setting_value) VALUES
(2, 'notify_order',  'true'),
(2, 'notify_message','true'),
(2, 'theme',         'dark'),
(3, 'notify_order',  'false'),
(3, 'language',      'zh-CN'),
(4, 'auto_accept',   'false');

-- ===================== VIP 会员 vip_memberships =====================
INSERT INTO vip_memberships (user_id, level, start_date, end_date, status, purchase_amount) VALUES
(4, 'gold',     '2026-01-01', '2026-12-31', 'active',  299.00),
(5, 'platinum', '2026-03-01', '2027-02-28', 'active',  599.00),
(8, 'platinum', '2026-05-01', '2027-04-30', 'active',  599.00),
(2, 'silver',   '2025-06-01', '2026-05-31', 'expired', 99.00);

SET FOREIGN_KEY_CHECKS = 1;

ALTER TABLE collections AUTO_INCREMENT = 100;
ALTER TABLE companion_requests AUTO_INCREMENT = 100;
ALTER TABLE drafts AUTO_INCREMENT = 100;
ALTER TABLE feedbacks AUTO_INCREMENT = 100;
ALTER TABLE power_leveling AUTO_INCREMENT = 100;
ALTER TABLE system_config AUTO_INCREMENT = 100;
ALTER TABLE user_collections AUTO_INCREMENT = 100;
ALTER TABLE user_settings AUTO_INCREMENT = 100;
ALTER TABLE vip_memberships AUTO_INCREMENT = 100;
