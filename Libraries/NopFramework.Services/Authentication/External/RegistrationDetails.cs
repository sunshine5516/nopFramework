using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Authentication.External
{
    /// <summary>
    /// 注册内容
    /// </summary>
    public struct RegistrationDetails
    {
        public RegistrationDetails(OpenAuthenticationParameters parameters)
            : this()
        {
            if (parameters.UserClaims != null)
            {
                foreach (var claim in parameters.UserClaims)
                {
                    if (string.IsNullOrEmpty(EmailAddress))
                    {
                        if (claim.Contact != null)
                        {
                            EmailAddress = claim.Contact.Email;
                            UserName = claim.Contact.Email;
                        }
                    }
                    if (string.IsNullOrEmpty(FirstName))
                        if (claim.Name != null)
                            FirstName = claim.Name.First;
                    if (string.IsNullOrEmpty(LastName))
                        if (claim.Name != null)
                            LastName = claim.Name.Last;
                }
            }
        }
        public string UserName { get; set; }
        public string EmailAddress { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}
