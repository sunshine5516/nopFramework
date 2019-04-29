using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using NopFramework.Core;
using NopFramework.Core.Caching;
using NopFramework.Core.Domains.Customers;
using NopFramework.Services.Authentication.External;
using NopFramework.Services.Configuration;
using Plugin.ExternalAuth.WeiXin.Models;

namespace Plugin.ExternalAuth.WeiXin.Core
{
    public class WeiXinProviderAuthorizer : IOAuthProviderWeiXinAuthorizer
    {
        #region 声明实例
        private const string WEIXIN_USER_CUSTOMERID = "WeiXin.UserInfo-{0}";
        private readonly IExternalAuthorizer _authorizer;
        private readonly IWebHelper _webHelper;
        private readonly ISettingService _settingService;
        private readonly HttpContextBase _httpContext;
        private readonly IOpenAuthenticationService _openAuthenticationService;
        private readonly IWorkContext _workContext;
        private readonly ICacheManager _cacheManager;
        #endregion
        #region 构造函数
        public WeiXinProviderAuthorizer(IWebHelper webHelper,
            ISettingService settingService, HttpContextBase httpContext
            , IExternalAuthorizer authorizer,IOpenAuthenticationService openAuthenticationService
            , IWorkContext workContext, ICacheManager cacheManager)
        {
            this._webHelper = webHelper;
            this._settingService = settingService;
            this._httpContext = httpContext;
            this._authorizer = authorizer;
            this._openAuthenticationService = openAuthenticationService;
            this._workContext = workContext;
            this._cacheManager = cacheManager;
        }
        #endregion
        /// <summary>
        /// 授权回复
        /// </summary>
        /// <param name="returnUrl"></param>
        /// <param name="verifyResponse"></param>
        /// <returns></returns>
        public AuthorizeState Authorize(string returnUrl, bool? verifyResponse = null)
        {
            if (!verifyResponse.HasValue)
                throw new ArgumentException("微信插件验证错误");
            if (verifyResponse.Value)
                return VerifyAuthentication(returnUrl);
            return RequestAuthentication();
        }

        private AuthorizeState VerifyAuthentication(string returnUrl)
        {
            var returnString = _httpContext.Request.QueryString;
            var sortedStr = returnString.AllKeys;
            WeChatUserInfo userInfo = null;
            SortedDictionary<string, string> sortedDic = new SortedDictionary<string, string>();
            for (int i = 0; i < sortedStr.Length; i++)
            {
                sortedDic.Add(sortedStr[i], returnString[sortedStr[i]]);
            }
            if (sortedDic.Keys.Contains("code"))
            {
                var code = sortedDic["code"];
                userInfo = GetUserInfo(code);
            }
            if (userInfo == null)
            {
                var currentCustomerId = _workContext.CurrentCustomer.Id;
                if (currentCustomerId != 0)
                {
                    var key = string.Format(WEIXIN_USER_CUSTOMERID, currentCustomerId);
                    userInfo = _cacheManager.Get<WeChatUserInfo>(key);
                    _cacheManager.Remove(key);
                }
            }
            var email = _httpContext.Request.Form["Email"]; //获取注册邮箱
            if (userInfo != null)
            {
                var parameters = new OAuthAuthenticationParameters(Provider.SystemName)
                {
                    ExternalIdentifier = userInfo.openid,
                    ExternalDisplayIdentifier = userInfo.unionid
                };
                // if (_externalAuthenticationSettings.AutoRegisterEnabled)
                ParseClaims(userInfo, parameters, email);
                var result = _authorizer.Authorize(parameters);
                return new AuthorizeState(returnUrl, result);
            }
            else
            {
                var state = new AuthorizeState(returnUrl, OpenAuthenticationStatus.Error);
                var error = "获取用户信息失败";
                state.AddError(error);
                return state;
            }
        }

        private void ParseClaims(WeChatUserInfo userInfo, OAuthAuthenticationParameters parameters
            ,string email)
        {
            Random random = new Random();
            var claims = new UserClaims();
            claims.Contact = new ContactClaims()
            {
                Email= email
            };
            claims.Contact.Address = new AddressClaims()
            {
                City = userInfo.city,
                State = userInfo.province,
                Country = userInfo.country,
            };
            claims.Name = new NameClaims()
            {
                Nickname = userInfo.nickname,
            };

            parameters.AddClaim(claims);
        }

        private AuthorizeState RequestAuthentication()
        {
            var authUrl = GenerateServiceLoginUrl().AbsoluteUri;
            return new AuthorizeState("", OpenAuthenticationStatus.RequiresRedirect) { Result = new RedirectResult(authUrl) };
        }

