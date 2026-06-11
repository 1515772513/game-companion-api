-- ============================================================
-- 游玩 (game-companion-api) 本地测试数据
-- 字符集 utf8mb4 ; 目标库 game_companion
-- 说明: 密码字段按 AuthService.VerifyPassword 的现有逻辑存明文 "123456"
-- ============================================================
SET NAMES utf8mb4;
SET FOREIGN_KEY_CHECKS = 0;

-- 清空(可重复执行)
TRUNCATE TABLE messages;
TRUNCATE TABLE conversations;
TRUNCATE TABLE order_reviews;
TRUNCATE TABLE orders;
TRUNCATE TABLE post_comments;
TRUNCATE TABLE post_likes;
TRUNCATE TABLE posts;
TRUNCATE TABLE game_circles;
TRUNCATE TABLE companion_games;
TRUNCATE TABLE companions;
TRUNCATE TABLE follows;
TRUNCATE TABLE notifications;
TRUNCATE TABLE coupons;
TRUNCATE TABLE transactions;
TRUNCATE TABLE companion_applications;
TRUNCATE TABLE games;
TRUNCATE TABLE users;

-- ===================== 游戏 games =====================
INSERT INTO games (id, name, name_en, type, description, status, sort_order, IsActive) VALUES
(1, '王者荣耀',   'Honor of Kings', 'MOBA',  '5v5 国民手游 MOBA', '1', 1, 1),
(2, '英雄联盟',   'League of Legends', 'MOBA', '经典 5v5 端游', '1', 2, 1),
(3, '和平精英',   'Game for Peace', 'FPS',   '战术竞技射击', '1', 3, 1),
(4, '原神',       'Genshin Impact', 'RPG',   '开放世界冒险', '1', 4, 1),
(5, '永劫无间',   'Naraka Bladepoint', 'ACT', '多人武侠竞技', '1', 5, 1);

-- ===================== 用户 users =====================
-- 密码均为 123456 ; user 1 为管理员
INSERT INTO users (id, username, password, nickname, real_name, phone, avatar, gender, age, user_id, bio, vip_level, points, balance, status, is_admin) VALUES
(1, 'admin',  '123456', '超级管理员', '管理员', '13800000000', 'https://i.pravatar.cc/150?img=1', 1, 30, '10000001', '系统管理员', 9, 99999, 8888.00, 1, 1),
(2, 'zhangsan', '123456', '张三', '张三', '13800000002', 'https://i.pravatar.cc/150?img=12', 1, 24, '10000002', '热爱游戏的萌新', 1, 200, 150.00, 1, 0),
(3, 'lisi',     '123456', '李四', '李四', '13800000003', 'https://i.pravatar.cc/150?img=13', 1, 27, '10000003', '上分小能手', 2, 520, 680.50, 1, 0),
(4, 'wangwu',   '123456', '王五-小仙女', '王梅', '13800000004', 'https://i.pravatar.cc/150?img=20', 2, 22, '10000004', '温柔陪玩，带你飞', 3, 1200, 2300.00, 1, 0),
(5, 'zhaoliu',  '123456', '赵六-大神', '赵强', '13800000005', 'https://i.pravatar.cc/150?img=33', 1, 26, '10000005', '王者百星，专业代练', 4, 3000, 5600.00, 1, 0),
(6, 'sunqi',    '123456', '孙七-学姐', '孙雨', '13800000006', 'https://i.pravatar.cc/150?img=45', 2, 23, '10000006', '声音甜美的游戏搭子', 2, 800, 980.00, 1, 0),
(7, 'zhouba',   '123456', '周八', '周斌', '13800000007', 'https://i.pravatar.cc/150?img=51', 1, 29, '10000007', '佛系玩家', 0, 50, 30.00, 1, 0),
(8, 'wujiu',    '123456', '吴九-电竞王者', '吴昊', '13800000008', 'https://i.pravatar.cc/150?img=60', 1, 25, '10000008', '前职业选手', 5, 5000, 12000.00, 1, 0);

-- ===================== 陪玩 companions =====================
-- status: 0待审核 1通过 2拒绝
INSERT INTO companions (id, user_id, nickname, real_name, phone, level, rating, total_orders, good_review_rate, bio, tags, status, online_status) VALUES
(1, 4, '小仙女', '王梅', '13800000004', 5, 4.95, 320, 99.00, '温柔细心，擅长带新手上分，声音甜美', '温柔,声优,上分', 1, 'online'),
(2, 5, '大神带飞', '赵强', '13800000005', 4, 4.80, 510, 96.50, '王者百星大神，专业技术流', '技术,上分,王者', 1, 'busy'),
(3, 6, '温柔学姐', '孙雨', '13800000006', 3, 4.70, 180, 95.00, '陪你聊天陪你玩，氛围感拉满', '陪聊,娱乐,温柔', 1, 'offline'),
(4, 8, '电竞王者', '吴昊', '13800000008', 5, 4.90, 760, 98.20, '前职业选手，多游戏全能', '职业,全能,大神', 1, 'online');

