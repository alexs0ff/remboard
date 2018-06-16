using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models;
using Remontinka.Server.WebPortal.Models.UserInterestReport;
using Remontinka.Server.WebPortal.Reports;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Отображение отчета по доходам пользователей.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class UserInterestReportViewerController : ReportControllerBase<UserInterestXtraReport, UserInterestReportDataAdapter, UserInterestReportParameters>
    {
        public UserInterestReportViewerController() : base(new UserInterestReportDataAdapter())
        {
        }

        public const string ControllerName = "UserInterestReportViewer";

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