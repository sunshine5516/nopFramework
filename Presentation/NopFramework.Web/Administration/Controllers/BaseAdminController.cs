﻿using NopFramework.Web.Framework.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Controllers
{
    [AdminAuthorize]
    public class BaseAdminController : BaseController
    {
        protected override JsonResult Json(object data, string contentType, Encoding contentEncoding, JsonRequestBehavior behavior)
        {
            return new JsonResult()
            {
                Data=data,
                ContentType=contentType,
                ContentEncoding=contentEncoding,
                JsonRequestBehavior=behavior,
                MaxJsonLength=int.MaxValue
            };
        }
        /// <summary>
        /// 拒绝访问
        /// </summary>
        /// <returns></returns>
        protected ActionResult AccessDeniedView()
        {
            return RedirectToAction("AccessDenied", "Security", new { pageUrl = this.Request.RawUrl });
        }
    }
}