using NopFramework.Core;
using NopFramework.Core.Domains.Customers;
using NopFramework.Services.Authentication;
using NopFramework.Services.Authentication.External;
using NopFramework.Services.Customers;
using NopFramework.Web.Models.Customer;
using System;
using System.Web.Mvc;

namespace NopFramework.Web.Controllers
{
    public class CustomerController : Controller
    {
        #region 声明实例
        ICustomerService _customerService;
        private readonly IWorkContext _workContext;
        private readonly IAuthenticationService _authenticationService;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly CustomerSettings _customerSettings;
        #endregion
        #region 构造函数
        public CustomerController(ICustomerService customerService,
            IWorkContext workContext,
            IAuthenticationService authenticationService,
            ICustomerRegistrationService customerRegistrationService,
            CustomerSettings customerSettings)
        {
            _customerService = customerService;
            this._workContext = workContext;
            this._authenticationService = authenticationService;
            this._customerRegistrationService = customerRegistrationService;
            _customerSettings = customerSettings;
        }
        #endregion
        #region 注销/登录
        /// <summary>
        /// 注销
        /// </summary>
        /// <returns></returns>
        public ActionResult Logout()
        {
            ExternalAuthorizerHelper.RemoveParameters();
            if (_workContext.OriginalCustomerIfImpersonated != null)
            { }
            _authenticationService.SignOut();
            return RedirectToRoute("HomePage");
        }
        /// <summary>
        /// 登录
        /// </summary>
        /// <returns></returns>
        public ActionResult Logon()
        {
            var model = new LoginModel();
            model.DisplayCaptcha = true;
            return View(model);
        }
        [HttpPost]
        public ActionResult Logon(LoginModel model,string returnUrl)
        {
            if (ModelState.IsValid)
            {
                var loginResult = _customerRegistrationService.ValidateCustomer(model.Email, model.Password);
                switch (loginResult)
                {
                    case CustomerLoginResults.Successful:
                        var customer = _customerService.GetCustomerByEmail(model.Email);
                        _authenticationService.SignIn(customer, model.RemberMe);
                        if (String.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                            return RedirectToRoute("HomePage");

                        return Redirect(returnUrl);                        
                    case CustomerLoginResults.CustomerNotExist:
                        ModelState.AddModelError("", "用户不存在");
                        break;
                    case CustomerLoginResults.Deleted:
                        ModelState.AddModelError("", "用户已被删除");
                        break;
                    case CustomerLoginResults.NotActive:
                        ModelState.AddModelError("", "帐户尚未激活");
                        break;
                    case CustomerLoginResults.NotRegistered:
                        ModelState.AddModelError("", "用户尚未注册");
                        break;
                    case CustomerLoginResults.WrongPassword:
                        ModelState.AddModelError("", "密码错误");
                        break;
                    default:
                        ModelState.AddModelError("", "验证错误");
                        break;
                }
            }
            
            return View(model);
        }
        #endregion
        #region 注册
        public ActionResult Register()
        {
            var model = new RegisterModel();
            model.DisplayCaptcha = true;
            return View(model);
        }
        [HttpPost]
        public ActionResult Register(RegisterModel model, string returnUrl,FormCollection form)
        {
            //var customer=new Customer();
            //var registrationRequest = new CustomerRegistrationRequest(customer,
            //       model.Email,
            //       model.UserName,
            //       model.Password,
            //       _customerSettings.DefaultPasswordFormat,
            //       true
            //       );
            //var registrationResult = _customerRegistrationService.RegisterCustomer(registrationRequest);



            if (_workContext.CurrentCustomer.IsRegistered())
            {
                _authenticationService.SignOut();
                _workContext.CurrentCustomer = _customerService.InsertGuestCustomer();
            }
            var customer = _workContext.CurrentCustomer;
            if (ModelState.IsValid)
            {
                var registrationRequest = new CustomerRegistrationRequest(customer,
                    model.Email,
                    model.UserName,
                    model.Password,
                    _customerSettings.DefaultPasswordFormat,
                    true
                    );
                var registrationResult = _customerRegistrationService.RegisterCustomer(registrationRequest);
                if (registrationResult.Success)
                {
                    //login customer now
                    //if (isApproved)
                    _authenticationService.SignIn(customer, true);
                    switch (_customerSettings.UserRegistrationType)
                    {
                        case UserRegistrationType.Standard:
                            var redirectUrl=Url.RouteUrl("RegisterResult", new { resultId = (int)UserRegistrationType.Standard });
                            return Redirect(redirectUrl);
                        default:
                            {
                                return RedirectToRoute("HomePage");
                            }
                    }
                }
                //错误信息
                foreach (var error in registrationResult.Errors)
                    ModelState.AddModelError("", error);
            }
            return View(model);
        }

        public ActionResult RegisterResult(int resultId)
        {
            var resultText = "";
            switch ((UserRegistrationType)resultId)
            {
                case UserRegistrationType.Disabled:
                    break;
                case UserRegistrationType.Standard:
                    resultText = "注册成功";
                    break;
                default:
                    break;
            }
            var model = new RegisterResultModel
            {
                Result = resultText
            };
            return View(model);
        }
        [HttpPost]
        public ActionResult RegisterResult(string returnUrl)
        {
            if (String.IsNullOrEmpty(returnUrl) || !Url.IsLocalUrl(returnUrl))
                return RedirectToRoute("HomePage");

            return Redirect(returnUrl);
        }
        #endregion
        // GET: Customer
        public ActionResult Index()
        {
            return View();
        }
    }
}