using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Remontinka.Server.WebPortal.Reports;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Numbers;
using Romontinka.Server.Core.Security;

namespace Remontinka.Server.WebPortal.Models.IncomingDocItemsReport
{
    /// <summary>
    /// Сервис данных для отчета по приходной накладной.
    /// </summary>
    public class IncomingDocItemsReportDataAdapter : ReportAdapterBase<IncomingDocItemsXtraReport, IncomingDocItemsReportParameters>
    {
        /// <summary>
        /// Создает отчет.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Отчет.</returns>
        public override IncomingDocItemsXtraReport CreateReport(SecurityToken token)
        {
            return new IncomingDocItemsXtraReport();
        }

        /// <summary>
        /// Создает параметры отчета.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="context">Контекст контроллера.</param>
        /// <returns>Созданный параметры.</returns>
        public override IncomingDocItemsReportParameters CreateReportParameters(SecurityToken token,ControllerContext context)
        {
            var idRaw = context.ParentActionViewContext.RequestContext.HttpContext.Request["IncomingDocID"];
            Guid docID;
            if (!Guid.TryParse(idRaw, out docID))
            {
                docID = Guid.Empty;
            }
            return new IncomingDocItemsReportParameters
            {
                IncomingDocID = docID
            };
        }

        /// <summary>
        /// Вызывается, когда необходимо обновление источника данных для отчета.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="report">Отчет.</param>
        /// <param name="reportParameters">Параметры отчета.</param>
        public override void UpdateDataSource(SecurityToken token, IncomingDocItemsXtraReport report,
            IncomingDocItemsReportParameters reportParameters)
        {
            var incomingDoc = RemontinkaServer.Instance.EntitiesFacade.GetIncomingDoc(token, reportParameters.IncomingDocID);

            if (incomingDoc == null)
            {
                throw new Exception("Нет такого документа");
            } //if

            report.Parameters["DocDate"].Value=incomingDoc.DocDate;
            report.Parameters["WarehouseTitle"].Value= incomingDoc.WarehouseTitle;
            report.Parameters["ContractorLegalName"].Value = incomingDoc.ContractorLegalName;
            report.Parameters["DocNumber"].Value = incomingDoc.DocNumber;
            report.Parameters["DocDescription"].Value = incomingDoc.DocDescription;

            var items = RemontinkaServer.Instance.EntitiesFacade.GetIncomingDocItems(token, reportParameters.IncomingDocID).ToList();
            var totalSum = items.Sum(i => i.Total * i.InitPrice);
            report.Parameters["TotalRusSum"].Value = RusCurrency.StrRur((double) totalSum);
            report.DataSource = items;
        }
    }
}