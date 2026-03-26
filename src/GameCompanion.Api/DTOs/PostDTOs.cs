namespace GameCompanion.Api.DTOs;

/// <summary>
/// 发布动态请求
/// </summary>
public class CreatePostRequest
{
    public string Content { get; set; } = string.Empty;
    public List<string>? Images { get; set; }
    public int? Game_id { get; set; }
    public int? Topic_id { get; set; }
    public int Visibility { get; set; } = 0; // 0-公开，1-仅粉丝可见，2-私密
    public string? Location { get; set; }
    public List<int>? Mentioned_users { get; set; }
}

/// <summary>
/// 动态响应数据
/// </summary>
public class PostResponse
{
    public long Post_id { get; set; }
    public int Status { get; set; } // 0-审核中，1-已发布，2-已拒绝，3-已删除
    public string Status_text { get; set; } = string.Empty;
    public int Audit_status { get; set; } // 0-待审核，1-审核通过，2-审核拒绝
    public string Audit_status_text { get; set; } = string.Empty;
    public string Content { get; set; } = string.Empty;
    public List<string> Images { get; set; } = new();
    public string Created_at { get; set; } = string.Empty;
    public string? Estimated_audit_time { get; set; }
}

/// <summary>
/// 动态详细信息
/// </summary>
public class PostDetailInfo
{
    public long Id { get; set; }
    public PostAuthorInfo User { get; set; } = new();
    public string Content { get; set; } = string.Empty;
    public List<string> Images { get; set; } = new();
    public GameSimpleInfo? Game { get; set; }
    public TopicInfo? Topic { get; set; }
    public string? Location { get; set; }
    public int Like_count { get; set; }
    public int Comment_count { get; set; }
    public int Collect_count { get; set; }
    public int Share_count { get; set; }
    public bool Is_liked { get; set; }
    public bool Is_collected { get; set; }
    public bool Is_author { get; set; }
    public string Created_at { get; set; } = string.Empty;
    public string Time_text { get; set; } = string.Empty;
    public List<CommentInfo> Hot_comments { get; set; } = new();
}

/// <summary>
/// 动态作者信息
/// </summary>
public class PostAuthorInfo
{
    public long Id { get; set; }
    public string Nickname { get; set; } = string.Empty;
    public string Avatar_url { get; set; } = string.Empty;
    public bool Is_verified { get; set; }
    public string? Bio { get; set; }
    public int? Level { get; set; } // 陪玩师等级
    public int? Followers_count { get; set; }
    public int? Following_count { get; set; }
}

/// <summary>
/// 话题信息
/// </summary>
public class TopicInfo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Participant_count { get; set; }
}

/// <summary>
/// 评论信息
/// </summary>
public class CommentInfo
{
    public long Id { get; set; }
    public PostAuthorInfo User { get; set; } = new();
    public string Content { get; set; } = string.Empty;
    public int Like_count { get; set; }
    public bool Is_liked { get; set; }
    public string Created_at { get; set; } = string.Empty;
    public List<CommentReplyInfo> Replies { get; set; } = new();
}

/// <summary>
/// 评论回复信息
/// </summary>
public class CommentReplyInfo
{
    public long Id { get; set; }
    public PostAuthorInfo User { get; set; } = new();
    public string Content { get; set; } = string.Empty;
    public string Created_at { get; set; } = string.Empty;
}

/// <summary>
/// 点赞/取消点赞请求
/// </summary>
public class LikeRequest
{
    public string Action { get; set; } = string.Empty; // like, unlike
}

/// <summary>
/// 收藏/取消收藏请求
/// </summary>
public class CollectRequest
{
    public string Action { get; set; } = string.Empty; // collect, uncollect
}

/// <summary>
/// 评论请求
/// </summary>
public class CommentRequest
{
    public string Content { get; set; } = string.Empty;
    public long? Parent_id { get; set; }
    public int? Reply_to_user_id { get; set; }
}

/// <summary>
/// 保存草稿请求
/// </summary>
public class SaveDraftRequest
{
    public string Content { get; set; } = string.Empty;
    public List<string>? Images { get; set; }
    public long? Draft_id { get; set; }
}
