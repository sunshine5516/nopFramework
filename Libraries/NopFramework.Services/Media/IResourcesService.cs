using NopFramework.Core;
using NopFramework.Core.Domains.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Media
{
    public interface IResourcesService : IBaseService<Resources>
    {
        void DeleteResource(Resources resources);
        Resources InsertResources(byte[] resourcesBinary, string mimeType, string seoFilename,
            bool isNew = true, bool validateBinary = true);
        string GetResourcesUrl(Resources resources);
        /// <summary>
        /// 分页关键字查找
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Resources> GetAllPagedList(string Name,DateTime? createdOnFrom = null,
            DateTime? createdOnTo = null, int pageIndex = 0, int pageSize = int.MaxValue);




    }
}
