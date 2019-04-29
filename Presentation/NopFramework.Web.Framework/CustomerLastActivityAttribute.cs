using NopFramework.Core;
using NopFramework.Core.Data;
using NopFramework.Core.Infrastructure;
using NopFramework.Services.Customers;
using System;
using System.Web.Mvc;

namespace NopFramework.Web.Framework
{
    public class CustomerLastActivityAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
             if (!DataSettingsHelper.DatabaseIsInstalled())
                return;
            if (filterContext == null || filterContext.HttpContext == null || filterContext.HttpContext.Request == null)
                return;

            //don't apply filter to child methods
            if (filterContext.IsChildAction)
                return;
            //only GET requests
            if (!String.Equals(filterContext.HttpContext.Request.HttpMethod, "GET", StringComparison.OrdinalIgnoreCase))
                return;
            var workContext = EngineContext.Current.Resolve<IWorkContext>();
            var customer = workContext.CurrentCustomer;
            if (customer.LastActivityDate.AddMinutes(1.0) < DateTime.Now)
            {
                var customerService = EngineContext.Current.Resolve<ICustomerService>();
                customer.LastActivityDate = DateTime.Now;
                customerService.UpdateCustomer(customer);
            }
            //base.OnActionExecuting(filterContext);
        }
    }
}
