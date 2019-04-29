using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Authentication.External
{
    /// <summary>
    /// 开放授权状态
    /// </summary>
    public enum OpenAuthenticationStatus
    {
        Unknown,
        Error,
        Authenticated,
        RequiresRedirect,
        AssociateOnLogon,
        AutoRegisteredEmailValidation,
        AutoRegisteredAdminApproval,
        AutoRegisteredStandard,
    }
}
