using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Models.Bugs
{
    public class BugListModel: BaseNopFrameworkEntityModel
    {
        
        public string Name { get; set; }//名称
        public string Department { get; set; }//部门
        public string Project { get; set; }//项目
        public string Summary { get; set; }//概要
        public string Severity { get; set; }//严重性
        public string Frequency { get; set; }//频率
        public string State { get; set; }//状态
        public string Rapporteur { get; set; }//发起者
        public DateTime? CreatedOn { get; set; }//创建日期
        public DateTime UpdateOn { get; set; }//最后更新日期
        public bool IsEdited { get; set; }//是否可编辑
        public bool IsVerify { get; set; }//是否可审核
    }
}