using FluentValidation;
using NopFramework.Web.Framework.Validators;
using NopFramework.Web.Models.Customer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Web.Validators.Customer
{
    public partial class RegisterValidator : BaseNopFrameworkValidator<RegisterModel>
    {
        public RegisterValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("邮箱不能为空");
            RuleFor(x => x.Email).EmailAddress().WithMessage("邮箱格式不正确");
            RuleFor(x => x.UserName).NotEmpty().WithMessage("用户名不能为空");
            RuleFor(x => x.Password).NotEmpty().WithMessage("请输入密码");
            RuleFor(x => x.Password).Length(1, 999).WithMessage("密码长度至少6 个字符");
            RuleFor(x => x.ConfirmPassword).NotEmpty().WithMessage("请输入确认密码");
            RuleFor(x => x.ConfirmPassword).Equal(x => x.Password).WithMessage("两次密码不匹配，请重新输入");
        }
    }
}