using NopFramework.Core.Plugins;
using NopFramework.Services.Authentication.External;
using NopFramework.Services.Configuration;
using System;

using System.Web.Routing;

namespace Plugin.ExternalAuth.WeiXin
{
    public class WeiXinExternalAuthMethod : BasePlugin, IExternalAuthenticationMethod
    {
        #region 声明实例
        private readonly ISettingService _settingService;
        #endregion
        #region 构造函数
        public WeiXinExternalAuthMethod(ISettingService settingService)
        {
            this._settingService = settingService;
        }
        #endregion
        #region 方法
        public void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "Configure";
            controllerName = "ExternalAuthWeiXin";
            routeValues = new RouteValueDictionary { { "Namespaces", "Plugin.ExternalAuth.WeiXin.Controllers" },{ "area",null} };
        }

        public void GetPublicInfoRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues)
        {
            actionName = "PublicInfo";
            controllerName = "ExternalAuthWeiXin";
            routeValues = new RouteValueDictionary { { "Namespaces", "Plugin.ExternalAuth.WeiXin.Controllers" }, { "area", null } };
        }
        /// <summary>
        /// 安装插件
        /// </summary>
        public override void Install()
        {
            // settings
            var settings = new WeiXinExternalAuthSettings()
            {
                AppId = "",
                AppSecret = ""
            };
            _settingService.SaveSetting(settings);
            base.Install();
        }
        /// <summary>
        /// 卸载插件
        /// </summary>
        public override void Uninstall()
        {
            _settingService.DeleteSetting<WeiXinExternalAuthSettings>();
            base.Uninstall();
        }
        #endregion

    }
}
