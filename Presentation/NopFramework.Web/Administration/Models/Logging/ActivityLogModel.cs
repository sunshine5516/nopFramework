using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Models.Logging
{
    public class ActivityLogModel: BaseNopFrameworkEntityModel
    {
        [NopFrameworkResourceDisplayName("用户Id")]
        public int CustomerId { get; set; }
        [NopFrameworkResourceDisplayName("内容")]
        public string Comment { get; set; }
        [NopFrameworkResourceDisplayName("开始日期")]
        public DateTime CreatedOn { get; set; }
        [NopFrameworkResourceDisplayName("日志类型")]
        public string ActivityLogTypeName { get; set; }
        [NopFrameworkResourceDisplayName("Ip")]
        public virtual string IpAddress { get; set; }
        [NopFrameworkResourceDisplayName("用户邮箱")]
        public string CustomerEmail { get; set; }
    }
}