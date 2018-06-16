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
    /// Контроллер отчета по использовальзованным запчастям.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class UsedDeviceItemsReportController : JRdlcReportControllerBase<UsedDeviceItemsReportInput>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "UsedDeviceItemsReport";

        public UsedDeviceItemsReportController()
        {
            ReportFile = "UsedDeviceItemsReport.rdlc";
            ReportTitle = "Отчет по установленным запчастям";
        }

        /// <summary>
        /// Создает модель для панели параметров формы отчета по умолчанию.
        /// </summary>
        /// <param name="token">Маркер безопасности.</param>
        /// <param name="urlParameters">Параметры с URL.</param>
        /// <returns>Созданная модель.</returns>
        public override UsedDeviceItemsReportInput CreateDefaultPanel(SecurityToken token, UsedDeviceItemsReportInput urlParameters)
        {
            return new UsedDeviceItemsReportInput { BeginDate = Utils.GetFirstDayOfMonth(DateTime.Today), EndDate = DateTime.Today };
        }

        /// <summary>
        /// Должен переопределиться для регистрации данных для отчета.
        /// </summary>
        /// <param name="token">Маркер безопасности.</param>
        /// <param name="report">Создаваемый отчет.</param>
        /// <param name="input">Значения полученные с формы ввода.</param>
        /// <returns>Название файла.</returns>
        public override string RegisterData(SecurityToken token, LocalReport report, UsedDeviceItemsReportInput input)
        {
            var items = RemontinkaServer.Instance.EntitiesFacade.GetUsedDeviceItemsReportItems(token, input.BranchID,
                                                                                               input.FinancialGroupID,
                                                                                               input.BeginDate,
                                                                                               input.EndDate);
            var dataSource = new ReportDataSource("ReportDataSet", items);
            report.DataSources.Add(dataSource);

            SetPeriodParameters(report, input.BeginDate, input.EndDate);

            return string.Format("Запчасти с {0:dd.MM.yyyy} по {1:dd.MM.yyyy}", input.BeginDate, input.EndDate);
        }
    }
}