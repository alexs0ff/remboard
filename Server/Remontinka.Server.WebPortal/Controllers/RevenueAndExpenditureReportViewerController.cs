using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.RevenueAndExpenditureReport;
using Remontinka.Server.WebPortal.Reports;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер отображения отчета дохода и расхода.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class RevenueAndExpenditureReportViewerController : ReportControllerBase<RevenueAndExpenditureXtraReport, RevenueAndExpenditureReportDataAdapter, RevenueAndExpenditureReportParameters>
    {
        public RevenueAndExpenditureReportViewerController() : base(new RevenueAndExpenditureReportDataAdapter())
        {
        }

        public const string ControllerName = "RevenueAndExpenditureReportViewer";

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