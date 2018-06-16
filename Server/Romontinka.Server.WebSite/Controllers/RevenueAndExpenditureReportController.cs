using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.Reporting.WebForms;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.DataLayer.Entities.ReportItems;
using Romontinka.Server.DataLayer.EntityFramework;
using Romontinka.Server.WebSite.Helpers;
using Romontinka.Server.WebSite.Metadata;
using Romontinka.Server.WebSite.Models.ReportInputs;

namespace Romontinka.Server.WebSite.Controllers
{
    /// <summary>
    /// Контроллер отчета о доходах и расходах.
    /// </summary>
    [ExtendedAuthorize(Roles = "Admin")]
    public class RevenueAndExpenditureReportController : JRdlcReportControllerBase<RevenueAndExpenditureReportInput>
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "RevenueAndExpenditureReport";

        public RevenueAndExpenditureReportController()
        {
            ReportFile = "RevenueAndExpenditureReport.rdlc";
            ReportTitle = "Отчет о дохадах и расходах";
        }

        /// <summary>
        /// Создает модель для панели параметров формы отчета по умолчанию.
        /// </summary>
        /// <param name="token">Маркер безопасности.</param>
        /// <param name="urlParameters">Параметры с URL.</param>
        /// <returns>Созданная модель.</returns>
        public override RevenueAndExpenditureReportInput CreateDefaultPanel(SecurityToken token, RevenueAndExpenditureReportInput urlParameters)
        {
            Guid? groupID = null;

            var groups =
                RemontinkaServer.Instance.EntitiesFacade.GetFinancialGroupItems(token).Select(i => i.FinancialGroupID).ToList();
            //Если в домене всего одна фин группа, выбираем ее.
            if (groups.Count == 1)
            {
                groupID = groups.FirstOrDefault();
            }

            return new RevenueAndExpenditureReportInput
            {
                BeginDate = Utils.GetFirstDayOfMonth(DateTime.Today),
                EndDate = Utils.GetLastDayOfMonth(DateTime.Today),
                FinancialGroupID = groupID
            };
        }

        /// <summary>
        /// Должен переопределиться для регистрации данных для отчета.
        /// </summary>
        /// <param name="token">Маркер безопасности.</param>
        /// <param name="report">Создаваемый отчет.</param>
        /// <param name="input">Значения полученные с формы ввода.</param>
        /// <returns>Название файла.</returns>
        public override string RegisterData(SecurityToken token, LocalReport report, RevenueAndExpenditureReportInput input)
        {
            var data = RemontinkaServer.Instance.EntitiesFacade.GetRevenueAndExpenditureReportItems(token,
                                                                                                    input.
                                                                                                        FinancialGroupID,
                                                                                                    input.BeginDate,
                                                                                                    input.EndDate).ToList();

            var finGroup = RemontinkaServer.Instance.EntitiesFacade.GetFinancialGroupItem(token, input.FinancialGroupID);

            ProcessWorkAndDeviceTotals(token, data, input, finGroup);
            ProcessWarehouseIncomingDocTotals(token, data, input, finGroup);

            var dataSource = new ReportDataSource("ReportDataSet", data);
            report.DataSources.Add(dataSource);

            SetPeriodParameters(report, input.BeginDate, input.EndDate);
            report.SetParameters(new ReportParameter("GroupLegalName", finGroup.Title));

            return string.Format("Доходы и расходы с {0:dd.MM.yyyy} по {1:dd.MM.yyyy} {2}", input.BeginDate,
                                 input.EndDate, finGroup.LegalName);
        }

        /// <summary>
        /// Название статьи доходов за ремонтные работы
        /// </summary>
        private const string OrderPaidTitleDefault = "Доход от оплат за ремонтные работы";

