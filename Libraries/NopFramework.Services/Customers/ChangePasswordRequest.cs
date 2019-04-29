using NopFramework.Core.Domains.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Customers
{
    /// <summary>
    /// 请求修改密码实体
    /// </summary>
    public class ChangePasswordRequest
    {
        public string Email { get; set; }
        public bool ValidateRequest { get; set; }
        public PasswordFormat NewPasswordFormat { get; set; }
        public string NewPassword { get; set; }
        /// <summary>
        /// Old password
        /// </summary>
        public string OldPassword { get; set; }

        public ChangePasswordRequest(string email, bool validateRequest,
      PasswordFormat newPasswordFormat, string newPassword, string oldPassword = "")
        {
            this.Email = email;
            this.ValidateRequest = validateRequest;
            this.NewPasswordFormat = newPasswordFormat;
            this.NewPassword = newPassword;
            this.OldPassword = oldPassword;
        }
    }
}
