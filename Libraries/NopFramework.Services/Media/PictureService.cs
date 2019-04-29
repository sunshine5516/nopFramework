using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NopFramework.Core;
using NopFramework.Core.Domains.Media;
using NopFramework.Core.Data;
using NopFramework.Services.Logging;
using NopFramework.Data;
using NopFramework.Services.Events;
using NopFramework.Services.Configuration;
using System.IO;
using ImageResizer;
using System.Threading;
using System.Drawing;
using NopFramework.Core.Domains.Catalog;
using NopFramework.Core.Domains.Messages;

namespace NopFramework.Services.Media
{
    /// <summary>
    /// 图片服务
    /// </summary>
    public class PictureService : IPictureService
    {
        #region 声明实例
        private readonly IRepository<Picture> _pictureRepository;

        //private readonly IRepository<Product> _productRepository;
        private readonly IRepository<TermPicture> _termPictureRepository;
        private readonly ISettingService _settingService;
        private readonly ILogger _logger;
        private readonly IDbContext _dbContext;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWebHelper _webHelper;
        private readonly IRepository<ProductPicture> _productPictureRepository;
        #endregion
        #region 构造函数
        public PictureService(IRepository<Picture> pictureRepository,
            ISettingService settingService, ILogger logger, IDbContext dbContext,
            IEventPublisher eventPublisher,IWebHelper _webHelper,
            IRepository<ProductPicture> _productPictureRepository,
            IRepository<TermPicture> termPictureRepository)
        {
            this._pictureRepository = pictureRepository;
            this._settingService = settingService;
            this._logger = logger;
            this._dbContext = dbContext;
            this._eventPublisher = eventPublisher;
            this._webHelper = _webHelper;
            this._productPictureRepository = _productPictureRepository;
            //this._productRepository=productRepository;
            this._termPictureRepository = termPictureRepository;
        }
        #endregion
        #region 方法
        /// <summary>
        /// 图像是否应存储在数据库中。
        /// </summary>
        public virtual bool StoreInDb
        {
            get { return true; }
            set { this.StoreInDb = value; }
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="picture"></param>
        public virtual void DeletePicture(Picture picture)
        {
            if (picture == null)
                throw new ArgumentNullException("picture");
            //delete from database
            _pictureRepository.Delete(picture);
            DeletePictureThumbs(picture);
            //event notification
            _eventPublisher.EntityDeleted(picture);
        }
        /// <summary>
        /// 获取默认图片URL
        /// </summary>
        /// <param name="targetSize">目标图片大小（最长边）</param>
        /// <param name="defaultPictureType">默认图片类型</param>
        /// <returns>Picture URL</returns>
        public string GetDefaultPictureUrl(int targetSize = 0, PictureType defaultPictureType =
            PictureType.Entity,string location = null)
        {
            //throw new NotImplementedException();
            string defaultImageFileName;
            switch (defaultPictureType)
            {
                case PictureType.Avatar:
                    defaultImageFileName=_settingService.GetSettingByKey("Media.Customer.DefaultAvatarImageName", "default-avatar.jpg");
                    break;
                case PictureType.Entity:
                default:
                    defaultImageFileName = _settingService.GetSettingByKey("Media.DefaultImageName", "default-image.png");
                    break;
            }
            string filePath = GetPictureLocalPath(defaultImageFileName);
            if (!File.Exists(filePath))
            {
                return "";
            }
            if (targetSize == 0)
            {
                string url = (!String.IsNullOrEmpty(location)
                                 ? location
                                 : _webHelper.GetStoreLocation())
                                 + "content/images/" + defaultImageFileName;
                return url;
            }
            else
            {
                string fileExtension = Path.GetExtension(filePath);
                string thumbFileName = string.Format("{0}_{1}{2}",
                    Path.GetFileNameWithoutExtension(filePath),
                    targetSize,
                    fileExtension);
                var thumbFilePath = GetThumbLocalPath(thumbFileName);
                if (!GeneratedThumbExists(thumbFilePath, thumbFileName))
                {
                    using (var b = new Bitmap(filePath))
                    {
                        using (var destStream = new MemoryStream())
                        {
                            var newSize = CalculateDimensions(b.Size, targetSize);
                            ImageBuilder.Current.Build(b, destStream, new ResizeSettings
                            {
                                Width = newSize.Width,
                                Height = newSize.Height,
                                Scale = ScaleMode.Both,
                                Quality = 80
                            });
                            var destBinary = destStream.ToArray();
                            SaveThumb(thumbFilePath, thumbFileName, destBinary);
                        }
                    }
                }
                var url = GetThumbUrl(thumbFileName, location);
                return url;
            }
            
        }

        public Picture GetPictureById(int pictureId)
        {
            if (pictureId == 0)
                return null;

            return _pictureRepository.GetById(pictureId);
        }

        public IPagedList<Picture> GetPictures(int pageIndex = 0, int pageSize = int.MaxValue)
        {
            var query = from p in _pictureRepository.Table
                        orderby p.Id descending
                        select p;
            var pics = new PagedList<Picture>(query, pageIndex, pageSize);
            return pics;
        }

        public IList<Picture> GetPicturesByProductId(int productId, int recordsToReturn = 0)
        {
            if (productId == 0)
                return new List<Picture>();


            var query = from p in _pictureRepository.Table
                        join pp in _productPictureRepository.Table on p.Id equals pp.PictureId
                        orderby pp.DisplayOrder
                        where pp.ProductId == productId
                        select p;

            if (recordsToReturn > 0)
                query = query.Take(recordsToReturn);

            var pics = query.ToList();
            return pics;
        }

        public IList<Picture> GetPicturesByTermId(int TermId, int recordsToReturn = 0)
        {
            if (TermId == 0)
                return new List<Picture>();

           
            var query = from p in _pictureRepository.Table
                        join tp in _termPictureRepository.Table on p.Id equals tp.PictureId
                        orderby tp.DisplayOrder
                        where tp.TermId == TermId
                        select p;
            if (recordsToReturn > 0)
                query = query.Take(recordsToReturn);
            var pics = query.ToList();
            return pics;
        }

        public string GetPictureSeName(string name)
        {
            throw new NotImplementedException();
        }

        public IDictionary<int, string> GetPicturesHash(int[] picturesIds)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 获取图片路径
        /// </summary>
        /// <param name="picture">图片实例</param>
        /// <param name="targetSize">图片大小</param>
        /// <param name="showDefaultPicture">A value indicating whether the default picture is shown</param>
        /// <param name="storeLocation">Store location URL; null to use determine the current store location automatically</param>
        /// <param name="defaultPictureType">Default picture type</param>
        /// <returns>Picture URL</returns>
        public string GetPictureUrl(Picture picture, int targetSize = 0, bool showDefaultPicture = true, PictureType defaultPictureType = PictureType.Entity)
        {
            string url = string.Empty;
            byte[] pictureBinary = null;
            if (picture != null)
                pictureBinary = LoadPictureBinary(picture);
            if (picture == null || pictureBinary == null || pictureBinary.Length == 0)
            {
                if (showDefaultPicture)
                {
                    url = GetDefaultPictureUrl(targetSize, defaultPictureType);
                }
                return url;
            }

            if (picture.IsNew)
            {
                DeletePictureThumbs(picture);

                //picture = UpdatePicture(picture.Id,
                //    pictureBinary,
                //    picture.MimeType,
                //    picture.SeoFilename,
                //    picture.AltAttribute,
                //    picture.TitleAttribute,
                //    false,
                //    false);

                picture = UpdatePicture(picture.Id,
                       pictureBinary,
                       picture.MimeType,
                       picture.SeoFilename,
                       picture.AltAttribute,
                       picture.TitleAttribute,
                       false,
                       false);
            }
            var seoFileName = picture.SeoFilename; // = GetPictureSeName(picture.SeoFilename); //just for sure

            string lastPart = GetFileExtensionFromMimeType(picture.MimeType);
            string thumbFileName;
            if (targetSize == 0)
            {
                thumbFileName = !String.IsNullOrEmpty(seoFileName)
                    ? string.Format("{0}_{1}.{2}", picture.Id.ToString("0000000"), seoFileName, lastPart)
                    : string.Format("{0}.{1}", picture.Id.ToString("0000000"), lastPart);
            }
            else
            {
                thumbFileName = !String.IsNullOrEmpty(seoFileName)
                    ? string.Format("{0}_{1}_{2}.{3}", picture.Id.ToString("0000000"), seoFileName, targetSize, lastPart)
                    : string.Format("{0}_{1}.{2}", picture.Id.ToString("0000000"), targetSize, lastPart);
            }
            string thumbFilePath = GetThumbLocalPath(thumbFileName);

            using (var mutex = new Mutex(false, thumbFileName))
            {
                if (!GeneratedThumbExists(thumbFilePath, thumbFileName))
                {
                    mutex.WaitOne();

                    //check, if the file was created, while we were waiting for the release of the mutex.
                    if (!GeneratedThumbExists(thumbFilePath, thumbFileName))
                    {
                        byte[] pictureBinaryResized;

                        //resizing required
                        if (targetSize != 0)
                        {
                            using (var stream = new MemoryStream(pictureBinary))
                            {
                                Bitmap b = null;
                                try
                                {
                                    //try-catch to ensure that picture binary is really OK. Otherwise, we can get "Parameter is not valid" exception if binary is corrupted for some reasons
                                    b = new Bitmap(stream);
                                }
                                catch (ArgumentException exc)
                                {
                                    _logger.Error(string.Format("Error generating picture thumb. ID={0}", picture.Id),
                                        exc);
                                }

                                if (b == null)
                                {
                                    //bitmap could not be loaded for some reasons
                                    return url;
                                }

                                using (var destStream = new MemoryStream())
                                {
                                    var newSize = CalculateDimensions(b.Size, targetSize);
                                    ImageBuilder.Current.Build(b, destStream, new ResizeSettings
                                    {
                                        Width = newSize.Width,
                                        Height = newSize.Height,
                                        Scale = ScaleMode.Both,
                                        Quality = 80
                                    });
                                    pictureBinaryResized = destStream.ToArray();
                                    b.Dispose();
                                }
                            }
                        }
                        else
                        {
                            //create a copy of pictureBinary
                            pictureBinaryResized = pictureBinary.ToArray();
                        }

                        SaveThumb(thumbFilePath, thumbFileName, pictureBinaryResized);
                    }

                    mutex.ReleaseMutex();
                }

            }


            url = GetThumbUrl(thumbFileName);
            return url;
        }
        /// <summary>
        /// 获取图片本地路径
        /// </summary>
        /// <param name="thumbFileName">Filename</param>
        /// <returns>Local picture thumb path</returns>
        protected virtual string GetThumbLocalPath(string thumbFileName)
        {
            var thumbsDirectoryPath = CommonHelper.MapPath("~/content/images/thumbs");
            var thumbFilePath = Path.Combine(thumbsDirectoryPath, thumbFileName);
            return thumbFilePath;
        }
        public string GetPictureUrl(int pictureId, int targetSize = 0, bool showDefaultPicture = true, PictureType defaultPictureType = PictureType.Entity)
        {
            var picture = GetPictureById(pictureId);
            return GetPictureUrl(picture, targetSize, showDefaultPicture, defaultPictureType);
        }

        public string GetThumbLocalPath(Picture picture, int targetSize = 0, bool showDefaultPicture = true)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// 插入图片
        /// </summary>
        /// <param name="pictureBinary">图片二进制</param>
        /// <param name="mimeType">图片MIME类型</param>
        /// <param name="seoFilename">SEO文件名</param>
        /// <param name="altAttribute">“img”HTML元素的“alt”属性</param>
        /// <param name="titleAttribute">“img”HTML元素的“title”属性</param>
        /// <param name="isNew">图片是否是新的值</param>
        /// <param name="validateBinary">指示是否验证提供的图片二进制的值</param>
        /// <returns>Picture</returns>
        public Picture InsertPicture(byte[] pictureBinary, string mimeType, string seoFilename, string altAttrirbute = null, string titleAttribute = null, bool isNew = true, bool validateBinary = true)
        {
            mimeType = CommonHelper.EnsureNotNull(mimeType);
            mimeType = CommonHelper.EnsureMaximumLength(mimeType, 40);
            seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);
            //if (validateBinary)
            //    pictureBinary = ValidatePicture(pictureBinary, mimeType);
            var picture = new Picture
            {
                CreatedOn=DateTime.Now,
                PictureBinary = this.StoreInDb ? pictureBinary : new byte[0],
                MimeType = mimeType,
                SeoFilename = seoFilename,
                AltAttribute = altAttrirbute,
                TitleAttribute = titleAttribute,
                IsNew = isNew
            };
            _pictureRepository.Insert(picture);
            if (!this.StoreInDb)
                SavePictureInFile(picture.Id, pictureBinary, mimeType);
            _eventPublisher.EntityInserted(picture);
            return picture;
            //throw new NotImplementedException();
        }


