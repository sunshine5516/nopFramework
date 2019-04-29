using FluentValidation;
using NopFramework.Web.Framework.Validators;
using NopFramework.Web.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Web.Validators.Customer
{
    public partial class LoginValidator: BaseNopFrameworkValidator<LoginModel>
    {
        public LoginValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("邮箱不能为空");
            RuleFor(x => x.Email).EmailAddress().WithMessage("邮箱格式不正确");
            RuleFor(x => x.Password).NotEmpty().WithMessage("密码不能为空");
        }
    }
}