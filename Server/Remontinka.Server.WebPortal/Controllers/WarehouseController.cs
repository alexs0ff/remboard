using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер грида складов.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class WarehouseController:BaseController
    {
        public ActionResult Index()
        {
            return View("Index");
        }

        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "Warehouse";
    }
}