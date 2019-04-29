using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains.Catalog
{
    public class ProductCategory:BaseEntity
    {
        /// <summary>
        /// 产品ID
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 属性Id
        /// </summary>
        public int CategoryId { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        public virtual Category Category { get; set; }

        /// <summary>
        /// 产品
        /// </summary>
        public virtual Product Product { get; set; }
    }
}
