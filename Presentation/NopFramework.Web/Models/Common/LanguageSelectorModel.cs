using NopFramework.Web.Framework.Mvc;
using System.Collections.Generic;

namespace NopFramework.Web.Models.Common
{
    public class LanguageSelectorModel : BaseNopFrameworkEntityModel
    {
        public LanguageSelectorModel()
        {
            AvailableLanguages = new List<LanguageModel>();
        }

        public IList<LanguageModel> AvailableLanguages { get; set; }

        public int CurrentLanguageId { get; set; }
    }
}