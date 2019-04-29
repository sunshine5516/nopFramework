using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System.Collections.Generic;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Term
{
    public class TermModel: BaseNopFrameworkEntityModel
    {
        [NopFrameworkResourceDisplayName("名称")]
        [AllowHtml]
        public string Name { get; set; }
        [NopFrameworkResourceDisplayName("摘要")]
        [AllowHtml]
        public string ShortDescription { get; set; }
        [NopFrameworkResourceDisplayName("详细描述")]
        [AllowHtml]
        public string FullDescription { get; set; }
        [NopFrameworkResourceDisplayName("词条类型")]
        [AllowHtml]
        public int TermType { get; set; }

        [NopFrameworkResourceDisplayName("词条类型")]
        public IList<SelectListItem> TermTypes { get; set; }//词条类型


        public TermPictureModel AddPictureModel { get; set; }
        public List<TermPictureModel> ProductPictureModels { get; set; }
        public TermModel()
        {
            AddPictureModel = new TermPictureModel();
            ProductPictureModels = new List<TermPictureModel>();
            TermTypes = new List<SelectListItem>();
        }
    }
}