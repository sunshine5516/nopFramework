using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NopFramework.Core;
using NopFramework.Core.Domains.News;
using NopFramework.Core.Data;

namespace NopFramework.Services.News
{
    public partial class NewsService : INewsService
    {
        #region 声明实例
        private readonly IRepository<NewsItem> _newsRepository;
        #endregion
        #region 构造函数
        public NewsService(IRepository<NewsItem> newsRepository)
        {
            this._newsRepository = newsRepository;
        }
        #endregion
        #region 方法
        /// <summary>
        /// 删除新闻
        /// </summary>
        /// <param name="newsItem"></param>
        public void DeleteNews(NewsItem newsItem)
        {
            if(newsItem==null)
                throw new ArgumentNullException("newsItem类型为空");
            _newsRepository.Delete(newsItem);
            
        }
        /// <summary>
        /// 获取所有新闻
        /// </summary>
        /// <returns></returns>
        public IList<NewsItem> GetAllNews()
        {
            var query = _newsRepository.Table.ToList();
            query = query.OrderByDescending(n=>n.CreatedOn).ToList();
            return query;
        }
        /// <summary>
        /// 分页获取数据
        /// </summary>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public IPagedList<NewsItem> GetAllPagedList(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _newsRepository.Table.OrderByDescending(ea => ea.CreatedOn);
            var newsItems = new PagedList<NewsItem>(query, pageIndex, pageSize);
            return newsItems;
        }
        /// <summary>
        /// 根据ID获取新闻实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public NewsItem GetNewsById(int id)
        {
            if (id == 0)
                return null;
            var model = _newsRepository.GetById(id);
            return model;
        }
        /// <summary>
        /// id查询新闻组
        /// </summary>
        /// <param name="newsIds"></param>
        /// <returns></returns>
        public IList<NewsItem> GetNewsByIds(int[] newsIds)
        {
            if (newsIds.Length <= 0)
                return null;
            //var query = _newsRepository.Table.Where(newsIds.Contains());
            var query =from n in _newsRepository.Table
                       where newsIds.Contains(n.Id)
                       orderby n.CreatedOn descending
                       select n;
            return query.ToList();
        }
        /// <summary>
        /// 添加新闻
        /// </summary>
        /// <param name="newsItem"></param>
        public void InsertNews(NewsItem newsItem)
        {
            if (newsItem == null)
                throw new ArgumentNullException("newsItem类型为空");
            newsItem.CreatedOn = DateTime.Now;
            newsItem.UpdateOn = DateTime.Now;
            _newsRepository.Insert(newsItem);
        }
        /// <summary>
        /// 更新数据
        /// </summary>
        /// <param name="newsItem"></param>
        public void UpdateNews(NewsItem newsItem)
        {
            if (newsItem == null)
                throw new ArgumentNullException("newsItem类型为空");
            newsItem.UpdateOn = DateTime.Now;
            _newsRepository.Update(newsItem);
        }
        #endregion

    }
}
