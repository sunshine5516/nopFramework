using NopFramework.Admin.Models.Vendors;
using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Inventory
{
    public class MechanicalModel : BaseNopFrameworkEntityModel
    {
        [NopFrameworkResourceDisplayName("名称")]
        [AllowHtml]
        public string Name { get; set; }
        [NopFrameworkResourceDisplayName("备注")]
        [AllowHtml]
        public string Remark { get; set; }
        [NopFrameworkResourceDisplayName("编号")]
        [AllowHtml]
        public string Code { get; set; }
        public DateTime? CreatedOn { get; set; }

        [NopFrameworkResourceDisplayName("描述")]
        [AllowHtml]
        public string FullDescription { get; set; }
        [NopFrameworkResourceDisplayName("总数")]
        [AllowHtml]
        public int TotalNumber { get; set; }

        [NopFrameworkResourceDisplayName("已用数")]
        [AllowHtml]
        public int UsedNumber { get; set; }

        [NopFrameworkResourceDisplayName("库存数")]
        [AllowHtml]
        public int InventoryNumber { get; set; }

        [NopFrameworkResourceDisplayName("供应商")]
        [UIHint("MultiSelect")]
        public int Vendor_Id { get; set; }

        public string VendorName { get; set; }

        public List<SelectListItem> AvailableVendors { get; set; }

        [NopFrameworkResourceDisplayName("类型")]
        [AllowHtml]
        public string MaterialType { get; set; }//频率
        [NopFrameworkResourceDisplayName("类型")]
        public IList<SelectListItem> MaterialTypes { get; set; }//类型


        public MechanicalModel()
        {
            //VendorModel = new VendorModel();
            this.AvailableVendors = new List<SelectListItem>();
            MaterialTypes= new List<SelectListItem>();
        }
    }
}