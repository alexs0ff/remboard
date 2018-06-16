using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.UsedDeviceItemsReport;
using Remontinka.Server.WebPortal.Reports;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер для проверки установленных запчастей.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class UsedDeviceItemsReportViewerController : ReportControllerBase<UsedDeviceItemsXtraReport, UsedDeviceItemsReportDataAdapter, UsedDeviceItemsReportParameters>
    {
        public UsedDeviceItemsReportViewerController() : base(new UsedDeviceItemsReportDataAdapter())
        {
        }

        public const string ControllerName = "UsedDeviceItemsReportViewer";

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