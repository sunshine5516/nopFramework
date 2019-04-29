using NopFramework.Admin.Extensions;
using NopFramework.Admin.Factories;
using NopFramework.Admin.Models.Localization;
using NopFramework.Core.Domains.Localization;
using NopFramework.Services.Localization;
using NopFramework.Services.Security;
using NopFramework.Web.Framework.Controllers;
using NopFramework.Web.Framework.Kendoui;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NopFramework.Admin.Controllers
{
    public class LanguageController : BaseAdminController
    {
        #region 声明实例
        private readonly ILanguageService _languageService;
        private readonly ILocalizationService _localizationService;
        private readonly IPermissionService _permissionService;
        private readonly ILanguageModelFactory _languageModelFactory;
        #endregion
        #region 构造函数
        public LanguageController(ILanguageService languageService,
            ILocalizationService localizationService, IPermissionService permissionService
            , ILanguageModelFactory languageModelFactory)
        {
            this._languageService = languageService;
            this._localizationService = localizationService;
            this._permissionService = permissionService;
            this._languageModelFactory = languageModelFactory;
        }
        #endregion
        #region 语言管理
        // GET: Language
        public ActionResult Index()
        {
            return RedirectToAction("List");
        }
        public ActionResult List()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageLanguages))
                return AccessDeniedView();

            return View();
        }
        [HttpPost]
        public ActionResult List(DataSourceRequest command)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageLanguages))
                return AccessDeniedView();
            var model = _languageModelFactory.PrepareLanguageListModel();
            //var model=_languageModelFactory.
            return new JsonResult
            {
                Data = model
            };
        }
        public ActionResult Create()
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageLanguages))
                return AccessDeniedView();
            var model = _languageModelFactory.PrepareLanguageModel(new LanguageModel(), null);

            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        public ActionResult Create(LanguageModel model,bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageLanguages))
                return AccessDeniedView();
            if (ModelState.IsValid)
            {
                var language = model.ToEntity();
                _languageService.Insert(language);
                SuccessNotification("添加成功");
                if (continueEditing)
                {
                    //selected tab
                    SaveSelectedTabName();

                    return RedirectToAction("Edit", new { id = language.Id });
                }
                return RedirectToAction("List");
            }
            return View(model);
        }
        public ActionResult Edit(int Id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageLanguages))
                return AccessDeniedView();
            var language = _languageService.GetEntityById(Id);
            if (language == null)
                return RedirectToAction("List");

            //prepare model
            var model = _languageModelFactory.PrepareLanguageModel(null, language);

            return View(model);
        }
        [HttpPost, ParameterBasedOnFormName("save-continue", "continueEditing")]
        [FormValueRequired("save", "save-continue")]
        public ActionResult Edit(LanguageModel model, bool continueEditing)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageLanguages))
                return AccessDeniedView();
            var language = _languageService.GetEntityById(model.Id);
            if (language == null)
                return RedirectToAction("List");
            if (ModelState.IsValid)
            {
                var allLanguages = _languageService.GetAllLanguages();
                if (allLanguages.Count == 1 && allLanguages[0].Id == language.Id &&
                    !model.Published)
                {
                    ErrorNotification("At least one published language is required.");
                    return RedirectToAction("Edit", new { id = language.Id });
                }
                language = model.ToEntity(language);
                _languageService.Update(language);
                SuccessNotification("更新语言成功");
                if (continueEditing)
                {
                    //selected tab
                    SaveSelectedTabName();

                    return RedirectToAction("Edit", new { id = language.Id });
                }
                return RedirectToAction("List");
            }
            _languageModelFactory.PrepareFlagsModel(model);
            return View(model);
        }


        [HttpPost]
        public ActionResult DeleteSelected(ICollection<int> selectedIds)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageLanguages))
                return AccessDeniedView();
            if (selectedIds != null)
            {
                var languages = _languageService.GetEntityByIds(selectedIds.ToArray());
                if (languages == null)
                    //No language found with the specified id
                    return RedirectToAction("List");
                var allLanguages = _languageService.GetAllLanguages();
                if (allLanguages.Count == 1)
                {
                    ErrorNotification("At least one published language is required.");
                }
                foreach (var language in languages)
                {
                    _languageService.Delete(language);
                }
            }
            SuccessNotification("删除成功");
            return Json(new { Result = true });
        }
        //[HttpPost]
        //public ActionResult Delete(int id)
        //{
        //    if (!_permissionService.Authorize(StandardPermissionProvider.ManageLanguages))
        //        return AccessDeniedView();
        //    var language = _languageService.GetEntityById(id);
        //    if (language == null)
        //        //No language found with the specified id
        //        return RedirectToAction("List");
        //    var allLanguages = _languageService.GetAllLanguages();
        //    if (allLanguages.Count == 1 && allLanguages[0].Id == language.Id)
        //    {
        //        ErrorNotification("At least one published language is required.");
        //        return RedirectToAction("Edit", new { id = language.Id });
        //    }

        //    //delete
        //    _languageService.Delete(language);

        //    //notification
        //    SuccessNotification("删除成功");
        //    return RedirectToAction("List");
        //}
        #endregion
        #region 资源管理
        [HttpPost]
        public ActionResult Resources(DataSourceRequest command,int languageId)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageLanguages))
                return AccessDeniedView();
            var language = _languageService.GetEntityById(languageId);
            if (language == null)
                return RedirectToAction("List");
            var query = _languageModelFactory.PrepareLocaleResourceListModel(command,languageId);
            return Json(query);
        }
        [HttpPost]
        public ActionResult ResourceUpdate(LocaleResourceModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageLanguages))
                return AccessDeniedView();
            if (model.Name != null)
                model.Name = model.Name.Trim();
            if (model.Value != null)
                model.Value = model.Value.Trim();

            var resource = _localizationService.GetEntityById(model.Id);

            resource.ResourceName = model.Name;
            resource.ResourceValue = model.Value;
            _localizationService.Update(resource);
            return new JsonResult();
        }

        [HttpPost]
        public ActionResult ResourceAdd(int languageId, [Bind(Exclude = "Id")] LocaleResourceModel model)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageLanguages))
                return AccessDeniedView();
            if (model.Name != null)
                model.Name = model.Name.Trim();
            if (model.Value != null)
                model.Value = model.Value.Trim();
            var res = _localizationService.GetLocaleStringResourceByName(model.Name, model.LanguageId, false);
            if (res == null)
            {
                var resource = new LocaleStringResource
                {
                    LanguageId = languageId,
                    ResourceName = model.Name,
                    ResourceValue = model.Value,
                    CreatedOn=DateTime.Now
                };

                _localizationService.Insert(resource);
            }
            else
            {
                return Json(new DataSourceResult { Errors = string.Format("资源文件已经存在", model.Name) });
            }

            return new JsonResult();
        }
        [HttpPost]
        public virtual ActionResult ResourceDelete(int id)
        {
            if (!_permissionService.Authorize(StandardPermissionProvider.ManageLanguages))
                return AccessDeniedView();
            var resource = _localizationService.GetEntityById(id);
            if (resource == null)
                throw new ArgumentException("No resource found with the specified id");
            _localizationService.Delete(resource);

            return new JsonResult();
        }
        #endregion
        #region 辅助方法

        #endregion

    }
}