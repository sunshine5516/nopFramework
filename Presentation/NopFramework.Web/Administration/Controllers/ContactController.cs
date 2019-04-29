using NopFramework.Services.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Controllers
{
    public class ContactController : BaseAdminController
    {
        #region 实例声明       
        private readonly IPermissionService _permissionService;
        #endregion
        #region 构造函数

        #endregion
        #region 方法
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult List()
        {
            return View();
        }
        #endregion

    }
}