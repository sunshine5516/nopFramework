using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Models.Media
{
    public class ResourceListModel : BaseNopFrameworkEntityModel
    {

        public string FileName { get; set; }

        public DateTime? CreatedOn { get; set; }//创建时间
        public string MimeType { get; set; }//文件类型
    }
}