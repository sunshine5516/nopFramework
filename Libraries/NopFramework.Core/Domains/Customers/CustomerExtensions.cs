using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains.Customers
{
    public static class CustomerExtensions
    {
        #region Customer role
        public static bool IsInCustomerRole(this Customer customer,
           string customerRoleSystemName, bool onlyActiveCustomerRoles = true)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            if (String.IsNullOrEmpty(customerRoleSystemName))
                throw new ArgumentNullException("customerRoleSystemName");
            var result = customer.CustomerRoles.FirstOrDefault(cr => (!onlyActiveCustomerRoles || cr.IsActive)
              && cr.SystemName == customerRoleSystemName) != null;
            return result;
        }
        /// <summary>
        /// 用户是否已经注册
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="onlyActiveCustomerRoles">活跃状态的用户</param>
        /// <returns></returns>
        public static bool IsRegistered(this Customer customer, bool onlyActiveCustomerRoles = true)
        {
            return IsInCustomerRole(customer, SystemCustomerRoleNames.Registered, onlyActiveCustomerRoles);
        }
        /// <summary>
        /// 是否是管理员
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="onlyActiveCustomerRoles"></param>
        /// <returns></returns>
        public static bool IsAdmin(this Customer customer, bool onlyActiveCustomerRoles = true)
        {
            return IsInCustomerRole(customer, SystemCustomerRoleNames.Administrators, onlyActiveCustomerRoles);
        }

        /// <summary>
        /// 是否是游客
        /// </summary>
        /// <param name="customer">用户</param>
        /// <param name="onlyActiveCustomerRoles">活跃状态的用户</param>
        /// <returns>Result</returns>
        public static bool IsGuest(this Customer customer, bool onlyActiveCustomerRoles = true)
        {
            return IsInCustomerRole(customer, SystemCustomerRoleNames.Guests, onlyActiveCustomerRoles);
        }
        #endregion
    }
}
