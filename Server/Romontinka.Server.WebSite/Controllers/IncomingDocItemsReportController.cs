using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Numbers;
using Romontinka.Server.Core.Security;
using Romontinka.Server.WebSite.Helpers;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.ReportInputs;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер приходных накладных.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class IncomingDocItemsReportController : JRdlcReportControllerBase<IncomingDocItemsReportInput>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "IncomingDocItemsReport";

        public IncomingDocItemsReportController()
        {
            ReportFile = "IncomingDocItemsReport.rdlc";
            ReportTitle = "Приходная накладная";
            AutoLoad = true;
        }

        /// <summary>
        /// Создает модель для панели параметров формы отчета по умолчанию.
        /// </summary>
        /// <param name="token">Маркер безопасности.</param>
        /// <param name="urlParameters">Параметры с URL.</param>
        /// <returns>Созданная модель.</returns>
        public override IncomingDocItemsReportInput CreateDefaultPanel(SecurityToken token, IncomingDocItemsReportInput urlParameters)
        {
            return new IncomingDocItemsReportInput {IncomingDocID = urlParameters.IncomingDocID};
        }

        /// <summary>
        /// Должен переопределиться для регистрации данных для отчета.
        /// </summary>
        /// <param name="token">Маркер безопасности.</param>
        /// <param name="report">Создаваемый отчет.</param>
        /// <param name="input">Значения полученные с формы ввода.</param>
        /// <returns>Название файла.</returns>
        public override string RegisterData(SecurityToken token, LocalReport report, IncomingDocItemsReportInput input)
        {
            var incomingDoc = RemontinkaServer.Instance.EntitiesFacade.GetIncomingDoc(token, input.IncomingDocID);

            if (incomingDoc==null)
            {
                throw new Exception("Нет такого документа");
            } //if
            
            var items = RemontinkaServer.Instance.EntitiesFacade.GetIncomingDocItems(token, input.IncomingDocID).ToList();
            var dataSource = new ReportDataSource("ReportDataSet", items);
            report.DataSources.Add(dataSource);

            report.SetParameters(new ReportParameter("DocDate", Utils.DateTimeToString(incomingDoc.DocDate)));
            report.SetParameters(new ReportParameter("WarehouseTitle", incomingDoc.WarehouseTitle));
            report.SetParameters(new ReportParameter("ContractorLegalName", incomingDoc.ContractorLegalName));
            report.SetParameters(new ReportParameter("DocNumber", incomingDoc.DocNumber));
            report.SetParameters(new ReportParameter("DocDescription", incomingDoc.DocDescription??string.Empty));
            report.SetParameters(new ReportParameter("TotalCount", Utils.IntToString(items.Count)));
            
            var totalSum = items.Sum(i => i.Total*i.InitPrice);

            report.SetParameters(new ReportParameter("TotalSum", Utils.DecimalToString(totalSum)));
            report.SetParameters(new ReportParameter("TotalRusSum", RusCurrency.StrRur((double)totalSum)));

            return string.Format("Приходная накладная номер {0} от {1:dd.MM.yyyy}", incomingDoc.DocNumber, incomingDoc.DocDate);
        }
    }
}