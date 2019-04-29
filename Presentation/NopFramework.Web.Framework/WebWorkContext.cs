using NopFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NopFramework.Core.Domains.Customers;
using System.Web;
using NopFramework.Services.Customers;
using NopFramework.Services.Authentication;
using NopFramework.Core.Domains.Localization;
using NopFramework.Services.Localization;
using NopFramework.Services.Common;

namespace NopFramework.Web.Framework
{
    public partial class WebWorkContext : IWorkContext
    {
        #region 常量
        private const string CustomerCookieName = "NopFramework.Customer";
        #endregion
        #region 声明实例
        private readonly HttpContextBase _httpContext;
        private readonly ICustomerService _customerService;
        private readonly IGenericAttributeService _genericAttributeService;
        private readonly ILanguageService _languageService;
        private readonly IAuthenticationService _authenticationService;
        private Customer _cachedCustomer;
        private Customer _originalCustomerIfImpersonated;
        private Language _cachedLanguage;
        #endregion
        #region 构造函数
        public WebWorkContext(HttpContextBase httpContext,
            ICustomerService customerService, IAuthenticationService authenticationService,
            ILanguageService languageService, IGenericAttributeService genericAttributeService)
        {
            this._httpContext = httpContext;
            this._customerService = customerService;
            this._authenticationService = authenticationService;
            this._languageService = languageService;
            this._genericAttributeService = genericAttributeService;
        }
        #endregion
        #region 属性
        public virtual Customer CurrentCustomer
        {
            get
            {
                if (_cachedCustomer != null)
                    return _cachedCustomer;
                Customer customer = null;
                if (_httpContext == null)
                {
                    customer = _customerService.GetCustomerBySystemName(SystemCustomerNames.BackgroundTask);
                }
                if (customer == null || customer.Deleted || !customer.IsActive)
                {
                    customer = _authenticationService.GetAuthenticatedCustomer();
                }

                if (customer == null || customer.Deleted || !customer.IsActive)
                {
                    var customerCookie = GetCustomerCookie();
                    if (customerCookie != null && !String.IsNullOrEmpty(customerCookie.Value))
                    {
                        Guid customerGuid;
                        if (Guid.TryParse(customerCookie.Value, out customerGuid))
                        {
                            var customerByCookie = _customerService.GetCustomerByGuid(customerGuid);
                            if (customerCookie != null)
                            {
                                if (customerByCookie != null &&!customerByCookie.IsRegistered())
                                {
                                    customer = customerByCookie;
                                }
                                
                            }
                        }
                    }
                }
                //create guest if not exists
                if (customer == null || customer.Deleted || !customer.IsActive)
                {
                    customer = _customerService.InsertGuestCustomer();
                }
                if (!customer.Deleted && customer.IsActive)
                {
                    SetCustomerCookie(customer.CustomerGuid);
                    _cachedCustomer = customer;
                }
                return _cachedCustomer;
            }
            set {
                SetCustomerCookie(value.CustomerGuid);
                _cachedCustomer = value;
            }
        }
        /// <summary>
        /// 获取或设置原始客户（如果当前的客户被假冒）
        /// </summary>
        public virtual Customer OriginalCustomerIfImpersonated
        {
            get
            {
                return _originalCustomerIfImpersonated;
            }
        }

        public virtual bool IsAdmin
        {
            get;
            set;
        }
        public virtual Language WorkingLanguage
        {
            get
            {
                if (_cachedLanguage != null)
                    return _cachedLanguage;
                //Language detectedLanguage = null;
                //if (detectedLanguage != null)
                //{
                //    if(this.CurrentCustomer.ge)
                ////}
                var allLanguages = _languageService.GetAllLanguages();
                var languageId = this.CurrentCustomer.GetAttribute<int>(SystemCustomerAttributeNames.LanguageId,
                    _genericAttributeService);
                var language = allLanguages.FirstOrDefault(x => x.Id == languageId);
                if (language == null)
                {
                    language = allLanguages.FirstOrDefault();
                }
                //cache
                _cachedLanguage = language;
                return _cachedLanguage;
            }
            set
            {
                var languageId = value != null ? value.Id : 0;
                _genericAttributeService.SaveAttribute(this.CurrentCustomer,
                    SystemCustomerAttributeNames.LanguageId,
                    languageId);

                //reset cache
                _cachedLanguage = null;
            }
        }
        #endregion
        #region 帮助方法
        protected virtual HttpCookie GetCustomerCookie()
        {
            if (_httpContext == null || _httpContext.Request == null)
                return null;
            return _httpContext.Request.Cookies[CustomerCookieName];

        }
        //protected virtual void SetCustomerCookie(Guid customerGuid)
        //{
        //    if (_httpContext != null && _httpContext.Request != null)
        //    {
        //        var cookie = new HttpCookie(CustomerCookieName);
        //        cookie.HttpOnly = true;
        //        cookie.Value = customerGuid.ToString();
        //        if (Guid.Empty == customerGuid)
        //        {
        //            cookie.Expires = DateTime.Now.AddMonths(-1);
        //        }
        //        else
        //        {
        //            int cookieExpires = 24 * 365;
        //            cookie.Expires = DateTime.Now.AddHours(cookieExpires);
        //        }
        //        _httpContext.Request.Cookies.Remove(CustomerCookieName);
        //        _httpContext.Request.Cookies.Add(cookie);
        //    }
        //}

        protected virtual void SetCustomerCookie(Guid customerGuid)
        {
            if (_httpContext != null && _httpContext.Response != null)
            {
                var cookie = new HttpCookie(CustomerCookieName);
                cookie.HttpOnly = true;
                cookie.Value = customerGuid.ToString();
                if (customerGuid == Guid.Empty)
                {
                    cookie.Expires = DateTime.Now.AddMonths(-1);
                }
                else
                {
                    int cookieExpires = 24 * 365; //TODO make configurable
                    cookie.Expires = DateTime.Now.AddHours(cookieExpires);
                }

                _httpContext.Response.Cookies.Remove(CustomerCookieName);
                _httpContext.Response.Cookies.Add(cookie);
            }
        }
        #endregion

    }
}
