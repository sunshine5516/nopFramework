using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Web.Models.Support
{
    public class ResourceModel : BaseNopFrameworkEntityModel
    {
        public string Name { get; set; }
        public string ResourceUrl { get; set; }
        public string ThumbName { get; set; }
    }
}