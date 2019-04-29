using NopFramework.Core.Data;
using NopFramework.Core.Domains.Catalog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Catalog
{
    public partial class CategoryService : BaseService<Category>, ICategoryService
    {
        #region 声明实例
        IRepository<Category> _categoryRepository;
        #endregion
        #region 构造函数
        public CategoryService(IRepository<Category> categoryRepository) 
            : base(categoryRepository)
        {
            this._categoryRepository = categoryRepository;
        }
        #endregion
        #region 方法

        #endregion
    }
}
