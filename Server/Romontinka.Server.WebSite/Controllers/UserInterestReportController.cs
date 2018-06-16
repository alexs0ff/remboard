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
    /// Контроллер отчета по вознаграждению пользователей.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class UserInterestReportController : JRdlcReportControllerBase<UserInterestReportInput>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "UserInterestReport";

        public UserInterestReportController()
        {
            ReportFile = "UserInterestReport.rdlc";
            ReportTitle = "Отчет по вознаграждениям";
        }

        /// <summary>
        /// Создает модель для панели параметров формы отчета по умолчанию.
        /// </summary>
        /// <param name="token">Маркер безопасности.</param>
        /// <param name="urlParameters">Параметры с URL.</param>
        /// <returns>Созданная модель.</returns>
        public override UserInterestReportInput CreateDefaultPanel(SecurityToken token, UserInterestReportInput urlParameters)
        {
            return new UserInterestReportInput
                   {
                       BeginDate = Utils.GetFirstDayOfMonth(DateTime.Today),
                       EndDate = DateTime.Today

                   };
        }

        /// <summary>
        /// Должен переопределиться для регистрации данных для отчета.
        /// </summary>
        /// <param name="token">Маркер безопасности.</param>
        /// <param name="report">Создаваемый отчет.</param>
        /// <param name="input">Значения полученные с формы ввода.</param>
        /// <returns>Название файла.</returns>
        public override string RegisterData(SecurityToken token, LocalReport report, UserInterestReportInput input)
        {
            var items = RemontinkaServer.Instance.EntitiesFacade.GetUserInterestReportItems(token, 
                                                                                              input.BeginDate,
                                                                                              input.EndDate).ToList();
            var dataSource = new ReportDataSource("ReportDataSet", items);
            report.DataSources.Add(dataSource);

            SetPeriodParameters(report, input.BeginDate, input.EndDate);

            return string.Format("Вознаграждения с {0:dd.MM.yyyy} по {1:dd.MM.yyyy}", input.BeginDate, input.EndDate);
        }
    }
}