using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Customers
{
    public class CustomerRoleModel : BaseNopFrameworkEntityModel
    {
        [NopFrameworkResourceDisplayName("名称")]
        [AllowHtml]
        public string Name { get; set; }
        [NopFrameworkResourceDisplayName("系统名称")]
        [AllowHtml]
        public string SystemName { get; set; }
        [NopFrameworkResourceDisplayName("已启用")]
        public bool IsActive { get; set; }
        [NopFrameworkResourceDisplayName("是系统角色")]        
        public bool IsSystemRole { get; set; }

    }
}