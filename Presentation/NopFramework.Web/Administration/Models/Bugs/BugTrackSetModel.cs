using NopFramework.Web.Framework.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace NopFramework.Admin.Models.Bugs
{
    public class BugTrackSetModel: BaseNopFrameworkEntityModel
    {
        public BugTrackSetModel()
        {
            BugTracks = new List<BugTrackModel>();
        }        
        public DateTime DayTime { get; set; }
        public IList<BugTrackModel> BugTracks { get; set; }
    }
}