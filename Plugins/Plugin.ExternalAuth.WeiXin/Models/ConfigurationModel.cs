using NopFramework.Web.Framework.Mvc;
using System;
using System.ComponentModel;
namespace Plugin.ExternalAuth.WeiXin.Models
{
    public class ConfigurationModel : BaseNopFrameworkModel
    {
        [DisplayNameAttribute("AppId")]
        public string AppId { get; set; }
        /// <summary>
        /// 唯一凭证密钥
        /// </summary>
        [DisplayNameAttribute("AppSecret")]
        public string AppSecret { get; set; }
    }
}
