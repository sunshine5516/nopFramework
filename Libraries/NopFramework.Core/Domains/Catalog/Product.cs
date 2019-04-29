using NopFramework.Core.Domains.Seo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains.Catalog
{
    /// <summary>
    /// 产品
    /// </summary>
    public partial  class Product:BaseEntity,ISlugSupported
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 概述
        /// </summary>
        public string ShortDescription { get; set; }
        /// <summary>
        /// 技术参数
        /// </summary>
        public string Parameters { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        public string FullDescription { get; set; }
        /// <summary>
        /// 是否首页展示
        /// </summary>
        public bool ShowOnHomePage { get; set; }
        public bool IsPublished { get; set; }

        public DateTime UpdatedOn { get; set; }
        /// <summary>
        /// Gets or sets the meta keywords
        /// </summary>
        public string MetaKeywords { get; set; }
        private ICollection<ProductPicture> _productPictures;//图片
        private ICollection<ProductCategory> _productCategories;//商品属性
        public virtual ICollection<ProductPicture> ProductPictures
        {
            get { return _productPictures ?? (_productPictures = new List<ProductPicture>()); }
            protected set { _productPictures = value; }
        }


        public virtual ICollection<ProductCategory> ProductCategories
        {
            get { return _productCategories ?? (_productCategories = new List<ProductCategory>()); }
            protected set { _productCategories = value; }
        }
    }
}
