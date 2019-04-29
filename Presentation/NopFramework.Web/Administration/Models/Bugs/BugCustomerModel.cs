using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Bugs
{
    /// <summary>
    /// 主要用在审核缺陷时，显示部门中所有人员部分
    /// </summary>
    public class BugCustomerModel: BaseNopFrameworkEntityModel
    {
        [NopFrameworkResourceDisplayName("用户名")]
        [AllowHtml]
        public string UserName { get; set; }
    }
}