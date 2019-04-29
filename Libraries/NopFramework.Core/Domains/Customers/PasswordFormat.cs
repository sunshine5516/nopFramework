using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains.Customers
{
    /// <summary>
    /// 密码加密格式
    /// </summary>
    public enum PasswordFormat
    {
        Clear=0,///未加密
        Hashed=1,///哈希加密
        Encrypted=2
    }
}