-- ===================== 陪玩-游戏定价 companion_games =====================
-- service_type: 1按局 2按小时
INSERT INTO companion_games (companion_id, game_id, game_level, service_type, price_per_game, price_per_hour) VALUES
(1, 1, '荣耀王者', '1', 15.00, 50.00),
(1, 4, '满命满精', '2', NULL, 40.00),
(2, 1, '百星王者', '1', 30.00, 100.00),
(2, 2, '最强王者', '1', 28.00, 90.00),
(3, 1, '钻石',     '2', NULL, 35.00),
(3, 3, '战神',     '2', NULL, 38.00),
(4, 2, '王者',     '1', 35.00, 120.00),
(4, 5, '宗师',     '2', NULL, 110.00);

-- ===================== 游戏圈子 game_circles =====================
INSERT INTO game_circles (id, game_id, name, description, member_count, post_count, status) VALUES
(1, 1, '王者荣耀交流圈', '王者上分、攻略、组队', 12500, 3, 'active'),
(2, 2, '英雄联盟召唤师峡谷', 'LOL 玩家聚集地', 8600, 1, 'active'),
(3, 3, '和平精英战术小队', '吃鸡组队开黑', 5400, 1, 'active'),
(4, 4, '原神旅行者协会', '提瓦特大陆冒险', 9800, 0, 'active'),
(5, 5, '永劫无间江湖', '武侠竞技交流', 3200, 0, 'active');

-- ===================== 订单 orders =====================
-- status(varchar): 0待付款 1进行中 2已完成 4售后
INSERT INTO orders (id, order_no, user_id, companion_id, game_id, service_type, play_time, duration_type, duration_value, unit_price, total_price, discount_amount, final_price, remark, status, pay_time, start_time, end_time) VALUES
(1, 'GP20260601100001', 2, 1, 1, '1', '2026-06-01 20:00:00', 'game', 5,  15.00, 75.00,  5.00,  70.00, '求带上分到星耀', '2', '2026-06-01 19:55:00', '2026-06-01 20:00:00', '2026-06-01 21:30:00'),
(2, 'GP20260603100002', 3, 2, 1, '1', '2026-06-03 21:00:00', 'game', 3,  30.00, 90.00,  0.00,  90.00, '冲百星',       '2', '2026-06-03 20:50:00', '2026-06-03 21:00:00', '2026-06-03 22:10:00'),
(3, 'GP20260608100003', 2, 4, 2, '2', '2026-06-08 19:30:00', 'hour', 2, 120.00, 240.00, 20.00, 220.00, '教学局',       '2', '2026-06-08 19:20:00', '2026-06-08 19:30:00', '2026-06-08 21:30:00'),
(4, 'GP20260611100004', 7, 3, 3, '2', '2026-06-11 21:00:00', 'hour', 1,  38.00, 38.00,  0.00,  38.00, '一起吃鸡',     '1', '2026-06-11 20:55:00', '2026-06-11 21:00:00', NULL),
(5, 'GP20260611100005', 3, 1, 4, '2', '2026-06-12 20:00:00', 'hour', 2,  40.00, 80.00,  0.00,  80.00, '原神大世界',   '0', NULL, NULL, NULL);

-- ===================== 订单评价 order_reviews =====================
INSERT INTO order_reviews (order_id, user_id, companion_id, rating, content, tags) VALUES
(1, 2, 1, 5, '小姐姐声音超好听，带我赢了好多把，强烈推荐！', '技术好,声音甜,有耐心'),
(2, 3, 2, 5, '大神就是大神，三把全胜直接上分！', '技术好,效率高'),
(3, 2, 4, 4, '教学很认真，就是稍微严厉了点哈哈', '认真,专业');

