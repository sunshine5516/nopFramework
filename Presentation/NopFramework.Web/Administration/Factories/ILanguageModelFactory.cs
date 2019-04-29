using NopFramework.Admin.Models.Localization;
using NopFramework.Core.Domains.Localization;
using NopFramework.Web.Framework.Kendoui;
namespace NopFramework.Admin.Factories
{
    /// <summary>
    /// 语言模型工厂
    /// </summary>
    public partial interface ILanguageModelFactory
    {
        DataSourceResult PrepareLanguageListModel();
        LanguageModel PrepareLanguageModel(LanguageModel model, Language language, bool excludeProperties = false);
        void PrepareFlagsModel(LanguageModel model);
        DataSourceResult PrepareLocaleResourceListModel(DataSourceRequest command,int languageId);
        LocaleResourceModel PrepareLocaleResourceModel(LocaleStringResource localeStringResource);
    }
}
