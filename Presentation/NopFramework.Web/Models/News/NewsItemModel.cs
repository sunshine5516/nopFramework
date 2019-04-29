using FluentValidation.Attributes;
using NopFramework.Web.Framework.Mvc;
using NopFramework.Web.Validators.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Web.Models.News
{
    [Validator(typeof(NewsItemModelValidator))]
    public class NewsItemModel : BaseNopFrameworkEntityModel
    {
        public string Title { get; set; }//标题
        public string Contents { get; set; }//内容
        public int AhthroId { get; set; }//
        public string MetaTitle { get; set; }//原标题
        public bool AllowComments { get; set; }//是否允许评论
        public DateTime? CreatedOn { get; set; }//添加时间
        public DateTime UpdateOn { get; set; }//修改时间 
        public string SeName { get; set; }
    }
}