using System;
using System.Linq;
using NopFramework.Core.Domains.Customers;
using NopFramework.Services.Security;
using NopFramework.Core;

namespace NopFramework.Services.Customers
{
    public partial class CustomerRegistrationService : ICustomerRegistrationService
    {
        #region 声明实例
        private readonly ICustomerService _customerService;
        private readonly IEncryptionService _encryptionService;
        private readonly CustomerSettings _customerSettings;
        #endregion
        #region 构造函数
        public CustomerRegistrationService(ICustomerService customerService,
            IEncryptionService encryptionService,
            CustomerSettings customerSettings)
        {
            this._customerService = customerService;
            this._encryptionService = encryptionService;
            this._customerSettings = customerSettings;
        }


        #endregion
        #region 方法
        /// <summary>
        /// 验证用户登录
        /// </summary>
        /// <param name="userNameOrEmail"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public CustomerLoginResults ValidateCustomer(string userNameOrEmail, string password)
        {
            var customer = _customerService.GetCustomerByEmail(userNameOrEmail);
            if (customer == null)
                return CustomerLoginResults.CustomerNotExist;
            if (customer.Deleted)
                return CustomerLoginResults.Deleted;
            if (!customer.IsActive)
                return CustomerLoginResults.NotActive;
            //是否注册
            if (!customer.IsRegistered())
                return CustomerLoginResults.NotRegistered;
            string pwd = "";
            switch (customer.PasswordFormat)
            {
                case PasswordFormat.Hashed:
                    pwd = _encryptionService.CreatePasswordHash(password, customer.PasswordSalt);
                    break;
                case PasswordFormat.Encrypted:
                    pwd = _encryptionService.EncryptText(password);
                    
                    break;
                default:
                    pwd = customer.Password;
                    break;
            }
            bool isValid = pwd == customer.Password;
            if (!isValid)
                return CustomerLoginResults.WrongPassword;
            customer.LastLoginDate = DateTime.Now;
            _customerService.UpdateCustomer(customer);
            return CustomerLoginResults.Successful;
        }
        /// <summary>
        /// 验证用户注册信息
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public CustomerRegistrationResult RegisterCustomer(CustomerRegistrationRequest request)
        {
            //_customerService.InsertCustomer(new Customer { IsActive=true,CreatedOn=DateTime.Now,Email="9000@",Deleted=false,LastActivityDate=DateTime.Now,LastLoginDate=DateTime.Now});
            if (request == null)
                throw new ArgumentNullException("request");
            if (request.Customer == null)
                throw new ArgumentException("Can't load current customer");
            var result = new CustomerRegistrationResult();
            if (request.Customer.IsRegistered())
            {
                result.AddErrors("用户已经注册");
                return result;
            }
            if (String.IsNullOrEmpty(request.Email))
            {
                result.AddErrors("邮箱不能为空");
                return result;
            }
            if (!CommonHelper.IsValidEmail(request.Email))
            {
                result.AddErrors("邮箱格式不正确");
                return result;
            }
            if (String.IsNullOrEmpty(request.UserName))
            {
                result.AddErrors("用户名不能为空");
                return result;
            }
            if (String.IsNullOrWhiteSpace(request.Password))
            {
                result.AddErrors("密码不能为空");
                return result;
            }
            if (_customerService.GetCustomerByEmail(request.Email) != null)//唯一性验证
            {
                result.AddErrors("该用户已经存在");
                return result;
            }
            request.Customer.Email = request.Email;
            request.Customer.UserName = request.UserName;
            request.Customer.PasswordFormat = request.PasswordFormat;
            switch (request.PasswordFormat)
            {
                case PasswordFormat.Clear:
                    request.Customer.Password = request.Password;
                    break;
                case PasswordFormat.Encrypted:
                    request.Customer.Password = _encryptionService.EncryptText(request.Password);
                    break;
                case PasswordFormat.Hashed:
                    {
                        string saltKey = _encryptionService.CreateSaltKey(5);
                        request.Customer.PasswordSalt = saltKey;
                        request.Customer.Password = _encryptionService.CreatePasswordHash(request.Password,
                            saltKey, _customerSettings.HashedPasswordFormat);
                    }
                    break;
                default:
                    break;
            }
            request.Customer.IsActive = true;
            ///添加“注册”角色
            var registeredRole = _customerService.GetCustomerRoleBySystemName(SystemCustomerRoleNames.Registered);
            if (registeredRole == null)
                throw new SunFrameworkException("'Registered' role could not be loaded");
            request.Customer.CustomerRoles.Add(registeredRole);
            ///移除客户角色
            var guestRole = request.Customer.CustomerRoles.FirstOrDefault(cr => cr.SystemName == SystemCustomerRoleNames.Guests);
            if (guestRole != null)
                request.Customer.CustomerRoles.Remove(guestRole);
            _customerService.UpdateCustomer(request.Customer);
            return result;
        }

        public virtual ChangePasswordResult ChangePassword(ChangePasswordRequest request)
        {

            if (request == null)
                throw new ArgumentNullException("request");

            var result = new ChangePasswordResult();
            if (String.IsNullOrWhiteSpace(request.Email))
            {
                result.AddError("邮箱不能为空");
                return result;
            }
            if (String.IsNullOrWhiteSpace(request.NewPassword))
            {
                result.AddError("密码不能为空");
                return result;
            }
            var customer = _customerService.GetCustomerByEmail(request.Email);
            if (customer == null)
            {
                result.AddError("未找到该用户");
                return result;
            }


            var requestIsValid = false;
            if (request.ValidateRequest)
            {
                string oldPwd;
                switch (request.NewPasswordFormat)
                {
                    case PasswordFormat.Hashed:
                        oldPwd = _encryptionService.CreatePasswordHash(request.OldPassword, customer.PasswordSalt, _customerSettings.HashedPasswordFormat);
                        break;
                    case PasswordFormat.Encrypted:
                        oldPwd = _encryptionService.EncryptText(request.OldPassword);
                        break;
                    default:
                        oldPwd = customer.Password;
                        break;

                }
                bool oldPasswordIsValid = oldPwd == customer.Password;
                if (!oldPasswordIsValid)
                    result.AddError("密码不正确");

                if (oldPasswordIsValid)
                    requestIsValid = true;
            }
            else
            {
                requestIsValid = true;
                
            }
            if (requestIsValid)
            {
                switch (request.NewPasswordFormat)
                {
                    case PasswordFormat.Hashed:
                        string saltKey = _encryptionService.CreateSaltKey(5);
                        customer.PasswordSalt = saltKey;
                        customer.Password = _encryptionService.CreatePasswordHash(request.NewPassword, saltKey, _customerSettings.HashedPasswordFormat);
                        break;
                    case PasswordFormat.Encrypted:
                        customer.Password = _encryptionService.EncryptText(request.NewPassword);
                        break;
                    default:
                        customer.Password = request.NewPassword;
                        break;
                }
                customer.PasswordFormat = request.NewPasswordFormat;
                _customerService.UpdateCustomer(customer);
            }
            return result;
        }
        #endregion

    }
}
