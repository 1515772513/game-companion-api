namespace GameCompanion.Api.DTOs.Settings
{
    /// <summary>
    /// 系统配置DTO（用于新增/修改/返回列表）
    /// </summary>
    public class SystemConfigDto
    {
        /// <summary>
        /// 配置ID（编辑时必填，新增时自动生成）
        /// </summary>
        public int? Id { get; set; } = null;

        /// <summary>
        /// 配置键（唯一，如home_banners）
        /// </summary>
        public string ConfigKey { get; set; } = string.Empty;

        /// <summary>
        /// 配置值（支持字符串/JSON/数字）
        /// </summary>
        public string ConfigValue { get; set; } = string.Empty;

        /// <summary>
        /// 配置类型：string/json/number
        /// </summary>
        public string ConfigType { get; set; } = "string";

        /// <summary>
        /// 配置名称（如首页轮播图）
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// 备注说明
        /// </summary>
        public string Remark { get; set; } = string.Empty;

        /// <summary>
        /// 创建时间（列表返回用）
        /// </summary>
        public string CreatedAt { get; set; } = string.Empty;

        /// <summary>
        /// 更新时间（列表返回用）
        /// </summary>
        public string UpdatedAt { get; set; } = string.Empty;
    }
}