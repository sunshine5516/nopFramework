using NopFramework.Web.Framework.Mvc.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using System.Web.Routing;

namespace Plugin.ExternalAuth.WeiXin
{
    public class RouteProvider : IRouteProvider
    {
        public int Priority
        {
            get { return 0;}
        }

        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapRoute("Plugin.ExternalAuth.WeiXin.Login",
                "Plugins/ExternalAuthWeiXin/Login",
                new { controller= "ExternalAuthWeiXin", action="Login" },
                new[] { "Plugin.ExternalAuth.WeiXin.Controllers" });
            routes.MapRoute("Plugin.ExternalAuth.WeiXin.LoginCallback",
                "Plugins/ExternalAuthWeiXin/LoginCallback",
                new { controller = "ExternalAuthWeiXin", action = "LoginCallback" },
                new[] { "Plugin.ExternalAuth.WeiXin.Controllers" });

            routes.MapRoute("Plugin.ExternalAuth.WeiXin.Register",
               "Plugins/ExternalAuthWeiXin/Register",
               new { controller = "ExternalAuthWeiXin", action = "Register" },
               new[] { "Plugin.ExternalAuth.WeiXin.Controllers" });


        }
    }
}
