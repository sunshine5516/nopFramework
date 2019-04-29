using NopFramework.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Inventory
{
    public class MechanicalSearchModel : BaseSearchModel
    {
        [NopFrameworkResourceDisplayName("资料类型")]
        [AllowHtml]
        public int MechanicalType { get; set; }

        [NopFrameworkResourceDisplayName("资料类型")]
        public IList<SelectListItem> MechanicalTypes { get; set; }//词条类型
    }
}