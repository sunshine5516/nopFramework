using FluentValidation.Attributes;
using NopFramework.Admin.Validators.Bugs;
using NopFramework.Core.Domains.Media;
using NopFramework.Web.Framework;
using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.Bugs
{

    [Validator(typeof(BugCreateValidator))]
    public class BugModel : BaseNopFrameworkEntityModel
    {
        [NopFrameworkResourceDisplayName("名称")]
        public string Name { get; set; }
        [NopFrameworkResourceDisplayName("摘要")]
        public string Summary { get; set; }
        [NopFrameworkResourceDisplayName("项目")]
        [AllowHtml]
        public int ProjectId { get; set; }//项目
        [NopFrameworkResourceDisplayName("项目")]
        public IList<SelectListItem> SearchBugsProjectIds { get; set; }//分组
        [NopFrameworkResourceDisplayName("部门")]
        [AllowHtml]
        public int DepartmentId { get; set; }//部门
        [NopFrameworkResourceDisplayName("部门")]
        //public IList<int> SearchBugsDepartmentIds { get; set; }//部门
        public IList<SelectListItem> SearchBugsDepartmentIds { get; set; }//部门
        [NopFrameworkResourceDisplayName("发起者")]
        [AllowHtml]
        public int RapporteurId { get; set; }//发起者
        [NopFrameworkResourceDisplayName("修改者")]
        [AllowHtml]
        public int UpdateBy { get; set; }//修改者

        [UIHint("DateNullable")]
        [NopFrameworkResourceDisplayName("出生日期")]
        public DateTime UpdateOn { get; set; }//修改时间

        [NopFrameworkResourceDisplayName("问题描述")]
        [AllowHtml]
        [DataType(DataType.MultilineText)]
        public string Description { get; set; }//描述
        [NopFrameworkResourceDisplayName("Bug优先级")]
        [AllowHtml]
        public string Priority { get; set; }//优先级
        [NopFrameworkResourceDisplayName("优先级")]
        public IList<SelectListItem> PriorityLevels { get; set; }//频率

        [NopFrameworkResourceDisplayName("频率")]
        [AllowHtml]
        public string Frequency { get; set; }//频率
        [NopFrameworkResourceDisplayName("频率")]
        public IList<SelectListItem> FrequencyLevels { get; set; }//频率

        [NopFrameworkResourceDisplayName("严重性")]
        [AllowHtml]
        public string Severity { get; set; }//严重性
        [NopFrameworkResourceDisplayName("严重性")]
        public IList<SelectListItem> SeverityLevels { get; set; }//严重性
        [NopFrameworkResourceDisplayName("产品版本")]
        [AllowHtml]
        public string ProductVersion { get; set; }//产品版本
        [NopFrameworkResourceDisplayName("问题重现步骤")]
        [AllowHtml]
        public string StepsReproduceProblem { get; set; }//问题重现步骤
        [NopFrameworkResourceDisplayName("查看权限")]
        [AllowHtml]
        public string ViewPermissions { get; set; }//查看权限

        [NopFrameworkResourceDisplayName("状态")]
        public IList<SelectListItem> States { get; set; }//状态

        [NopFrameworkResourceDisplayName("状态")]
        public string State { get; set; }//状态
        [NopFrameworkResourceDisplayName("审核信息备注")]
        public string VerifyMessage { get; set; }//审核信息备注

        //public Picture Picture1 { get; set; }
        //public Picture Picture2 { get; set; }
        //public Picture Picture3 { get; set; }
        public BugPictureModel AddPictureModel { get; set; }
        public List<BugPictureModel> BugPictureModels { get; set; }

        public BugModel()
        {
            SearchBugsDepartmentIds = new List<SelectListItem>();
            SearchBugsProjectIds = new List<SelectListItem>();
            SeverityLevels = new List<SelectListItem>();
            FrequencyLevels = new List<SelectListItem>();
            States = new List<SelectListItem>();
            PriorityLevels = new List<SelectListItem>();
            AddPictureModel = new BugPictureModel();
            BugPictureModels = new List<BugPictureModel>();
        }
    }

    
}