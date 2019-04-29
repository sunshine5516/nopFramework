using NopFramework.Core.Caching;
using NopFramework.Core.Domains.Logging;
using NopFramework.Core.Domains.Messages;
using NopFramework.Services.Logging;
using NopFramework.Services.Messages;
using NopFramework.Web.Models.Contact;
using NopFramework.Web.Models.Introduce;
using NopFramework.Web.Models.Message;
using NopFramework.Web.Models.Term;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace NopFramework.Web.Controllers
{
    public class ContactController : BasePublicController
    {
        #region 声明实例
        private readonly IContactMessageService _contactMessageService;
        private readonly IQueuedEmailService _queuedEmailService;
        private readonly IEmailAccountService _emailAccountService;
        private readonly ICacheManager _cacheManager;
        private readonly ICustomerActivityService _customerActivityService;
        private readonly ITermService _termService;
        private readonly EmailAccountSettings _emailAccountSettings;
        #endregion
        #region 构造函数
        public ContactController(IContactMessageService contactMessageService,
            IQueuedEmailService queuedEmailService, 
            IEmailAccountService emailAccountService, ICacheManager cacheManager,
            ICustomerActivityService customerActivityService,
            ITermService termService,
            EmailAccountSettings emailAccountSettings)
        {
            this._contactMessageService = contactMessageService;
            this._cacheManager = cacheManager;
            this._emailAccountService = emailAccountService;
            this._queuedEmailService = queuedEmailService;
            this._customerActivityService = customerActivityService;
            this._emailAccountSettings = emailAccountSettings;
            this._termService = termService;
        }
        #endregion
        #region 方法
        // GET: Contact
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ContactUs()
        {
            ContactUsModel contactUs = new ContactUsModel {};
            return View(contactUs);
        }
        [HttpPost]
        public ActionResult ContactUs(ContactUsModel contactUs)
        {
            if (ModelState.IsValid)
            {
                string email = contactUs.Email.Trim();
                string body = Core.Html.HtmlHelper.FormatText(contactUs.Content, false, true, false, false, false, false);
                var emailAccount = _emailAccountService.GetEmailAccountById(_emailAccountSettings.DefaultEmailAccountId);
                if (emailAccount == null)
                    emailAccount = _emailAccountService.GetAllEmailAccounts().FirstOrDefault();
                if (emailAccount == null)
                    throw new Exception("No email account could be loaded");
                string from= email;
                string fromName=contactUs.Name;
                body = string.Format("<strong>From</strong>: {0} - {1}<br /><br />{2}",
                       Server.HtmlEncode(contactUs.Name),
                       Server.HtmlEncode(email), body);

                _queuedEmailService.InsertQueuedEmail(new QueuedEmail {
                    From=from,
                    FromName=fromName,
                    To=emailAccount.Email,
                    ToName=emailAccount.DisplayName,
                    ReplyTo= email,
                    ReplyToName=fromName,
                    Priority= QueuedEmailPriority.High,
                    Subject="联系我们",
                    Body=body,
                    CreatedOn=DateTime.Now,
                    EmailAccountId=emailAccount.Id

                });

                var contact = new ContactMessage
                {
                    Email= contactUs.Email,
                    Name=contactUs.Name,
                    Content=contactUs.Content,
                    CreatedOn=DateTime.Now,
                    Telephone=contactUs.Telephone
                };
                contactUs.SuccessfullySent = true;
                contactUs.Result = "发送成功";

                _contactMessageService.InsertContactMessage(contact);
                _customerActivityService.InsertActivity(LogType.联系衡芯.ToString(), LogType.联系衡芯.ToString() + "(Name = {0})");
                

                //string body = Core.Html.HtmlHelper.FormatText(model.Enquiry, false, true, false, false, false, false);
            }
            return View(contactUs);

        }
        public ActionResult SubmitOnline()
        {
            return View();
        }

        public ActionResult JobApplication()
        {
            var query = _termService.GetAllEntities().
               Where(q => q.TermType== Convert.ToInt32(TermTypeModel.Recruitment))
               .FirstOrDefault() ;
            if (query == null)
            {
                var temp = new TermModel
                {
                    Name = "招聘信息",
                    FullDescription = "暂无"
                };
                return View(temp);
            }
            var model = PrepareTermModel(query);
            return View(model);
        }
        public ActionResult JobDetails(int recruitmentId)
        {
           
            return View();
        }

        [ChildActionOnly]
        public ActionResult ContactMenu()
        {
            return PartialView();
        }

        [ChildActionOnly]
        public ActionResult JobsMenu()
        {
           
        }

        #endregion
        #region 辅助方法
        public IList<RecruitmentModel> PrepareJobModels(IList<Recruitment> recruitments,
             bool preparePictureModel = true)
        {
            IList<RecruitmentModel> recruitmentModels = new List<RecruitmentModel>();
            foreach (var product in recruitments)
            {
                var recruitment = new RecruitmentModel
                {
                    Id = product.Id,
                    Name = product.Name,
                    FullDescription = product.FullDescription,
                    ShortDescription = product.ShortDescription,
                    UpdatedOn = product.UpdatedOn
                };
                recruitmentModels.Add(recruitment);
            }
            return recruitmentModels;
        }

        public RecruitmentModel PrepareJobModel(Recruitment recruitments,
           bool preparePictureModel = true)
        {
           
                var recruitment = new RecruitmentModel
                {
                    Id = recruitments.Id,
                    Name = recruitments.Name,
                    FullDescription = recruitments.FullDescription,
                    ShortDescription = recruitments.ShortDescription,
                    UpdatedOn = recruitments.UpdatedOn
                };

            return recruitment;
        }


        protected virtual TermModel PrepareTermModel(Term term)
        {
            var termModel = new TermModel
            {
                Id = term.Id,
                Name = term.Name,
                ShortDescription = term.ShortDescription,
                FullDescription = term.FullDescription,
                TermType = term.TermType
            };            
            return termModel;
        }
        #endregion
    }
}