using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains.Common
{
    /// <summary>
    /// 通用属性
    /// </summary>
    public partial class GenericAttribute : BaseEntity
    {
        public int EntityId { get; set; }
        /// <summary>
        /// 分组键值对
        /// </summary>
        public string KeyGroup { get; set; }
        
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
