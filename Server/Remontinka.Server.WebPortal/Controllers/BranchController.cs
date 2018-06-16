using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер для управления филиалами.
    /// </summary>
    [ExtendedAuthorize]
    public class BranchController : BaseController
    {
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}