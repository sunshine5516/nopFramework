using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Models.Security
{
    public partial class PermissionRecordModel : BaseNopFrameworkEntityModel
    {
        public string Name { get; set; }
        public string SystemName { get; set; }
    }
}