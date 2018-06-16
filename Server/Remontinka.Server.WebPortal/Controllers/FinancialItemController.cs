using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер статей бюджета.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class FinancialItemController : BaseController
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "FinancialItem";

        public ActionResult Index()
        {
            return View("Index");
        }
    }
}