using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Reports;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.WebSite.Helpers;

namespace Remontinka.Server.WebPortal.Models.WarehouseFlowReport
{
    /// <summary>
    /// Сервис данных по приходу и расходу на складах.
    /// </summary>
    public class WarehouseFlowReportDataAdapter : ReportAdapterBase<WarehouseFlowXtraReport, WarehouseFlowReportParameters>
    {
        /// <summary>
        /// Создает отчет.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Отчет.</returns>
        public override WarehouseFlowXtraReport CreateReport(SecurityToken token)
        {
            return new WarehouseFlowXtraReport();
        }

        /// <summary>
        /// Создает параметры отчета.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="context">Контекст контроллера.</param>
        /// <returns>Созданный параметры.</returns>
        public override WarehouseFlowReportParameters CreateReportParameters(SecurityToken token, ControllerContext context)
        {
            Guid? warehauseID = Guid.Empty;
            warehauseID =RemontinkaServer.Instance.EntitiesFacade.GetWarehouses(token).Select(i => i.WarehouseID).FirstOrDefault();
            return new WarehouseFlowReportParameters
            {
                BeginDate = Utils.GetFirstDayOfMonth(DateTime.Today),
                EndDate = DateTime.Today,
                Warehouses = RemontinkaServer.Instance.EntitiesFacade.GetWarehouses(token),
                WarehouseID = warehauseID
            };
        }

        /// <summary>
        /// Вызывается, когда необходимо обновление источника данных для отчета.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="report">Отчет.</param>
        /// <param name="reportParameters">Параметры отчета.</param>
        public override void UpdateDataSource(SecurityToken token, WarehouseFlowXtraReport report,
            WarehouseFlowReportParameters reportParameters)
        {
            var data = RemontinkaServer.Instance.EntitiesFacade.GetWarehouseFlowReportItems(token,
                                                                                                    reportParameters.
                                                                                                        WarehouseID,
                                                                                                    reportParameters.BeginDate,
                                                                                                    reportParameters.EndDate).ToList();

            var warehouse = RemontinkaServer.Instance.EntitiesFacade.GetWarehouse(token, reportParameters.WarehouseID);
            
            report.DataSource = data;

            SetPeriodParameters(report, reportParameters.BeginDate, reportParameters.EndDate);
            report.Parameters["WarehouseTitle"].Value = warehouse.Title;
        }
    }
}