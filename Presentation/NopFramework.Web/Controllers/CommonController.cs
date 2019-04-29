using NopFramework.Core;
using NopFramework.Core.Caching;
using NopFramework.Core.Domains.Catalog;
using NopFramework.Core.Domains.Customers;
using NopFramework.Services.Catalog;
using NopFramework.Services.Localization;
using NopFramework.Services.Security;
using NopFramework.Web.Infrastructure.Cache;
using NopFramework.Web.Models.Catalog;
using NopFramework.Web.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NopFramework.Web.Controllers
{
    public class CommonController : BasePublicController
    {
        #region 声明实例
        private readonly IWorkContext _workContext;
        private readonly IPermissionService _permissionService;
        private readonly IProductService _productService;
        private readonly ICacheManager _cacheManager;
        private readonly ILanguageService _languageService;
        #endregion
        #region 构造函数
        public CommonController(IWorkContext workContext
            , IPermissionService permissionService,
            IProductService productService, ICacheManager cacheManager,
            ILanguageService languageService)
        {
            this._workContext = workContext;
            this._permissionService = permissionService;
            this._productService = productService;
            this._cacheManager = cacheManager;
            this._languageService = languageService;
        }
        #endregion
        #region 方法
        public ActionResult PageNotFound()
        {
            this.Response.StatusCode = 404;
            this.Response.TrySkipIisCustomErrors = true;
            return View();
        }

        [ChildActionOnly]
        public ActionResult AdminHeaderLinks()
        {
            var customer = _workContext.CurrentCustomer;
            var model = new AdminHeaderLinksModel
            {
                ImpersonatedCustomerEmailUsername = customer.IsRegistered() ? customer.Email : "",
                IsCustomerImpersonated = _workContext.OriginalCustomerIfImpersonated != null,
                DisplayAdminLink = _permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel)
            };
            return PartialView(model);
        }
        [ChildActionOnly]
        public ActionResult Footer()
        {
            var query = _productService.GetAllEntities();
            var model = PrepareMetaProducts(query, true);
            return PartialView(model);
        }
        #region Menu导航条


        [ChildActionOnly]
        public ActionResult NewsMenu()
        {
            return PartialView();
        }


        #endregion
        #endregion
        // GET: Common
        public ActionResult Index()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult LanguageSelector()
        {
            var model = new LanguageSelectorModel();
            model.CurrentLanguageId = _workContext.WorkingLanguage.Id;
            model.AvailableLanguages = _languageService.GetAllLanguages()
                .Select(x => new LanguageModel
            {
                Id = x.Id,
                Name = x.Name,
                FlagImageFileName = x.FlagImageFileName,
            })
                    .ToList();
            return PartialView(model);
        }
        public ActionResult SetLanguage(int langid, string returnUrl = "")
        {
            var language = _languageService.GetEntityById(langid);
            if (language != null)
            {
                _workContext.WorkingLanguage = language;
            }
            //home page
            if (String.IsNullOrEmpty(returnUrl))
                returnUrl = Url.Action("Index", "Home", new { area = "Admin" });
            //prevent open redirection attack
            if (!Url.IsLocalUrl(returnUrl))
                return RedirectToAction("Index", "Home", new { area = "Admin" });
            return Redirect(returnUrl);
        }
        #region 辅助方法
        protected virtual IList<ProductDetailsModel> PrepareMetaProducts(IList<Product> products,
        bool preparePictureModel = true)
        {
            IList<ProductDetailsModel> productDetailsModels = new List<ProductDetailsModel>();
            foreach (var product in products)
            {
                var productDetail = new ProductDetailsModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    ShortDescription = product.ShortDescription,
                    FullDescription = product.FullDescription,
                    Parameters = product.Parameters
                };
                //defaultProductPictureCacheKey = string.Format(ModelCacheEventConsumer.PRODUCT_DEFAULTPICTURE_MODEL_KEY, product.Id);
                //productDetail.DefaultPictureModel = _cacheManager.Get(defaultProductPictureCacheKey, () =>
                //{
                //    var picture = _pictureService.GetPicturesByProductId(product.Id, 1).FirstOrDefault();
                //    var pictureModel = new PictureModel
                //    {
                //        ImageUrl = _pictureService.GetPictureUrl(picture),
                //        FullSizeImageUrl = _pictureService.GetPictureUrl(picture)
                //    };
                //    return pictureModel;
                //});
                productDetailsModels.Add(productDetail);
                //productDetail.DefaultPictureModel.ImageUrl=product.ProductPictures.FirstOrDefault().Picture.
            }
            return productDetailsModels;
        }
        #endregion
    }
}