using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер текущих расходов и доходов.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class FinancialItemValueController:BaseController
    {
        public ActionResult Index()
        {
            return View("Index");
        }

        public const string ControllerName = "FinancialItemValue";
    }
}