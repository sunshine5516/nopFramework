using NopFramework.Core.Domains.News;
using NopFramework.Services.News;
using NopFramework.Services.Seo;
using NopFramework.Web.Models.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Web.Controllers
{
    public class NewsController : BasePublicController
    {
        #region 声明实例
        private readonly INewsService _newsService;
        #endregion
        #region 构造函数
        public NewsController(INewsService newsService)
        {
            this._newsService = newsService;
        }
        #endregion
        #region 方法
        // GET: News
        public ActionResult Index()
        {
            return View();
        }
        /// <summary>
        /// 新闻节目列表
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public ActionResult List(NewsPagingFilteringModel command)
        {
            var model = new NewsItemListModel();
            if (command.PageSize <= 0) command.PageSize = 9;
            if (command.PageNumber <= 0) command.PageNumber = 1;
            var newsItems = _newsService.GetAllPagedList(command.PageNumber - 1, command.PageSize);
            model.PagingFilteringContext.LoadPagedList(newsItems);

            model.NewsItems = newsItems.Select
                (x =>
                {
                    var newsModel = new NewsItemModel();
                    PrepareNewsItemModel(newsModel, x, false);
                    return newsModel;
                }).ToList();
            //};
            return View(model);
        }
        /// <summary>
        /// 首页新闻
        /// </summary>
        /// <returns></returns>
        [ChildActionOnly]
        public ActionResult HomePageNews()
        {
            var newsItems = _newsService.GetAllPagedList(0, 3);
            var model = new HomePageNewsItemsModel
            {
                NewsItems = newsItems.Select
                (x =>
                {
                    var newsModel = new NewsItemModel();
                    PrepareNewsItemModel(newsModel,x,false);
                    return newsModel;
                }).ToList()
            };
            return PartialView(model);
        }
        /// <summary>
        /// 新闻详情
        /// </summary>
        /// <param name="newsItemId"></param>
        /// <returns></returns>
        public ActionResult NewsItem(int newsItemId)
        {
            var newsItem = _newsService.GetNewsById(newsItemId);
            if (newsItem == null||!newsItem.IsPublished)
                return RedirectToRoute("HomePage");
            var model = new NewsItemModel();
            PrepareNewsItemModel(model, newsItem, true);
            return View(model);
        }
        #endregion
        #region 辅助方法
        [NonAction]
        protected virtual void PrepareNewsItemModel(NewsItemModel model, NewsItem newsItem, bool prepareComments)
        {
            if (newsItem == null)
                throw new ArgumentNullException("newsItem");

            if (model == null)
                throw new ArgumentNullException("model");

            model.Id = newsItem.Id;
            model.MetaTitle = newsItem.MetaTitle;

            model.Contents = newsItem.Contents;

            model.Title = newsItem.Title;
            model.SeName = newsItem.GetSeName(0, ensureTwoPublishedLanguages: false);
            model.AllowComments = newsItem.AllowComments;
            model.CreatedOn = newsItem.CreatedOn;
            model.UpdateOn = newsItem.UpdateOn;
        }
        #endregion
    }


}

