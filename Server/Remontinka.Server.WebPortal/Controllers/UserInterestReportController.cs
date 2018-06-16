using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Отчет по доходам пользователей.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class UserInterestReportController:BaseController
    {
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}