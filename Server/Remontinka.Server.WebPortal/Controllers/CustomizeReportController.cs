using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер грида управления документами для клиентов
    /// </summary>
    [ExtendedAuthorize]
    public class CustomizeReportController : BaseController
    {
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}