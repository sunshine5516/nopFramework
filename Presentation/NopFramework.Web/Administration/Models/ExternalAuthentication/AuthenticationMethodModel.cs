using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NopFramework.Admin.Models.ExternalAuthentication
{
    public partial class AuthenticationMethodModel: BaseNopFrameworkModel
    {
        [NopFrameworkResourceDisplayName("友好名称")]
        [AllowHtml]
        public string FriendlyName { get; set; }
        [NopFrameworkResourceDisplayName("系统名称")]
        [AllowHtml]
        public string SystemName { get; set; }
        [NopFrameworkResourceDisplayName("显示顺序")]
        [AllowHtml]
        public int DisplayOrder { get; set; }
        [NopFrameworkResourceDisplayName("是否激活")]
        [AllowHtml]
        public bool IsActive { get; set; }
        public string ConfigurationActionName { get; set; }
        public string ConfigurationControllerName { get; set; }
        public RouteValueDictionary ConfigurationRouteValues { get; set; }
    }
}