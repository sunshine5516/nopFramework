﻿using FluentValidation.Attributes;
using NopFramework.Admin.Validators.Organization;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Organization
{
    [Validator(typeof(DepartmentValidator))]
    public class DepartmentModel : BaseNopFrameworkEntityModel
    {
        [DisplayNameAttribute("名称")]
        [AllowHtml]
        public string DepartmentName { get; set; }
        [DisplayNameAttribute("描述")]
        [AllowHtml]
        public string Description { get; set; }
    }
}