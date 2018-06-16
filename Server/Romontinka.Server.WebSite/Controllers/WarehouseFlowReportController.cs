using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.WebSite.Helpers;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.ReportInputs;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер по доходам и расходам.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class WarehouseFlowReportController : JRdlcReportControllerBase<WarehouseFlowReportInput>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "WarehouseFlowReport";

        public WarehouseFlowReportController()
        {
            ReportFile = "WarehouseFlowReport.rdlc";
            ReportTitle = "Отчет по приходу и расходу на складе";
        }

        /// <summary>
        /// Создает модель для панели параметров формы отчета по умолчанию.
        /// </summary>
        /// <param name="token">Маркер безопасности.</param>
        /// <param name="urlParameters">Параметры с URL.</param>
        /// <returns>Созданная модель.</returns>
        public override WarehouseFlowReportInput CreateDefaultPanel(SecurityToken token, WarehouseFlowReportInput urlParameters)
        {
            return new WarehouseFlowReportInput
                   {
                       BeginDate = Utils.GetFirstDayOfMonth(DateTime.Today),
                       EndDate = DateTime.Today,
                   };
        }

        /// <summary>
        /// Должен переопределиться для регистрации данных для отчета.
        /// </summary>
        /// <param name="token">Маркер безопасности.</param>
        /// <param name="report">Создаваемый отчет.</param>
        /// <param name="input">Значения полученные с формы ввода.</param>
        /// <returns>Название файла.</returns>
        public override string RegisterData(SecurityToken token, LocalReport report, WarehouseFlowReportInput input)
        {
            var data = RemontinkaServer.Instance.EntitiesFacade.GetWarehouseFlowReportItems(token,
                                                                                                    input.
                                                                                                        WarehouseID,
                                                                                                    input.BeginDate,
                                                                                                    input.EndDate).ToList();

            var warehouse = RemontinkaServer.Instance.EntitiesFacade.GetWarehouse(token, input.WarehouseID);

            var dataSource = new ReportDataSource("ReportDataSet", data);
            report.DataSources.Add(dataSource);

            SetPeriodParameters(report, input.BeginDate, input.EndDate);
            report.SetParameters(new ReportParameter("WarehouseTitle", warehouse.Title));

            return string.Format("Приход и расход с {0:dd.MM.yyyy} по {1:dd.MM.yyyy} на складе {2}", input.BeginDate,
                                 input.EndDate, warehouse.Title);
        }
    }
}