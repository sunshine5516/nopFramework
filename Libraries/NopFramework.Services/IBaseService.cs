using NopFramework.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services
{
    public interface IBaseService<T> where T : BaseEntity
    {
        void Insert(T entity);
        void Delete(T project);
        void Update(T project);
        T GetEntityById(int id);
        IList<T> GetAllEntities();
        IPagedList<T> GetAllPagedList(int pageIndex = 0, int pageSize = int.MaxValue);
        IList<T> GetEntityByIds(int[] tIds);
    }
}
