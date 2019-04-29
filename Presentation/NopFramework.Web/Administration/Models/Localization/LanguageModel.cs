using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Localization
{
    public class LanguageModel:BaseNopFrameworkEntityModel
    {

        public LanguageModel()
        {
            FlagFileNames = new List<string>();
            AvailableCurrencies = new List<SelectListItem>();
        }

        [NopFrameworkResourceDisplayName("Admin.Configuration.Languages.Fields.Name")]
        [AllowHtml]
        public string Name { get; set; }

        [NopFrameworkResourceDisplayName("Admin.Configuration.Languages.Fields.LanguageCulture")]
        [AllowHtml]
        public string LanguageCulture { get; set; }

        [NopFrameworkResourceDisplayName("Admin.Configuration.Languages.Fields.UniqueSeoCode")]
        [AllowHtml]
        public string UniqueSeoCode { get; set; }

        //flags
        [NopFrameworkResourceDisplayName("Admin.Configuration.Languages.Fields.FlagImageFileName")]
        [AllowHtml]
        public string FlagImageFileName { get; set; }
        public IList<string> FlagFileNames { get; set; }

        [NopFrameworkResourceDisplayName("Admin.Configuration.Languages.Fields.Rtl")]
        public bool Rtl { get; set; }

        //default currency
        [NopFrameworkResourceDisplayName("Admin.Configuration.Languages.Fields.DefaultCurrency")]
        [AllowHtml]
        public int DefaultCurrencyId { get; set; }
        public IList<SelectListItem> AvailableCurrencies { get; set; }

        [NopFrameworkResourceDisplayName("Admin.Configuration.Languages.Fields.Published")]
        public bool Published { get; set; }

        [NopFrameworkResourceDisplayName("Admin.Configuration.Languages.Fields.DisplayOrder")]
        public int DisplayOrder { get; set; }

    }
}