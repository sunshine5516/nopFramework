using ImageResizer.Configuration.Logging;
using NopFramework.Admin.Controllers;
using NopFramework.Admin.Models.Customers;
using NopFramework.Core;
using NopFramework.Core.Data;
using NopFramework.Models.Security;
using NopFramework.Services.Customers;
using NopFramework.Services.Security;
using NopFramework.Services.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NopFramework.Core.Domains.Customers;

namespace NopFramework.Admin.Controllers
{
    public class SecurityController : BaseAdminController
    {
        #region 声明实例
        private readonly IPermissionService _permissionService;
        private readonly IWorkContext _workContext;
        private readonly ICustomerService _customerService;
        private readonly Services.Logging.ILogger _logger;
        #endregion
        #region 构造函数
        public SecurityController(IPermissionService permissionService
            , IWorkContext workContext
            , ICustomerService customerService, Services.Logging.ILogger logger)
        {
            this._permissionService = permissionService;
            this._workContext = workContext;
            this._customerService = customerService;
            this._logger = logger;
        }
        #endregion
        #region 方法
        public ActionResult AccessDenied(string pageUrl)
        {
            var currentCustomer = _workContext.CurrentCustomer;
            if (currentCustomer == null || currentCustomer.IsGuest())
            {
                _logger.Information(string.Format("{0}拒绝匿名访问请求", pageUrl));
                return View();
            }

            _logger.Information(string.Format("用户#{0} '{1}' 访问 {2}无权限", currentCustomer.Email, currentCustomer.Email, pageUrl));


            return View();
        }
        // GET: Security
        public ActionResult Index()
        {
            return RedirectToAction("PermissionList");
        }
        [HttpPost]
        public ActionResult test()
        {
            return View();
        }
        public ActionResult PermissionList()
        {
            var permissionRecords = _permissionService.GetAllPermissionRecords();
            var customerRoles = _customerService.GetAllCustomerRoles(true);
            var permissionRecordModel = new PermissionMappingModel();

            foreach (var customerRole in customerRoles)
            {
                permissionRecordModel.AvailableCustomerRoles.Add(new CustomerRoleModel
                {
                    Id = customerRole.Id,
                    Name = customerRole.Name
                });
            }
            foreach (var permissionRecord in permissionRecords)
            {
                permissionRecordModel.AvailablePermissions.Add(new PermissionRecordModel
                {
                    Id = permissionRecord.Id,
                    Name = permissionRecord.Name,
                    SystemName = permissionRecord.SystemName
                });
            }

            foreach (var permissionRecord in permissionRecords)
            {
                Dictionary<int, bool> dir = new Dictionary<int, bool>();
               
                foreach (var customerRole in customerRoles)
                {
                    //if (customerRole.PermissionRecords.Contains(permissionRecord))
                    //{
                    bool allowed = permissionRecord.CustomerRoles.Count(x => x.Id == customerRole.Id) > 0;
                    if (!permissionRecordModel.Allowed.ContainsKey(permissionRecord.SystemName))
                        permissionRecordModel.Allowed[permissionRecord.SystemName] = new Dictionary<int, bool>();
                    permissionRecordModel.Allowed[permissionRecord.SystemName][customerRole.Id]=allowed;
                    //}
                }

            }
            return View(permissionRecordModel);
        }
        [HttpPost,ActionName("PermissionList")]
        public ActionResult PermissionsSave(FormCollection form)
        {
            var permissionRecords = _permissionService.GetAllPermissionRecords();
            var customerRoles = _customerService.GetAllCustomerRoles(true);


            foreach (var cr in customerRoles)
            {
                string formKey = "allow_" + cr.Id;
                var permissionRecordSystemNamesToRestrict = form[formKey] != null ? form[formKey].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();

                foreach (var pr in permissionRecords)
                {

                    bool allow = permissionRecordSystemNamesToRestrict.Contains(pr.SystemName);
                    if (allow)
                    {
                        if (pr.CustomerRoles.FirstOrDefault(x => x.Id == cr.Id) == null)
                        {
                            pr.CustomerRoles.Add(cr);
                            _permissionService.UpdatePermissionRecord(pr);
                        }
                    }
                    else
                    {
                        if (pr.CustomerRoles.FirstOrDefault(x => x.Id == cr.Id) != null)
                        {
                            pr.CustomerRoles.Remove(cr);
                            _permissionService.UpdatePermissionRecord(pr);
                        }
                    }
                }
            }

            SuccessNotification("权限更新成功");
            return RedirectToAction("PermissionList");
        }
        #endregion
        #region 辅助方法

        #endregion

    }
}