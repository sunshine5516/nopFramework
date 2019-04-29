using NopFramework.Core;
using NopFramework.Core.Domains.Customers;
using NopFramework.Services.Customers;
using NopFramework.Services.Events;
using NopFramework.Services.Logging;
namespace NopFramework.Services.Authentication.External
{
    public partial class ExternalAuthorizer : IExternalAuthorizer
    {
        #region 声明实例
        private readonly IAuthenticationService _authenticationService;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IWorkContext _workContext;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly CustomerSettings _customerSettings;
        private readonly ExternalAuthenticationSettings _externalAuthenticationSettings;
        private readonly ICustomerRegistrationService _customerRegistrationService;
        private readonly IEventPublisher _eventPublisher;
        #endregion
        #region 构造函数
        public ExternalAuthorizer(IAuthenticationService authenticationService,
            IOpenAuthenticationService openAuthenticationService, IWorkContext workContext,
            CustomerSettings customerSettings, ExternalAuthenticationSettings externalAuthenticationSettings,
            ICustomerRegistrationService customerRegistrationService, IEventPublisher eventPublisher,
            ICustomerActivityService customerActivityService)
        {
            this._authenticationService = authenticationService;
            this._openAuthenticationService = openAuthenticationService;
            this._workContext = workContext;
            this._customerSettings = customerSettings;
            this._externalAuthenticationSettings = externalAuthenticationSettings;
            this._customerRegistrationService = customerRegistrationService;
            this._eventPublisher = eventPublisher;
            this._customerActivityService = customerActivityService;
        }
        #endregion
        #region 方法
        public AuthorizationResult Authorize(OpenAuthenticationParameters parameters)
        {
            var userFound = _openAuthenticationService.GetUser(parameters);
            var userLoggedIn = _workContext.CurrentCustomer.IsRegistered() ? _workContext.CurrentCustomer : null;
            if (AccountAlreadyExists(userFound, userLoggedIn))
            {
                if (AccountIsAssignedToLoggedOnAccount(userFound, userLoggedIn))
                {
                    return new AuthorizationResult(OpenAuthenticationStatus.AutoRegisteredStandard);
                }
                var result = new AuthorizationResult(OpenAuthenticationStatus.Error);
                result.AddError("Account is already assigned");
                return result;
            }
            if (AccountDoesNotExistAndUserIsNotLoggedOn(userFound, userLoggedIn))
            {
                ExternalAuthorizerHelper.StoreParametersForRoundTrip(parameters);
                if (AutoRegistrationIsEnabled())
                {
                    #region 注册用户
                    var currentCustomer = _workContext.CurrentCustomer;
                    var details = new RegistrationDetails(parameters);
                    var randomPassword = CommonHelper.GenerateRandomDigitCode(20);
                    bool isApproved = (_customerSettings.UserRegistrationType == UserRegistrationType.Standard) ||
                        (_customerSettings.UserRegistrationType == UserRegistrationType.EmailValidation &&
                         !_externalAuthenticationSettings.RequireEmailValidation);
                    var registrationRequest = new CustomerRegistrationRequest(currentCustomer,
                        details.EmailAddress, details.EmailAddress,
                        randomPassword, PasswordFormat.Clear,
                        isApproved);
                    var registrationResult = _customerRegistrationService.RegisterCustomer(registrationRequest);
                    if (registrationResult.Success)
                    {
                        userFound = currentCustomer;
                        _openAuthenticationService.AssociateExternalAccountWithUser(currentCustomer, parameters);
                        ExternalAuthorizerHelper.RemoveParameters();
                        if (isApproved)
                            _authenticationService.SignIn(userFound ?? userLoggedIn, false);
                        //_eventPublisher.Publish(new CustomerRegisteredEvent(currentCustomer));
                        if (isApproved)
                        {
                            return new AuthorizationResult(OpenAuthenticationStatus.AutoRegisteredStandard);
                        }
                        else if (_customerSettings.UserRegistrationType == UserRegistrationType.EmailValidation)
                        {
                            return new AuthorizationResult(OpenAuthenticationStatus.AutoRegisteredEmailValidation);
                        }
                        else if (_customerSettings.UserRegistrationType == UserRegistrationType.AdminApproval)
                        {
                            //result
                            return new AuthorizationResult(OpenAuthenticationStatus.AutoRegisteredAdminApproval);
                        }
                    }
                    else
                    {
                        ExternalAuthorizerHelper.RemoveParameters();

                        var result = new AuthorizationResult(OpenAuthenticationStatus.Error);
                        foreach (var error in registrationResult.Errors)
                            result.AddError(string.Format(error));
                        return result;
                    }
                    #endregion
                }
                else if (RegistrationIsEnabled())
                {
                    return new AuthorizationResult(OpenAuthenticationStatus.AssociateOnLogon);
                }
                else
                {
                    ExternalAuthorizerHelper.RemoveParameters();
                    var result = new AuthorizationResult(OpenAuthenticationStatus.Error);
                    result.AddError("Registration is disabled");
                    return result;
                }
            }
            if (userFound == null)
            {
                _openAuthenticationService.AssociateExternalAccountWithUser(userLoggedIn, parameters);
            }
            _authenticationService.SignIn(userFound ?? userLoggedIn, false);
            //发布事件
            _eventPublisher.Publish(new CustomerLoggedinEvent(userFound ?? userLoggedIn));
            //日志
            _customerActivityService.InsertActivity("PublicStore.Login", "登录",
               userFound ?? userLoggedIn);
            return new AuthorizationResult(OpenAuthenticationStatus.Authenticated);
        }
        #endregion
        #region 辅助方法
        private bool AccountAlreadyExists(Customer userFound, Customer userLoggedIn)
        {
            return userFound != null && userLoggedIn != null;
        }
        private bool AccountIsAssignedToLoggedOnAccount(Customer userFound, Customer userLoggedIn)
        {
            return userFound.Id.Equals(userLoggedIn.Id);
        }
        private bool AccountDoesNotExistAndUserIsNotLoggedOn(Customer userFound, Customer userLoggedIn)
        {
            return userFound == null && userLoggedIn == null;
        }
        private bool AutoRegistrationIsEnabled()
        {
            return _customerSettings.UserRegistrationType != UserRegistrationType.Disabled && _externalAuthenticationSettings.AutoRegisterEnabled;
        }
        private bool RegistrationIsEnabled()
        {
            return _customerSettings.UserRegistrationType != UserRegistrationType.Disabled &&
                !_externalAuthenticationSettings.AutoRegisterEnabled;
        }
        #endregion

    }
}
