using NopFramework.Core.Domains.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains.Customers
{
    /// <summary>
    /// 
    /// </summary>
    public partial class CustomerRole:BaseEntity
    {
        private ICollection<PermissionRecord> _permissionRecords;
        public string Name { get; set; }//名称
        public bool IsActive { get; set; }//是否可用
        public bool IsSystemRole { get; set; }//是否是系统角色
        public string SystemName { get; set; }//获取或设置角色系统名称
        public virtual ICollection<PermissionRecord> PermissionRecords
        {
            get { return _permissionRecords ??( _permissionRecords=new List<PermissionRecord>()); }
            protected set { _permissionRecords = value; }
        }
    }
}
