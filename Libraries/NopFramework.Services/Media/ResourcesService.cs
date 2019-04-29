using ImageResizer;
using NopFramework.Core;
using NopFramework.Core.Data;
using NopFramework.Core.Domains.Media;
using NopFramework.Services.Events;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NopFramework.Services.Media
{
    public class ResourcesService : BaseService<Resources>, IResourcesService
    {
        #region 声明实例
        private readonly IRepository<Resources> _resourcesRepository;
        private readonly IEventPublisher _eventPublisher;
        private readonly IWebHelper _webHelper;
        #endregion
        #region 构造函数
        public ResourcesService(IRepository<Resources> resourcesRepository,
            IEventPublisher eventPublisher, IWebHelper webHelper) : base(resourcesRepository)
        {
            this._resourcesRepository = resourcesRepository;
            this._eventPublisher = eventPublisher;
            this._webHelper = webHelper;
        }


        #endregion
        #region 方法

        public override IList<Resources> GetAllEntities()
        {
            var query=from r in _resourcesRepository.TableNoTracking
                      where (r.IsDeleted == false)
                      select new
                      {
                          Id = r.Id,
                          SeoFilename = r.SeoFilename,
                          ResourceType = r.ResourceType,
                          CreatedOn = r.CreatedOn
                      };
            IList<Resources>resourcesList = new List<Resources>();
            foreach (var item in query)
            {
                var resource = new Resources
                {
                    Id = item.Id,
                    SeoFilename = item.SeoFilename,
                    ResourceType = item.ResourceType,
                    CreatedOn = item.CreatedOn
                };
                resourcesList.Add(resource);
            }
            return resourcesList;
        }


        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="picture"></param>
        public virtual void DeleteResource(Resources resources)
        {
            if (resources == null)
                throw new ArgumentNullException("resources");
            //delete from database
            _resourcesRepository.Delete(resources);
            DeleteResourceThumbs(resources);
            //event notification
            //_eventPublisher.EntityDeleted(resources);
        }

        public IPagedList<Resources> GetAllPagedList(string Name, DateTime? createdOnFrom = null, DateTime? createdOnTo = null, int pageIndex = 0, int pageSize = int.MaxValue)
        {
            //var query = (_resourcesRepository.Table.Where(p => p.IsDeleted == false)).ToList().
            //             Select(r=> new Resources
            //             {
            //                 Id = r.Id,
            //                 SeoFilename = r.SeoFilename,
            //                 ResourceType = r.ResourceType,
            //                 CreatedOn = r.CreatedOn
            //             }).ToList();

            ///查找部分字段
            var query = from r in _resourcesRepository.TableNoTracking
                         where (r.IsDeleted == false)
                         select new
                         {
                             Id = r.Id,
                             SeoFilename = r.SeoFilename,
                             ResourceType = r.ResourceType,
                             CreatedOn = r.CreatedOn
                         };
            IList<Resources> queryable=new List<Resources>();
          
            //var query = _resourcesRepository.TableNoTracking.Where(p => p.IsDeleted == false);
            if (!String.IsNullOrEmpty(Name))                
                query = query.Where(p => p.SeoFilename == Name);
            //if (resourceType != 0)
            //    query = query.Where(p => p. == resourceType);

            if (createdOnFrom.HasValue)
                query = query.Where(p => (createdOnFrom.Value <= p.CreatedOn));
            if (createdOnTo.HasValue)
                query = query.Where(p => createdOnTo.Value >= p.CreatedOn);

            query = query.OrderByDescending(c => c.CreatedOn);
            foreach (var item in query)
            {
                var resource = new Resources
                {
                    Id = item.Id,
                    SeoFilename = item.SeoFilename,
                    ResourceType = item.ResourceType,
                    CreatedOn = item.CreatedOn
                };
                queryable.Add(resource);
            }
            var ResourcesList = new PagedList<Resources>(queryable, pageIndex, pageSize);
            return ResourcesList;
        }
        public Resources InsertResources(byte[] resourcesBinary, string mimeType,
            string seoFilename, bool isNew = true, bool validateBinary = true)
        {
            mimeType = CommonHelper.EnsureNotNull(mimeType);
            mimeType = CommonHelper.EnsureMaximumLength(mimeType, 40);
            seoFilename = CommonHelper.EnsureMaximumLength(seoFilename, 100);
            var resources = new Resources
            {
                CreatedOn = DateTime.Now,
                ResourcesBinary = resourcesBinary,
                MimeType = mimeType,
                SeoFilename = seoFilename,
                IsNew = isNew
            };
           
            _resourcesRepository.Insert(resources);
            string thumbName = GetThumbName(resources);
            resources.ThumbName = thumbName;
            _resourcesRepository.Update(resources);
            return resources;
        }
        public string GetResourcesUrl(Resources resources)
        {
            string url = string.Empty;
            byte[] resourcesBinary = null;
            if (resources != null)
                resourcesBinary = LoadResourcesBinary(resources);

            string thumbFileName = GetThumbName(resources);

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
                        //using (var stream = new MemoryStream(resourcesBinary))
                        //{
                        //    Bitmap b = null;
                        //    try
                        //    {
                        //        //try-catch to ensure that picture binary is really OK. Otherwise, we can get "Parameter is not valid" exception if binary is corrupted for some reasons
                        //        b = new Bitmap(stream);
                        //    }
                        //    catch (ArgumentException exc)
                        //    {
                        //        //_logger.Error(string.Format("Error generating picture thumb. ID={0}", picture.Id),
                        //        //    exc);
                        //    }

                        //    if (b == null)
                        //    {
                        //        //bitmap could not be loaded for some reasons
                        //        return url;
                        //    }

                        //    //using (var destStream = new MemoryStream())
                        //    //{
                        //    //    var newSize = CalculateDimensions(b.Size, targetSize);
                        //    //    ImageBuilder.Current.Build(b, destStream, new ResizeSettings
                        //    //    {
                        //    //        Width = newSize.Width,
                        //    //        Height = newSize.Height,
                        //    //        Scale = ScaleMode.Both,
                        //    //        Quality = 80
                        //    //    });
                        //    //    pictureBinaryResized = destStream.ToArray();
                        //    //    b.Dispose();
                        //    //}
                        //}
                        // pictureBinaryResized = pictureBinary.ToArray();
                        SaveThumb(thumbFilePath, thumbFileName, resourcesBinary);
                    }

                    mutex.ReleaseMutex();
                }

            }


            url = GetThumbUrl(thumbFileName);
            return url;
        }
        public string GetThumbName(Resources resources)
        {
            var seoFileName = resources.SeoFilename; // = GetPictureSeName(picture.SeoFilename); //just for sure

            string lastPart = GetFileExtensionFromMimeType(resources.MimeType);
            string thumbFileName;

            thumbFileName = !String.IsNullOrEmpty(seoFileName)
                ? string.Format("{0}_{1}", resources.Id.ToString("0000000"), seoFileName)
                : string.Format("{0}.{1}", resources.Id.ToString("0000000"), lastPart);
            return thumbFileName;
        }
        protected virtual string GetThumbUrl(string thumbFileName, string storeLocation = null)
        {
            storeLocation = !String.IsNullOrEmpty(storeLocation)
                                    ? storeLocation
                                    : _webHelper.GetStoreLocation();
            var url = storeLocation + "content/resources/";
            url = url + thumbFileName;
            return url;
        }

        protected virtual void SaveThumb(string thumbFilePath, string thumbFileName, byte[] binary)
        {



            File.WriteAllBytes(thumbFilePath, binary);
        }
        #endregion

        #region 辅助方法
        /// <summary>
        /// 根据图片存储设置获取加载的图片二进制
        /// </summary>
        /// <param name="picture">Picture</param>
        /// <returns>Picture binary</returns>
        public virtual byte[] LoadResourcesBinary(Resources resources)
        {
            return LoadResourcesBinary(resources, true);
        }
        public byte[] LoadResourcesBinary(Resources resources, bool fromDb)
        {
            if (resources == null)
                throw new ArgumentNullException("resources");

            var result = fromDb
                ? resources.ResourcesBinary
                : LoadPictureFromFile(resources.Id, resources.MimeType);
            return result;
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
                case "msword"://word 文档
                    lastPart = "doc";
                    break;
                case "octet-stream"://压缩文件
                    lastPart = "zip";
                    break;
                case "plain"://txt文件
                    lastPart = "txt";
                    break;
                case "vnd.openxmlformats-officedoc"://excel文件
                    lastPart = "xlsx";
                    break;
            }
            return lastPart;
        }
        /// <summary>
        /// 获取本地路径。 当图像存储在文件系统（不在数据库中）时使用
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns></returns>
        protected virtual string GetPictureLocalPath(string fileName)
        {
            return Path.Combine(CommonHelper.MapPath("~/content/resources/"), fileName);
        }

        /// <summary>
        /// 删除本地thumb文件
        /// </summary>
        /// <param name="picture"></param>
        protected virtual void DeleteResourceThumbs(Resources resources)
        {
            string filter = string.Format("{0}*.*", resources.Id.ToString("0000000"));
            var thumbDirectoryPath = CommonHelper.MapPath("~/content/resources");
            string[] currentFiles = Directory.GetFiles(thumbDirectoryPath, filter, SearchOption.AllDirectories);
            foreach (var currentFileName in currentFiles)
            {
                var thumbFilePath = GetThumbLocalPath(currentFileName);
                File.Delete(thumbFilePath);
            }
        }


        protected virtual string GetThumbLocalPath(string thumbFileName)
        {
            var thumbsDirectoryPath = CommonHelper.MapPath("~/content/resources");
            var thumbFilePath = Path.Combine(thumbsDirectoryPath, thumbFileName);
            return thumbFilePath;
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

        #endregion
    }
}
