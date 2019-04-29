using NopFramework.Admin.Extensions;
using NopFramework.Admin.Models.Cms;
using NopFramework.Core.Domains.Cms;
using NopFramework.Core.Plugins;
using NopFramework.Services.Cms;
using NopFramework.Services.Configuration;
using NopFramework.Web.Framework.Kendoui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace NopFramework.Admin.Controllers
{
    public partial class WidgetController : Controller
    {
        #region 声明实例
        private readonly IWidgetService _widgetService;
        private readonly WidgetSettings _widgetSettings;
        private readonly ISettingService _settingService;
        private readonly IPluginFinder _pluginFinder;
        #endregion
        #region 构造函数
        public WidgetController(IWidgetService widgetService, WidgetSettings widgetSettings
            , ISettingService settingService, IPluginFinder pluginFinder)
        {
            this._widgetService = widgetService;
            this._widgetSettings = widgetSettings;
            this._settingService = settingService;
            this._pluginFinder = pluginFinder;
        }
        #endregion
        #region 方法
        // GET: Widget
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AllWidgets()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AllWidgets(DataSourceRequest command)
        {
            var tempModels = _widgetService.LoadAllWidgets();
            var widgetsModel = new List<WidgetModel>();
            foreach (var widget in tempModels)
            {
                var tmpl = widget.ToModel();
                tmpl.IsActive= widget.IsWidgetActive(_widgetSettings);
                widgetsModel.Add(tmpl);
            }
            widgetsModel = widgetsModel.ToList();
            var gridModel = new DataSourceResult
            {
                Data = widgetsModel,
                Total = widgetsModel.Count
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        [ChildActionOnly]
        public ActionResult WidgetsByZone(string widgetZone)
        {
            return View();
        }
        public ActionResult ConfigureWidget(string systemName)
        {
            var widget = _widgetService.LoadWidgetBySystemName(systemName);
            if (widget == null)
                return RedirectToAction("AllWidgets");
            var model = widget.ToModel();
            string actionName, controllerName;
            RouteValueDictionary routeValues;

            widget.GetConfigurationRoute(out actionName, out controllerName, out routeValues);
            model.ConfigurationActionName = actionName;
            model.ConfigurationControllerName = controllerName;
            model.ConfigurationRouteValues = routeValues;
            return View(model);
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult UpdateWidget([Bind(Exclude = "ConfigurationRouteValues")]WidgetModel model)
        {
            //var settingModel = _settingService.UpdateSetting();
            if (model == null)
                return RedirectToAction("AllWidgets");
            var widget = _widgetService.LoadWidgetBySystemName(model.SystemName);
            if (widget.IsWidgetActive(_widgetSettings))
            {
                if (!model.IsActive)
                {
                    _widgetSettings.ActiveWidgetSystemNames.Remove(widget.PluginDescriptor.SystemName);
                    _settingService.SaveSetting(_widgetSettings);
                }
            }
            else
            {
                if (model.IsActive)
                {
                    //mark as active
                    _widgetSettings.ActiveWidgetSystemNames.Add(widget.PluginDescriptor.SystemName);
                    _settingService.SaveSetting(_widgetSettings);
                }
            }
            var pluginDescriptor = widget.PluginDescriptor;
            pluginDescriptor.DisplayOrder = model.DisplayOrder;
            PluginFileParser.SavePluginDescriptionFile(pluginDescriptor);
            _pluginFinder.ReloadPlugins();
            return new JsonResult();
        }
        #endregion

    }
}