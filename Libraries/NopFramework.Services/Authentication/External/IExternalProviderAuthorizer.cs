using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Authentication.External
{
    /// <summary>
    /// 外部授权接口
    /// </summary>
    public interface IExternalProviderAuthorizer
    {
        AuthorizeState Authorize(string returnUrl, bool? verifyResponse = null);
    }
}
