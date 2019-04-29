using NopFramework.Admin.Extensions;
using NopFramework.Admin.Models.News;
using NopFramework.Services.News;
using NopFramework.Services.Security;
using NopFramework.Services.Seo;
using NopFramework.Web.Framework.Controllers;
using NopFramework.Web.Framework.Kendoui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Controllers
{
    public class NewsController : BaseAdminController
    {
        #region 实例声明
        private readonly INewsService _newsService;
        private readonly IPermissionService _permissionService;
        private readonly IUrlRecordService _urlRecordService;
        #endregion


        #region 构造函数
        public NewsController(INewsService newsService,
            IPermissionService permissionService,
            IUrlRecordService urlRecordService)
        {
            this._newsService = newsService;
            this._permissionService = permissionService;
            this._urlRecordService = urlRecordService;
        }
        #endregion
        #region 方法
        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();
            return View();
        }
        [HttpPost]
        public ActionResult List(DataSourceRequest command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();
            var query = _newsService.GetAllPagedList(command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = query,
                Total = query.TotalCount
            };
            return Json(gridModel);
        }
        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();
            var model = new NewsItemModel();
            model.IsPublished = true;
            model.AllowComments = true;
            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(NewsItemModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();
            if (ModelState.IsValid)
            {
                var newsItem = model.ToEntity();
                newsItem.CreatedOn = DateTime.Now;

                _newsService.InsertNews(newsItem);
                _urlRecordService.SaveSlug(newsItem, newsItem.Title, 0);
                //var seName=newsItem.ValidateSeName
                if (continueEditing)
                {
                    return RedirectToAction("Edit", new { id = newsItem.Id });
                }
                return RedirectToAction("List");
            }
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();
            var newsItem = _newsService.GetNewsById(id);
            if (newsItem == null)
                return RedirectToAction("List");
            return View(newsItem.ToModel());
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(NewsItemModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageNews))
                return AccessDeniedView();
            var newsItem = _newsService.GetNewsById(model.Id);
            if (newsItem == null)
                //No news item found with the specified id
                return RedirectToAction("List");
            if (ModelState.IsValid)
            {
                newsItem = model.ToEntity(newsItem);
                
                _newsService.UpdateNews(newsItem);

                //search engine name
                var seName = newsItem.ValidateSeName(model.SeName, model.Title, true);
                _urlRecordService.SaveSlug(newsItem, seName, 0);

                SuccessNotification("新闻编辑成功");

                if (continueEditing)
                {                    
                    return RedirectToAction("Edit", new { id = newsItem.Id });
                }
                return RedirectToAction("List");
            }
            return View(model);
        }

        /// <summary>
        /// 删除所选商品
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (selectedIds != null)
            {
                var newsItems = _newsService.GetNewsByIds(selectedIds.ToArray());
                foreach (var newsItem in newsItems)
                {
                    _newsService.DeleteNews(newsItem);
                }

            }
            return Json(new { Result = true });
        }

        // GET: News
        public ActionResult Index()
        {
            return View();
        }
        #endregion

    }
}