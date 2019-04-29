using NopFramework.Core.Domains.Customers;
using NopFramework.Services.Authentication.External;
using Plugin.ExternalAuth.WeiXin.Models;
namespace Plugin.ExternalAuth.WeiXin.Core
{
    public interface IOAuthProviderWeiXinAuthorizer: IExternalProviderAuthorizer
    {
        WeChatUserInfo GetUserInfo(string code);
        Customer GetUser(WeChatUserInfo userInfo);
    }
}
