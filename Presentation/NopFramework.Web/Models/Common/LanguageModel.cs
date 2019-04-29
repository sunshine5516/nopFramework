using NopFramework.Web.Framework.Mvc;
namespace NopFramework.Web.Models.Common
{
    public partial class LanguageModel : BaseNopFrameworkEntityModel
    {
        public string Name { get; set; }
        public string FlagImageFileName { get; set; }

    }
}