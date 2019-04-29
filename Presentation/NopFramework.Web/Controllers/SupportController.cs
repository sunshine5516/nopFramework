using NopFramework.Services.Media;
using NopFramework.Web.Models.Support;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Web.Controllers
{
    public class SupportController : BasePublicController
    {
        #region 声明实例
        private readonly IResourcesService _resourcesService;
        #endregion
        #region 构造函数
        public SupportController(IResourcesService resourcesService)
        {
            this._resourcesService = resourcesService;
        }
        #endregion
        #region 方法
        public ActionResult LoadAll()
        {
            var query = _resourcesService.GetAllEntities();
            var resourceModel = new ResourceModel();

            IList<ResourceModel> resourceModels = query.Select
                 (x =>
                 {
                     var model = new ResourceModel();
                     model.Id = x.Id;
                     model.Name = x.SeoFilename;
                     return model;
                 }).ToList();
            return View("InstructionsList",resourceModels);
        }
        public ActionResult InstructionsList(int TypeId)
        {
            var query = _resourcesService.GetAllEntities().
                Where(r => r.ResourceType == TypeId.ToString());
            var resourceModel = new ResourceModel();

            IList<ResourceModel> resourceModels = query.Select
                 (x =>
                 {
                     var model = new ResourceModel();
                     model.Id = x.Id;
                     model.Name = x.SeoFilename;
                     return model;
                 }).ToList();
            return View(resourceModels);
        }
        public FileResult DownloadInstruction(int resourceId)
        {
            var resource = _resourcesService.GetEntityById(resourceId);
            var resourceModel = new ResourceModel
            {
                ThumbName=resource.ThumbName,
                Name=resource.SeoFilename,
                ResourceUrl = _resourcesService.GetResourcesUrl(resource)
            };           
            string root = Server.MapPath("~/Content/resources/");
            string fileName = resource.ThumbName;
            string filePath = Path.Combine(root, fileName);
            string s = MimeMapping.GetMimeMapping(resourceModel.Name);

            return File(filePath, s, Path.GetFileName(filePath));
        }
        public ActionResult CommonData()
        {
            return View();
        }
        [ChildActionOnly]
        public ActionResult SupportMenu()
        {
            return PartialView();
        }
        #endregion
        // GET: Support
        public ActionResult Index()
        {
            return View();
        }
    }
}