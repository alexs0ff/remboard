using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер для отображаения отчета по выполненным работам.
    /// </summary>
    [ExtendedAuthorize]
    public class EngineerWorkReportController:BaseController
    {
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}