using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.CustomizeReportGridForm;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер грида управления документами.
    /// </summary>
    [ExtendedAuthorize]
    public class CustomizeReportGridController : GridControllerBase<Guid, CustomizeReportGridModel, CustomizeReportCreateModel, CustomizeReportCreateModel>
    {
        public CustomizeReportGridController() : base(new CustomizeReportGridDataAdapter())
        {
        }

        public const string ControllerName = "CustomizeReportGrid";

        /// <summary>
        /// Получает название контроллера.
        /// </summary>
        /// <returns>Название контроллера.</returns>
        protected override string GetControllerName()
        {
            return ControllerName;
        }

        /// <summary>
        /// Получает название грида.
        /// </summary>
        /// <returns>Название грида.</returns>
        protected override string GetGridName()
        {
            return "CustomizeReportsGrid";
        }


        public ActionResult HtmlEditor()
        {
            return PartialView("HtmlEditor");
        }
    }
}