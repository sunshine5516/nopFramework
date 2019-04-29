using NopFramework.Core.Domains.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Customers
{
    /// <summary>
    /// 用户注册接口
    /// </summary>
    public partial interface ICustomerRegistrationService
    {
        /// <summary>
        ///验证用户登录
        /// </summary>
        /// <param name="userNameOrEmail"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        CustomerLoginResults ValidateCustomer(string userNameOrEmail, string password);
        /// <summary>
        /// 验证注册
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        CustomerRegistrationResult RegisterCustomer(CustomerRegistrationRequest request);
        /// <summary>
        /// 更新密码
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        ChangePasswordResult ChangePassword(ChangePasswordRequest request);
    }
}
