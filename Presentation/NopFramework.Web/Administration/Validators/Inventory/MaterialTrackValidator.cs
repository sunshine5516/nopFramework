using FluentValidation;
using NopFramework.Admin.Models.Inventory;
using NopFramework.Data;
using NopFramework.Web.Framework.Validators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Validators.Inventory
{
    public class MaterialTrackValidator : BaseNopFrameworkValidator<MaterialTrackModel>
    {
        public MaterialTrackValidator(IDbContext dbContext)
        {
            RuleFor(m=>m.Description).NotEmpty().WithMessage("描述信息不能为空");
            RuleFor(x => x.Number).NotEmpty();
            SetStringPropertiesMaxLength<MaterialTrackModel>(dbContext);
        }
    }
}