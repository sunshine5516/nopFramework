using FluentValidation.Attributes;
using NopFramework.Web.Validators.News;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Web.Models.News
{
    [Validator(typeof(HomePageNewsItemsValidator))]
    public partial class HomePageNewsItemsModel
    {
        public IList<NewsItemModel> NewsItems { get; set; }
        public HomePageNewsItemsModel()
        {
            NewsItems = new List<NewsItemModel>();
        }
    }
}