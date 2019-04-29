using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Models.Catalog
{
    public partial class ProductListModel: BaseNopFrameworkEntityModel
    {
        /// <summary>
        /// Name
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 概述
        /// </summary>
        public string ShortDescription { get; set; }

        public bool IsPublished { get; set; }

        public string Category { get; set; }
        public DateTime? CreatedOn { get; set; }//创建时间
    }
}