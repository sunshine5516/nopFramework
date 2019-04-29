using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Web.Framework.UI.Paging
{
    /// <summary>
    /// 已分解成页面的对象的集合
    /// </summary>
    public interface IPageableModel
    {
        int PageIndex { get; }
        int PageNumber { get; set;}
        int PageSize { get; set; }
        int TotalItems { get; set; }
        int TotalPages { get; set; }
        int FirstItem { get; set; }
        int LastItem { get; set; }
        bool HasPreviousPage { get; set; }
        bool HasNextPage { get; }
    }
    public interface IPagination<T> : IPageableModel
    {

    }
}
