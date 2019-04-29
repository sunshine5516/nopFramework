using System;
using System.IO;
using System.Linq;
using NopFramework.Admin.Extensions;
using NopFramework.Admin.Models.Localization;
using NopFramework.Core;
using NopFramework.Core.Domains.Localization;
using NopFramework.Services.Localization;
using NopFramework.Web.Framework.Kendoui;

namespace NopFramework.Admin.Factories
{
    /// <summary>
    /// 语言模型工厂
    /// </summary>
    public class LanguageModelFactory : ILanguageModelFactory
    {
        #region 声明实例
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        #endregion
        #region 构造函数
        public LanguageModelFactory(ILanguageService languageService,
            ILocalizationService localizationService)
        {
            this._languageService = languageService;
            this._localizationService = localizationService;
        }

        
        #endregion
        #region 方法
        public DataSourceResult PrepareLanguageListModel()
        {
            var languages = _languageService.GetAllLanguages(true);
            var gridModel = new DataSourceResult
            {
                Data = languages.Select(x => x.ToModel()),
                Total = languages.Count()
            };
            return gridModel;
        }

        public LanguageModel PrepareLanguageModel(LanguageModel model, Language language, bool excludeProperties = false)
        {
            if (language != null)
            {
                model = model ?? language.ToModel();
            }
            if (language == null)
            {
                var allLanguages = _languageService.GetAllLanguages();
                model.DisplayOrder = allLanguages.Count!=0? allLanguages.Max(l => l.DisplayOrder) + 1:1;
                model.Published = true;
            }
            this.PrepareFlagsModel(model);
            return model;
        }
        public void PrepareFlagsModel(LanguageModel model)
        {
            if (model == null)
                throw new ArgumentNullException("model");

            model.FlagFileNames = Directory
                .EnumerateFiles(CommonHelper.MapPath("~/Content/Images/flags/"), "*.png", SearchOption.TopDirectoryOnly)
                .Select(Path.GetFileName)
                .ToList();
        }
        /// <summary>
        /// 准备语言资源文件
        /// </summary>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public DataSourceResult PrepareLocaleResourceListModel(DataSourceRequest command,int languageId)
        {
            if(languageId==0)
                throw new ArgumentNullException("model");
            var localResources = _localizationService.GetAllResourceValues(languageId
                ,null, null,pageIndex: command.Page - 1,
                pageSize: command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = localResources.Select(PrepareLocaleResourceModel),
                Total = localResources.Count
            };
            return gridModel;
        }

        public LocaleResourceModel PrepareLocaleResourceModel(LocaleStringResource localeStringResource)
        {
            LocaleResourceModel localeResourceModel = new LocaleResourceModel
            {
                Name = localeStringResource.ResourceName,
                Value = localeStringResource.ResourceValue,
                Id = localeStringResource.Id,
                LanguageId = localeStringResource.LanguageId
            };
            return localeResourceModel;
        }
        #endregion

    }
}