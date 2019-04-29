using NopFramework.Core;
using NopFramework.Services.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace NopFramework.Admin.Controllers
{
    public class PictureController : Controller
    {
        #region 声明实例
        private readonly IPictureService _pictureService;
        #endregion
        #region 构造函数
        public PictureController(IPictureService pictureService)
        {
            this._pictureService = pictureService;
        }
        #endregion
        // GET: Picture
        public ActionResult Index()
        {
            return View();
        }
        #region 方法
        public ActionResult AsyncUpload()
        {
            Stream stream = null;
            var fileName = "";
            var contentType = "";
            if (String.IsNullOrEmpty(Request["qqfile"]))
            {
                HttpPostedFileBase httpPostedFile = Request.Files[0];
                if (httpPostedFile == null)
                    throw new ArgumentException("No file uploaded");
                stream = httpPostedFile.InputStream;
                fileName = Path.GetFileName(httpPostedFile.FileName);
                contentType = httpPostedFile.ContentType;
            }
            else
            {
                stream = Request.InputStream;
                fileName = Request["qqfile"];
            }
            var fileBinary = new byte[stream.Length];
            stream.Read(fileBinary, 0, fileBinary.Length);
            var fileExtension = Path.GetExtension(fileName);
            if (!String.IsNullOrEmpty(fileExtension))
                fileExtension = fileExtension.ToLowerInvariant();
            if (String.IsNullOrEmpty(contentType))
            {
                switch (fileExtension)
                {
                    case ".bmp":
                        contentType = MimeTypes.ImageBmp;
                        break;
                    case ".gif":
                        contentType = MimeTypes.ImageGif;
                        break;
                    case ".jpeg":
                    case ".jpg":
                    case ".jpe":
                    case ".jfif":
                    case ".pjpeg":
                    case ".pjp":
                        contentType = MimeTypes.ImageJpeg;
                        break;
                    case ".png":
                        contentType = MimeTypes.ImagePng;
                        break;
                    case ".tiff":
                    case ".tif":
                        contentType = MimeTypes.ImageTiff;
                        break;
                    //case "":
                       
                    //    break;
                    default:
                        break;  
                }
            }
            //if(contentType== "application/x-zip-compressed")
            //contentType = "application/zip";
           
            var picture = _pictureService.InsertPicture(fileBinary, contentType, null);
            return Json(new
            {
                success = true,
                pictureId = picture.Id,
                imageUrl = _pictureService.GetPictureUrl(picture, 100)
            },
               MimeTypes.TextPlain);
        }
        //public ActionResult AsyncDelete()
        //{

        //}
        #endregion
    }
}