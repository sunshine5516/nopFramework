using FluentValidation.Attributes;
using NopFramework.Web.Framework.Mvc;
using NopFramework.Web.Validators.Customer;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Web.Models.Customer
{
    [Validator(typeof(LoginValidator))]
    public partial class LoginModel : BaseNopFrameworkEntityModel
    {
        [DisplayNameAttribute("电子邮箱")]
        [AllowHtml]
        public string Email { get; set; }
        [DisplayNameAttribute("用户名")]
        [AllowHtml]
        public string UserName { get; set; }
        [DataType(DataType.Password)]
        [DisplayNameAttribute("密码")]
        [AllowHtml]
        public string Password { get; set; }
        [DisplayNameAttribute("记住我?")]
        [AllowHtml]
        public bool RemberMe { get; set; }
        public bool DisplayCaptcha { get; set; }
    }
}