using NopFramework.Core.Domains.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Customers
{
    /// <summary>
    /// 用户注册需求信息
    /// </summary>
    public class CustomerRegistrationRequest
    {
        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="customer">用户</param>
        /// <param name="email">电子邮件</param>
        /// <param name="username">用户名</param>
        /// <param name="password">密码</param>
        /// <param name="passwordFormat">Password format</param>
        /// <param name="storeId">Store identifier</param>
        /// <param name="isApproved">Is approved</param>
        public CustomerRegistrationRequest(Customer customer, string email, string username,
            string password,
            PasswordFormat passwordFormat,
            bool isApproved = true)
        {
            this.Customer = customer;
            this.Email = email;
            this.UserName = username;
            this.Password = password;
            this.PasswordFormat = passwordFormat;
            this.IsApproved = isApproved;
        }
        public Customer Customer { get; set; }
        public string Email { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public PasswordFormat PasswordFormat { get; set; }
        public bool IsApproved { get; set; }
    }
    
}
