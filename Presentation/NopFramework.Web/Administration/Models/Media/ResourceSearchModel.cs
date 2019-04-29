using NopFramework.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Media
{
    public class ResourceSearchModel:BaseSearchModel
    {
        [NopFrameworkResourceDisplayName("词条类型")]
        [AllowHtml]
        public int ResourceType { get; set; }

        [NopFrameworkResourceDisplayName("词条类型")]
        public IList<SelectListItem> ResourceTypes { get; set; }//词条类型
    }
}