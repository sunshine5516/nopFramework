using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Customers
{
    public class CustomerListModel : BaseNopFrameworkModel
    {
        [UIHint("MultiSelect")]
        [NopFrameworkResourceDisplayName("客户角色")]
        public IList<int> SearchCustomerRoleIds { get; set; }
        public IList<SelectListItem> AvailableCustomerRoles { get; set; }
        public CustomerListModel()
        {
            SearchCustomerRoleIds = new List<int>();
            AvailableCustomerRoles = new List<SelectListItem>();
        }
        [NopFrameworkResourceDisplayName("电子邮箱")]
        [AllowHtml]
        public string SearchEmail { get; set; }
        [NopFrameworkResourceDisplayName("用户名")]
        [AllowHtml]
        public string SearchUserName { get; set; }
        public bool UsernamesEnabled { get; set; }
        [NopFrameworkResourceDisplayName("姓氏")]
        [AllowHtml]
        public string SearchFirstName { get; set; }
        [NopFrameworkResourceDisplayName("名称")]
        [AllowHtml]
        public string SearchLastName { get; set; }
        [NopFrameworkResourceDisplayName("出生日期")]
        [AllowHtml]
        public string SearchDayOfBirth { get; set; }
        [NopFrameworkResourceDisplayName("出生日期")]
        [AllowHtml]
        public string SearchMonthOfBirth { get; set; }
        public bool DateOfBirthEnabled { get; set; }
        [NopFrameworkResourceDisplayName("公司")]
        [AllowHtml]
        public string SearchCompany { get; set; }
        public bool CompanyEnabled { get; set; }
        [NopFrameworkResourceDisplayName("电话")]
        [AllowHtml]
        public string SearchPhone { get; set; }
        public bool PhoneEnabled { get; set; }

        [NopFrameworkResourceDisplayName("SearchZipCode")]
        [AllowHtml]
        public string SearchZipPostalCode { get; set; }
        public bool ZipPostalCodeEnabled { get; set; }

        [NopFrameworkResourceDisplayName("IP地址")]
        public string SearchIpAddress { get; set; }

    }
}