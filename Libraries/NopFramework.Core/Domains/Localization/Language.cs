using System.Collections.Generic;
namespace NopFramework.Core.Domains.Localization
{
    public partial class Language: BaseEntity
    {
        private ICollection<LocaleStringResource> _localeStringResources;
        public string Name { get; set; }
        public string LanguageCulture { get; set; }
        public string UniqueSeoCode { get; set; }
        public string FlagImageFileName { get; set; }
        public bool Rtl { get; set; }
        public int DefaultCurrencyId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the language is published
        /// </summary>
        public bool Published { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }
        public virtual ICollection<LocaleStringResource> LocaleStringResources
        {
            get { return _localeStringResources ?? (_localeStringResources = new List<LocaleStringResource>()); }
            protected set { _localeStringResources = value; }
        }
    }
}
