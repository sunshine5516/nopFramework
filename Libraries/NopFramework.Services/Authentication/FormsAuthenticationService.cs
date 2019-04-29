using System;
using NopFramework.Core.Domains.Customers;
using System.Web;
using NopFramework.Services.Customers;
using System.Web.Security;
namespace NopFramework.Services.Authentication
{
    public partial class FormsAuthenticationService : IAuthenticationService
    {
        #region 声明实例
        private readonly HttpContextBase _httpContext;
        private readonly ICustomerService _customerService;
        private readonly TimeSpan _expirationTimeSpan;
        private Customer _cachedCustomer;
        #endregion
        #region 构造函数
        public FormsAuthenticationService(HttpContextBase httpBase,
            ICustomerService customerService)
        {
            this._customerService = customerService;
            this._httpContext = httpBase;
            this._expirationTimeSpan = FormsAuthentication.Timeout;
        }
        #endregion
        #region 方法
        public virtual Customer GetAuthenticatedCustomer()
        {
            if (_cachedCustomer != null)
                return _cachedCustomer;
            if (_httpContext == null || _httpContext.Request == null ||
                !_httpContext.Request.IsAuthenticated ||
                !(_httpContext.User.Identity is FormsIdentity))
            {
                return null;
            }
            var formsIdentity = (FormsIdentity)_httpContext.User.Identity;
            var customer = GetAuthenticatedCustomerFromTicket(formsIdentity.Ticket);
            _cachedCustomer = customer;
            return _cachedCustomer;
            //throw new NotImplementedException();
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="customer"></param>
        /// <param name="createPersistentCookie"></param>
        public void SignIn(Customer customer, bool createPersistentCookie)
        {
            var now = DateTime.Now.ToLocalTime();
            var ticket = new FormsAuthenticationTicket(1, customer.Email,
                now, now.Add(_expirationTimeSpan),
                createPersistentCookie,
                customer.Email,
                FormsAuthentication.FormsCookiePath);
            var encryptedTicket = FormsAuthentication.Encrypt(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encryptedTicket);
            cookie.HttpOnly = true;
            if (ticket.IsPersistent)
            {
                cookie.Expires = ticket.Expiration;
            }
            cookie.Secure = FormsAuthentication.RequireSSL;
            cookie.Path = FormsAuthentication.FormsCookiePath;
            if (FormsAuthentication.CookieDomain != null)
            {
                cookie.Domain = FormsAuthentication.CookieDomain;
            }
            _httpContext.Response.Cookies.Add(cookie);
            _cachedCustomer = customer;
        }
        /// <summary>
        /// 注销
        /// </summary>
        public void SignOut()
        {
            _cachedCustomer = null;
            FormsAuthentication.SignOut();
        }
        #endregion
        #region 辅助方法
        protected virtual Customer GetAuthenticatedCustomerFromTicket(FormsAuthenticationTicket ticket)
        {
            if (ticket == null)
            {
                throw new ArgumentNullException("ticket");
            }
            var usernameOrEmail = ticket.UserData;
            if (String.IsNullOrWhiteSpace(usernameOrEmail))
                return null;
            var customer = _customerService.GetCustomerByEmail(usernameOrEmail);
            return customer;
        }
        #endregion

    }
}
