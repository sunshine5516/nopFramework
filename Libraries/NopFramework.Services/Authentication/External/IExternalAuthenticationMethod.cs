using NopFramework.Core.Plugins;
using System.Web.Routing;

namespace NopFramework.Services.Authentication.External
{
    /// <summary>
    /// 创建外部身份验证方法的接口
    /// </summary>
    public interface IExternalAuthenticationMethod:IPlugin
    {
        /// <summary>
        /// 获取插件配置的路由
        /// </summary>
        /// <param name="actionName"></param>
        /// <param name="controllerName"></param>
        /// <param name="routeValues"></param>
        void GetConfigurationRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues);
        void GetPublicInfoRoute(out string actionName, out string controllerName, out RouteValueDictionary routeValues);
    }
}
