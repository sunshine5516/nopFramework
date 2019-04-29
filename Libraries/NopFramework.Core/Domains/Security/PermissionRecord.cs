using NopFramework.Core.Domains.Customers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains.Security
{
    /// <summary>
    /// 权限
    /// </summary>
    public partial class PermissionRecord:BaseEntity
    {
        private ICollection<CustomerRole> _customerRoles;
        public string Name { get; set; }//权限名称
        public string SystemName { get; set; }//权限系统名称
        public string Category { get; set; }//权限类别
        public virtual ICollection<CustomerRole> CustomerRoles
        {
            get { return _customerRoles ?? (_customerRoles=new List<CustomerRole>()); }
            protected set { _customerRoles = value; }
        }
    }
}
