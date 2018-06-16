using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Models.HomeForm;
using Romontinka.Server.Core;

namespace Remontinka.Server.WebPortal.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

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

        public ActionResult GetRevervedFile()
        {
            var path = Request.MapPath("~");
            path = path + "..\\Tmp\\message.txt";

            return File(path, "text");
        }


    }
}