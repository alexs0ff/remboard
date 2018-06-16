using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models.EngineerWorkReport;
using Remontinka.Server.WebPortal.Reports;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер для отчетов по выполненным работам.
    /// </summary>
    [ExtendedAuthorize]
    public class EngineerWorkReportViewerController: ReportControllerBase<EngineerWorkXtraReport, EngineerWorkReportDataAdapter, EngineerWorkReportParameters>
    {
        public EngineerWorkReportViewerController() : base(new EngineerWorkReportDataAdapter())
        {
        }

        public const string ControllerName = "EngineerWorkReportViewer";

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