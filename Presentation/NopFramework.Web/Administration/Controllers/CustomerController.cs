using NopFramework.Admin.Controllers;
using NopFramework.Admin.Extensions;
using NopFramework.Admin.Models.Customers;
using NopFramework.Core;
using NopFramework.Core.Domains.Customers;
using NopFramework.Services.Authentication;
using NopFramework.Services.Authentication.External;
using NopFramework.Services.Common;
using NopFramework.Services.Customers;
using NopFramework.Services.Security;
using NopFramework.Web.Framework.Controllers;
using NopFramework.Web.Framework.Kendoui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Controllers
{
    public class CustomerController : BaseAdminController
    {
        #region 声明实例
        ICustomerService _customerService;
        private readonly IWorkContext _workContext;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly CustomerSettings _customerSettings;
        private readonly IPermissionService _permissionService;
        #endregion
        #region 构造函数
        public CustomerController(ICustomerService customerService,
            IWorkContext workContext,
            IAuthenticationService authenticationService,
            CustomerSettings customerSettings,
            ICustomerRegistrationService customerRegistrationService,
            IPermissionService permissionService)
        {
            _customerService = customerService;
            this._workContext = workContext;
            this._authenticationService = authenticationService;
            this._customerSettings = customerSettings;
            this._customerRegistrationService = customerRegistrationService;
            this._permissionService = permissionService;
        }
        #endregion
        // GET: Customer
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        #region 方法

        public ActionResult List()
        {
            var defaultRoleIds = new List<int> { _customerService.GetCustomerRoleBySystemName
                (SystemCustomerRoleNames.Registered).Id };
            //var defaultRoleIds = new List<int> { 1, 2, 3 };
            var model = new CustomerListModel
            {
                SearchCustomerRoleIds = defaultRoleIds
            };
            var allRoles = _customerService.GetAllCustomerRoles();
            foreach (var role in allRoles)
            {
                model.AvailableCustomerRoles.Add(new SelectListItem
                {
                    Text = role.Name,
                    Value = role.Id.ToString(),
                    Selected = defaultRoleIds.Any(x => x == role.Id)
                });
            }
            //for (int i = 0; i < 3; i++)
            //{
            //    model.AvailableCustomerRoles.Add(new SelectListItem
            //    {
            //        Text = "role" + i,
            //        Value = i.ToString(),
            //        Selected = false
            //    });
            //}
            return View(model);
        }
        public ActionResult CustomerList(DataSourceRequest command, CustomerListModel model,
            int[] searchCustomerRoleIds)
        {
            var searchDayOfBirth = 0;
            int searchMonthOfBirth = 0;
            if (!String.IsNullOrWhiteSpace(model.SearchDayOfBirth))
                searchDayOfBirth = Convert.ToInt32(model.SearchDayOfBirth);
            if (!String.IsNullOrWhiteSpace(model.SearchMonthOfBirth))
                searchMonthOfBirth = Convert.ToInt32(model.SearchMonthOfBirth);
            var customers = _customerService.GetAllCustomers(
                customerRoleIds: searchCustomerRoleIds,
                email: model.SearchEmail, userName: model.SearchUserName,
                firstName: model.SearchFirstName,
                lastName: model.SearchLastName,
                dayOfBirth: searchDayOfBirth,
                monthOfBirth: searchMonthOfBirth,
                phone: model.SearchPhone,
                ipAddress: model.SearchIpAddress,
                pageIndex: command.Page - 1,
                pageSize: command.PageSize);
            var gridModel = new DataSourceResult
            {
                Data = customers.Select(PrepareCustomerModelForList),
                Total = customers.TotalCount
            };
            return Json(gridModel);
        }
        public ActionResult Create()
        {
            var model = PrepareCustomerModel(null, false);
            //for (int i = 0; i < 5; i++)
            //{
            //    model.AvailableCustomerRoles.Add(new SelectListItem
            //    {
            //        Text = "role" + i,
            //        Value = i.ToString(),
            //        Selected = false
            //    });
            //}

            //var allRoles = _customerService.GetAllCustomerRoles(true);
            //foreach (var role in allRoles)
            //{
            //    model.AvailableCustomerRoles.Add(new SelectListItem
            //    {
            //        Text = role.Name,
            //        Value = role.Id.ToString()
            //        //Selected = model.SelectedCustomerRoleIds.Contains(role.Id)
            //    });
            //}
            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateInput(false)]
        public ActionResult Create(CustomerModel model, bool continueEditing, FormCollection form)
        {
            if (!String.IsNullOrEmpty(model.Email))
            {
                var cust = _customerService.GetCustomerByEmail(model.Email);
                if (cust != null)
                    ModelState.AddModelError("", "Email is already registered");
            }
            //validate customer roles
            var allCustomerRoles = _customerService.GetAllCustomerRoles(true);
            var newCustomerRoles = new List<CustomerRole>();
            foreach (var role in allCustomerRoles)
            {
                if (model.SelectedCustomerRoleIds.Contains(role.Id))
                    newCustomerRoles.Add(role);
            }
            if (ModelState.IsValid)
            {
                var customer = new Customer
                {
                    CustomerGuid = Guid.NewGuid(),
                    Email = model.Email,
                    CreatedOn = DateTime.Now,
                    LastActivityDate = DateTime.Now,
                    IsActive = model.Active,
                    UserName = model.UserName,
                    AdminComment = model.AdminComment
                };
                _customerService.InsertCustomer(customer);
                
                foreach (var role in newCustomerRoles)
                {
                    if (role.SystemName == SystemCustomerRoleNames.Administrators &&
                        !_workContext.CurrentCustomer.IsAdmin())
                        continue;

                    customer.CustomerRoles.Add(role);
                }
                //password
                if (!String.IsNullOrWhiteSpace(model.Password))
                {
                    var changePassReqeust = new ChangePasswordRequest(model.Email, false, _customerSettings.DefaultPasswordFormat
                        , model.Password);
                    var changePassResult = _customerRegistrationService.ChangePassword(changePassReqeust);
                    if (!changePassResult.Success)
                    {
                        foreach (var changePassError in changePassResult.Errors)
                            ErrorNotification(changePassError);
                    }
                }
                _customerService.UpdateCustomer(customer);
            }

            return View(model);
        }
        public ActionResult Edit(int id)
        {
            var customer = _customerService.GetCustomerById(id);
            if (customer == null || customer.Deleted)
                return RedirectToAction("List");
            var model = PrepareCustomerModel(customer, false);

            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]        
        [ValidateInput(false)]
        public ActionResult Edit(CustomerModel model, bool continueEditing, FormCollection form)
        {
            var customer = _customerService.GetCustomerById(model.Id);
            if (customer == null || customer.Deleted)
                return RedirectToAction("List");
            customer = model.ToEntity(customer);
            customer.UserName = model.UserName;
            customer.Email = model.Email;
            if (ModelState.IsValid)
            {
                try
                {
                    ///用户角色
                    var allRoles = _customerService.GetAllCustomerRoles(true);
                    foreach (var role in allRoles)
                    {
                        ///不能添加管理员角色
                        if (role.SystemName == SystemCustomerRoleNames.Administrators
                            && !_workContext.CurrentCustomer.IsAdmin())
                        {
                            ErrorNotification("非管理员角色不能添加管理员");
                            continue;
                        }
                        else
                        {
                            if (model.SelectedCustomerRoleIds.Contains(role.Id))
                            {
                                if (customer.CustomerRoles.Count(cr => cr.Id == role.Id) == 0)
                                    customer.CustomerRoles.Add(role);
                            }
                            else
                            {
                                if (role.SystemName == SystemCustomerRoleNames.Administrators)
                                {
                                    ErrorNotification("不能删除管理员角色");
                                    continue;
                                }
                                if (customer.CustomerRoles.Count(cr => cr.Id == role.Id) > 0)
                                    customer.CustomerRoles.Remove(role);
                            }
                        }
                    }
                    _customerService.UpdateCustomer(customer);
                    SuccessNotification("修改成功");
                    if (continueEditing)
                    {
                        //selected tab
                        //SaveSelectedTabName();

                        return RedirectToAction("Edit", new { id = customer.Id });
                    }
                    return RedirectToAction("List");
                }
                catch (Exception exc)
                {
                    ErrorNotification(exc.Message, false);
                }
            }

            return View(model);
        }

        [HttpPost, ActionName("Edit")]
        [FormValueRequired("changepassword")]
        public ActionResult ChangePassword(CustomerModel customer)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageCustomers))
                return AccessDeniedView();

            var model = _customerService.GetCustomerById(customer.Id);
            if (customer == null)
                //No customer found with the specified id
                return RedirectToAction("List");
            if (model.IsAdmin()&&!_workContext.CurrentCustomer.IsAdmin())
            {
                ErrorNotification("只有管理员可以修改密码");
                return RedirectToAction("Edit", new { id = customer.Id });
            }
            if (ModelState.IsValid)
            {
                var changePassReqeust = new ChangePasswordRequest(customer.Email, false,
                    _customerSettings.DefaultPasswordFormat
                       , customer.Password);
                var changePassResult = _customerRegistrationService.ChangePassword(changePassReqeust);
                if (!changePassResult.Success)
                {
                    foreach (var changePassError in changePassResult.Errors)
                        ErrorNotification(changePassError);
                }
                else {
                    SuccessNotification("密码修改成功");
                }
            }
            return RedirectToAction("Edit", new { id = customer.Id });
        }
        /// <summary>
        /// 删除所选用户
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (selectedIds != null)
            {
                var customers = _customerService.GetCustomersbyIds(selectedIds.ToArray());
                foreach (var customer in customers)
                {
                    _customerService.DeleteCustomer(customer);
                }

            }
            return Json(new { Result = true });
        }
        #endregion

        #region 帮助方法
        [NonAction]
        protected virtual CustomerModel PrepareCustomerModelForList(Customer customer)
        {
            var allRoles = _customerService.GetAllCustomerRoles(true);
            string customerRoleNames = "";
            foreach (var role in allRoles)
            {
                if (customer.CustomerRoles.Contains(role))
                    customerRoleNames += role.Name + " ";
            }
            CustomerModel model = new CustomerModel();
            model = customer.ToModel();
            model.Phone = customer.GetAttribute<string>(SystemCustomerAttributeNames.Company);
            model.Company = customer.GetAttribute<string>(SystemCustomerAttributeNames.Company);
            model.FullName = customer.GetFullName();
            model.CustomerRoleNames = customerRoleNames;
            //model.Id = customer.Id;
            //customer roles
            
            //var customerModel = new CustomerModel
            //{
            //    Id = customer.Id,
            //    CustomerRoleNames = customerRoleNames,
            //    Email = customer.IsRegistered() ? customer.Email : "Guest@google.com",
            //    UserName = customer.UserName,
            //    FullName = customer.GetFullName(),
            //    Company = customer.GetAttribute<string>(SystemCustomerAttributeNames.Company),
            //    Phone = customer.GetAttribute<string>(SystemCustomerAttributeNames.Phone),
            //    LastIpAddress = customer.LastIpAddress,
            //    Active = customer.IsActive,
            //};

            return model;
        }
        [NonAction]
        protected virtual CustomerModel PrepareCustomerModel( Customer customer, bool excludeProperties)
        {
            CustomerModel model =new CustomerModel();
            if (customer != null)
            {
                model.Id = customer.Id;
                //customer roles
                model = customer.ToModel();
                //if (!excludeProperties)
                //{
                //    model.Email = customer.Email;
                //    model.UserName = customer.UserName;
                //    model.AdminComment = customer.AdminComment;
                //    model.Active = customer.IsActive;
                model.SelectedCustomerRoleIds = customer.CustomerRoles.Select(cr => cr.Id).ToList();
                //}
            }
            var allRoles = _customerService.GetAllCustomerRoles(true);
            var adminRole = allRoles.FirstOrDefault(c => c.SystemName == SystemCustomerRoleNames.Registered);
            //precheck Registered Role as a default role while creating a new customer through admin
            if (customer == null && adminRole != null)
            {
                model.SelectedCustomerRoleIds.Add(adminRole.Id);
            }
            foreach (var role in allRoles)
            {
                model.AvailableCustomerRoles.Add(new SelectListItem
                {
                    Text = role.Name,
                    Value = role.Id.ToString(),
                    Selected = model.SelectedCustomerRoleIds.Contains(role.Id)
                });
            }
            return model;
        }
        #endregion

    }
}