-- ===================== 动态 posts =====================
INSERT INTO posts (id, user_id, circle_id, content, images, tags, visibility, status, like_count, comment_count, share_count) VALUES
(1, 2, 1, '今天终于上星耀了！感谢小仙女陪玩带飞 🎉', 'https://picsum.photos/seed/p1/600/400', '上分,王者', 'public', 'published', 56, 2, 3),
(2, 5, 1, '分享一个打野思路：前期反野，中期抓单，后期控龙。', 'https://picsum.photos/seed/p2/600/400', '攻略,打野', 'public', 'published', 128, 1, 12),
(3, 4, 1, '接陪玩单啦，温柔小姐姐在线，欢迎来撩~', 'https://picsum.photos/seed/p3/600/400', '陪玩', 'public', 'published', 89, 0, 5),
(4, 8, 2, 'LOL 新赛季上分攻略，详细版后续更新。', NULL, '攻略,LOL', 'public', 'published', 200, 1, 20),
(5, 3, 3, '求带吃鸡，萌新一枚 😂', 'https://picsum.photos/seed/p5/600/400', '吃鸡,组队', 'public', 'published', 23, 0, 1);

-- ===================== 评论 post_comments =====================
INSERT INTO post_comments (post_id, user_id, parent_id, content, status) VALUES
(1, 5, 0, '恭喜恭喜，继续冲王者！', 'normal'),
(1, 4, 0, '谢谢支持，下次再带你~', 'normal'),
(2, 3, 0, '学到了，感谢大佬分享', 'normal'),
(4, 2, 0, '蹲一个后续更新', 'normal');

-- ===================== 点赞 post_likes =====================
INSERT INTO post_likes (post_id, user_id) VALUES
(1, 3), (1, 5), (2, 2), (2, 3), (4, 2);

-- ===================== 会话 conversations & 消息 messages =====================
INSERT INTO conversations (id, user_id, companion_id, last_message, last_message_time, unread_count) VALUES
(1, 2, 1, '好的，晚上8点准时开黑~', '2026-06-01 19:50:00', 0),
(2, 3, 2, '大神什么时候有空？', '2026-06-10 18:00:00', 1);

INSERT INTO messages (conversation_id, sender_id, receiver_id, content, message_type, is_read) VALUES
(1, 2, 4, '在吗？想约今晚的陪玩', 'text', 1),
(1, 4, 2, '在的~ 几点开始呢', 'text', 1),
(1, 2, 4, '晚上8点可以吗', 'text', 1),
(1, 4, 2, '好的，晚上8点准时开黑~', 'text', 1),
(2, 3, 5, '大神什么时候有空？', 'text', 0);

-- ===================== 关注 follows =====================
INSERT INTO follows (follower_id, following_id) VALUES
(2, 4), (2, 5), (3, 5), (7, 4), (3, 8);

-- ===================== 通知 notifications =====================
INSERT INTO notifications (user_id, type, title, content, tag, is_read, action_url) VALUES
(2, 'order',  '订单已完成', '您的订单 GP20260601100001 已完成，快去评价吧', '订单', 0, '/orders/1'),
(2, 'system', '欢迎加入游玩', '感谢注册，新人专享优惠券已发放', '系统', 1, '/coupons'),
(4, 'order',  '收到新订单', '用户 张三 下单了您的陪玩服务', '订单', 0, '/orders/1');

-- ===================== 优惠券 coupons =====================
INSERT INTO coupons (user_id, code, type, amount, discount, min_amount, max_discount, expire_date, status) VALUES
(2, 'WELCOME10', 'cash',     10.00, NULL, 50.00,  NULL,  '2026-12-31', 'unused'),
(2, 'USED05',    'cash',     5.00,  NULL, 0.00,   NULL,  '2026-06-30', 'used'),
(3, 'VIP20OFF',  'discount', NULL,  20.00, 100.00, 50.00, '2026-12-31', 'unused');

-- ===================== 交易记录 transactions =====================
INSERT INTO transactions (user_id, type, amount, description, order_id, status) VALUES
(2, 'recharge', 200.00, '账户充值',           NULL,                'success'),
(2, 'consume',  -70.00, '订单消费',           'GP20260601100001',  'success'),
(5, 'income',   90.00,  '陪玩收入',           'GP20260603100002',  'success');

-- ===================== 陪玩申请 companion_applications =====================
INSERT INTO companion_applications (user_id, game_category, skill_level, self_introduction, hourly_rate, available_time, status) VALUES
(7, '王者荣耀', '钻石', '想成为一名陪玩，性格开朗有耐心', 30.00, '晚上19:00-23:00', '待审核');

SET FOREIGN_KEY_CHECKS = 1;

-- 自增起点修正
ALTER TABLE games AUTO_INCREMENT = 100;
ALTER TABLE users AUTO_INCREMENT = 100;
ALTER TABLE companions AUTO_INCREMENT = 100;
ALTER TABLE orders AUTO_INCREMENT = 100;
ALTER TABLE posts AUTO_INCREMENT = 100;
ALTER TABLE game_circles AUTO_INCREMENT = 100;
ALTER TABLE conversations AUTO_INCREMENT = 100;
