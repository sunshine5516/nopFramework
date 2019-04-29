using NopFramework.Admin.Models.Media;
using NopFramework.Core;
using NopFramework.Core.Domains.Media;
using NopFramework.Services;
using NopFramework.Services.Media;
using NopFramework.Services.Security;
using NopFramework.Web.Framework.Controllers;
using NopFramework.Web.Framework.Kendoui;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Controllers
{
    public class ResourcesController : BaseAdminController
    {
        #region 声明实例
        private readonly IResourcesService _resourcesService;
        private readonly IPermissionService _permissionService;
        #endregion
        #region 构造函数
        public ResourcesController(IResourcesService resourcesService,
            IPermissionService permissionService)
        {
            this._resourcesService = resourcesService;
            this._permissionService = permissionService;
        }
        #endregion
        #region 方法

        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageResource))
                return AccessDeniedView();
            var model = new ResourceSearchModel();
            model.ResourceTypes = ResourceTypeModel.Instructions.ToSelectList(false).ToList();
            model.ResourceTypes.Insert(0, new SelectListItem { Text = "--加载所有--", Value = "0" });
            return View(model);
        }
        [HttpPost]
        public ActionResult List(DataSourceRequest command,ResourceSearchModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageResource))
                return AccessDeniedView();
            var query = _resourcesService.GetAllPagedList(model.SearchName, model.SearchStartDate,
                model.SearchEndDate, command.Page - 1, command.PageSize);
            var modelList = new List<ResourceListModel>();
            PrepareTermListModel(modelList, query);
            var gridModel = new DataSourceResult
            {
                Data = modelList,
                Total = query.TotalCount
            };
            return Json(gridModel);
        }

        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageResource))
                return AccessDeniedView();
            var model = new ResourceModel();
            model.ResourceTypes = ResourceTypeModel.Instructions.ToSelectList(false).ToList();           
            return View(model);
        }

        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Create(ResourceModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageResource))
                return AccessDeniedView();
            if (ModelState.IsValid)
            {
                var resource = new Resources
                {
                    CreatedOn = DateTime.Now,
                    ResourceType = model.ResourceType.ToString()
                };
                _resourcesService.Insert(resource);
                return RedirectToAction("Edit", new { id = resource.Id });
                //}
            }
            return View(model);
        }
        public ActionResult Edit(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageResource))
                return AccessDeniedView();
            var item = _resourcesService.GetEntityById(id);
            if (item == null)
                return RedirectToAction("List");
            var model = new ResourceModel
            {
                Id=item.Id,
                ResourceType=Convert.ToInt32(item.ResourceType),
                ResourceTypes = ResourceTypeModel.Instructions.ToSelectList(false).ToList(),
            };
            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        [ValidateInput(false)]
        public ActionResult Edit(ResourceModel resourceModel, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageResource))
                return AccessDeniedView();
            var item = _resourcesService.GetEntityById(resourceModel.Id);
            if (item == null)
                //No email account found with the specified id
                return RedirectToAction("List");
            if (ModelState.IsValid)
            {
                item.ResourceType = resourceModel.ResourceType.ToString();
                _resourcesService.Update(item);
                return continueEditing ? RedirectToAction("Edit", new { id = item.Id }) : RedirectToAction("List");
                //return Json("Success");
            }
                return View();
        }
        public ActionResult AsyncUpload()
        {
            Stream stream = null;
            var fileName = "";
            var contentType = "";
            if (String.IsNullOrEmpty(Request["qqfile"]))
            {
                HttpPostedFileBase httpPostedFile = Request.Files[0];
                if (httpPostedFile == null)
                    throw new ArgumentException("No file uploaded");
                stream = httpPostedFile.InputStream;
                fileName = Path.GetFileName(httpPostedFile.FileName);
                contentType = httpPostedFile.ContentType;
            }
            else
            {
                stream = Request.InputStream;
                fileName = Request["qqfile"];
            }
            var fileBinary = new byte[stream.Length];
            stream.Read(fileBinary, 0, fileBinary.Length);
            var fileExtension = Path.GetExtension(fileName);
            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();
            var resource = _resourcesService.InsertResources(fileBinary, contentType, fileName);
            return Json(new
            {
                success = true,
                pictureId = resource.Id,
                imageUrl = _resourcesService.GetResourcesUrl(resource)
            },
               MimeTypes.TextPlain);
            //ResourceModel model = new ResourceModel
            //{
            //    Id = resource.Id
            //};
            //return RedirectToAction("Edit", new { id = 113 });
        }

        public ActionResult ResourceAdd(ResourceModel resource)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageResource))
                return AccessDeniedView();
            var query = _resourcesService.GetEntityById(resource.Id);
            if (query == null)
                throw new ArgumentException("未找到指定实体");
            query.ResourceType = resource.ResourceType.ToString();
            _resourcesService.Update(query);
            return Json(new { Result = true }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// 删除所选商品
        /// </summary>
        /// <param name="selectedIds"></param>
        /// <returns></returns>
        [HttpPost]
        public ActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageResource))
                return AccessDeniedView();
            if (selectedIds != null)
            {
                var resources = _resourcesService.GetEntityByIds(selectedIds.ToArray());
                foreach (var resource in resources)
                {                   
                    _resourcesService.DeleteResource(resource);//删除本地
                }

            }
            return Json(new { Result = true });
        }

        #endregion
        // GET: Resources
        public ActionResult Index()
        {
            return View();
        }
        #region 辅助方法
        private List<ResourceListModel> PrepareTermListModel(List<ResourceListModel> modelList, IPagedList<Resources> pagedList)
        {
            if (pagedList.Count > 0 && pagedList != null)
            {
                foreach (var product in pagedList)
                {
                    var model = new ResourceListModel();
                    model.Id = product.Id;
                    model.FileName = product.SeoFilename;
                    model.CreatedOn = product.CreatedOn;
                    modelList.Add(model);
                }
            }
            return modelList;
        }

        #endregion
    }
}