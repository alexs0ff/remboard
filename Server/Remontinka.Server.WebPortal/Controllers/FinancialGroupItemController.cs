using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер финансовых филиалов.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class FinancialGroupItemController : BaseController
    {
        public ActionResult Index()
        {
            return View("Index");
        }

        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "FinancialGroupItem";
    }
}