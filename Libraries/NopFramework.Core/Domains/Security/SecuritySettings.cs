using NopFramework.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains.Security
{
    public partial  class SecuritySettings: ISettings
    {
        /// <summary>
        /// 指示所有页面是否被强制使用SSL（无论指定的[NopHttpsRequirementAttribute]属性）
        /// </summary>
        public bool ForceSslForAllPages { get; set; }
        /// <summary>
        /// 加密密钥
        /// </summary>
        public string EncryptionKey { get; set; }
        /// <summary>
        /// 管理区域允许的IP地址列表
        /// </summary>
        public IList<string> AdminAreaAllowedIpAddresses { get; set; }
        /// <summary>
        /// 是否应启用管理区域的XSRF保护
        /// </summary>
        public bool EnableXsrfProtectionForAdminArea { get; set; }
        /// <summary>
        ///是否应启用公共存储的XSRF保护
        /// </summary>
        public bool EnableXsrfProtectionForPublicStore { get; set; }

        /// <summary>
        /// 是否在注册页面上启用蜜罐技术
        /// </summary>
        public bool HoneypotEnabled { get; set; }
        /// <summary>
        /// 蜜罐输入名称
        /// </summary>
        public string HoneypotInputName { get; set; }
    }
}
