using FluentValidation;
using NopFramework.Admin.Models.Organization;
using NopFramework.Web.Framework.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Validators.Organization
{
    public class DepartmentValidator : BaseNopFrameworkValidator<DepartmentModel>
    {
        public DepartmentValidator()
        {
            RuleFor(n => n.DepartmentName).NotEmpty().WithMessage("标题不能为空");
            RuleFor(n => n.Description).NotEmpty().WithMessage("内容不能为空");
        }
    }
}