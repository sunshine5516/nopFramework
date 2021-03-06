﻿using NopFramework.Core.Infrastructure;
using NopFramework.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace NopFramework.Web.Framework.Controllers
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, Inherited = true, AllowMultiple = true)]
    public class AdminAuthorizeAttribute : FilterAttribute, IAuthorizationFilter
    {
        private readonly bool _dontValidate;
        public AdminAuthorizeAttribute():this(false)
            { }
        public AdminAuthorizeAttribute(bool dontValidate)
        {
            this._dontValidate = dontValidate;
        }
        public void OnAuthorization(AuthorizationContext filterContext)
        {
            if (_dontValidate)
                return;
            if (filterContext == null)
                throw new ArgumentNullException("filterContext");
            if(OutputCacheAttribute.IsChildActionCacheActive(filterContext))
                throw new InvalidOperationException("You cannot use [AdminAuthorize] attribute when a child action cache is active");
            if (IsAdminPageRequested(filterContext))
            {
                if (!this.HasAdminAccess(filterContext))
                    this.HandleUnauthorizedRequest(filterContext);
            }
        }


        private IEnumerable<AdminAuthorizeAttribute> GetAdminAuthorizeAttributes(ActionDescriptor descriptor)
        {
            return descriptor.GetCustomAttributes(typeof(AdminAuthorizeAttribute), true)
                .Concat(descriptor.ControllerDescriptor.GetCustomAttributes(typeof(AdminAuthorizeAttribute), true))
                .OfType<AdminAuthorizeAttribute>();
        }

        private bool IsAdminPageRequested(AuthorizationContext filterContext)
        {
            var adminAttributes = GetAdminAuthorizeAttributes(filterContext.ActionDescriptor);
            if (adminAttributes != null && adminAttributes.Any())
                return true;
            return false;
        }

        public virtual bool HasAdminAccess(AuthorizationContext filterContext)
        {
            var permissionService = EngineContext.Current.Resolve<IPermissionService>();
            bool result = permissionService.Authorize(StandardPermissionProvider.AccessAdminPanel);
            return result;
        }
        private void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        {
            filterContext.Result = new HttpUnauthorizedResult();
        }

    }
}
