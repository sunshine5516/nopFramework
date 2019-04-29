using FluentValidation;
using NopFramework.Admin.Models.News;
using NopFramework.Web.Framework.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Validators.News
{
    public partial class NewsItemValidator: BaseNopFrameworkValidator<NewsItemModel>
    {
        public NewsItemValidator()
        {
            RuleFor(n => n.Title).NotEmpty().WithMessage("标题不能为空");
            RuleFor(n => n.Contents).NotEmpty().WithMessage("内容不能为空");
        }
    }
}