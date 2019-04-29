using NopFramework.Core;
using NopFramework.Core.Domains.Localization;
using System;
using System.Collections.Generic;

namespace NopFramework.Services.Localization
{
   public partial interface ILocalizationService : IBaseService<LocaleStringResource>
    {
        Dictionary<string, KeyValuePair<int, string>> GetAllResourceValues(int languageId);
        LocaleStringResource GetLocaleStringResourceByName(string resourceName, int languageId, bool logIfNotFound = true);
        IPagedList<LocaleStringResource> GetAllResourceValues(int languageId, DateTime? createdFrom = default(DateTime?),
            DateTime? createdTo = default(DateTime?),int pageIndex = 0, int pageSize = int.MaxValue);
        string GetResource(string resourceKey);
        string GetResource(string resourceKey, int languageId, bool logIfNotFound = true,
            string defaultValue = "", bool returnEmptyIfNotFound = false);
    }
}
