using FluentValidation;
using NopFramework.Web.Framework.Validators;
using NopFramework.Web.Models.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Web.Validators.Message
{
    public partial class ContactUsValidator : BaseNopFrameworkValidator<ContactUsModel>
    {
        public ContactUsValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("邮箱不能为空");
            RuleFor(x => x.Email).EmailAddress().WithMessage("邮箱格式不正确");
            RuleFor(x => x.Name).NotEmpty().WithMessage("姓名不能为空");
            RuleFor(x => x.Telephone).NotEmpty().WithMessage("电话不能为空");           
            RuleFor(x => x.Content).NotEmpty().WithMessage("内容不能为空");

        }
    }
}