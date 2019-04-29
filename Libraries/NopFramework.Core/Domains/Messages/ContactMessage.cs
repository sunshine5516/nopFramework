using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains.Messages
{
    /// <summary>
    /// 联系消息实体
    /// </summary>
    public partial class ContactMessage:BaseEntity
    {
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Name { get; set; }
        /// <summary>
        /// 创建日期
        /// </summary>
        //public DateTime CreatedOn { get; set; }
        public string Content { get; set; }
    }
}
