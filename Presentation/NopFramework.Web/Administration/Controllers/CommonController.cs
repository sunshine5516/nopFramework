using NopFramework.Admin.Extensions;
using NopFramework.Admin.Models.Common;
using NopFramework.Core;
using NopFramework.Core.Caching;
using NopFramework.Core.Infrastructure;
using NopFramework.Services.Localization;
using NopFramework.Services.Security;
using NopFramework.Web.Framework.Kendoui;
using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Controllers
{
    public class CommonController : BaseAdminController
    {
        #region 声明实例
        private readonly IPermissionService _permissionService;
        private readonly IWebHelper _webHelper;
        private readonly HttpContextBase _httpContext;
        private readonly IWorkContext _workContext;
        private readonly ILanguageService _languageService;
        #endregion
        #region 构造函数
        public CommonController(IPermissionService permissionService,
            IWebHelper webHelper, HttpContextBase httpContext,IWorkContext workContext,
            ILanguageService languageService)
        {
            this._permissionService = permissionService;
            this._webHelper = webHelper;
            this._httpContext = httpContext;
            this._workContext = workContext;
            this._languageService = languageService;
        }
        #endregion
        // GET: Common
        public ActionResult Index()
        {
            return View();
        }
        #region SeName友好名称管理

        public ActionResult SeNames()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMaintenance))
                return AccessDeniedView();
            var model = new UrlRecordListModel();
            return View(model);
        }
        [HttpPost]
        public ActionResult SeNames(DataSourceRequest command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageMaintenance))
                return AccessDeniedView();
            return View();
        }
        #endregion

        #region 方法
        /// <summary>
        /// 清清除缓存
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult ClearCache(string returnUrl = "")
        {
            var cacheManager = EngineContext.Current.ContainerManager.Resolve<ICacheManager>("nop_cache_static");
            cacheManager.Clear();
            //home page
            if (String.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            return Redirect(returnUrl);
        }
        public ActionResult SystemInfo()
        {
            //if (!_permissionService.Authorize(StandardPermissionProvider.ManageMaintenance))
            //    return AccessDeniedView();
            var model = new SystemInfoModel();
            model.SysVersion = SysVersion.CurrentVersion;
            try
            {
                model.OperationSystem = Environment.OSVersion.VersionString;
            }
            catch (Exception) { }
            try
            {
                model.AspNetInfo = RuntimeEnvironment.GetSystemVersion();
            }
            catch (Exception) { }
            try
            {
                model.IsFullTrust = AppDomain.CurrentDomain.IsFullyTrusted.ToString();
            }
            catch (Exception) { }
            model.ServerTimeZone = TimeZone.CurrentTimeZone.StandardName;
            model.ServerLocalTime = DateTime.Now;
            model.UtcTime = DateTime.UtcNow;
            model.CurrentUserTime = DateTime.Now;
            model.HttpHost = _webHelper.ServerVariables("HTTP_HOST");
            foreach (var key in _httpContext.Request.ServerVariables.AllKeys)
            {
                model.ServerVariables.Add(new SystemInfoModel.ServerVariableModel
                {
                    Name = key,
                    Value = _httpContext.Request.ServerVariables[key]
                });
            }
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                model.LoadedAssemblies.Add(new SystemInfoModel.LoadedAssembly
                {
                    FullName = assembly.FullName,

                });
            }
            return View(model);
        }
        [ChildActionOnly]
        public ActionResult LanguageSelector()
        {
            var model = new LanguageSelectorModel();
            model.CurrentLanguage = _workContext.WorkingLanguage.ToModel();
            model.AvailableLanguages = _languageService.GetAllLanguages()
                .Select(x => x.ToModel()).ToList();
            return PartialView(model);
        }
        public ActionResult SetLanguage(int langid, string returnUrl = "")
        {
            var language = _languageService.GetEntityById(langid);
            if (language != null)
            {
                _workContext.WorkingLanguage = language;
            }
            //home page
            if (String.IsNullOrEmpty(returnUrl))
                returnUrl = Url.Action("Index", "Home", new { area = "Admin" });
            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            return Redirect(returnUrl);
        }
        #endregion
    }
}