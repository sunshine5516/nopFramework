using NopFramework.Admin.Models.Localization;
using NopFramework.Web.Framework.Mvc;
using System.Collections.Generic;
namespace NopFramework.Admin.Models.Common
{
    public class LanguageSelectorModel: BaseNopFrameworkModel
    {
        public LanguageSelectorModel()
        {
            AvailableLanguages = new List<LanguageModel>();
        }
        public IList<LanguageModel> AvailableLanguages { get; set; }

        public LanguageModel CurrentLanguage { get; set; }
    }
}