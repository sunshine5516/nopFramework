using NopFramework.Web.Framework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Models
{
    public class BaseSearchModel
    {
        [NopFrameworkResourceDisplayName("名称")]
        [AllowHtml]
        public string SearchName { get; set; }
        [NopFrameworkResourceDisplayName("开始日期")]
        [UIHint("DateNullable")]
        public DateTime? SearchStartDate { get; set; }
        [NopFrameworkResourceDisplayName("结束日期")]
        [UIHint("DateNullable")]
        public DateTime? SearchEndDate { get; set; }
    }
}