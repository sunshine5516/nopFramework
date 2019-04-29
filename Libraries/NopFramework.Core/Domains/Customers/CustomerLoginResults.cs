using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains.Customers
{
    public enum CustomerLoginResults
    {

        Successful=1,///登录成功
        CustomerNotExist=2,//客户不存在（电子邮件或用户名）
        WrongPassword=3,//密码错误
        NotActive=4,//帐户尚未激活        
        Deleted = 5,//已经删除
        NotRegistered = 6,//尚未注册
    }
}
