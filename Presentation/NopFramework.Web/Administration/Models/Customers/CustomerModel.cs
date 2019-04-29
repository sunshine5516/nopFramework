using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Customers
{
    public partial class CustomerModel : BaseNopFrameworkEntityModel
    {
        public CustomerModel()
        {
            this.AvailableCustomerRoles = new List<SelectListItem>();
            this.SelectedCustomerRoleIds = new List<int>();
        }
        public bool AllowUserToChangeUsernames { get; set; }
        
        public bool UsernamesEnabled { get; set; }
        [NopFrameworkResourceDisplayName("用户名")]
        [AllowHtml]
        public string UserName { get; set; }
        [NopFrameworkResourceDisplayName("电子邮箱")]
        [AllowHtml]
        public string Email { get; set; }
        [NopFrameworkResourceDisplayName("密码")]

        [AllowHtml]
        public string Password { get; set; }

        [NopFrameworkResourceDisplayName("管理员备注")]
        [AllowHtml]
        public string AdminComment { get; set; }
        public bool GenderEnabled { get; set; }
        [NopFrameworkResourceDisplayName("性别")]
        public string Gender { get; set; }
        [NopFrameworkResourceDisplayName("姓氏")]
        [AllowHtml]
        public string FirstName { get; set;}
        [NopFrameworkResourceDisplayName("名称")]
        [AllowHtml]
        public string LastName { get; set; }
        [NopFrameworkResourceDisplayName("全名")]
        [AllowHtml]
        public string FullName { get; set; }
        public bool DateOfBirthEnabled { get; set; }
        [UIHint("DateNullable")]
        [NopFrameworkResourceDisplayName("出生日期")]
        public DateTime? DateOfBirth { get; set; }
        public bool CompanyEnabled { get; set; }
        [NopFrameworkResourceDisplayName("公司")]
        [AllowHtml]
        public string Company { get; set; }
        public bool StreetAddressEnable { get; set; }
        [NopFrameworkResourceDisplayName("街道地址")]
        [AllowHtml]
        public string StreetAddress { get; set; }
        public bool ZipPostalCodeEnabled { get; set; }
        [NopFrameworkResourceDisplayName("邮政编码")]
        [AllowHtml]
        public string ZipPostalCode { get; set; }
        public bool CityEnable { get; set; }
        [NopFrameworkResourceDisplayName("城市")]
        [AllowHtml]
        public string City { get; set; }
        public bool PhoneEnable { get; set; }
        [NopFrameworkResourceDisplayName("电话")]
        [AllowHtml]
        public string Phone { get; set; }
        [NopFrameworkResourceDisplayName("是否启用？")]
        public bool Active { get; set; }
        [NopFrameworkResourceDisplayName("创建时间")]
        public DateTime? CreatedOn { get; set;}
        public DateTime LastActivityDate { get; set; }
        [NopFrameworkResourceDisplayName("IP地址")]
        public string LastIpAddress { get; set; }
        [NopFrameworkResourceDisplayName("最后访问页面")]
        public string LastVisitedPage { get; set; }
        [NopFrameworkResourceDisplayName("用户角色")]
        public string CustomerRoleNames { get; set; }
        public List<SelectListItem> AvailableCustomerRoles { get; set; }
        [NopFrameworkResourceDisplayName("客户角色")]
        [UIHint("MultiSelect")]
        public IList<int> SelectedCustomerRoleIds { get; set; }
        //public string SendEmail

    }
}