        private Uri GenerateServiceLoginUrl()
        {
            string redirect_uri = string.Format("{0}plugins/ExternalAuthWeiXin/logincallback/", _webHelper.GetStoreLocation());
            var url = this.GetAuthorizeUrl(redirect_uri: redirect_uri, scope: "snsapi_userinfo");
            return new Uri(url);
        }
        public WeChatUserInfo GetUserInfo(string code)
        {
            var accessToken = this.GetAccessToken(code);
            var userInfo = new WeChatUserInfo();
            if (accessToken!=null)
            {
                userInfo = GetUserInfo(accessToken.access_token, accessToken.openid);
                var currentCustomerId = _workContext.CurrentCustomer.Id;
                if (currentCustomerId != 0)
                {
                    var key = string.Format(WEIXIN_USER_CUSTOMERID, currentCustomerId);
                    _cacheManager.Set(key, userInfo, 5); //保存5分钟
                }
                return userInfo;
            }
            return null;
        }
        #region 微信接口
        public string GetAuthorizeUrl(string redirect_uri, string scope = "", string state = "")
        {
            var weiXinExternalAuthSettings = _settingService.LoadSetting<WeiXinExternalAuthSettings>();
            var builder = new UriBuilder("https://open.weixin.qq.com/connect/oauth2/authorize");
            var args=new Dictionary<string, string>();
            args.Add("appid", weiXinExternalAuthSettings.AppId);
            args.Add("redirect_uri", redirect_uri);
            args.Add("response_type", "code");
            args.Add("scope", "snsapi_userinfo");
            args.Add("state", "State");
            AppendQueryArgs(builder, args);
            return builder.Uri.AbsoluteUri+ "#wechat_redirect";

        }
        private WeChatAccessToken GetAccessToken(string code)
        {
            var weiXinExternalAuthSettings = _settingService.LoadSetting<WeiXinExternalAuthSettings>();
            var builder = new UriBuilder("https://api.weixin.qq.com/sns/oauth2/access_token");
            var args = new Dictionary<string, string>();
            args.Add("appid", weiXinExternalAuthSettings.AppId);
            args.Add("secret", weiXinExternalAuthSettings.AppSecret);
            args.Add("code", code);
            args.Add("scope", "snsapi_login");
            args.Add("grant_type", "authorization_code");
            AppendQueryArgs(builder, args);
            var url = builder.Uri.AbsoluteUri;
            var request = WebRequest.Create(url);
            using (var response = request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        var responseFromServer = reader.ReadToEnd();
                        var accessToken = JsonConvert.DeserializeObject<WeChatAccessToken>(responseFromServer);
                        return accessToken;
                    }
                }
                
            }
            //return "hello";
        }
        public WeChatUserInfo GetUserInfo(string access_token,string openid)
        {
            var weiXinExternalAuthSettings = _settingService.LoadSetting<WeiXinExternalAuthSettings>();
            var builder = new UriBuilder("https://api.weixin.qq.com/sns/userinfo");
            var args = new Dictionary<string, string>();
            args.Add("access_token", access_token);
            args.Add("openid", openid);
            args.Add("lang", "zh_CN");
            AppendQueryArgs(builder, args);
            var url = builder.Uri.AbsoluteUri;
            var request = WebRequest.Create(url);
            using (var response = request.GetResponse())
            {
                using (var responseStream = response.GetResponseStream())
                {
                    using (var reader = new StreamReader(responseStream))
                    {
                        var responseFromServer = reader.ReadToEnd();
                        var userInfo = JsonConvert.DeserializeObject<WeChatUserInfo>(responseFromServer);
                        return userInfo;
                    }
                }

            }
        }
        public Customer GetUser(WeChatUserInfo userInfo)
        {
            var parameters = new OAuthAuthenticationParameters(Provider.SystemName)
            {
                ExternalIdentifier = userInfo.openid,
                ExternalDisplayIdentifier = userInfo.unionid,
            };
            return _openAuthenticationService.GetUser(parameters);
        }

        public bool RefreshToken(string refresh_token)
        {
            return false;
        }
        #endregion
        #region 辅助方法
        private void AppendQueryArgs(UriBuilder builder, IEnumerable<KeyValuePair<string, string>> args)
        {
            if (args != null && args.Any())
            {
                var builder2 = new StringBuilder();
                if (!string.IsNullOrEmpty(builder.Query))
                {
                    builder2.Append(builder.Query.Substring(1));
                    builder2.Append('&');
                }
                builder2.Append(CreateQueryString(args));
                builder.Query = builder2.ToString();
            }
        }
        private string CreateQueryString(IEnumerable<KeyValuePair<string, string>> args)
        {
            if (!args.Any())
            {
                return string.Empty;
            }
            var builder = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in args)
            {
                builder.Append(EscapeUriDataStringRfc3986(pair.Key));
                builder.Append('=');
                builder.Append(EscapeUriDataStringRfc3986(pair.Value));
                builder.Append('&');
            }
            builder.Length--;
            return builder.ToString();
        }
        private readonly string[] UriRfc3986CharsToEscape = new string[] { "!", "*", "'", "(", ")" };
        private string EscapeUriDataStringRfc3986(string value)
        {
            var builder = new StringBuilder(Uri.EscapeDataString(value));
            for (int i = 0; i < UriRfc3986CharsToEscape.Length; i++)
            {
                builder.Replace(UriRfc3986CharsToEscape[i], Uri.HexEscape(UriRfc3986CharsToEscape[i][0]));
            }
            return builder.ToString();
        }

       
        #endregion
    }
}
