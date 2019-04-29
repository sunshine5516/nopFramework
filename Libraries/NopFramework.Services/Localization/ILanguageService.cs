using NopFramework.Core.Domains.Localization;
using System.Collections.Generic;

namespace NopFramework.Services.Localization
{
    public partial interface ILanguageService : IBaseService<Language>
    {
        IList<Language> GetAllLanguages(bool showHidden = false, int storeId = 0);        
    }
}
