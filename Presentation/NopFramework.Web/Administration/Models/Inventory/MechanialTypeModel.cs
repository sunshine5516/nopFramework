using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Models.Inventory
{
    public enum MechanialTypeModel
    {
        //All=0,
        [Description("机械材料")]
        Machine = 1,//新建
        [Description("电子材料")]
        Electron = 2
    }
    public enum OperateTypeModel
    {
        [Description("添加")]
        Create = 0,
        //All=0,
        [Description("入库")]
        Storage = 1,//新建
        [Description("出库")]
        Electron = 2,
        
    }
}