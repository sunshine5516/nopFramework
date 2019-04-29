using FluentValidation.Attributes;
using NopFramework.Web.Framework.Mvc;
using NopFramework.Web.Validators.Message;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Web.Models.Message
{
    [Validator(typeof(ContactUsValidator))]
    public class ContactUsModel : BaseNopFrameworkEntityModel
    {
        [DisplayNameAttribute("电子邮箱")]
        [AllowHtml]
        public string Email { get; set; }
        [DisplayNameAttribute("电话")]
        [AllowHtml]
        public string Telephone { get; set; }
        [DisplayNameAttribute("姓名")]
        [AllowHtml]
        public string Name { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        //public DateTime CreatedOn { get; set; }
        [DisplayNameAttribute("内容")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Content { get; set; }
        [DisplayNameAttribute("结果")]
        [AllowHtml]
        public string Result { get; set; }
        [DisplayNameAttribute("是否发送")]
        [AllowHtml]
        public bool SuccessfullySent { get; set; }
    }
}