        /// <summary>
        /// Обрабатывает статью о приходе сервисном дейстельности и ремонте.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="data">Данные по другим статьям.</param>
        /// <param name="input">Данные ввода пользователя. </param>
        /// <param name="financialGroup">Финансовая группы. </param>
        private void ProcessWorkAndDeviceTotals(SecurityToken token, List<RevenueAndExpenditureReportItem> data, RevenueAndExpenditureReportInput input,FinancialGroupItem financialGroup)
        {
            var totals = RemontinkaServer.Instance.EntitiesFacade.GetOrderPaidAmountByOrderIssueDate(token,
                                                                                                             input.
                                                                                                                 FinancialGroupID,
                                                                                                             input.
                                                                                                                 BeginDate,
                                                                                                             input.
                                                                                                                 EndDate);

            if (totals!=null)
            {
                var finItem =
                RemontinkaServer.Instance.DataStore.GetFinancialItemByFinancialItemKind(
                    FinancialItemKindSet.OrderPaid.FinancialItemKindID, 
                    token.User.UserDomainID);

                var reportItem = new RevenueAndExpenditureReportItem
                                 {
                                     EventDate = input.EndDate,
                                     FinancialGroupLegalName = financialGroup.LegalName,
                                     FinancialGroupTitle = financialGroup.Title
                                 };
                if (finItem!=null)
                {
                    reportItem.Title = finItem.Title;
                    if (finItem.TransactionKindID == TransactionKindSet.Revenue.TransactionKindID)
                    {
                        reportItem.RevenueAmount = totals.TotalAmount ?? decimal.Zero;
                    } //if
                    else
                    {
                        reportItem.ExpenditureAmount = totals.TotalAmount ?? decimal.Zero;
                    } //else
                } //if
                else
                {
                    reportItem.Title = OrderPaidTitleDefault;
                    reportItem.RevenueAmount = totals.TotalAmount ?? decimal.Zero;
                } //else
                data.Add(reportItem);
            } //if
        }

        /// <summary>
        /// Название статьи расходов по приходным накладным.
        /// </summary>
        private const string IncomingDocTitleDefault = "Оплата запчастей по приходной накладной";

        /// <summary>
        /// Обрабатывает итоги по приходным накладным..
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="data">Данные по другим статьям.</param>
        /// <param name="input">Данные ввода пользователя. </param>
        /// <param name="financialGroup">Финансовая группы. </param>
        private void ProcessWarehouseIncomingDocTotals(SecurityToken token, List<RevenueAndExpenditureReportItem> data, RevenueAndExpenditureReportInput input, FinancialGroupItem financialGroup)
        {
            var totals = RemontinkaServer.Instance.EntitiesFacade.GetWarehouseDocTotalItems(token,
                                                                                            input.FinancialGroupID,
                                                                                            input.BeginDate,
                                                                                            input.EndDate);

            var finItem =
                RemontinkaServer.Instance.DataStore.GetFinancialItemByFinancialItemKind(
                    FinancialItemKindSet.WarhouseItemsPaid.FinancialItemKindID,
                    token.User.UserDomainID);

            foreach (var warehouseDocTotalItem in totals)
            {
                var reportItem = new RevenueAndExpenditureReportItem
                {
                    EventDate = warehouseDocTotalItem.DocDate,
                    FinancialGroupLegalName = financialGroup.LegalName,
                    FinancialGroupTitle = financialGroup.Title
                };

                if (finItem != null)
                {
                    reportItem.Title = finItem.Title;
                    if (finItem.TransactionKindID == TransactionKindSet.Revenue.TransactionKindID)
                    {
                        reportItem.RevenueAmount = warehouseDocTotalItem.SumInitPriceTotal;
                    } //if
                    else
                    {
                        reportItem.ExpenditureAmount = warehouseDocTotalItem.SumInitPriceTotal;
                    } //else
                } //if
                else
                {
                    reportItem.Title = IncomingDocTitleDefault;
                    reportItem.ExpenditureAmount = warehouseDocTotalItem.SumInitPriceTotal;
                } //else

                reportItem.Title = string.Format("{0} (накладная номер {1} от {2})", reportItem.Title,
                                                 warehouseDocTotalItem.DocNumber,
                                                 Utils.DateTimeToString(warehouseDocTotalItem.DocDate));
                
                data.Add(reportItem);
            }

        }
    }
}