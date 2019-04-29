using NopFramework.Core;
using NopFramework.Core.Caching;
using NopFramework.Core.Data;
using NopFramework.Core.Domains.Catalog;
using NopFramework.Services.Events;
using System;
using System.Collections.Generic;
using System.Linq;

namespace NopFramework.Services.Catalog
{
    public partial class ProductService : BaseService<Product>, IProductService
    {
        #region 常量
        private const string PRODUCTS_BY_ID_KEY = "Hx.product.id-{0}";
        /// <summary>
        /// 
        /// </summary>
        private const string PRODUCTS_PATTERN_KEY = "Hx.product.";
        #endregion
        #region 声明实例
        private readonly IRepository<Product> _productRepository;
        private readonly IRepository<ProductPicture> _productPictureRespository;
        private readonly IEventPublisher _eventPublisher;
        private readonly ICacheManager _cacheManager;        
        #endregion
        #region 构造函数
        public ProductService(IRepository<Product> productRepository,
            IRepository<ProductPicture> productPictureRespository,
            IEventPublisher eventPublisher, ICacheManager cacheManager)
            : base(productRepository)
        {
            this._productRepository = productRepository;
            this._productPictureRespository = productPictureRespository;
            this._eventPublisher = eventPublisher;
            this._cacheManager = cacheManager;
        }
        #endregion
        #region 方法

        public IPagedList<Product> GetAllPagedList(string ProductName, int categoryId, 
            DateTime? createdOnFrom = null, DateTime? createdOnTo = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _productRepository.Table.Where(p=>p.IsDeleted==false);
            if (categoryId != 0)
                query = from p in query
                        from pc in p.ProductCategories.Where(pc => pc.Id == categoryId)
                        select p;
            if (!String.IsNullOrEmpty(ProductName))
                query = query.Where(p => p.Name == ProductName);
            if (createdOnFrom.HasValue)
                query = query.Where(p => (createdOnFrom.Value <= p.CreatedOn));
            if (createdOnTo.HasValue)
                query = query.Where(p => createdOnTo.Value >= p.CreatedOn);

            query = query.OrderByDescending(c => c.CreatedOn);
            var productsList = new PagedList<Product>(query, pageIndex, pageSize);
            return productsList;
            //query = query.Where(p => p.ProductCategories.Where(pc=>pc.Id==categoryId));
        }
        public IList<ProductPicture> GetPagedProductPicture(int productId)
        {
            var query = _productPictureRespository.Table.Where(bp => bp.ProductId == productId);
            var bugPictures = query.ToList();
            return bugPictures;
        }
        public ProductPicture GetPictureById(int Id)
        {
            var query = _productPictureRespository.Table.Where(bp => bp.Id == Id).FirstOrDefault();
            if (query == null)
                throw new ArgumentNullException("productPicture is null");
            return query;
        }

        public void InsertProductPicture(ProductPicture productPicture)
        {            
            if (productPicture == null)
                throw new ArgumentNullException("productPicture");
            _productPictureRespository.Insert(productPicture);
            _eventPublisher.EntityInserted<ProductPicture>(productPicture);
        }
        /// <summary>
        /// 删除ProductPicture
        /// </summary>
        /// <param name="bugPicture"></param>
        public void DeleteProductPicture(ProductPicture productPicture)
        {
            if (productPicture == null)
                throw new ArgumentNullException("productPicture");
            _productPictureRespository.Delete(productPicture);
            _eventPublisher.EntityDeleted<ProductPicture>(productPicture);
        }

       
        #endregion
        #region 辅助方法

        #endregion
    }
}
