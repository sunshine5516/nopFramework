using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Vendors
{
    public class VendorModel : BaseNopFrameworkEntityModel
    {
        public VendorModel()
        {
            if (PageSize < 1)
            {
                PageSize = 5;
            }

            //Locales = new List<VendorLocalizedModel>();
            //AssociatedCustomers = new List<AssociatedCustomerInfo>();
        }

        [NopFrameworkResourceDisplayName("名称")]
        [AllowHtml]
        public string Name { get; set; }
        [NopFrameworkResourceDisplayName("电话")]
        [AllowHtml]
        public string Pheon { get; set; }

        [NopFrameworkResourceDisplayName("电子邮件")]
        [AllowHtml]
        public string Email { get; set; }

        [NopFrameworkResourceDisplayName("描述")]
        [AllowHtml]
        public string Description { get; set; }

        [UIHint("Picture")]
        [NopFrameworkResourceDisplayName("图片")]
        public int PictureId { get; set; }

        [NopFrameworkResourceDisplayName("管理员备注")]
        [AllowHtml]
        public string AdminComment { get; set; }

        [NopFrameworkResourceDisplayName("已启用")]
        public bool Active { get; set; }

        [NopFrameworkResourceDisplayName("显示顺序")]
        public int DisplayOrder { get; set; }

        public int PageSize { get; set; }

        [NopFrameworkResourceDisplayName("允许客户选择页面大小	")]
        public bool AllowCustomersToSelectPageSize { get; set; }

        [NopFrameworkResourceDisplayName("页面大小选项(逗号分隔)")]
        public string PageSizeOptions { get; set; }
    }
}