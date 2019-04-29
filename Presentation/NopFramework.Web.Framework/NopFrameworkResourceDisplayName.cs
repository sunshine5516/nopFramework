using NopFramework.Core;
using NopFramework.Core.Infrastructure;
using NopFramework.Services.Localization;
using NopFramework.Web.Framework.Mvc;
using System.ComponentModel;
namespace NopFramework.Web.Framework
{
    public class NopFrameworkResourceDisplayName : DisplayNameAttribute, IModelAttribute
    {
        private string _resourceValue = string.Empty;
        public string ResourceKey { get; set; }
        public NopFrameworkResourceDisplayName(string resourceKey)
            : base(resourceKey)
        {
            ResourceKey = resourceKey;
        }
        public override string DisplayName
        {
            get
            {
                //do not cache resources because it causes issues when you have multiple languages
                //if (!_resourceValueRetrived)
                //{
                //var langId = EngineContext.Current.Resolve<IWorkContext>().WorkingLanguage.Id;
                //_resourceValue = EngineContext.Current
                //    .Resolve<ILocalizationService>()
                //    .GetResource(ResourceKey, langId, true, ResourceKey);
                //    _resourceValueRetrived = true;
                //}
                var langId = EngineContext.Current.Resolve<IWorkContext>().WorkingLanguage.Id;
                _resourceValue = EngineContext.Current
                    .Resolve<ILocalizationService>().GetResource(ResourceKey, langId, true, ResourceKey);
                return _resourceValue;
            }
        }
        public string Name
        {
            get { return "NopFrameworkResourceDisplayName"; }
        }
    }
}