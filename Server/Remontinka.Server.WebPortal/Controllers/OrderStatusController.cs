using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер статусов заказов.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class OrderStatusController : BaseController
    {
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}