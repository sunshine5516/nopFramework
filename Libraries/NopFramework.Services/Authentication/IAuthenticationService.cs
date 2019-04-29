using NopFramework.Core.Domains.Customers;
namespace NopFramework.Services.Authentication
{
    /// <summary>
    /// 认证服务接口
    /// </summary>
    public partial interface IAuthenticationService
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="createPersistentCookie"></param>
        void SignIn(Customer customer, bool createPersistentCookie);
        /// <summary>
        /// 注销
        /// </summary>
        void SignOut();
        /// <summary>
        /// 获得认证客户
        /// </summary>
        /// <returns></returns>
        Customer GetAuthenticatedCustomer();
    }
}
