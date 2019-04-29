using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Models.Bugs
{
    /// <summary>
    /// 优先级
    /// </summary>
    public enum BugPriorityLevelModel
    {
        //All=0,
        [Description("无")]
        None =1,//无
        [Description("低")]
        Low =2,//低
        [Description("一般")]
        Normal =3,//一般
        [Description("高")]
        High =4,//高
        [Description("紧急")]
        Urgent =5,//紧急
    }
}