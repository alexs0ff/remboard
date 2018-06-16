using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Romontinka.Server.Core;

namespace Remontinka.Server.WebPortal.Controllers
{

    /// <summary>
    /// Контроллер заказов.
    /// </summary>
    [ExtendedAuthorize]
    public class RepairOrderController:BaseController
    {
        public ActionResult Index()
        {
            ViewData["SecurityToken"] = GetToken();
            return View("Index");
        }
    }
}