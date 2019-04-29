using NopFramework.Core;
using NopFramework.Core.Domains.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Messages
{
    public partial interface ITermService: IBaseService<Term>
    {
        /// <summary>
        /// 分页关键字查找
        /// </summary>
        /// <param name="projectName"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        IPagedList<Term> GetAllPagedList(string TermName,int termType, DateTime? createdOnFrom = null,
            DateTime? createdOnTo = null, int pageIndex = 0, int pageSize = int.MaxValue);


        TermPicture GetPictureById(int Id);
        IList<TermPicture> GetPagedTermPicture(int productId);
        void InsertTermPicture(TermPicture productPicture);
        /// <summary>
        /// 删除ProductPicture
        /// </summary>
        /// <param name="bugPicture"></param>
        void DeleteTermPicture(TermPicture productPicture);
    }
}
