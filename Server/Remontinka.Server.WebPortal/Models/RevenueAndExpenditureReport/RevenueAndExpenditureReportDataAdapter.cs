using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.DocumentServices.ServiceModel.DataContracts;
using Remontinka.Server.WebPortal.Helpers;
using Remontinka.Server.WebPortal.Reports;
using Romontinka.Server.Core;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.DataLayer.Entities.ReportItems;
using Romontinka.Server.WebSite.Helpers;

namespace Remontinka.Server.WebPortal.Models.RevenueAndExpenditureReport
{
    /// <summary>
    /// Сервис данных для отчета по использованным параметрам.
    /// </summary>
    public class RevenueAndExpenditureReportDataAdapter : ReportAdapterBase<RevenueAndExpenditureXtraReport, RevenueAndExpenditureReportParameters>
    {
        /// <summary>
        /// Создает отчет.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <returns>Отчет.</returns>
        public override RevenueAndExpenditureXtraReport CreateReport(SecurityToken token)
        {
            return new RevenueAndExpenditureXtraReport();
        }

        /// <summary>
        /// Создает параметры отчета.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="context">Контекст контроллера.</param>
        /// <returns>Созданный параметры.</returns>
        public override RevenueAndExpenditureReportParameters CreateReportParameters(SecurityToken token, ControllerContext context)
        {
            Guid? groupID = null;

            var groups =
                RemontinkaServer.Instance.EntitiesFacade.GetFinancialGroupItems(token).Select(i => i.FinancialGroupID).ToList();
            //Если в домене всего одна фин группа, выбираем ее.
            if (groups.Count == 1)
            {
                groupID = groups.FirstOrDefault();
            }

            return new RevenueAndExpenditureReportParameters
            {
                BeginDate = Utils.GetFirstDayOfMonth(DateTime.Today),
                EndDate = Utils.GetLastDayOfMonth(DateTime.Today),
                FinancialGroupID = groupID,
                FinancialGroups = RemontinkaServer.Instance.EntitiesFacade.GetFinancialGroupItems(token)
            };
        }

        /// <summary>
        /// Вызывается, когда необходимо обновление источника данных для отчета.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="report">Отчет.</param>
        /// <param name="reportParameters">Параметры отчета.</param>
        public override void UpdateDataSource(SecurityToken token, RevenueAndExpenditureXtraReport report,
            RevenueAndExpenditureReportParameters reportParameters)
        {
            var data = RemontinkaServer.Instance.EntitiesFacade.GetRevenueAndExpenditureReportItems(token,
                                                                                                    reportParameters.
                                                                                                        FinancialGroupID,
                                                                                                    reportParameters.BeginDate,
                                                                                                    reportParameters.EndDate).ToList();

            var finGroup = RemontinkaServer.Instance.EntitiesFacade.GetFinancialGroupItem(token, reportParameters.FinancialGroupID);

            ProcessWorkAndDeviceTotals(token, data, reportParameters, finGroup);
            ProcessWarehouseIncomingDocTotals(token, data, reportParameters, finGroup);

            report.DataSource = data;
            SetPeriodParameters(report,reportParameters.BeginDate,reportParameters.EndDate);
            report.Parameters["GroupLegalName"].Value = finGroup.Title;
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
        private void ProcessWorkAndDeviceTotals(SecurityToken token, List<RevenueAndExpenditureReportItem> data, RevenueAndExpenditureReportParameters input, FinancialGroupItem financialGroup)
        {
            var totals = RemontinkaServer.Instance.EntitiesFacade.GetOrderPaidAmountByOrderIssueDate(token,
                                                                                                             input.
                                                                                                                 FinancialGroupID,
                                                                                                             input.
                                                                                                                 BeginDate,
                                                                                                             input.
                                                                                                                 EndDate);

            if (totals != null)
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
                if (finItem != null)
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
        private void ProcessWarehouseIncomingDocTotals(SecurityToken token, List<RevenueAndExpenditureReportItem> data, RevenueAndExpenditureReportParameters input, FinancialGroupItem financialGroup)
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