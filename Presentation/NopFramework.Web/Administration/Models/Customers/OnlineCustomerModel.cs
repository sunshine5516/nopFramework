using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Models.Customers
{
    public class OnlineCustomerModel : BaseNopFrameworkEntityModel
    {
        [NopFrameworkResourceDisplayName("用户信息")]
        public string CustomerInfo { get; set; }
        [NopFrameworkResourceDisplayName("IP地址")]

        public string LastIpAddress { get; set; }
        [NopFrameworkResourceDisplayName("位置")]
        public string Location { get; set; }

        [NopFrameworkResourceDisplayName("最后活动日期")]
        public DateTime LastActivityDate { get; set; }
        [NopFrameworkResourceDisplayName("最后浏览页面")]
        public string LastVisitedPage { get; set; }
    }
}