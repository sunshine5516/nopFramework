using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Core.Domains.Customers
{
    public static partial class SystemCustomerRoleNames
    {
        public static string Administrators { get { return "管理员"; } }
        public static string ForumModerators { get { return "ForumModerators"; } }
        public static string Registered { get { return "Registered"; } }
        public static string Guests { get { return "Guests"; } }

    }
}
