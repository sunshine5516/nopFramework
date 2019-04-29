using NopFramework.Admin.Controllers;
using NopFramework.Services.Customers;
using NopFramework.Web.Framework.Kendoui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NopFramework.Core.Domains.Customers;
using NopFramework.Admin.Models.Customers;
using NopFramework.Admin.Extensions;
using NopFramework.Web.Framework.Controllers;

namespace NopFramework.Admin.Controllers
{
    public class CustomerRoleController : BaseAdminController
    {
        #region 声明实例
        private readonly ICustomerService _customerService;
        #endregion
        #region 构造函数
        public CustomerRoleController(ICustomerService customerService)
        {
            this._customerService = customerService;
        }
        #endregion
        #region 方法
        // GET: CustomerRole
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        public ActionResult List()
        {
            return View();
        }
        public ActionResult RoleList(DataSourceRequest command)
        {
            var customreRoles = _customerService.GetAllPagedList(command.Page - 1, command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = customreRoles.Select(PrepareCustomerRoleModel),
                Total = customreRoles.Count()
            };
            return new JsonResult
            {
                Data = gridModel
            };
        }
        public ActionResult Create()
        {
            var model = new CustomerRoleModel();
            model.IsActive = true;
            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(CustomerRoleModel model, bool continueEditing)
        {
            if (ModelState.IsValid)
            {
                model.IsActive = true;
                var customerRole = model.ToEntity();
                _customerService.InsertCustomerRole(customerRole);
                return continueEditing ? RedirectToAction("Edit", new { id = customerRole.Id }) : RedirectToAction("List");
            }

            //If we got this far, something failed, redisplay form
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            var customerRole = _customerService.GetCustomerRoleById(id);
            if (customerRole == null)
                //No customer role found with the specified id
                return RedirectToAction("List");

            var model = PrepareCustomerRoleModel(customerRole);
            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Edit(CustomerRoleModel model, bool continueEditing)
        {
            var customerRole = _customerService.GetCustomerRoleById(model.Id);
            if (customerRole == null)
                //No customer role found with the specified id
                return RedirectToAction("List");
            customerRole = model.ToEntity(customerRole);
            _customerService.UpdateCustomerRole(customerRole);
            return continueEditing ? RedirectToAction("Edit", new { id = customerRole.Id }) : RedirectToAction("List");
        }
        [HttpPost]
        public ActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (selectedIds != null)
            {
                _customerService.DeleteCustomerRoles(_customerService.GetCustomerRoleByIds(selectedIds.ToArray()));
            }
            return Json(new { Result = true });
        }
        #endregion
        #region 辅助方法
        [NonAction]
        private CustomerRoleModel PrepareCustomerRoleModel(CustomerRole customerRole)
        {
            var model=customerRole.ToModel();

            return model;
        }
        #endregion

    }
}