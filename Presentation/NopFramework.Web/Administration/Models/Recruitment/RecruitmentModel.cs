using NopFramework.Admin.Models.Customers;
using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Recruitment
{
    public class RecruitmentModel: BaseNopFrameworkEntityModel
    {
        [NopFrameworkResourceDisplayName("名称")]
        [AllowHtml]
        public string Name { get; set; }
        [NopFrameworkResourceDisplayName("摘要")]
        [AllowHtml]
        public string ShortDescription { get; set; }
        [NopFrameworkResourceDisplayName("详细描述")]
        [AllowHtml]
        public string FullDescription { get; set; }
        [NopFrameworkResourceDisplayName("是否可用")]
        [AllowHtml]
        public bool IsActive { get; set; }//
        public CustomerModel customer { get; set; }
        public RecruitmentModel()
        {
            customer = new CustomerModel();
        }
    }
}