using NopFramework.Core.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Plugin.ExternalAuth.WeiXin
{
    public class WeiXinExternalAuthSettings:ISettings
    {
        /// <summary>
        /// 唯一凭证
        /// </summary>
        public string AppId { get; set; }
        /// <summary>
        /// 唯一凭证密钥
        /// </summary>
        public string AppSecret { get; set; }
    }
}
