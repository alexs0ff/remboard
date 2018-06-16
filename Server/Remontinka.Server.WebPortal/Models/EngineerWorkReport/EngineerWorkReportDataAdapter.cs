using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.XtraReports.UI;
using Remontinka.Server.WebPortal.Helpers;
using Remontinka.Server.WebPortal.Reports;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.EngineerWorkReport
{
    /// <summary>
    /// Сервис данных для отчета по выполненным работам.
    /// </summary>
    public class EngineerWorkReportDataAdapter: ReportAdapterBase<EngineerWorkXtraReport, EngineerWorkReportParameters>
    {
        /// <summary>
        /// Создает отчет.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Отчет.</returns>
        public override EngineerWorkXtraReport CreateReport(SecurityToken token)
        {
            return new EngineerWorkXtraReport();
        }

        /// <summary>
        /// Создает параметры отчета.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Созданный параметры.</returns>
        public override EngineerWorkReportParameters CreateReportParameters(SecurityToken token,ControllerContext context)
        {
            return new EngineerWorkReportParameters {EngineerWorkReportBeginDate = DateTime.Today,
                EngineerWorkReportEndDate = DateTime.Today,
                Engineers = UserHelper.GetUserList(token, ProjectRoleSet.Engineer.ProjectRoleID), };
        }

        /// <summary>
        /// Вызывается, когда необходимо обновление источника данных для отчета.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="report">Отчет.</param>
        /// <param name="reportParameters">Параметры отчета.</param>
        public override void UpdateDataSource(SecurityToken token, EngineerWorkXtraReport report, EngineerWorkReportParameters reportParameters)
        {
            report.DataSource = RemontinkaServer.Instance.EntitiesFacade.GetEngineerWorkReportItems(token, reportParameters.EngineerWorkReportUserID,
                                                                                            reportParameters.EngineerWorkReportBeginDate??DateTime.Today,
                                                                                            reportParameters.EngineerWorkReportEndDate ?? DateTime.Today);
        }
    }
}