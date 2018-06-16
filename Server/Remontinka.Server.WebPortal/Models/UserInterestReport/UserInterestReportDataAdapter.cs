using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Helpers;
using Remontinka.Server.WebPortal.Reports;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.WebSite.Helpers;

namespace Remontinka.Server.WebPortal.Models.UserInterestReport
{
    /// <summary>
    /// Сервис данных по вознаграждениям пользователей.
    /// </summary>
    public class UserInterestReportDataAdapter : ReportAdapterBase<UserInterestXtraReport, UserInterestReportParameters>
    {
        /// <summary>
        /// Создает отчет.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Отчет.</returns>
        public override UserInterestXtraReport CreateReport(SecurityToken token)
        {
            return new UserInterestXtraReport() ;
        }

        /// <summary>
        /// Создает параметры отчета.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="context">Контекст контроллера.</param>
        /// <returns>Созданный параметры.</returns>
        public override UserInterestReportParameters CreateReportParameters(SecurityToken token, ControllerContext context)
        {
            return new UserInterestReportParameters
            {
                BeginDate = Utils.GetFirstDayOfMonth(DateTime.Today),
                EndDate = DateTime.Today
            };
        }

        /// <summary>
        /// Вызывается, когда необходимо обновление источника данных для отчета.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="report">Отчет.</param>
        /// <param name="reportParameters">Параметры отчета.</param>
        public override void UpdateDataSource(SecurityToken token, UserInterestXtraReport report, UserInterestReportParameters reportParameters)
        {
            var items = RemontinkaServer.Instance.EntitiesFacade.GetUserInterestReportItems(token,
                                                                                             reportParameters.BeginDate,
                                                                                             reportParameters.EndDate).ToList();

            report.DataSource = items;

            SetPeriodParameters(report, reportParameters.BeginDate, reportParameters.EndDate);
        }
    }
}