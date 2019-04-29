using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains.Security
{
    /// <summary>
    /// 默认的权限记录
    /// </summary>
    public class DefaultPermissionRecord
    {
        public DefaultPermissionRecord()
        {
            this.PermissionRecords = new List<PermissionRecord>();
        }
        public string CustomerRoleSystemName { get; set; }
        public IEnumerable<PermissionRecord> PermissionRecords { get; set; }
    }
}
