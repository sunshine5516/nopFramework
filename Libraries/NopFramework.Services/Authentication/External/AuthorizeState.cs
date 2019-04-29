using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NopFramework.Services.Authentication.External
{
    /// <summary>
    /// 授权状态
    /// </summary>
    public partial class AuthorizeState
    {
        #region 声明实例
        /// <summary>
        /// 错误信息
        /// </summary>
        public IList<string> Errors { get; set; }
        public OpenAuthenticationStatus AuthenticationStatus { get; private set; }
        /// <summary>
        /// Result
        /// </summary>
        public ActionResult Result { get; set; }
        #endregion
        #region 构造函数
        public AuthorizeState(string returnUrl, OpenAuthenticationStatus openAuthenticationStatus)
        {
            Errors = new List<string>();
            AuthenticationStatus = openAuthenticationStatus;
            if (AuthenticationStatus == OpenAuthenticationStatus.Authenticated)
                Result = new RedirectResult(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/");
        }
        public AuthorizeState(string returnUrl, AuthorizationResult authorizationResult)
            : this(returnUrl, authorizationResult.Status)
        {
            Errors = authorizationResult.Errors;
        }
        #endregion
        #region 方法
        public bool Success
        {
            get { return (!this.Errors.Any()); }
        }
        public void AddError(string error)
        {
            this.Errors.Add(error);
        }
        #endregion


    }
}