        /// <summary>
        /// 根据图片存储设置获取加载的图片二进制
        /// </summary>
        /// <param name="picture">Picture</param>
        /// <returns>Picture binary</returns>
        public virtual byte[] LoadPictureBinary(Picture picture)
        {
            return LoadPictureBinary(picture, this.StoreInDb);
        }
        public byte[] LoadPictureBinary(Picture picture, bool fromDb)
        {
            if (picture == null)
                throw new ArgumentNullException("picture");

            var result = fromDb
                ? picture.PictureBinary
                : LoadPictureFromFile(picture.Id, picture.MimeType);
            return result;
        }

        public Picture SetSeoFilename(int pictureId, string seoFileName)
        {
            throw new NotImplementedException();
        }

        public Picture UpdatePicture(int pictureId, byte[] pictureBinary, string mimeType, string seoFilename,
            string altAttribute = null, string titleAttribute = null, bool isNew = true, bool validateBinary = true)
        {
            mimeType = CommonHelper.EnsureNotNull(mimeType);
            mimeType = CommonHelper.EnsureMaximumLength(mimeType, 40);

            seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);

            if (validateBinary)
                pictureBinary = ValidatePicture(pictureBinary, mimeType);

            var picture = GetPictureById(pictureId);
            if (picture == null)
                return null;

