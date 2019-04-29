using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Catalog
{
    public class ProductModel : BaseNopFrameworkEntityModel
    {
        [NopFrameworkResourceDisplayName("名称")]
        [AllowHtml]
        public string Name { get; set; }
        [NopFrameworkResourceDisplayName("摘要")]
        [AllowHtml]
        public string ShortDescription { get; set; }
        [NopFrameworkResourceDisplayName("参数")]
        [AllowHtml]
        public string Parameters { get; set; }
        [NopFrameworkResourceDisplayName("详细描述")]
        [AllowHtml]
        public string FullDescription { get; set; }
        [NopFrameworkResourceDisplayName("显示在首页")]
        public bool ShowOnHomePage { get; set; }
        [NopFrameworkResourceDisplayName("是否发布")]
        public bool IsPublished { get; set; }
        [NopFrameworkResourceDisplayName("友好名称")]
        [AllowHtml]
        public string SeName { get; set; }//
        public ProductPictureModel AddPictureModel { get; set; }
        public List<ProductPictureModel> ProductPictureModels { get; set; }
        public ProductModel()
        {
            AddPictureModel = new ProductPictureModel();
            ProductPictureModels = new List<ProductPictureModel>();
        }
    }
}