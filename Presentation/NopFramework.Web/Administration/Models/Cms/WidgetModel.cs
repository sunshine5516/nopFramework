using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NopFramework.Admin.Models.Cms
{
    public partial class WidgetModel: BaseNopFrameworkModel
    {
        [DisplayNameAttribute("友好名称")]
        [AllowHtml]
        public string FriendlyName { get; set; }
        [DisplayNameAttribute("系统中的名称")]
        [AllowHtml]
        public string SystemName { get; set; }
        [DisplayNameAttribute("序号")]
        public int DisplayOrder { get; set; }
        [DisplayNameAttribute("是否可用")]
        public bool IsActive { get; set; }
        public string ConfigurationActionName { get; set; }
        public string ConfigurationControllerName { get; set; }
        public RouteValueDictionary ConfigurationRouteValues { get; set; }
    }
}