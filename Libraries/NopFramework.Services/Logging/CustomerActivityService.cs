using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NopFramework.Core;
using NopFramework.Core.Domains.Customers;
using NopFramework.Core.Domains.Logging;
using NopFramework.Core.Data;
using NopFramework.Services.Events;

namespace NopFramework.Services.Logging
{
    public partial class CustomerActivityService : ICustomerActivityService
    {
        #region 声明实例
        private readonly IRepository<ActivityLog> _activityLogRepository;
        private readonly IRepository<ActivityLogType> _activityLogTypeRepository;
        private readonly IWorkContext _workContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWebHelper _webHelper;
        #endregion
        #region 构造函数
        public CustomerActivityService(IRepository<ActivityLog> activityLogRepository,
            IRepository<ActivityLogType> activityLogTypeRepository,
            IWorkContext workContext, IEventPublisher eventPublisher,
            IWebHelper webHelper)
        {
            this._activityLogRepository = activityLogRepository;
            this._activityLogTypeRepository = activityLogTypeRepository;
            this._workContext = workContext;
            this._eventPublisher = eventPublisher;
            this._webHelper = webHelper;
        }
        #endregion
        #region 方法
        public void ClearAllActivities()
        {
            var logList = _activityLogRepository.Table.ToList();
            foreach (var log in logList)
            {
                _activityLogRepository.Delete(log);
            }
        }

        public void DeleteActivity(ActivityLog activityLog)
        {
            if (activityLog == null)
                throw new ArgumentNullException("activityLog");
            _activityLogRepository.Delete(activityLog);
        }

        public void DeleteActivityType(ActivityLogType activityLogType)
        {
            if (activityLogType == null)
                throw new ArgumentNullException("activityLogType");
            _activityLogTypeRepository.Delete(activityLogType);
            _eventPublisher.EntityDeleted<ActivityLogType>(activityLogType);
        }

        public ActivityLog GetActivityById(int activityLogId)
        {
            if (activityLogId == 0)
                return null;
            var query = _activityLogRepository.GetById(activityLogId);
            return query;
        }
        /// <summary>
        /// 根据ID获取日志类型
        /// </summary>
        /// <param name="activityLogTypeId"></param>
        /// <returns></returns>
        public ActivityLogType GetActivityTypeById(int activityLogTypeId)
        {
            if (activityLogTypeId == 0)
                return null;
            return _activityLogTypeRepository.GetById(activityLogTypeId);

        }

        public IPagedList<ActivityLog> GetAllActivities(DateTime? createdOnFrom = default(DateTime?),
            DateTime? createdOnTo = default(DateTime?), int? customerId = default(int?),
            int activityLogTypeId = 0, int pageIndex = 0, int pageSize = int.MaxValue, string ipAddress = null)
        {
            var query = _activityLogRepository.Table;
            if (!String.IsNullOrEmpty(ipAddress))
                query = query.Where(al => al.IpAddress.Contains(ipAddress));
            if (createdOnFrom.HasValue)
                query = query.Where(al => createdOnFrom.Value <= al.CreatedOn);
            if (createdOnTo.HasValue)
                query = query.Where(al => createdOnTo.Value >= al.CreatedOn);
            if (activityLogTypeId > 0)
                query = query.Where(al => activityLogTypeId == al.ActivityLogTypeId);
            if (customerId.HasValue)
                query = query.Where(al => customerId.Value == al.CustomerId);

            query = query.OrderByDescending(al => al.CreatedOn);
            var activityLog = new PagedList<ActivityLog>(query, pageIndex, pageSize);
            return activityLog;
        }

        public IList<ActivityLogType> GetAllActivityTypes()
        {
            return _activityLogTypeRepository.Table.OrderBy(a=>a.Name).ToList();
            
        }
        public IPagedList<ActivityLogType> GetTypesByPaged(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query= _activityLogTypeRepository.Table.OrderBy(a => a.Name).ToList();
            var activityLogTypes = new PagedList<ActivityLogType>(query, pageIndex, pageSize);
            return activityLogTypes;
        }


        /// <summary>
        /// 添加一条日志记录
        /// </summary>
        /// <param name="systemKeyword"></param>
        /// <param name="comment"></param>
        /// <param name="commentParams"></param>
        /// <returns></returns>
        public ActivityLog InsertActivity(string systemKeyword, string comment, params object[] commentParams)
        {
            return InsertActivity(_workContext.CurrentCustomer, systemKeyword, comment, commentParams);
        }
        /// <summary>
        /// 添加一条日志记录
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="systemKeyword"></param>
        /// <param name="comment"></param>
        /// <param name="commentParams"></param>
        /// <returns></returns>
        public ActivityLog InsertActivity(Customer customer, string systemKeyword, string comment, params object[] commentParams)
        {
            if (customer == null)
                return null;
            var activityTypes = GetAllActivityTypes();
            var activityType = activityTypes.ToList().Find(at => at.SystemKeyword == systemKeyword);
            if (activityType == null || !activityType.Enable)
                return null;
            comment = string.Format(comment, commentParams);
            var activity = new ActivityLog();
            activity.ActivityLogTypeId = activityType.Id;
            activity.Customer = customer;
            activity.Comment = comment;
            activity.CreatedOn = DateTime.Now;
            activity.IpAddress = _webHelper.GetCurrentIpAddress();

            _activityLogRepository.Insert(activity);
            return activity;
        }

        public void InsertActivityType(ActivityLogType activityLogType)
        {
            if (activityLogType == null)
                throw new ArgumentNullException("activityLogType");
            _activityLogTypeRepository.Insert(activityLogType);
            _eventPublisher.EntityInserted<ActivityLogType>(activityLogType);
        }

        public void UpdateActivityType(ActivityLogType activityLogType)
        {
            if (activityLogType == null)
                throw new ArgumentNullException("activityLogType");
            _activityLogTypeRepository.Update(activityLogType);
            _eventPublisher.EntityUpdated<ActivityLogType>(activityLogType);
        }
        #endregion
        #region 辅助方法

        #endregion

    }
}
