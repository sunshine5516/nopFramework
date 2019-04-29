using NopFramework.Core.Caching;
using NopFramework.Core.Domains.Catalog;
using NopFramework.Services.Catalog;
using NopFramework.Services.Media;
using NopFramework.Web.Infrastructure.Cache;
using NopFramework.Web.Models.Catalog;
using NopFramework.Web.Models.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Web.Controllers
{
    public class ProductController : BasePublicController
    {
        #region 声明实例
        private readonly IProductService _productService;
        private readonly ICacheManager _cacheManager;
        private readonly IPictureService _pictureService;
        #endregion
        #region 构造函数
        public ProductController(IProductService productService,
            ICacheManager cacheManager,
            IPictureService pictureService)
        {
            this._productService = productService;
            this._cacheManager = cacheManager;
            this._pictureService = pictureService;
        }
        #endregion
        #region 方法
        // GET: Product
        public ActionResult Index()
        {
            var query = _productService.GetAllEntities();

            return View(PrepareMetaProducts(query, true));
        }
        [ChildActionOnly]
        public ActionResult HomeProduct()
        {
            var query = _productService.GetAllEntities();
            var model = PrepareMetaProducts(query, true);
            return PartialView(model);
        }
        public ActionResult ProductItems()
        {
            var query = _productService.GetAllEntities();
            var model = PrepareMetaProducts(query, true);
            return PartialView(model);
        }
        public ActionResult ProductDetail(int productId)
        {
            var product = _productService.GetEntityById(productId);
            if (product == null || product.IsDeleted)
                return InvokeHttp404();
            var model= PrepareProductDetailsModel(product);
            return View(model);
        }

        [ChildActionOnly]
        public ActionResult ProductMenu()
        {
            return PartialView();
        }
        #endregion
        #region 辅助方法
        protected virtual IList<ProductDetailsModel> PrepareMetaProducts(IList<Product> products,
             bool preparePictureModel = true)
        {
            IList<ProductDetailsModel> productDetailsModels = new List<ProductDetailsModel>();
            string defaultProductPictureCacheKey;
            foreach (var product in products)
            {
                var productDetail = new ProductDetailsModel {
                    Id=product.Id,
                    Name=product.Name,
                    ShortDescription=product.ShortDescription,
                    FullDescription=product.FullDescription,
                    Parameters=product.Parameters
                };
                defaultProductPictureCacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_DEFAULTPICTURE_MODEL_KEY, product.Id);
                productDetail.DefaultPictureModel = _cacheManager.Get(defaultProductPictureCacheKey, () =>
                {
                    var picture = _pictureService.GetPicturesByProductId(product.Id, 1).FirstOrDefault();
                    var pictureModel = new PictureModel
                    {
                        ImageUrl = _pictureService.GetPictureUrl(picture),
                        FullSizeImageUrl = _pictureService.GetPictureUrl(picture)
                    };
                    return pictureModel;
                });
                productDetailsModels.Add(productDetail);
                //productDetail.DefaultPictureModel.ImageUrl=product.ProductPictures.FirstOrDefault().Picture.
            }
            return productDetailsModels;
        }
        /// <summary>
        /// 商品详情
        /// </summary>
        /// <param name="product"></param>
        /// <returns></returns>
        protected virtual ProductDetailsModel PrepareProductDetailsModel(Product product)
        {
            string defaultProductPictureCacheKey;
            var model = new ProductDetailsModel
            {
                Id=product.Id,
                Name = product.Name,
                ShortDescription = product.ShortDescription,
                FullDescription = product.FullDescription,
                Parameters = product.Parameters
            };
            defaultProductPictureCacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_DETAILS_PICTURES_MODEL_KEY, product.Id);
            var cachedPictures = _cacheManager.Get(defaultProductPictureCacheKey, () =>
            {
                var pictures = _pictureService.GetPicturesByProductId(product.Id);
                var defaultPictureModel = new PictureModel
                {
                    ImageUrl = _pictureService.GetPictureUrl(pictures.FirstOrDefault()),
                    FullSizeImageUrl = _pictureService.GetPictureUrl(pictures.FirstOrDefault())
                };
                var pictureModels = new List<PictureModel>();
                foreach (var picture in pictures)
                {
                    var pictureModel = new PictureModel
                    {
                        ImageUrl = _pictureService.GetPictureUrl(picture),
                        FullSizeImageUrl = _pictureService.GetPictureUrl(picture)
                    };
                    pictureModels.Add(pictureModel);
                }
                return new { DefaultPictureModel = defaultPictureModel, PictureModels = pictureModels };
            });
            model.DefaultPictureModel = cachedPictures.DefaultPictureModel;
            model.PictureModels = cachedPictures.PictureModels;

            ///面包屑
            var breadcrumbModel = new ProductBreadcrumbModel
            {
                Enabled =true,
                ProductId = product.Id,
                ProductName = product.Name,
                ProductSeName = product.Name
            };
            model.Breadcrumb = breadcrumbModel;
            return model;
        }
        #endregion
    }
}