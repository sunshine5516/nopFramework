using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Models.Media
{
    public enum ResourceTypeModel
    {
        //All=0,
        [Description("仪器资料")]
        Instrument = 1,//新建
        [Description("说明书")]
        Instructions = 2
    }
}