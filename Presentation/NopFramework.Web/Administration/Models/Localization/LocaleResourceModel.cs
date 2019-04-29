using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
namespace NopFramework.Admin.Models.Localization
{
    public class LocaleResourceModel : BaseNopFrameworkEntityModel
    {
        [NopFrameworkResourceDisplayName("Admin.Configuration.Resources.Fields.Name")]
        public string Name { get; set; }
        [NopFrameworkResourceDisplayName("Admin.Configuration.Resources.Fields.Value")]
        public string Value { get; set; }
        public int LanguageId { get; set; }
    }
}