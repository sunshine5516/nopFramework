using NopFramework.Core;
using NopFramework.Core.Caching;
using NopFramework.Core.Domains.Catalog;
using NopFramework.Core.Domains.Common;
using NopFramework.Core.Domains.Customers;
using NopFramework.Core.Domains.Logging;
using NopFramework.Core.Domains.Security;
using NopFramework.Core.Events;
using NopFramework.Services.Events;
namespace NopFramework.Admin.Infrastructure.Cache
{
    public partial class ModelCacheEventConsumer : IConsumer<EntityUpdated<CustomerRole>>
        , IConsumer<EntityInserted<CustomerRole>>, IConsumer<EntityDeleted<PermissionRecord>>,
        IConsumer<EntityInserted<PermissionRecord>>, IConsumer<EntityUpdated<PermissionRecord>>,
        IConsumer<EntityInserted<ActivityLogType>>, IConsumer<EntityUpdated<ActivityLogType>>, IConsumer<EntityDeleted<ActivityLogType>>,
    IConsumer<EntityInserted<GenericAttribute>>,IConsumer<EntityUpdated<GenericAttribute>>,IConsumer<EntityDeleted<GenericAttribute>>
    {
        #region 常量
        /// <summary>
        ///所有用户角色
        /// </summary>
        private const string CUSTOMERROLES_ALL_KEY = "NopFramework.customerrole.all-{0}";//角色
        private const string PERMISSIONS_PATTERN_KEY = "NopFramework.permission.";//权限
        private const string LOGTYPE_PATTERN_KEY = "NopFramework.LogType";//日志类型
        private const string GENERICATTRIBUTE_PATTERN_KEY = "Nop.genericattribute.";
        #endregion
        #region 声明实例
        private readonly ICacheManager _cacheManager;
        private readonly IWorkContext _workContext;
        #endregion
        #region 构造函数
        public ModelCacheEventConsumer(ICacheManager cacheManage,
            IWorkContext workContext)
        {
            this._cacheManager = cacheManage;
            this._workContext = workContext;
        }
        #endregion
        #region 方法
        //用户角色
        public void HandleEvent(EntityUpdated<CustomerRole> eventMessage)
        {
            string key = string.Format(CUSTOMERROLES_ALL_KEY, false);
            _cacheManager.RemoveByPattern(key);
            //throw new NotImplementedException();
        }

        public void HandleEvent(EntityInserted<CustomerRole> eventMessage)
        {
            string key = string.Format(CUSTOMERROLES_ALL_KEY, false);
            _cacheManager.RemoveByPattern(key);
            //throw new NotImplementedException();
        }


        //权限
        public void HandleEvent(EntityInserted<PermissionRecord> eventMessage)
        {
            _cacheManager.RemoveByPattern(PERMISSIONS_PATTERN_KEY);
        }

        public void HandleEvent(EntityUpdated<PermissionRecord> eventMessage)
        {
            _cacheManager.RemoveByPattern(PERMISSIONS_PATTERN_KEY);
        }

        public void HandleEvent(EntityDeleted<PermissionRecord> eventMessage)
        {
            _cacheManager.RemoveByPattern(PERMISSIONS_PATTERN_KEY);
        }
        /// <summary>
        /// 日志类型
        /// </summary>
        /// <param name="eventMessage"></param>
        public void HandleEvent(EntityUpdated<ActivityLogType> eventMessage)
        {
            _cacheManager.RemoveByPattern(LOGTYPE_PATTERN_KEY);
        }

        public void HandleEvent(EntityDeleted<ActivityLogType> eventMessage)
        {
            _cacheManager.RemoveByPattern(LOGTYPE_PATTERN_KEY);
        }

        public void HandleEvent(EntityInserted<ActivityLogType> eventMessage)
        {
            _cacheManager.RemoveByPattern(LOGTYPE_PATTERN_KEY);
        }


        #endregion
        #region 商品图片处理事件
        public void HandleEvent(EntityInserted<GenericAttribute> eventMessage)
        {
            _cacheManager.RemoveByPattern(GENERICATTRIBUTE_PATTERN_KEY);
        }

        public void HandleEvent(EntityUpdated<GenericAttribute> eventMessage)
        {

            _cacheManager.RemoveByPattern(GENERICATTRIBUTE_PATTERN_KEY);
        }

        public void HandleEvent(EntityDeleted<GenericAttribute> eventMessage)
        {
            _cacheManager.RemoveByPattern(GENERICATTRIBUTE_PATTERN_KEY);
        }
        #endregion
    }
}