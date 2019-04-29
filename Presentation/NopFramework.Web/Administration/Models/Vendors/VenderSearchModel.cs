using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Vendors
{
    public class VenderSearchModel: BaseNopFrameworkModel
    {
        [NopFrameworkResourceDisplayName("名称")]
        [AllowHtml]
        public string SearchName { get; set; }
        [NopFrameworkResourceDisplayName("电话")]
        [AllowHtml]
        public string PhoneNumber { get; set; }
    }
}