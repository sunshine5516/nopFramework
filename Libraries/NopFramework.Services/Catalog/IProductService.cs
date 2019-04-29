using NopFramework.Core;
using NopFramework.Core.Domains.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Catalog
{
    public partial interface IProductService 
        : IBaseService<Product>
    {

        /// <summary>
        /// 分页关键字查找
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Product> GetAllPagedList(string ProductName, int categoryId, DateTime? createdOnFrom = null,
            DateTime? createdOnTo = null, int pageIndex = 0, int pageSize = int.MaxValue);

        ProductPicture GetPictureById(int Id);
        IList<ProductPicture> GetPagedProductPicture(int productId);
        void InsertProductPicture(ProductPicture productPicture);
        /// <summary>
        /// 删除ProductPicture
        /// </summary>
        /// <param name="bugPicture"></param>
        void DeleteProductPicture(ProductPicture productPicture);

        //void InsertProductPicture(ProdPicture bugPicture);
    }
}
