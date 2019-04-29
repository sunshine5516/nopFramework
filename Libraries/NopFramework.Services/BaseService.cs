using NopFramework.Core;
using NopFramework.Core.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services
{
    public abstract class BaseService<T> : BaseEntity, IBaseService<T> where T : BaseEntity
    {
        #region 声明实例
        private readonly IRepository<T> _baseRepository;
        #endregion
        public BaseService(IRepository<T> baseRepository)
        {
            this._baseRepository = baseRepository;
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Insert(T entity) {
            if (entity == null)
                throw new ArgumentNullException("entity类型为空");
            _baseRepository.Insert(entity);
        }
        /// <summary>
        /// 删除实体
        /// </summary>
        /// <param name="project"></param>
        public virtual void Delete(T entity) {
            if (entity == null)
                throw new ArgumentNullException("deleteEntityError");

            _baseRepository.Delete(entity);            
        }
        /// <summary>
        /// 更新实体
        /// </summary>
        /// <param name="entity"></param>
        public virtual void Update(T entity) {
            if (entity == null)
                throw new ArgumentNullException("deleteEntityError");
            _baseRepository.Update(entity);
        }
        /// <summary>
        /// 根据ID获取实体
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual T GetEntityById(int id) {
            if (id == 0)
                return null;
            var model = _baseRepository.GetById(id);
            if (model.IsDeleted)
                return null;
            return model;
        }
        public virtual IList<T> GetAllEntities() {
            var query = _baseRepository.Table.Where(b=>b.IsDeleted==false).ToList();
            query = query.OrderByDescending(n => n.CreatedOn).ToList();
            return query;
        }
        public virtual IPagedList<T> GetAllPagedList(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _baseRepository.Table.OrderByDescending(t=>t.CreatedOn);
            var tItems = new PagedList<T>(query, pageIndex, pageSize);
            return tItems;
        }
        public virtual IList<T> GetEntityByIds(int[] tIds)
        {
            if (tIds.Length <= 0)
                return null;
            //var query = _newsRepository.Table.Where(newsIds.Contains());
            var query = from n in _baseRepository.Table
                        where tIds.Contains(n.Id)
                        orderby n.CreatedOn descending
                        select n;
            return query.ToList();
        }
    }
}
