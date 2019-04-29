using System.Collections.Generic;
using System.Linq;
using NopFramework.Core.Caching;
using NopFramework.Core.Data;
using NopFramework.Core.Domains.Localization;
namespace NopFramework.Services.Localization
{
    public partial class LanguageService : BaseService<Language>, ILanguageService
    {
        #region 常量
        
        #endregion
        #region 声明实例
        private readonly IRepository<Language> _languageRepository;
        private readonly ICacheManager _cacheManager;
        #endregion
        #region 构造函数
        public LanguageService(IRepository<Language> languageRepository,
            ICacheManager cacheManager)
            :base(languageRepository)
        {
            this._languageRepository = languageRepository;
            this._cacheManager = cacheManager;
        }


        #endregion
        #region 方法
        public IList<Language> GetAllLanguages(bool showHidden = false, int storeId = 0)
        {
            string key = string.Format(LocalizationDefaults.LANGUAGES_ALL_KEY, showHidden); ;
            var languages = _cacheManager.Get(key, () =>
               {
                   var query = _languageRepository.Table;
                   if (!showHidden)
                       query = query.Where(l => l.Published);
                   query = query.OrderBy(l => l.DisplayOrder);
                   return query.ToList();
               });
            return languages;
        }
        public override void Insert(Language entity)
        {
            string key = string.Format(LocalizationDefaults.LANGUAGES_ALL_KEY, true); ;
            _cacheManager.RemoveByPattern(key);
            base.Insert(entity);
        }
        public override void Delete(Language entity)
        {
            string key = string.Format(LocalizationDefaults.LANGUAGES_ALL_KEY, true); ;
            _cacheManager.RemoveByPattern(key);
            base.Delete(entity);
        }
        public override void Update(Language entity)
        {
            string key = string.Format(LocalizationDefaults.LANGUAGES_ALL_KEY, true); ;
            _cacheManager.RemoveByPattern(key);
            base.Update(entity);
        }
        #endregion

    }
}
