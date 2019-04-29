﻿using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Web.Models.Media
{
    public partial class PictureModel : BaseNopFrameworkEntityModel
    {
        public string ImageUrl { get; set; }

        public string FullSizeImageUrl { get; set; }

        public string Title { get; set; }

        public string AlternateText { get; set; }
    }
}