using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Models.Bugs
{
    public enum BugFrequencyLevelsModel
    {
        //All=0,
        [Description("无法重现")]
        CanNotReproduce =1,//无法重现
        [Description("很少出现")]
        Seldom =2,//很少
        [Description("经常出现")]
        Often =3,//经常
        [Description("一直出现")]
        Always =4//总是
    }
}