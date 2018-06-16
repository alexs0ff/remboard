using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Romontinka.Server.Core;
using Romontinka.Server.WebSite.Models;
using Romontinka.Server.WebSite.Models.HomeForm;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Главный контроллер сайта.
    /// </summary>
    public class HomeController : BaseController
    {
        //public ActionResult Index()
        //{
        //    return View("Test");
        //}

        /// <summary>
        /// Метод получения главной страницы.
        /// </summary>
        public ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Метод получения описания.
        /// </summary>
        public ActionResult Description()
        {
            return View();
        }

        /// <summary>
        /// Метод получения описания бухгалтерии.
        /// </summary>
        /// <returns></returns>
        public ActionResult AccountingDescription()
        {
            return View();
        }

        /// <summary>
        /// Метод получения описания складского учета.
        /// </summary>
        /// <returns></returns>
        public ActionResult WarehouseDescription()
        {
            return View();
        }

        /// <summary>
        /// Метод получения описания offline клиента.
        /// </summary>
        /// <returns></returns>
        public ActionResult ClientDescription()
        {
            return View();
        }

        [Authorize(Users = "ivanov")]
        [HttpGet]
        public ActionResult ReverseFile()
        {
            return View("ReverseFile");
        }

        [Authorize(Users = "ivanov")]
        [HttpPost]
        public ActionResult ReverseFile(HttpPostedFileBase file)
        {
            try
            {
                if (file != null && file.ContentLength > 0)
                {
                    // extract only the filename
                    var path = Request.MapPath("~");
                    path = path + "..\\Tmp\\message.txt";
                    file.SaveAs(path);
                }
            }
            catch (Exception ex)
            {
                
                
            }
            return View("ReverseFile");
        }

        /// <summary>
        /// Страница с контактными данными и формой отзыва.
        /// </summary>
        [HttpGet]
        public ActionResult Feedback()
        {
            return View(new FeedbackModel());
        }

        /// <summary>
        /// Метод получения фидбека.
        /// </summary>
        /// <param name="model">Модель.</param>
        [HttpPost]
        public ActionResult Feedback(FeedbackModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            } //if

            var result = new FeedbackModel
                         {
                             ResultText =
                                 "Ваш запрос успешно отправлен. Ответ будет выслан в ближайшее время. Сложность вопроса может увеличить время ожидания до 24 рабочих часов. Благодарим за ожидание."
                                 ,
                             Caption = string.Empty,
                             Contact = string.Empty,
                             Text = string.Empty
                         };

            if (!string.IsNullOrWhiteSpace(model.Text))
            {
                RemontinkaServer.Instance.MailingService.Send(
                    RemontinkaServer.Instance.MailingService.FeedbackEmail,
                    string.Format("Feedback {0}:{1}", model.Caption, Request.UserHostName),
                    string.Format("{0}{1}Контактные данные:{2}", model.Text, Environment.NewLine, model.Contact));
            } //if
            ModelState.Clear();
            return View(result);
        }

        /// <summary>
        /// Страница со списком отчетов.
        /// </summary>
        [Authorize]
        public ActionResult ReportList()
        {
            return View();
        }

        public ActionResult GetRevervedFile()
        {
            var path = Request.MapPath("~");
            path = path + "..\\Tmp\\message.txt";

            return File(path, "text");
        }
    }
}
