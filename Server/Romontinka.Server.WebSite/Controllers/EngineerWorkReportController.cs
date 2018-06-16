using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.ReportInputs;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер отчета по выполненным работам исполнителей. 
    /// </summary>
     [ExtendedAuthorize(Roles = "Admin, Engineer")]
    public class EngineerWorkReportController : JRdlcReportControllerBase<EngineerWorkReportInput>
    {
         /// <summary>
         /// Содержит имя контроллера.
         /// </summary>
        public const string ControllerName = "EngineerWorkReport";

        public EngineerWorkReportController()
        {
            ReportFile = "EngineerWorkReport.rdlc";
            ReportTitle = "Выполненные работы исполнителей";
        }

        /// <summary>
        /// Создает модель для панели параметров формы отчета по умолчанию.
        /// </summary>
        /// <param name="token">Маркер безопасности.</param>
        /// <param name="urlParameters">Параметры с URL.</param>
        /// <returns>Созданная модель.</returns>
        public override EngineerWorkReportInput CreateDefaultPanel(SecurityToken token, EngineerWorkReportInput urlParameters)
        {
            return new EngineerWorkReportInput { BeginDate = DateTime.Today, EndDate = DateTime.Today };
        }

        /// <summary>
        /// Должен переопределиться для регистрации данных для отчета.
        /// </summary>
        /// <param name="token">Маркер безопасности.</param>
        /// <param name="report">Создаваемый отчет.</param>
        /// <param name="input">Значения полученные с формы ввода.</param>
        /// <returns>Название файла.</returns>
        public override string RegisterData(SecurityToken token, LocalReport report, EngineerWorkReportInput input)
        {
            var items = RemontinkaServer.Instance.EntitiesFacade.GetEngineerWorkReportItems(token, input.UserID,
                                                                                            input.BeginDate,
                                                                                            input.EndDate);
            var dataSource = new ReportDataSource("EngineerWorkDataSet", items);
            report.DataSources.Add(dataSource);

            SetPeriodParameters(report, input.BeginDate, input.EndDate);

            return string.Format("Исполнители с {0:dd.MM.yyyy} по {1:dd.MM.yyyy}", input.BeginDate, input.EndDate);
        }


    }
}