using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.WarehouseFlowReport;
using Remontinka.Server.WebPortal.Reports;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер отображения отчета по приходу и расходу на складе.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class WarehouseFlowReportViewerController : ReportControllerBase<WarehouseFlowXtraReport, WarehouseFlowReportDataAdapter, WarehouseFlowReportParameters>
    {
        public WarehouseFlowReportViewerController() : base(new WarehouseFlowReportDataAdapter())
        {
        }

        public const string ControllerName = "WarehouseFlowReportViewer";

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