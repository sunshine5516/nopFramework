using System.Collections.Generic;
using NopFramework.Core.Caching;
using NopFramework.Core.Data;
using NopFramework.Core.Domains.Localization;
using System.Linq;
using NopFramework.Services.Logging;
using NopFramework.Core;
using System;

namespace NopFramework.Services.Localization
{
    public class LocalizationService : BaseService<LocaleStringResource>, ILocalizationService
    {
        #region 常量
       
        #endregion
        #region 声明实例
        private readonly IRepository<LocaleStringResource> _lsrRepository;
        private readonly ICacheManager _cacheManager;
        private readonly ILogger _logger;
        private readonly IWorkContext _workContext;
        #endregion
        #region 构造函数
        public LocalizationService(IRepository<LocaleStringResource> lsrRepository, 
            ICacheManager cacheManager, ILogger logger, IWorkContext workContext)
            : base(lsrRepository)
        {
            this._lsrRepository = lsrRepository;
            this._cacheManager = cacheManager;
            this._logger = logger;
            this._workContext = workContext;
        }


        #endregion
        #region 方法
        /// <summary>
        /// 根据语言ID获取本地资源文件
        /// </summary>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public virtual Dictionary<string, KeyValuePair<int, string>> GetAllResourceValues(int languageId)
        {
            var key = string.Format(LocalizationDefaults.LocaleStringResourcesAllCacheKey, languageId);
            var lrs = _cacheManager.Get(key, () =>
            {
                var query = _lsrRepository.TableNoTracking.Where(l => l.LanguageId == languageId);
                return ResourceValuesToDictionary(query);
            });
            return lrs;
        }
        public override void Update(LocaleStringResource entity)
        {
           
            base.Update(entity);
            _cacheManager.RemoveByPattern(LocalizationDefaults.LocaleStringResourcesPatternCacheKey);
        }
        public override void Insert(LocaleStringResource entity)
        {
            base.Insert(entity);
            _cacheManager.RemoveByPattern(LocalizationDefaults.LocaleStringResourcesPatternCacheKey);
        }
        /// <summary>
        /// 根据名称和语言ID获取资源文件
        /// </summary>
        /// <param name="resourceName"></param>
        /// <param name="languageId"></param>
        /// <param name="logIfNotFound"></param>
        /// <returns></returns>
        public LocaleStringResource GetLocaleStringResourceByName(string resourceName, int languageId, bool logIfNotFound = true)
        {
            var localeStringResource = _lsrRepository.TableNoTracking.Where(l => l.LanguageId == languageId
            &&l.ResourceName==resourceName).FirstOrDefault();
            if (localeStringResource == null && logIfNotFound)
                _logger.Warning($"Resource string ({resourceName}) not found. Language ID = {languageId}");
            return localeStringResource;
        }

        public IPagedList<LocaleStringResource> GetAllResourceValues(int languageId, DateTime? createdFrom = null, DateTime? createdTo = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = _lsrRepository.Table;
            query = query.Where(c =>c.LanguageId==languageId);
            query = query.OrderByDescending(c => c.CreatedOn);
            var resources = new PagedList<LocaleStringResource>(query, pageIndex, pageSize);
            return resources;
        }
        /// <summary>
        /// 基于指定的ResourceKey属性获取资源字符串。
        /// </summary>
        /// <param name="resourceKey">key</param>
        /// <returns>请求的资源字符串的字符串</returns>
        public virtual string GetResource(string resourceKey)
        {
            if (_workContext.WorkingLanguage != null)
                return GetResource(resourceKey, _workContext.WorkingLanguage.Id);

            return "";
        }
        /// <summary>
        /// 根据名称返回语言文件信息
        /// </summary>
        /// <param name="resourceKey">名称</param>
        /// <param name="languageId">语言</param>
        /// <param name="logIfNotFound">是否错误记录</param>
        /// <param name="defaultValue">返回的默认值</param>
        /// <param name="returnEmptyIfNotFound"></param>
        /// <returns></returns>
        public string GetResource(string resourceKey, int languageId, bool logIfNotFound = true, string defaultValue = "", bool returnEmptyIfNotFound = false)
        {
            string result = string.Empty;
            if (resourceKey == null)
                resourceKey = string.Empty;
            resourceKey = resourceKey.Trim().ToLowerInvariant();

            string key = string.Format(LocalizationDefaults.LOCALSTRINGRESOURCES_BY_RESOURCENAME_KEY, languageId, resourceKey);
            string lsr = _cacheManager.Get(key, () =>
            {
                var query = from l in _lsrRepository.Table
                            where l.ResourceName == resourceKey
                            && l.LanguageId == languageId
                            && l.LanguageId == languageId
                            select l.ResourceValue;
                return query.FirstOrDefault();
            });

            if (lsr != null)
                result = lsr;
            if (String.IsNullOrEmpty(result))
            {
                if (logIfNotFound)
                    _logger.Warning(string.Format("Resource string ({0}) is not found. Language ID = {1}", resourceKey, languageId));

                if (!String.IsNullOrEmpty(defaultValue))
                {
                    result = defaultValue;
                }
                else
                {
                    if (!returnEmptyIfNotFound)
                        result = resourceKey;
                }
            }
            return result;
        }

        #endregion
        #region 辅助方法
        private static Dictionary<string, KeyValuePair<int, string>> ResourceValuesToDictionary(IEnumerable<LocaleStringResource> locales)
        {
            var dictionary = new Dictionary<string, KeyValuePair<int, string>>();
            foreach (var locale in locales)
            {
                var resourceName = locale.ResourceName.ToLowerInvariant();
                if (!dictionary.ContainsKey(resourceName))
                    dictionary.Add(resourceName, new KeyValuePair<int, string>(locale.Id, locale.ResourceValue));
            }

            return dictionary;
        }

        

        #endregion
    }
}
