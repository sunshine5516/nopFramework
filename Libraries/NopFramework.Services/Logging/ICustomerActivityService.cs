using NopFramework.Core;
using NopFramework.Core.Domains.Customers;
using NopFramework.Core.Domains.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Logging
{
    /// <summary>
    /// 用户活动日志接口
    /// </summary>
    public partial interface ICustomerActivityService
    {
        #region 日志类型
        /// <summary>
        /// 添加日志类型
        /// </summary>
        /// <param name="activityLogType">Activity log type item</param>
        void InsertActivityType(ActivityLogType activityLogType);

        /// <summary>
        /// 更新日志类型
        /// </summary>
        /// <param name="activityLogType">Activity log type item</param>
        void UpdateActivityType(ActivityLogType activityLogType);

        /// <summary>
        /// 删除日志类型
        /// </summary>
        /// <param name="activityLogType">Activity log type</param>
        void DeleteActivityType(ActivityLogType activityLogType);

        /// <summary>
        /// 获取所有日志类型
        /// </summary>
        /// <returns>Activity log type items</returns>
        IList<ActivityLogType> GetAllActivityTypes();
        IPagedList<ActivityLogType> GetTypesByPaged(int pageIndex = 0, int pageSize = int.MaxValue);
        /// <summary>
        /// 根据ID获取日志类型
        /// </summary>
        /// <param name="activityLogTypeId">Activity log type identifier</param>
        /// <returns>Activity log type item</returns>
        ActivityLogType GetActivityTypeById(int activityLogTypeId);
        #endregion
        #region 日志
        /// <summary>
        /// 添加一条日志记录
        /// </summary>
        /// <param name="systemKeyword">关键字</param>
        /// <param name="comment">内容</param>
        /// <param name="commentParams">The activity comment parameters for string.Format() function.</param>
        /// <returns>Activity log item</returns>
        ActivityLog InsertActivity(string systemKeyword, string comment, params object[] commentParams);

        /// <summary>
        /// 添加一条日志记录
        /// </summary>
        /// <param name="customer">用户</param>
        /// <param name="systemKeyword">系统关键字</param>
        /// <param name="comment">内容</param>
        /// <param name="commentParams">The activity comment parameters for string.Format() function.</param>
        /// <returns>Activity log item</returns>
        ActivityLog InsertActivity(Customer customer, string systemKeyword, string comment, params object[] commentParams);

        /// <summary>
        /// 删除日志
        /// </summary>
        /// <param name="activityLog">Activity log</param>
        void DeleteActivity(ActivityLog activityLog);

        /// <summary>
        /// 获取所有日志记录
        /// </summary>
        /// <param name="createdOnFrom">日志起始时间，null加载所有客户</param>
        /// <param name="createdOnTo">日志结束时间，null加载所有客户</param>
        /// <param name="customerId">用户ID，null加载所有客户</param>
        /// <param name="activityLogTypeId">日志类型ID</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="ipAddress">IP，null加载所有客户</param>
        /// <returns>Activity log items</returns>
        IPagedList<ActivityLog> GetAllActivities(DateTime? createdOnFrom = null,
            DateTime? createdOnTo = null, int? customerId = null, int activityLogTypeId = 0,
            int pageIndex = 0, int pageSize = int.MaxValue, string ipAddress = null);

        /// <summary>
        /// 根据ID获取日志
        /// </summary>
        /// <param name="activityLogId">Activity log identifier</param>
        /// <returns>Activity log item</returns>
        ActivityLog GetActivityById(int activityLogId);

        /// <summary>
        /// 清空日志
        /// </summary>
        void ClearAllActivities();
        #endregion
    }
}
