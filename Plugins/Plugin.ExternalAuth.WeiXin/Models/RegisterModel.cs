using NopFramework.Web.Framework.Mvc;
using System.ComponentModel;
using System.Web.Mvc;

namespace Plugin.ExternalAuth.WeiXin.Models
{
    public partial class RegisterModel : BaseNopFrameworkModel
    {

        [DisplayNameAttribute("Account.Fields.Email")]
        [AllowHtml]
        public string Email { get; set; }
        public bool EnteringEmailTwice { get; set; }
        [DisplayNameAttribute("Account.Fields.ConfirmEmail")]
        [AllowHtml]
        public string ConfirmEmail { get; set; }

    }
}
