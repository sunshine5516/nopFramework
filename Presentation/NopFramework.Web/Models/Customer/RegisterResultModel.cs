using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Web.Models.Customer
{
    public partial class RegisterResultModel : BaseNopFrameworkEntityModel

    { 
        public string Result { get; set; }
    }
}