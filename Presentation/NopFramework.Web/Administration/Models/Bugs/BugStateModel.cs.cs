using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Models.Bugs
{
    public enum BugStateModel
    {
        //All=0,
        [Description("新建")]
        New=1,//新建
        [Description("未通过")]
        Rejected = 2,//未通过，
        [Description("已确认")]
        Confirmed =3,//已确认,
        [Description("已分配")]
        Assigned =4,//已分配,
        [Description("已解决")]
        Solved =5,//已解决，
        [Description("已完成")]
        Finsihed =6,//已完成
    }
}