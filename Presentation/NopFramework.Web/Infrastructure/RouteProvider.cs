using NopFramework.Web.Framework.Localization;
using NopFramework.Web.Framework.Mvc.Routes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Routing;

namespace NopFramework.Web.Infrastructure
{
    public class RouteProvider : IRouteProvider
    {
        public int Priority
        {
            get
            {
                return 0;
            }
        }
        /// <summary>
        /// 注册路由
        /// </summary>
        /// <param name="routes"></param>
        public void RegisterRoutes(RouteCollection routes)
        {
            routes.MapLocalizedRoute(
                "HomePage",
                "",
                new { controller = "Home", action = "Index" },
                new[] { "NopFramework.Web.Controllers" }
                );
            routes.MapLocalizedRoute("RegisterResult",
                 "registerresult/{resultId}",
                  new { controller = "Customer", action = "RegisterResult" },
                            new { resultId = @"\d+" },
                            new[] { "NopFramework.Web.Controllers" });
            routes.MapLocalizedRoute("Logon",
                "logon/",
                new { controller = "Customer", action = "Logon" },
                new[] { "NopFramework.Web.Controllers" });
            //新闻
            routes.MapLocalizedRoute("NewsItems",
                "news",
                new { controller = "News", action = "List" },
                new[] { "NopFramework.Web.Controllers" });
            //联系我们
            routes.MapLocalizedRoute("ContactUs",
                "ContactUs",
                new { controller = "Contact", action = "ContactUs" },
                new[] { "NopFramework.Web.Controllers" });
            //关于我们
            routes.MapLocalizedRoute("AboutUs",
                "AboutUs",
                new { controller = "Introduce", action = "AboutUs" },
                new[] { "NopFramework.Web.Controllers" });
            //企业文化
            routes.MapLocalizedRoute("CompCulture",
                "CompCulture",
                new { controller = "Introduce", action = "CompCulture" },
                new[] { "NopFramework.Web.Controllers" });
            //领军人物
            routes.MapLocalizedRoute("Leadship",
                "Leadship",
                new { controller = "Introduce", action = "Leadship" },
                new[] { "NopFramework.Web.Controllers" });
            //产品
            routes.MapLocalizedRoute("Product",
                "Product",
                new { controller = "Product", action = "Index" },
                new[] { "NopFramework.Web.Controllers" });

            //服务与支持
            routes.MapLocalizedRoute("Support",
                "Support",
                new { controller = "Support", action = "LoadAll" },
                new[] { "NopFramework.Web.Controllers" });

            //routes.MapLocalizedRoute(
            //    "Category",
            //    "{SeName}",
            //    new { controller = "Home", action = "Index" },
            //    new[] { "NopFramework.Web.Controllers" });
        }
    }
}