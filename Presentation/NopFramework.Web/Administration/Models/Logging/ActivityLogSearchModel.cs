using NopFramework.Web.Framework.Mvc;
using NopFramework.Web.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Logging
{
    public class ActivityLogSearchModel : BaseNopFrameworkEntityModel
    {
        [NopFrameworkResourceDisplayName("开始日期")]
        [UIHint("DateNullable")]
        public DateTime? SearchStartDate { get; set; }
        [NopFrameworkResourceDisplayName("结束日期")]
        [UIHint("DateNullable")]
        public DateTime? SearchEndDate { get; set; }
        [NopFrameworkResourceDisplayName("IP")]
        public string IpAddress { get; set; }
        [NopFrameworkResourceDisplayName("日志类型")]
        public IList<SelectListItem> ActivityLogTypes { get; set; }
        [NopFrameworkResourceDisplayName("日志类型Id")]
        public int ActivityLogTypeId { get; set; }
        public ActivityLogSearchModel()
        {
            ActivityLogTypes = new List<SelectListItem>();
        }
    }
}