using NopFramework.Core.Domains.Customers;
using NopFramework.Core.Plugins;
using NopFramework.Services.Authentication.External;
using NopFramework.Services.Configuration;
using NopFramework.Services.Security;
using NopFramework.Web.Framework.Controllers;
using Plugin.ExternalAuth.WeiXin.Core;
using Plugin.ExternalAuth.WeiXin.Models;
using System.Web.Mvc;
using NopFramework.Web.Framework;
using NopFramework.Core;

namespace Plugin.ExternalAuth.WeiXin.Controllers
{
    public class ExternalAuthWeiXinController : BasePluginController
    {
        #region 声明实例
        private readonly IOAuthProviderWeiXinAuthorizer _oAuthProviderWeiXinAuthorizer;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly ExternalAuthenticationSettings _externalAuthenticationSettings;
        private readonly ISettingService _settingService;
        private readonly IPermissionService _permissionService;
        private readonly IPluginFinder _pluginFinder;
        private readonly IWorkContext _workContext;
        private readonly CustomerSettings _customerSettings;
        #endregion
        #region 构造函数
        public ExternalAuthWeiXinController(IOAuthProviderWeiXinAuthorizer oAuthProviderWeiXinAuthorizer,
            ISettingService settingService, IPermissionService permissionService,
            IPluginFinder pluginFinder, IOpenAuthenticationService openAuthenticationService,
            ExternalAuthenticationSettings externalAuthenticationSettings,IWorkContext workContext,
            CustomerSettings customerSettings)
        {
            this._oAuthProviderWeiXinAuthorizer = oAuthProviderWeiXinAuthorizer;
            this._settingService = settingService;
            this._permissionService = permissionService;
            this._pluginFinder = pluginFinder;
            this._openAuthenticationService = openAuthenticationService;
            this._externalAuthenticationSettings = externalAuthenticationSettings;
            this._workContext = workContext;
            this._customerSettings = customerSettings;
        }
        #endregion
        #region 方法
        public ActionResult Test()
        {
            return Content("Hello java world!");
        }
        [ChildActionOnly]
        [AdminAuthorize]
        public ActionResult Configure()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return Content("Access denied");
            var weixinExternalAuthSettings = _settingService.LoadSetting<WeiXinExternalAuthSettings>();
            var model = new ConfigurationModel();
            model.AppId = weixinExternalAuthSettings.AppId;
            model.AppSecret = weixinExternalAuthSettings.AppSecret;
            return View("~/Plugins/ExternalAuth.WeiXin/Views/ExternalAuthWeiXin/Configure.cshtml", model);
        }
        [HttpPost]
        [AdminAuthorize]
        [ChildActionOnly]
        public ActionResult Configure(ConfigurationModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return Content("Access denied");

            if (!ModelState.IsValid)
                return Configure();
            var weiXinExternalAuthSettings = _settingService.LoadSetting<WeiXinExternalAuthSettings>();

            //save settings
            weiXinExternalAuthSettings.AppId = model.AppId;
            weiXinExternalAuthSettings.AppSecret = model.AppSecret;
            _settingService.SaveSetting<WeiXinExternalAuthSettings>(weiXinExternalAuthSettings);

            SuccessNotification("保存成功");

            return Configure();
        }
        [ChildActionOnly]
        public ActionResult PublicInfo()
        {
            return View("~/Plugins/ExternalAuth.WeiXin/Views/ExternalAuthWeiXin/PublicInfo.cshtml");
        }
        public ActionResult Login(string returnUrl)
        {
            return LoginInternal(returnUrl, false);
        }
        public ActionResult LoginCallback(string returnUrl)
        {
            if (!_workContext.CurrentCustomer.IsRegistered())
            {
                var code = Request.QueryString["code"];
                var userInfo = _oAuthProviderWeiXinAuthorizer.GetUserInfo(code);
                if (_oAuthProviderWeiXinAuthorizer.GetUser(userInfo) == null)
                {
                    return RedirectToAction("Register"); //未注册过则调到注册页面
                }
            }
            return LoginInternal(returnUrl, true);
        }
        [NonAction]
        public ActionResult LoginInternal(string returnUrl, bool verifyResponse)
        {
            var processor =
                _openAuthenticationService.LoadExternalAuthenticationMethodBySystemName("ExternalAuth.WeiXin");
            //if (processor == null ||
            //!processor.IsMethodActive(_externalAuthenticationSettings) ||
            //!processor.PluginDescriptor.Installed)
                //throw new Exception("微信登录插件没有加载");
            var result = _oAuthProviderWeiXinAuthorizer.Authorize(returnUrl, verifyResponse);
            switch (result.AuthenticationStatus)
            {
                case OpenAuthenticationStatus.Error:
                    {
                        if(!result.Success)
                            foreach(var error in result.Errors)
                                ExternalAuthorizerHelper.AddErrorsToDisplay(error);
                        return new RedirectResult(Url.LogOn(returnUrl));
                    }
                case OpenAuthenticationStatus.AssociateOnLogon:
                    {
                        return new RedirectResult(Url.LogOn(returnUrl));
                    }
                case OpenAuthenticationStatus.AutoRegisteredEmailValidation:
                    {
                        //result
                        return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.EmailValidation });
                    }
                case OpenAuthenticationStatus.AutoRegisteredAdminApproval:
                    {
                        return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.AdminApproval });
                    }
                case OpenAuthenticationStatus.AutoRegisteredStandard:
                    {
                        return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Standard });
                    }
                default:
                    break;
            }
            if (result.Result != null) return result.Result;
            return HttpContext.Request.IsAuthenticated? new RedirectResult(!string.IsNullOrEmpty(returnUrl) ? returnUrl : "~/") : new RedirectResult(Url.LogOn(returnUrl));
        }
        #endregion
        #region 用户注册
        public ActionResult Register()
        {
            if (_customerSettings.UserRegistrationType == UserRegistrationType.Disabled)
                return RedirectToRoute("RegisterResult", new { resultId = (int)UserRegistrationType.Disabled });
            var model=new RegisterModel();
            model.EnteringEmailTwice = true;
            return View("~/Plugins/ExternalAuth.WeiXin/Views/ExternalAuthWeiXin/Register.cshtml", model);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Register(RegisterModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                return LoginInternal(returnUrl, true);
            }
            else
            {
                return View("~/Plugins/ExternalAuth.WeiXin/Views/ExternalAuthWeiXin/Register.cshtml", model);
            }
        }
        #endregion
    }
}
