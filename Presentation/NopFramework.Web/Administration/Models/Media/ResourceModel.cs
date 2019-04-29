using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Media
{
    public class ResourceModel : BaseNopFrameworkEntityModel
    {
        [UIHint("Resource")]
        [NopFrameworkResourceDisplayName("图片")]
        public override int Id { get; set; }

        [NopFrameworkResourceDisplayName("资源类型")]
        [AllowHtml]
        public int ResourceType { get; set; }

        [NopFrameworkResourceDisplayName("资源类型")]
        public IList<SelectListItem> ResourceTypes { get; set; }//资源类型
        public ResourceModel()
        {
            ResourceTypes=new List<SelectListItem>();
        }
    }
}