using NopFramework.Services.Authentication.External;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.ExternalAuth.WeiXin.Core
{
    [Serializable]
    public class OAuthAuthenticationParameters: OpenAuthenticationParameters
    {
        private readonly string _providerSystemName;
        private IList<UserClaims> _claims;
        public OAuthAuthenticationParameters(string providerSystemName)
        {
            this._providerSystemName = providerSystemName;
        }
        public override IList<UserClaims> UserClaims
        {
            get
            {
                return _claims;
            }
        }
        public void AddClaim(UserClaims claims)
        {
            if (_claims == null)
                _claims = new List<UserClaims>();
            _claims.Add(claims);
        }
        public override string ProviderSystemName
        {
            get { return _providerSystemName; }
        }
    }
    
}
