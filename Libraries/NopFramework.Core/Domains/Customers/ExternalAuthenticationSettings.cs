using NopFramework.Core.Configuration;
using System.Collections.Generic;

namespace NopFramework.Core.Domains.Customers
{
    public class ExternalAuthenticationSettings:ISettings
    {
        #region 构造函数
        public ExternalAuthenticationSettings()
        {
            ActiveAuthenticationMethodSystemNames = new List<string>();
        }

        #endregion
        /// <summary>
        /// 是否自动注入
        /// </summary>
        public bool AutoRegisterEnabled { get; set; }

        /// <summary>
        /// 是否需要邮箱验证
        /// </summary>
        public bool RequireEmailValidation { get; set; }

        /// <summary>
        /// 已启用的授权名称
        /// </summary>
        public List<string> ActiveAuthenticationMethodSystemNames { get; set; }
    }
}
