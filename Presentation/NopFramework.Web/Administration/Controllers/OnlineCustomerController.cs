using NopFramework.Admin.Models.Customers;
using NopFramework.Core.Domains.Customers;
using NopFramework.Services.Customers;
using NopFramework.Services.Security;
using NopFramework.Web.Framework.Kendoui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Controllers
{
    public class OnlineCustomerController : BaseAdminController
    {
        #region 声明实例
        private readonly ICustomerService _customerService;
        private readonly IPermissionService _permissionService;
        private readonly CustomerSettings _customerSettings;
        #endregion
        #region 构造函数
        public OnlineCustomerController(ICustomerService customerService,
            IPermissionService permissionService, CustomerSettings customerSettings)
        {
            this._customerService = customerService;
            this._permissionService = permissionService;
            this._customerSettings = customerSettings;
        }
        #endregion
        #region 方法
        // GET: OnlineCustomer
        public ActionResult Index()
        {
            return RedirectToAction("OnlineList");
        }
        public ActionResult OnlineList()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            return View();
        }
        [HttpPost]
        public ActionResult OnlineList(DataSourceRequest command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();
            var customers = _customerService.GetOnlineCustomers(DateTime.Now.AddMinutes(-_customerSettings.OnlineCustomerMinutes),
                null, command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = customers.Select(x => new OnlineCustomerModel
                {
                    Id=x.Id,
                    CustomerInfo= x.IsRegistered() ? x.Email:"匿名用户",
                    LastIpAddress=x.LastIpAddress,
                    Location="",
                    LastActivityDate=x.LastActivityDate,
                    LastVisitedPage=""
                }),
                Total = customers.TotalCount
            };
            return Json(gridModel);
        }
        #endregion

    }
}