            //delete old thumbs if a picture has been changed
            if (seoFilename != picture.SeoFilename)
                DeletePictureThumbs(picture);

            picture.PictureBinary = this.StoreInDb ? pictureBinary : new byte[0];
            picture.MimeType = mimeType;
            picture.SeoFilename = seoFilename;
            picture.AltAttribute = altAttribute;
            picture.TitleAttribute = titleAttribute;
            picture.IsNew = isNew;

            _pictureRepository.Update(picture);

            if (!this.StoreInDb)
                SavePictureInFile(picture.Id, pictureBinary, mimeType);

            //event notification
            _eventPublisher.EntityUpdated(picture);

            return picture;
        }
        /// <summary>
        /// 验证输入图片尺寸
        /// </summary>
        /// <param name="pictureBinary"></param>
        /// <param name="mineType"></param>
        /// <returns></returns>
        public byte[] ValidatePicture(byte[] pictureBinary, string mineType)
        {
            using (var destStream = new MemoryStream())
            {
                ImageBuilder.Current.Build(pictureBinary, destStream, new ResizeSettings
                {
                    //MaxWidth = _mediaSettings.MaximumImageSize,
                    //MaxHeight = _mediaSettings.MaximumImageSize,
                    //Quality = _mediaSettings.DefaultImageQuality
                });
                return destStream.ToArray();
            }
        }
        #endregion
        #region 辅助方法
        /// <summary>
        /// 图片保存在文件系统上
        /// </summary>
        /// <param name="pictureId"></param>
        /// <param name="pictureBinary"></param>
        /// <param name="mimeType"></param>
        private void SavePictureInFile(int pictureId, byte[] pictureBinary, string mimeType)
        {
            string lastPart = GetFileExtensionFromMimeType(mimeType);
            string fileName = string.Format("{0}_0.{1}", pictureId.ToString("0000000"), lastPart);
            File.WriteAllBytes(GetPictureLocalPath(fileName), pictureBinary);
            //throw new NotImplementedException();
        }
        /// <summary>
        /// 获取图片本地路径。 当图像存储在文件系统（不在数据库中）时使用
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected virtual string GetPictureLocalPath(string fileName)
        {
            return Path.Combine(CommonHelper.MapPath("~/content/images/"), fileName);
        }

