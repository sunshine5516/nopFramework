using NopFramework.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains.Customers
{
    public class CustomerSettings: ISettings
    {
        public PasswordFormat DefaultPasswordFormat { get; set; }
        public string HashedPasswordFormat { get; set; }//密码散列时的客户密码格式（SHA1，MD5）
        public UserRegistrationType UserRegistrationType { get; set; }///用户注册类型
        public int OnlineCustomerMinutes { get; set; }///“在线客户”模块的分钟数
    }
}
