using NopFramework.Services.Authentication.External;
using NopFramework.Services.Security;
using NopFramework.Admin.Extensions;
using System.Web.Mvc;
using System.Web.Routing;

namespace NopFramework.Admin.Controllers
{
    public class ExternalAuthenticationController : BaseAdminController
    {
        #region 声明实例
        private readonly IPermissionService _permissionService;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        #endregion
        #region 构造函数
        public ExternalAuthenticationController(IPermissionService permissionService,
            IOpenAuthenticationService openAuthenticationService)
        {
            this._permissionService = permissionService;
            this._openAuthenticationService = openAuthenticationService;
        }
        #endregion
        #region 控制器
        // GET: ExternalAuthentication
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ConfigureMethod(string systemName)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageExternalAuthenticationMethods))
                return AccessDeniedView();
            var eam = _openAuthenticationService.LoadExternalAuthenticationMethodBySystemName(systemName);
            if (eam == null)
                return RedirectToAction("Methods");
            var model = eam.ToModel();
            string actionName, controllerName;
            RouteValueDictionary routeValues;
            eam.GetConfigurationRoute(out actionName, out controllerName,out routeValues);
            model.ConfigurationActionName = actionName;
            model.ConfigurationControllerName = controllerName;
            model.ConfigurationRouteValues = routeValues;
            return View(model);
        }
        #endregion

    }
}