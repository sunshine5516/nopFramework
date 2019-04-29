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
    public class BugSearchModel : BaseNopFrameworkModel
    {

        //[UIHint("MultiSelect")]
        [NopFrameworkResourceDisplayName("部门")]
        //public IList<int> SearchBugsDepartmentIds { get; set; }//部门
        public IList<SelectListItem> SearchBugsDepartmentIds { get; set; }//部门
        [NopFrameworkResourceDisplayName("部门")]
        public int SearchDepartmentId { get; set; }

        

        [NopFrameworkResourceDisplayName("项目")]
        public IList<SelectListItem> SearchBugsProjectIds { get; set; }//分组
        [NopFrameworkResourceDisplayName("项目")]
        public int SearchBugsProjectId { get; set; }//分组

        [NopFrameworkResourceDisplayName("开始日期")]
        [UIHint("DateNullable")]
        public DateTime? SearchStartDate { get; set; }
        [NopFrameworkResourceDisplayName("结束日期")]
        [UIHint("DateNullable")]
        public DateTime? SearchEndDate { get; set; }
        [NopFrameworkResourceDisplayName("名称")]
        public string BugName { get; set; }

        [NopFrameworkResourceDisplayName("严重性")]
        public IList<SelectListItem> SeverityLevels { get; set; }//严重性

        [NopFrameworkResourceDisplayName("严重性")]
        public string SeverityLevel { get; set; }//严重性

        [NopFrameworkResourceDisplayName("频率")]
        public IList<SelectListItem> FrequencyLevels { get; set; }//频率

        [NopFrameworkResourceDisplayName("频率")]
        public string FrequencyLevel { get; set; }//频率
        [NopFrameworkResourceDisplayName("状态")]
        public IList<SelectListItem> States { get; set; }//状态

        [NopFrameworkResourceDisplayName("状态")]
        public string State { get; set; }//状态

        [NopFrameworkResourceDisplayName("优先级")]
        public IList<SelectListItem> Priorities { get; set; }//优先级

        [NopFrameworkResourceDisplayName("优先级")]
        public string Priority { get; set; }//优先级

        public BugSearchModel()
        {
            SearchBugsDepartmentIds = new List<SelectListItem>();
            SearchBugsProjectIds = new List<SelectListItem>();
            SeverityLevels = new List<SelectListItem>();
            FrequencyLevels = new List<SelectListItem>();
            States= new List<SelectListItem>();
            Priorities= new List<SelectListItem>();
        }
    }
}