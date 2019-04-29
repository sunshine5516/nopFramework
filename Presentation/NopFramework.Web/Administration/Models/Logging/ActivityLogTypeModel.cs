using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Models.Logging
{
    public class ActivityLogTypeModel : BaseNopFrameworkEntityModel
    {
        [NopFrameworkResourceDisplayName("名称")]
        public string Name { get; set; }
        [NopFrameworkResourceDisplayName("是否启用")]
        public bool Enable { get; set; }
    }
}