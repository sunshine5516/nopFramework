using NopFramework.Web.Framework.Mvc;
using NopFramework.Web.Models.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Web.Models.Introduce
{
    public class TermModel: BaseNopFrameworkEntityModel
    {

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public string FullDescription { get; set; }

        public int TermType { get; set; }



        public PictureModel AddPictureModel { get; set; }
        public List<PictureModel> TermPictureModels { get; set; }
        public TermModel()
        {
            AddPictureModel = new PictureModel();
            TermPictureModels = new List<PictureModel>();
        }
    }
}