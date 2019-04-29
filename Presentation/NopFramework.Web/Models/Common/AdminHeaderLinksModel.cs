using NopFramework.Web.Framework.Mvc;
namespace NopFramework.Web.Models.Common
{
    public partial class AdminHeaderLinksModel : BaseNopFrameworkEntityModel
    {
        public string ImpersonatedCustomerEmailUsername { get; set; }
        public bool IsCustomerImpersonated { get; set; }
        public bool DisplayAdminLink { get; set; }
        public string EditPageUrl { get; set; }
    }
}