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

namespace Remontinka.Server.WebPortal.Models.UsedDeviceItemsReport
{
    /// <summary>
    /// Сервис данных для отчета по использованным запчастям.
    /// </summary>
    public class UsedDeviceItemsReportDataAdapter : ReportAdapterBase<UsedDeviceItemsXtraReport, UsedDeviceItemsReportParameters>
    {
        /// <summary>
        /// Создает отчет.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Отчет.</returns>
        public override UsedDeviceItemsXtraReport CreateReport(SecurityToken token)
        {
            return new UsedDeviceItemsXtraReport();
        }

        /// <summary>
        /// Создает параметры отчета.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="context">Контекст контроллера.</param>
        /// <returns>Созданный параметры.</returns>
        public override UsedDeviceItemsReportParameters CreateReportParameters(SecurityToken token, ControllerContext context)
        {
            return new UsedDeviceItemsReportParameters { BeginDate = Utils.GetFirstDayOfMonth(DateTime.Today),
                EndDate = DateTime.Today,
                Branches = RemontinkaServer.Instance.EntitiesFacade.GetBranches(token),
                FinancialGroups = RemontinkaServer.Instance.EntitiesFacade.GetFinancialGroupItems(token)
            };
        }

        /// <summary>
        /// Вызывается, когда необходимо обновление источника данных для отчета.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="report">Отчет.</param>
        /// <param name="reportParameters">Параметры отчета.</param>
        public override void UpdateDataSource(SecurityToken token, UsedDeviceItemsXtraReport report,
            UsedDeviceItemsReportParameters reportParameters)
        {
            var items = RemontinkaServer.Instance.EntitiesFacade.GetUsedDeviceItemsReportItems(token, reportParameters.BranchID,
                                                                                               reportParameters.FinancialGroupID,
                                                                                               reportParameters.BeginDate,
                                                                                               reportParameters.EndDate);

            report.DataSource = items;

            SetPeriodParameters(report, reportParameters.BeginDate, reportParameters.EndDate);
        }
    }
}