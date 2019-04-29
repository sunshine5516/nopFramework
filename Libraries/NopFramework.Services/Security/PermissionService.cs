using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NopFramework.Core.Domains.Customers;
using NopFramework.Core.Domains.Security;
using NopFramework.Core.Data;
using NopFramework.Services.Customers;
using NopFramework.Core.Caching;
using NopFramework.Services.Events;
using NopFramework.Core;

namespace NopFramework.Services.Security
{
    public partial class PermissionService : IPermissionService
    {
        #region 常量
        private const string PERMISSIONS_ALLOWED_KEY = "NopFramework.permission.allowed-{0}-{1}";
        private const string PERMISSIONS_PATTERN_KEY = "NopFramework.permission.";
        #endregion
        #region 声明实例
        private readonly IRepository<PermissionRecord> _permissionRecordRepository;
        private readonly ICustomerService _customerService;
        private readonly ICacheManager _cacheManager;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWorkContext _workContext;
        #endregion
        #region 构造函数
        public PermissionService(IRepository<PermissionRecord> permissionRecordRepository,
            ICustomerService customerService, IEventPublisher eventPublisher,
            ICacheManager cacheManager, IWorkContext workContext)
        {
            this._permissionRecordRepository = permissionRecordRepository;
            this._customerService = customerService;
            this._cacheManager = cacheManager;
            this._eventPublisher = eventPublisher;
            this._workContext = workContext;
        }
        #endregion
        #region 方法
        /// <summary>
        /// 权限验证
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        public bool Authorize(PermissionRecord permission)
        {
            return Authorize(permission, _workContext.CurrentCustomer);
        }

        public bool Authorize(PermissionRecord permission, Customer customer)
        {
            if (permission == null)
                return false;
            if (customer == null)
                return false;
            return Authorize(permission.SystemName, customer);
        }

        public bool Authorize(string permissionRecordSystemName)
        {
            return Authorize(permissionRecordSystemName, _workContext.CurrentCustomer);
        }

        public bool Authorize(string permissionRecordSystemName, Customer customer)
        {
            if (string.IsNullOrEmpty(permissionRecordSystemName))
                return false;
            var customerRoles = customer.CustomerRoles.Where(cr => cr.IsActive);
            foreach (var role in customerRoles)
            {
                if (Authorize(permissionRecordSystemName, role))
                    return true;
            }
            return false;
        }
        /// <summary>
        /// 删除权限
        /// </summary>
        /// <param name="permission"></param>
        public void DeletePermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");
            _permissionRecordRepository.Delete(permission);
            _eventPublisher.EntityDeleted<PermissionRecord>(permission);
        }
        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public IList<PermissionRecord> GetAllPermissionRecords()
        {
            return _permissionRecordRepository.Table.OrderBy(p => p.Name).ToList();
        }
        /// <summary>
        /// 根据ID获取权限
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        public PermissionRecord GetPermissionRecordById(int permissionId)
        {
            if (permissionId == 0)
                return null;
            return _permissionRecordRepository.GetById(permissionId);
        }
        /// <summary>
        /// 根据系统名称获取权限
        /// </summary>
        /// <param name="systemName"></param>
        /// <returns></returns>
        public PermissionRecord GetPermissionRecordBySystemName(string systemName)
        {
            if (String.IsNullOrWhiteSpace(systemName))
                return null;
            var query = _permissionRecordRepository.Table.Where(p => p.SystemName == systemName).FirstOrDefault();
            return query;
        }
        /// <summary>
        /// 添加权限
        /// </summary>
        /// <param name="permission"></param>
        public void InsertPermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");
            _permissionRecordRepository.Insert(permission);
            _eventPublisher.EntityInserted<PermissionRecord>(permission);
        }

        public void InstallPermissions(IPermissionProvider permissionProvider)
        {
            throw new NotImplementedException();
        }

        public void UninstallPermissions(IPermissionProvider permissionProvider)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="permission"></param>
        public void UpdatePermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            _permissionRecordRepository.Update(permission);
        }
        #endregion
        #region 帮助方法
        protected virtual bool Authorize(string permissionRecordSystemName, CustomerRole customerRole)
        {
            if (String.IsNullOrEmpty(permissionRecordSystemName))
                return false;
            string key = string.Format(PERMISSIONS_ALLOWED_KEY, customerRole.Id, permissionRecordSystemName);
            return _cacheManager.Get(key, () => 
            {
                foreach (var permission in customerRole.PermissionRecords)
                {
                    if (permission.SystemName.Equals(permissionRecordSystemName, StringComparison.InvariantCultureIgnoreCase))
                        return true;                    
                }
                return false;
            });
        }
        #endregion
    }


}
