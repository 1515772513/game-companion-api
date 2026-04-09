using GameCompanion.Api.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace GameCompanion.Api.DTOs.Companion
{
  /// <summary>
  /// 前端陪玩师列表查询请求
  /// </summary>
  public class CompanionListFrontRequest
  {
    /// <summary>
    /// 页码
    /// </summary>
    [FromQuery(Name = "page")]
    public int Page { get; set; } = 1;

    /// <summary>
    /// 每页数量
    /// </summary>
    [FromQuery(Name = "page_size")]
    public int PageSize { get; set; } = 10;

    /// <summary>
    /// 搜索关键词：昵称/真实姓名
    /// </summary>
    [FromQuery(Name = "keyword")]
    public string? Keyword { get; set; }

    /// <summary>
    /// 游戏ID
    /// </summary>
    [FromQuery(Name = "game_id")]
    public int? GameId { get; set; }

    /// <summary>
    /// 服务类型：voice/video/game
    /// </summary>
    [FromQuery(Name = "service_type")]
    public string? ServiceType { get; set; }

    /// <summary>
    /// 等级
    /// </summary>
    [FromQuery(Name = "level")]
    public int? Level { get; set; }

    /// <summary>
    /// 排序：default/price_asc/price_desc
    /// </summary>
    [FromQuery(Name = "sort")]
    public string? Sort { get; set; }

    /// <summary>
    /// 在线状态：0-全部 1-仅在线
    /// </summary>
    [FromQuery(Name = "online_status")]
    public int? OnlineStatus { get; set; }
  }

  /// <summary>
  /// 前端陪玩师列表项响应
  /// </summary>
  public class CompanionItemFrontResponse
  {
    /// <summary>
    /// 陪玩师ID
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// 昵称
    /// </summary>
    public string Nickname { get; set; } = string.Empty;

    /// <summary>
    /// 头像
    /// </summary>
    public string AvatarUrl { get; set; } = string.Empty;

    /// <summary>
    /// 等级
    /// </summary>
    public int? Level { get; set; } = null;

    /// <summary>
    /// 等级名称
    /// </summary>
    public string LevelName { get; set; } = string.Empty;

    /// <summary>
    /// 标签列表
    /// </summary>
    public List<string> Tags { get; set; } = new();

    /// <summary>
    /// 价格
    /// </summary>
    public decimal Price { get; set; }

    /// <summary>
    /// 价格单位
    /// </summary>
    public string PriceUnit { get; set; } = "局";

    /// <summary>
    /// 评分
    /// </summary>
    public decimal Rating { get; set; }

    /// <summary>
    /// 订单数
    /// </summary>
    public int OrderCount { get; set; }

    /// <summary>
    /// 在线状态 1-在线 0-离线
    /// </summary>
    public int OnlineStatus { get; set; }
  }

  /// <summary>
  /// 前端陪玩师列表分页响应
  /// </summary>
  public class CompanionListFrontResponse
  {
    /// <summary>
    /// 列表数据
    /// </summary>
    public List<CompanionItemFrontResponse> Items { get; set; } = new();

    /// <summary>
    /// 分页信息
    /// </summary>
    public cPaginationInfo Pagination { get; set; } = new();
  }

  /// <summary>
  /// 分页信息
  /// </summary>
  public class cPaginationInfo
  {
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int Total { get; set; }
    public bool HasMore { get; set; }
  }
}