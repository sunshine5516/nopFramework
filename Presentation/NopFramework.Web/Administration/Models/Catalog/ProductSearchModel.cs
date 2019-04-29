using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Catalog
{
    public class ProductSearchModel: BaseNopFrameworkEntityModel
    {
        [NopFrameworkResourceDisplayName("名称")]
        [AllowHtml]
        public string SearchProductName { get; set; }

        [NopFrameworkResourceDisplayName("类型")]
        public int SearchCategoryId { get; set; }

        [NopFrameworkResourceDisplayName("开始日期")]
        [UIHint("DateNullable")]
        public DateTime? SearchStartDate { get; set; }
        [NopFrameworkResourceDisplayName("结束日期")]
        [UIHint("DateNullable")]
        public DateTime? SearchEndDate { get; set; }
        public ProductSearchModel()
        {
            AvailableCategories = new List<SelectListItem>();
        }

        public IList<SelectListItem> AvailableCategories { get; set; }
    }
}