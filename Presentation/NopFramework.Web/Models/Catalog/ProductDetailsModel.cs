using NopFramework.Web.Framework.Mvc;
using NopFramework.Web.Models.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Web.Models.Catalog
{
    public partial class ProductDetailsModel : BaseNopFrameworkEntityModel
    {
        public ProductDetailsModel()
        {
            DefaultPictureModel = new PictureModel();
            PictureModels = new List<PictureModel>();
            Breadcrumb = new ProductBreadcrumbModel();
        }
        public PictureModel DefaultPictureModel { get; set; }
        public IList<PictureModel> PictureModels { get; set; }
        /// <summary>
        /// 面包屑
        /// </summary>
        public ProductBreadcrumbModel Breadcrumb { get; set; }

        public string Name { get; set; }
        public string ShortDescription { get; set; }
        public string FullDescription { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        public string Parameters { get; set; }
        public bool ShowOnHomePage { get; set; }
    }
    /// <summary>
    /// 面包屑导航类
    /// </summary>
    public partial class ProductBreadcrumbModel : BaseNopFrameworkEntityModel
    {
        public ProductBreadcrumbModel()
        {
            //CategoryBreadcrumb = new List<CategorySimpleModel>();
        }

        public bool Enabled { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductSeName { get; set; }
        //public IList<CategorySimpleModel> CategoryBreadcrumb { get; set; }
    }
}