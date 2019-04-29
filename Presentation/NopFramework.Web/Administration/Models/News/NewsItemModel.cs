using FluentValidation.Attributes;
using NopFramework.Admin.Validators.News;
using NopFramework.Web.Framework.Mvc;
using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace NopFramework.Admin.Models.News
{
    [Validator(typeof(NewsItemValidator))]
    public partial class NewsItemModel : BaseNopFrameworkEntityModel
    {
        [DisplayNameAttribute("标题")]
        [AllowHtml]
        public string Title { get; set; }
        [DisplayNameAttribute("内容")]
        [AllowHtml]
        public string Contents { get; set; }
        [DisplayNameAttribute("原标题")]
        [AllowHtml]
        public string MetaTitle { get; set; }//原标题
        [DisplayNameAttribute("是否允许评论")]
        public bool AllowComments { get; set; }//是否允许评论
        [DisplayNameAttribute("添加时间")]
        [UIHint("DateNullable")]
        public DateTime CreatedOn { get; set; }//添加时间
        [DisplayNameAttribute("修改时间")]
        [UIHint("DateNullable")]
        public DateTime UpdateOn { get; set; }//修改时间
        [DisplayNameAttribute("友好名称")]
        [AllowHtml]
        public string SeName { get; set; }//
        [DisplayNameAttribute("是否发布")]
        public bool IsPublished { get; set; }//是否发布

    }
}