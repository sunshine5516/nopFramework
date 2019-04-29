using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Models.Bugs
{
    /// <summary>
    /// 严重性
    /// </summary>
    public enum BugSeverityLevelsModel
    {
        //All=0,
        [Description("低")]
        Low =1,//低
        [Description("一般")]
        Normal =2,//一般
        [Description("高")]
        High =3,//高
        [Description("严重")]
        Serious =4,//严重
        [Description("紧急崩溃")]
        Crash =5,//紧急崩溃
    }
}