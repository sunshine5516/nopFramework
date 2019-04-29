using FluentValidation;
using NopFramework.Admin.Models.Bugs;
using NopFramework.Web.Framework.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Validators.Bugs
{
    public class BugCreateValidator : BaseNopFrameworkValidator<BugModel>
    {
        public BugCreateValidator()
        {
            RuleFor(x => x.Name).NotEmpty().WithMessage("名称不能为空");
            RuleFor(x => x.Summary).NotEmpty().WithMessage("摘要不能为空");
        }
    }
}