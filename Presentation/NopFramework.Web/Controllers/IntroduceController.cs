using NopFramework.Core.Domains.Messages;
using NopFramework.Services.Media;
using NopFramework.Services.Messages;
using NopFramework.Web.Models.Introduce;
using NopFramework.Web.Models.Media;
using NopFramework.Web.Models.Term;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Web.Controllers
{
    public class IntroduceController : BasePublicController
    {
        #region 声明实例
        private readonly ITermService _termService;
        private readonly IPictureService _pictureService;
        #endregion
        #region 构造函数
        public IntroduceController(ITermService termService, IPictureService pictureService)
        {
            this._termService = termService;
            this._pictureService = pictureService;
        }
        #endregion
        #region 方法
        // GET: Product
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult HomeCoreTechnology()
        {
            var query = _termService.GetAllEntities().Where
                (t => t.TermType ==Convert.ToInt32(TermTypeModel.CoreTec)&&t.IsDeleted==false).ToList();
            return PartialView(PrepareMetaTerms(query));
        }
        public ActionResult AboutUs()
        {
            var query = _termService.GetAllEntities().Where
               (t => t.TermType == Convert.ToInt32(TermTypeModel.AboutUs) && t.IsDeleted == false).FirstOrDefault();
            return PartialView(PrepareTermModel(query));
        }
        public ActionResult CompCulture()
        {
            var query = _termService.GetAllEntities().Where
                (t => t.TermType == Convert.ToInt32(TermTypeModel.Culture) && t.IsDeleted == false).FirstOrDefault();
            return PartialView(PrepareTermModel(query));
        }
        public ActionResult Leadship()
        {
            var query = _termService.GetAllEntities().Where
                (t => t.TermType == Convert.ToInt32(TermTypeModel.Leadership) && t.IsDeleted == false).ToList();
            return PartialView(PrepareMetaTerms(query));
        }
        public ActionResult CompanyAddress()
        {
            return View();
        }
        public ActionResult TechnologyItem(int termId)
        {
            var term = _termService.GetEntityById(termId);
            if (term == null || term.IsDeleted)
                return InvokeHttp404();

            var model = PrepareTermModel(term);
            return View(model);
        }
        [ChildActionOnly]
        public ActionResult AboutUsMenu()
        {
            return PartialView();
        }
        #endregion
        #region 辅助方法
        protected virtual IList<TermModel> PrepareMetaTerms(IList<Term> terms,
            bool preparePictureModel = true)
        {
            IList<TermModel> termModels = new List<TermModel>();
            foreach (var term in terms)
            {
                var termModel = new TermModel
                {
                    Id=term.Id,
                    Name=term.Name,
                    ShortDescription=term.ShortDescription,
                    FullDescription=term.FullDescription,
                    TermType=term.TermType
                };



                var pictures = _pictureService.GetPicturesByTermId(term.Id);
                var defaultPictureModel = new PictureModel
                {
                    ImageUrl = _pictureService.GetPictureUrl(pictures.FirstOrDefault()),
                    FullSizeImageUrl = _pictureService.GetPictureUrl(pictures.FirstOrDefault())
                };
                var pictureModels = new List<PictureModel>();
                foreach (var picture in pictures)
                {
                    var pictureModel = new PictureModel
                    {
                        ImageUrl = _pictureService.GetPictureUrl(picture),
                        FullSizeImageUrl = _pictureService.GetPictureUrl(pictures.FirstOrDefault())
                    };
                    pictureModels.Add(pictureModel);
                }
                termModel.TermPictureModels = pictureModels;
                termModels.Add(termModel);
            }
            return termModels;
        }

        protected virtual TermModel PrepareTermModel(Term term)
        {
            var termModel = new TermModel
            {
                Id = term.Id,
                Name = term.Name,
                ShortDescription = term.ShortDescription,
                FullDescription = term.FullDescription,
                TermType = term.TermType
            };


            var pictures = _pictureService.GetPicturesByTermId(term.Id);
            var defaultPictureModel = new PictureModel
            {
                ImageUrl = _pictureService.GetPictureUrl(pictures.FirstOrDefault()),
                FullSizeImageUrl = _pictureService.GetPictureUrl(pictures.FirstOrDefault())
            };
            var pictureModels = new List<PictureModel>();
            foreach (var picture in pictures)
            {
                var pictureModel = new PictureModel
                {
                    ImageUrl = _pictureService.GetPictureUrl(picture),
                    FullSizeImageUrl = _pictureService.GetPictureUrl(pictures.FirstOrDefault())
                };
                pictureModels.Add(pictureModel);
            }
            termModel.TermPictureModels = pictureModels;
            return termModel;
        }
        #endregion
    }
}