        /// <summary>
        /// 从mime类型返回文件扩展名。
        /// </summary>
        /// <param name="mimeType"></param>
        /// <returns></returns>
        protected virtual string GetFileExtensionFromMimeType(string mimeType)
        {
            if (mimeType == null)
                return null;
            string[] parts = mimeType.Split('/');
            string lastPart = parts[parts.Length - 1];
            switch (lastPart)
            {
                case "pjpeg":
                    lastPart = "jpg";
                    break;
                case "x-png":
                    lastPart = "png";
                    break;
                case "x-icon":
                    lastPart = "ico";
                    break;
            }
            return lastPart;
        }
        protected virtual string GetThumbUrl(string thumbFileName, string storeLocation = null)
        {
            storeLocation = !String.IsNullOrEmpty(storeLocation)
                                    ? storeLocation
                                    : _webHelper.GetStoreLocation();
            var url = storeLocation+"content/images/thumbs/";
            url = url + thumbFileName;
            return url;
        }

        protected virtual byte[] LoadPictureFromFile(int pictureId, string mimeType)
        {
            string lastPart = GetFileExtensionFromMimeType(mimeType);
            string fileName = string.Format("{0}_0.{1}", pictureId.ToString("0000000"), lastPart);
            var filePath = GetPictureLocalPath(fileName);
            if (!File.Exists(filePath))
                return new byte[0];
            return File.ReadAllBytes(filePath);
        }
        /// <summary>
        /// 删除本地thumb文件
        /// </summary>
        /// <param name="picture"></param>
        protected virtual void DeletePictureThumbs(Picture picture)
        {
            string filter = string.Format("{0}*.*", picture.Id.ToString("0000000"));
            var thumbDirectoryPath = CommonHelper.MapPath("~/content/images/thumbs");
            string[] currentFiles = Directory.GetFiles(thumbDirectoryPath, filter, SearchOption.AllDirectories);
            foreach (var currentFileName in currentFiles)
            {
                var thumbFilePath = GetThumbLocalPath(currentFileName);
                File.Delete(thumbFilePath);
            }
        }
        /// <summary>
        /// 检查某些文件（thumb）是否已经存在
        /// </summary>
        /// <param name="thumbFilePath">Thumb file path</param>
        /// <param name="thumbFileName">Thumb file name</param>
        /// <returns>Result</returns>
        protected virtual bool GeneratedThumbExists(string thumbFilePath, string thumbFileName)
        {
            return File.Exists(thumbFilePath);
        }
        /// <summary>
        /// 计算图片尺寸
        /// </summary>
        /// <param name="originalSize"></param>
        /// <param name="targetSize"></param>
        /// <param name="resizeType"></param>
        /// <param name="ensureSizePositive"></param>
        /// <returns></returns>
        protected virtual Size CalculateDimensions(Size originalSize, int targetSize,
            ResizeType resizeType = ResizeType.LongestSide, bool ensureSizePositive = true)
        {
            float width, height;

            switch (resizeType)
            {
                case ResizeType.LongestSide:
                    if (originalSize.Height > originalSize.Width)
                    {
                        // portrait
                        width = originalSize.Width * (targetSize / (float)originalSize.Height);
                        height = targetSize;
                    }
                    else
                    {
                        // landscape or square
                        width = targetSize;
                        height = originalSize.Height * (targetSize / (float)originalSize.Width);
                    }
                    break;
                case ResizeType.Width:
                    width = targetSize;
                    height = originalSize.Height * (targetSize / (float)originalSize.Width);
                    break;
                case ResizeType.Height:
                    width = originalSize.Width * (targetSize / (float)originalSize.Height);
                    height = targetSize;
                    break;
                default:
                    throw new Exception("Not supported ResizeType");
            }

            if (ensureSizePositive)
            {
                if (width < 1)
                    width = 1;
                if (height < 1)
                    height = 1;
            }

            //we invoke Math.Round to ensure that no white background is rendered - http://www.nopcommerce.com/boards/t/40616/image-resizing-bug.aspx
            return new Size((int)Math.Round(width), (int)Math.Round(height));
        }
        protected virtual void SaveThumb(string thumbFilePath, string thumbFileName, byte[] binary)
        {
            File.WriteAllBytes(thumbFilePath, binary);
        }

    
        #endregion
    }
}
