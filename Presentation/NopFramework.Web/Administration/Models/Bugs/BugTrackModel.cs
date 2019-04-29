using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Models.Bugs
{
    public class BugTrackModel: BaseNopFrameworkEntityModel
    {
        public DateTime Date { get; set; }
        public string ShortTime { get; set; }
        public string Description { get; set; }
        public string UserName { get; set; }
    }
}