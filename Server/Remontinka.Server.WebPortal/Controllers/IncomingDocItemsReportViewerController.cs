using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.IncomingDocItemsReport;
using Remontinka.Server.WebPortal.Reports;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер для Viewerа приходных накладных.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class IncomingDocItemsReportViewerController : ReportControllerBase<IncomingDocItemsXtraReport, IncomingDocItemsReportDataAdapter, IncomingDocItemsReportParameters>
    {
        public IncomingDocItemsReportViewerController() : base(new IncomingDocItemsReportDataAdapter())
        {
        }

        public const string ControllerName = "IncomingDocItemsReportViewer";

        /// <summary>
        /// Возвращает имя контроллера.
        /// </summary>
        /// <returns></returns>
        protected override string GetControllerName()
        {
            return ControllerName;
        }
    }
}