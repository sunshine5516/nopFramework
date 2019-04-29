using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Models.Term
{
    public class TermPictureModel : BaseNopFrameworkEntityModel
    {
        public int ProductId { get; set; }
        [UIHint("Picture")]
        [NopFrameworkResourceDisplayName("图片")]
        public int PictureId { get; set; }
        [NopFrameworkResourceDisplayName("顺序")]
        public int DisplayOrder { get; set; }
        [NopFrameworkResourceDisplayName("摘要")]
        public string Description { get; set; }
        [NopFrameworkResourceDisplayName("URL")]
        public string PictureUrl { get; set; }
    }
}