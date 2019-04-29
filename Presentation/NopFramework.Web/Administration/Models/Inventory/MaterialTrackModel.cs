using FluentValidation.Attributes;
using NopFramework.Admin.Validators.Inventory;
using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Inventory
{
    [Validator(typeof(MaterialTrackValidator))]
    public class MaterialTrackModel: BaseNopFrameworkEntityModel
    {
        [NopFrameworkResourceDisplayName("日期")]
        [AllowHtml]
        public DateTime Date { get; set; }
        [NopFrameworkResourceDisplayName("名称")]
        [AllowHtml]
        public string ShortTime { get; set; }
        [NopFrameworkResourceDisplayName("操作人员")]
        [AllowHtml]
        public string Operator { get; set; }
        [NopFrameworkResourceDisplayName("类型")]
        [AllowHtml]
        public string MaterialType { get; set; }
        [NopFrameworkResourceDisplayName("数量")]
        [AllowHtml]
        public int Number { get; set; }
        [NopFrameworkResourceDisplayName("描述")]
        [AllowHtml]
        public string Description { get; set; }
        [NopFrameworkResourceDisplayName("是否删除")]
        [AllowHtml]
        public bool IsDeleted { get; set; }
        public int MaterialId { get; set; }
        public IList<SelectListItem> MaterialTypes { get; set; }//库存操作类型
        public MaterialTrackModel()
        {
            MaterialTypes = new List<SelectListItem>();
        }
    }
}