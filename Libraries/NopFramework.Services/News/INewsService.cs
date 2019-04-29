using NopFramework.Core;
using NopFramework.Core.Domains.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.News
{
    public interface INewsService
    {
        void InsertNews(NewsItem newsItem);
        void DeleteNews(NewsItem newsItem);
        void UpdateNews(NewsItem newsItem);
        NewsItem GetNewsById(int id);
        IList<NewsItem> GetAllNews();
        IPagedList<NewsItem> GetAllPagedList(int pageIndex = 0, int pageSize = int.MaxValue);
        IList<NewsItem> GetNewsByIds(int[] newsIds);
    }
}
