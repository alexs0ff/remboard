using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер документов списания.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class CancellationDocController:BaseController
    {
        public ActionResult Index()
        {
            return View("Index");
        }
    }
}