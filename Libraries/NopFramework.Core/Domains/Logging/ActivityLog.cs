using NopFramework.Core.Domains.Customers;
namespace NopFramework.Core.Domains.Logging
{
    /// <summary>
    /// 活动日志记录
    /// </summary>
    public partial class ActivityLog : BaseEntity
    {
        /// <summary>
        /// 活动日志类型ID
        /// </summary>
        public int ActivityLogTypeId { get; set; }
        public int CustomerId { get; set; }
        public string Comment { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        //public DateTime CreatedOn { get; set; }
        public virtual Customer Customer { get; set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public virtual ActivityLogType ActivityLogType { get; set; }
        /// <summary>
        /// Ip地址
        /// </summary>
        public virtual string IpAddress { get; set; }
    }
}
