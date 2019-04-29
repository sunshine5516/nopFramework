using NopFramework.Core.Data;
using NopFramework.Core.Infrastructure;
using NopFramework.Services.Localization;
using NopFramework.Web.Framework.Localization;
namespace NopFramework.Web.Framework.ViewEngines.Razor
{
    public abstract class WebViewPage<TModel> : System.Web.Mvc.WebViewPage<TModel>
    {
        private ILocalizationService _localizationService;
        private Localizer _localizer;
        public Localizer T
        {
            get
            {
                if (_localizer == null)
                {
                    _localizer = (format, args) =>
                     {
                         var resFormate = _localizationService.GetResource(format);
                         if (string.IsNullOrEmpty(resFormate))
                         {
                             return new LocalizedString(format);
                         }
                         return new LocalizedString((args == null || args.Length == 0)
                             ? resFormate : string.Format(resFormate, args));
                     };
                }
                return _localizer;
            }
        }
        public string L(string name)
        {
            string result = string.Empty;
            var resFormate = _localizationService.GetResource(name);
            return resFormate;
        }
        public override void InitHelpers()
        {
            base.InitHelpers();
            if (DataSettingsHelper.DatabaseIsInstalled())
            {
                _localizationService = EngineContext.Current.Resolve<ILocalizationService>();
            }
        }
        //public override string Layout
        //{
        //    get
        //    {
        //        var layout = base.Layout;

        //        if (!string.IsNullOrEmpty(layout))
        //        {
        //            var filename = Path.GetFileNameWithoutExtension(layout);
        //            ViewEngineResult viewResult = System.Web.Mvc.ViewEngines.Engines.FindView(ViewContext.Controller.ControllerContext, filename, "");

        //            if (viewResult.View != null && viewResult.View is RazorView)
        //            {
        //                layout = (viewResult.View as RazorView).ViewPath;
        //            }
        //        }

        //        return layout;
        //    }
        //    set
        //    {
        //        base.Layout = value;
        //    }
        //}
    }
    public abstract class WebViewPage : WebViewPage<dynamic>
    {
    }
}
