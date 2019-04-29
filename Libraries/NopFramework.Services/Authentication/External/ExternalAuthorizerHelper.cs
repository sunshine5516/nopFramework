using NopFramework.Core.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace NopFramework.Services.Authentication.External
{
    public static partial class ExternalAuthorizerHelper
    {
        #region 常量
        private const string EXTERNALAUTH = "nopFramework.externalauth.parameters";
        #endregion
        #region 方法
        private static HttpSessionStateBase GetSession()
        {
            var session = EngineContext.Current.Resolve<HttpSessionStateBase>();
            return session;
        }
        public static void StoreParametersForRoundTrip(OpenAuthenticationParameters parameters)
        {
            var session = GetSession();
            session["externalauth.parameters"] = parameters;
        }
        public static void RemoveParameters()
        {
            var session = GetSession();
            session.Remove(EXTERNALAUTH);
        }
        public static void AddErrorsToDisplay(string error)
        {
            var session = GetSession();
            var errors = session["externalauth.errors"] as IList<string>;
            if (errors != null)
            {
                errors = new List<string>();
                session.Add("nop.externalauth.errors", errors);
            }
            errors.Add(error);
        }
        #endregion
    }
}
