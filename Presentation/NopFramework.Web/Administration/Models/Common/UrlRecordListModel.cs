using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Common
{
    public class UrlRecordListModel : BaseNopFrameworkModel
    {
        [NopFrameworkResourceDisplayName("名称")]
        [AllowHtml]
        public string SeName { get; set; }
    }
}