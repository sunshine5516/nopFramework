using NopFramework.Core.Domains.Customers;
using NopFramework.Services.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NopFramework.Services.Customers
{
    public static partial class CustomerExtensions
    {
        /// <summary>
        /// 获取全名
        /// </summary>
        /// <param name="customer"></param>
        /// <returns></returns>
        public static string GetFullName(this Customer customer)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");
            var firstName = customer.GetAttribute<string>(SystemCustomerAttributeNames.FirstName);
            var lastName = customer.GetAttribute<string>(SystemCustomerAttributeNames.LastName);
            return "1";
        }
    }
}
