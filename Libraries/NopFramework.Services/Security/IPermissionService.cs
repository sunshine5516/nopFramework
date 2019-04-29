using NopFramework.Core.Domains.Customers;
using NopFramework.Core.Domains.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Security
{
    /// <summary>
    /// 权限接口
    /// </summary>
    public partial interface IPermissionService
    {
        /// <summary>
        /// 删除权限
        /// </summary>
        void DeletePermissionRecord(PermissionRecord permission);
        /// <summary>
        /// 根据ID获取权限
        /// </summary>
        /// <param name="permissionId"></param>
        /// <returns></returns>
        PermissionRecord GetPermissionRecordById(int permissionId);
        /// <summary>
        /// 根据系统名称获取权限
        /// </summary>
        /// <param name="systemName"></param>
        /// <returns></returns>
        PermissionRecord GetPermissionRecordBySystemName(string systemName);
        /// <summary>
        /// 获取所有权限
        /// </summary>
        /// <returns></returns>
        IList<PermissionRecord> GetAllPermissionRecords();
        /// <summary>
        /// 插入一条权限
        /// </summary>
        /// <param name="permission"></param>
        void InsertPermissionRecord(PermissionRecord permission);
        /// <summary>
        /// 更新权限
        /// </summary>
        /// <param name="permission"></param>
        void UpdatePermissionRecord(PermissionRecord permission);
        /// <summary>
        /// 授权许可
        /// </summary>
        /// <param name="permission"></param>
        /// <returns></returns>
        bool Authorize(PermissionRecord permission);

        bool Authorize(PermissionRecord permission, Customer customer);
        /// <summary>
        /// 授权许可Authorize permission
        /// </summary>
        /// <param name="permissionRecordSystemName">Permission record system name</param>
        /// <returns>true - authorized; otherwise, false</returns>
        bool Authorize(string permissionRecordSystemName);

        /// <summary>
        /// Authorize permission
        /// </summary>
        /// <param name="permissionRecordSystemName">Permission record system name</param>
        /// <param name="customer">Customer</param>
        /// <returns>true - authorized; otherwise, false</returns>
        bool Authorize(string permissionRecordSystemName, Customer customer);
        void InstallPermissions(IPermissionProvider permissionProvider);
        void UninstallPermissions(IPermissionProvider permissionProvider);
    }
}
