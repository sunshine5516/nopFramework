using System;
using System.Collections.Generic;
using System.Linq;
using NopFramework.Core;
using NopFramework.Core.Data;
using NopFramework.Core.Domains.Messages;
namespace NopFramework.Services.Messages
{
    public partial class TermService : BaseService<Term>, ITermService
    {
        #region 声明实例
        IRepository<Term> _termRepository;
        private readonly IRepository<TermPicture> _termPictureRespository;
        #endregion
        public TermService(IRepository<Term> termRepository, IRepository<TermPicture> termPictureRespository)
            : base(termRepository)
        {
            this._termRepository = termRepository;
            this._termPictureRespository = termPictureRespository;
        }

      
        #region 方法
        public IPagedList<Term> GetAllPagedList(string TermName,int termType, DateTime? createdOnFrom = null,
            DateTime? createdOnTo = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _termRepository.Table.Where(p => p.IsDeleted == false);
            if (!String.IsNullOrEmpty(TermName))
                query = query.Where(p => p.Name == TermName);
            if (termType != 0)
                query = query.Where(p => p.TermType == termType);

            if (createdOnFrom.HasValue)
                query = query.Where(p => (createdOnFrom.Value <= p.CreatedOn));
            if (createdOnTo.HasValue)
                query = query.Where(p => createdOnTo.Value >= p.CreatedOn);

            query = query.OrderByDescending(c => c.CreatedOn);
            var TermsList = new PagedList<Term>(query, pageIndex, pageSize);
            return TermsList;
        }

        public IList<TermPicture> GetPagedTermPicture(int productId)
        {
            var query = _termPictureRespository.Table.Where(bp => bp.TermId == productId);
            var bugPictures = query.ToList();
            return bugPictures;
        }
        public void DeleteTermPicture(TermPicture productPicture)
        {
            if (productPicture == null)
                throw new ArgumentNullException("productPicture");
            _termPictureRespository.Delete(productPicture);
        }
        public TermPicture GetPictureById(int Id)
        {
            var query = _termPictureRespository.Table.Where(bp => bp.Id == Id).FirstOrDefault();
            if (query == null)
                throw new ArgumentNullException("termPicture is null");
            return query;
        }

        public void InsertTermPicture(TermPicture productPicture)
        {
            if (productPicture == null)
                throw new ArgumentNullException("productPicture");
            _termPictureRespository.Insert(productPicture);
        }
        #endregion

    }
}
