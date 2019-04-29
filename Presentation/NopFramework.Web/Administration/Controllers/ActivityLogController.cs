using NopFramework.Admin.Controllers;
using NopFramework.Admin.Extensions;
using NopFramework.Admin.Models.Logging;
using NopFramework.Core.Domains.Logging;
using NopFramework.Services.Logging;
using NopFramework.Services.Security;
using NopFramework.Web.Framework.Kendoui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Controllers
{
    public class ActivityLogController : BaseAdminController
    {
        #region 声明实例
        private readonly ICustomerActivityService _customerActivityService;
        private readonly IPermissionService _permissionService;
        #endregion
        #region 构造函数
        public ActivityLogController(ICustomerActivityService customerActivityService,
            IPermissionService permissionService)
        {
            this._customerActivityService = customerActivityService;
            this._permissionService = permissionService;
        }
        #endregion
        #region 日志类型
        // GET: ActivityLog
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListTypes()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ListTypes(DataSourceRequest command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageActivityLog))
                return AccessDeniedView();
            var model = _customerActivityService.GetTypesByPaged(command.Page - 1, command.PageSize);
            var logTypeModels = model.Select(x => x.ToModel()).ToList();
            var gridModel = new DataSourceResult
            {
                Data = logTypeModels,
                Total = model.TotalCount
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        [HttpPost]
        public ActionResult TypeAdd([Bind(Exclude = "Id")] ActivityLogTypeModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageActivityLog))
                return AccessDeniedView();
            if (model.Name != null)
                model.Name = model.Name.Trim();
            if(model==null)
                return RedirectToAction("ListTypes");
            var logType = new ActivityLogType
            {
                SystemKeyword= model.Name,
                Name = model.Name,
                Enable = model.Enable
            };
            _customerActivityService.InsertActivityType(logType);
            _customerActivityService.InsertActivity(LogType.添加日志类型.ToString(), LogType.添加日志类型.ToString() + "(Name = {0})");
            return new JsonResult();
        }
        [HttpPost]
        public ActionResult TypeUpdate(ActivityLogTypeModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageActivityLog))
                return AccessDeniedView();
            if (model == null)
                return RedirectToAction("ListTypes");
            if (ModelState.IsValid)
            {
                var logType = _customerActivityService.GetActivityTypeById(model.Id);
                if (logType == null)
                    return Content("No logType could be loaded with the specified ID");
                logType.Name = model.Name;
                logType.Enable = model.Enable;
                _customerActivityService.UpdateActivityType(logType);
                _customerActivityService.InsertActivity(LogType.编辑日志类型.ToString(), LogType.编辑日志类型.ToString()+"(Name = {0})", logType.Name);
            }
            return new JsonResult();
        }
        public ActionResult TypeDelete(ActivityLogTypeModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageActivityLog))
                return AccessDeniedView();
            if (model == null)
                return RedirectToAction("ListTypes");
            if (ModelState.IsValid)
            {
                var logType = _customerActivityService.GetActivityTypeById(model.Id);
                if (logType == null)
                    return Content("No logType could be loaded with the specified ID");
                _customerActivityService.DeleteActivityType(logType);
                _customerActivityService.InsertActivity(LogType.删除日志类型.ToString(), LogType.删除日志类型.ToString() + "(Name = {0})", logType.Name);
            }
            return new JsonResult();
        }
        #endregion
        #region 活动日志
        public ActionResult ListLogs()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageActivityLog))
                return AccessDeniedView();
            var logTypes = _customerActivityService.GetAllActivityTypes();
            var model = new ActivityLogSearchModel();
            model.ActivityLogTypes.Add(new SelectListItem
            {
                Value = "0",
                Text = "All"
            });
            foreach (var at in logTypes)
            {
                model.ActivityLogTypes.Add(new SelectListItem
                {
                    Value = at.Id.ToString(),
                    Text = at.Name
                });
            }
            return View(model);
        }
        [HttpPost]
        public ActionResult ListLogs(DataSourceRequest command, ActivityLogSearchModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageActivityLog))
                return AccessDeniedView();
            var customerLogs = _customerActivityService.GetAllActivities(model.SearchStartDate,
                model.SearchEndDate,null,model.ActivityLogTypeId,command.Page-1,
                command.PageSize,model.IpAddress);
            var modle=customerLogs.Select(x => x.ToModel()).ToList();
            var gridModel = new DataSourceResult
            {
                Data = modle,
                Total = customerLogs.TotalCount
            };
            return Json(gridModel);
        }
        public ActionResult LogsDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageActivityLog))
                return AccessDeniedView();
            var activityLog = _customerActivityService.GetActivityById(id);
            if (activityLog == null)
            {
                throw new ArgumentException("No activity log found with the specified id");
            }
            _customerActivityService.DeleteActivity(activityLog);
            _customerActivityService.InsertActivity(LogType.删除日志.ToString(), LogType.删除日志.ToString() + "(ID = {0})", activityLog.Id);
            return new JsonResult();
        }
        public ActionResult ClearAll()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageActivityLog))
                return AccessDeniedView();
            _customerActivityService.ClearAllActivities();
            _customerActivityService.InsertActivity(LogType.清空日志.ToString(), LogType.清空日志.ToString());
            return RedirectToAction("ListLogs");
        }
        #endregion
        #region 辅助方法

        #endregion

    }
}