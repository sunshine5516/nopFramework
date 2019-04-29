using NopFramework.Admin.Models.Customers;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Models.Security
{
    public partial class PermissionMappingModel : BaseNopFrameworkEntityModel
    {
        public IList<CustomerRoleModel> AvailableCustomerRoles { get; set; }
        public IList<PermissionRecordModel> AvailablePermissions { get; set; }
        //[permission system name] / [customer role id] / [allowed]
        public IDictionary<string, IDictionary<int, bool>> Allowed { get; set; }
        public PermissionMappingModel()
        {
            AvailablePermissions = new List<PermissionRecordModel>();
            AvailableCustomerRoles = new List<CustomerRoleModel>();
            Allowed = new Dictionary<string, IDictionary<int, bool>>();
        }
    }
}