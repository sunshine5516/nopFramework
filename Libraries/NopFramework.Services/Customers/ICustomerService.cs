using NopFramework.Core;
using NopFramework.Core.Domains.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Customers
{
    /// <summary>
    /// 用户管理接口
    /// </summary>
    public partial interface ICustomerService
    {
        #region 用户管理
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <param name="createdFrom">创建日期; null加载所有记录</param>
        /// <param name="createdTo">创建日期; null加载所有记录</param>
        /// <param name="customerRoleIds">要过滤的客户角色标识符列表（至少一个匹配）; 传递空或空列表以加载所有客户;</param>
        /// <param name="email">电子邮件; null加载所有客户</param>
        /// <param name="userName">用户名; null加载所有客户</param>
        /// <param name="firstName">名字; null加载所有客户</param>
        /// <param name="lastName">名字; null加载所有客户</param>
        /// <param name="dayOfBirth">出生日期; 0加载所有客户</param>
        /// <param name="monthOfBirth">出生月份; 0加载所有客户</param>
        /// <param name="phone">电话; null加载所有客户</param>
        /// <param name="ipAddress">IP地址; null加载所有客户</param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Customer> GetAllCustomers(DateTime? createdFrom = null,
            DateTime? createdTo = null, int[] customerRoleIds = null, string email = null, string userName = null
            , string firstName = null, string lastName = null, int dayOfBirth = 0, int monthOfBirth = 0,
            string phone = null, string ipAddress = null, int pageIndex = 0, int pageSize = int.MaxValue);
        /// <summary>
        /// 根据密码格式获取用户
        /// </summary>
        /// <param name="passwordFormat"></param>
        /// <returns></returns>
        IList<Customer> GetAllCustomersByPasswordFormat(PasswordFormat passwordFormat);
        /// <summary>
        /// 获取在线用户
        /// </summary>
        /// <param name="lastActivityFrom"></param>
        /// <param name="customerRoleIds"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Customer> GetOnlineCustomers(DateTime lastActivityFrom,
            int[] customerRoleIds, int pageIndex = 0, int pageSize = int.MaxValue);
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="customer"></param>
        void DeleteCustomer(Customer customer);
        /// <summary>
        /// 根据ID获取用户
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Customer GetCustomerById(int customerId);
        IList<Customer> GetCustomersbyIds(int[] customerIds);
        Customer GetCustomerByGuid(Guid customerGuid);
        Customer GetCustomerByEmail(string email);
        Customer GetCustomerByUsername(string username);
        Customer InsertGuestCustomer();
        Customer GetCustomerBySystemName(string systemName);
        void InsertCustomer(Customer customer);
        void UpdateCustomer(Customer customer);
        #endregion
        #region 用户角色
        void DeleteCustomerRole(CustomerRole customerRole);
        void DeleteCustomerRoles(IList<CustomerRole> customerRoles);
        CustomerRole GetCustomerRoleById(int customerRoleId);
        IList<CustomerRole> GetCustomerRoleByIds(int[] customerRoleIds);
        CustomerRole GetCustomerRoleBySystemName(string systemName);
        IList<CustomerRole> GetAllCustomerRoles(bool showHidden = false);
        void InsertCustomerRole(CustomerRole customerRole);
        void UpdateCustomerRole(CustomerRole customerRole);
        IPagedList<CustomerRole> GetAllPagedList(int pageIndex = 0, int pageSize = int.MaxValue);
        #endregion
        #region 其他

        #endregion
    }
}
