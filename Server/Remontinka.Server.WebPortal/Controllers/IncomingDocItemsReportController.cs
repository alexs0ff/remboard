using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер представления отчета приходных накладных.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class IncomingDocItemsReportController:BaseController
    {
        public ActionResult Index()
        {
            return View("Index");
        }

        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "IncomingDocItemsReport";
    }
}