namespace NopFramework.Core.Domains.Logging
{
    /// <summary>
    /// 日志类型
    /// </summary>
    public partial class ActivityLogType: BaseEntity
    {
        /// <summary>
        /// 系统关键字
        /// </summary>
        public string SystemKeyword { get; set; }
        public string Name { get; set; }
        public bool Enable { get; set; }
    }
    /// <summary>
    /// 日志类型枚举类
    /// </summary>
    public enum LogType
    {
        添加用户,
        添加角色,
        添加日志类型,
        编辑用户,
        编辑角色,
        编辑日志类型,
        删除用户,
        删除角色,
        删除日志,
        删除日志类型,
        清空日志,
        联系衡芯

    }
}
