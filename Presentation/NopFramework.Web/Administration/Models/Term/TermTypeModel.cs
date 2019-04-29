using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Models.Term
{
    public enum TermTypeModel
    {
        //All=0,
        [Description("核心技术")]
        CoreTec = 1,//新建
        [Description("关于衡芯")]
        AboutUs = 2,//未通过，
        [Description("企业文化")]
        Culture = 3,//已确认,
        [Description("领军人物")]
        Leadership = 4,//已分配,
        [Description("招聘信息")]
        Recruitment = 5//已分配,

    }
}