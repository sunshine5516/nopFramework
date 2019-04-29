using NopFramework.Core.Domains.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Security
{
    public partial interface IPermissionProvider
    {
        /// <summary>
        /// 获取权限
        /// </summary>
        /// <returns></returns>
        IEnumerable<PermissionRecord> GetPermissions();
        /// <summary>
        /// 获取默认权限
        /// </summary>
        /// <returns></returns>
        IEnumerable<DefaultPermissionRecord> GetDefaultPermissions();
    }
}
