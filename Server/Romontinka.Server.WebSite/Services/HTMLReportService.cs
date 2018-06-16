using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using log4net;

#if CLIENT
using Remontinka.Client.DataLayer.Entities;
using Remontinka.Client.Core.Context;
using Remontinka.Client.Core.Numbers;
#else
using Romontinka.Server.Core;
using Romontinka.Server.Core.Context;
using Romontinka.Server.Core.Numbers;
using Romontinka.Server.Core.Security;
using Romontinka.Server.DataLayer.Entities;
using Romontinka.Server.WebSite.Helpers;

#endif

#if CLIENT
namespace Remontinka.Client.Core.Services
{
#else
namespace Romontinka.Server.WebSite.Services
{
#endif

    /// <summary>
    /// Реализация генерации html представления для отчетов.
    /// </summary>
    public class HTMLReportService : IHTMLReportService
    {
        /// <summary>
        ///   Текущий логер.
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(HTMLReportService));

        private const string NotFoundMessage = "Not found";

         /// <summary>
        /// Создает html представление отчетов связанных с заказами.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="customReportID">Код необходимого отчета. </param>
        /// <param name="repairOrder">Связанный заказ.</param>
        /// <returns>Сгенерированный результат.</returns>
        public string CreateRepairOrderReport(SecurityToken token, Guid? customReportID, RepairOrderDTO repairOrder)
         {
             _logger.InfoFormat("Начало создания отчета по объекту заказа {0} для заказа {1} пользователем {2}",
                                customReportID,
                                repairOrder.RepairOrderID, token.LoginName);
#if CLIENT
             var customReport = ClientCore.Instance.DataStore.GetCustomReportItem(customReportID);
#else

             var customReport = RemontinkaServer.Instance.EntitiesFacade.GetCustomReportItem(token,customReportID);
#endif


             if (customReport == null || string.IsNullOrWhiteSpace(customReport.HtmlContent))
             {
                 _logger.ErrorFormat("Отчет не найден или не задан {0}", customReportID);
                 return NotFoundMessage;
             } //if

            var contextBuilder = new ContextStringBuilder();

#if CLIENT
             var branch = ClientCore.Instance.DataStore.GetBranch(repairOrder.BranchIDGuid);
#else
            var branch = RemontinkaServer.Instance.EntitiesFacade.GetBranch(token,repairOrder.BranchID);
            var domain = RemontinkaServer.Instance.EntitiesFacade.GetUserDomain(token);
            contextBuilder.Add(new RepairOrderGlobalReferenceContextItem(repairOrder,domain));

#endif

            contextBuilder.Add(new TodayContextItem());
            contextBuilder.Add(new RepairOrderContextItem(repairOrder));
            contextBuilder.Add(new BranchContextItem(branch));

#if CLIENT
#else
            contextBuilder.Add(new BarcodeContextItem(repairOrder));
#endif

             foreach (
                 var key in
                     _contextTableMap.Keys.Where(i => customReport.HtmlContent.Contains(string.Format("{{{0}}}", i))))
             {
                 contextBuilder.Add(new SimpleContextItem(key, _contextTableMap[key](token, repairOrder, branch)));
             } //foreach
#if CLIENT
             var deviceInfo = ClientCore.Instance.DataStore.GetDeviceItemsTotal(repairOrder.RepairOrderIDGuid);
             var workInfo = ClientCore.Instance.DataStore.GetWorkItemsTotal(repairOrder.RepairOrderIDGuid);
#else
             var deviceInfo = RemontinkaServer.Instance.DataStore.GetDeviceItemsTotal(repairOrder.RepairOrderID);
             var workInfo = RemontinkaServer.Instance.DataStore.GetWorkItemsTotal(repairOrder.RepairOrderID);
#endif
             var totalCount = 0.0M;
             var totalPrice = 0.0M;
             if (deviceInfo!=null)
             {
                 totalCount = (decimal?)deviceInfo.Count ?? decimal.Zero;
                 totalPrice = (decimal?)deviceInfo.TotalAmount ?? decimal.Zero;
             } //if

             if (workInfo!=null)
             {
                 totalCount += (decimal?)workInfo.Count ?? 0;
                 totalPrice += (decimal?)workInfo.Amount ?? decimal.Zero;
             } //if

             contextBuilder.Add(new SimpleContextItem(ContextConstants.TotalItemsCount,DecimalToString(totalCount)));
             contextBuilder.Add(new SimpleContextItem(ContextConstants.TotalItemsAmount, DecimalToString(totalPrice)));
             contextBuilder.Add(new SimpleContextItem(ContextConstants.TotalItemsAmountRus,
                                                      RusCurrency.StrRur((double) totalPrice)));

             return contextBuilder.Create(customReport.HtmlContent);
         }

        /// <summary>
        /// Создает html представление отчетов связанных с заказами.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="customReportID">Код необходимого отчета. </param>
        /// <param name="repairOrderID">Код связанного заказа.</param>
        /// <returns>Сгенерированный результат.</returns>
        public string CreateRepairOrderReport(SecurityToken token, Guid? customReportID, Guid? repairOrderID)
        {
            _logger.InfoFormat("Начало создания отчета по коду заказа {0} для заказа {1} пользователем {2}", customReportID,
                                repairOrderID, token.LoginName);
#if CLIENT
            var order = ClientCore.Instance.DataStore.GetRepairOrderDTO(repairOrderID);
#else
            var order = RemontinkaServer.Instance.EntitiesFacade.GetOrder(token, repairOrderID);
#endif

            if (order==null)
            {
                _logger.ErrorFormat("Заказ не найден {0}", repairOrderID);
                return NotFoundMessage;
            } //if

            return CreateRepairOrderReport(token, customReportID, order);
        }

        #region Context Tables 

        /// <summary>
        /// Содержит сылки на функции создания встроенных html таблиц.
        /// </summary>
        private static readonly IDictionary<string, Func<SecurityToken, RepairOrderDTO, Branch, string>>
            _contextTableMap = new Dictionary<string, Func<SecurityToken, RepairOrderDTO, Branch, string>>
                               {
                                   {ContextConstants.DeviceAndWorkItemsList,CreateDeviceAndWorkItemsList},
                                   {ContextConstants.ProductList,CreateProductList},
                               };

        private const string DeviceAndWorkItemsListTableFormat = @"
    <table class='infoTable'>
    <tr><th>Наименование работы (услуги)</th><th>Кол-во</th><th>Цена, руб</th><th>Сумма, руб</th></tr>
    {0}
    <tr><td colspan='3' class='bottomtext'>Итого:</td><td>{1}</td></tr>
    <tr><td colspan='3' class='bottomtext'>Аванс:</td><td>{2}</td></tr>
    <tr><td colspan='3' class='bottomtext'>К оплате:</td><td>{3}</td></tr>
</table>
";

        private const string DeviceAndWorkItemsListItemFormat = @"
    <tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td></tr>
";

        /// <summary>
        /// Создает таблицу список выполненных работ и установленных запчастей.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="repairOrder">Заказ.</param>
        /// <param name="branch">Филиал.</param>
        /// <returns>Созданная таблица.</returns>
        private static string CreateDeviceAndWorkItemsList(SecurityToken token, RepairOrderDTO repairOrder, Branch branch)
        {
            var total = decimal.Zero;
            var prePayment = (decimal?)repairOrder.PrePayment ?? decimal.Zero;
#if CLIENT
            var workItems = ClientCore.Instance.DataStore.GetWorkItems(repairOrder.RepairOrderIDGuid);
            var deviceItems = ClientCore.Instance.DataStore.GetDeviceItems(repairOrder.RepairOrderIDGuid);
#else
            var workItems = RemontinkaServer.Instance.DataStore.GetWorkItems(repairOrder.RepairOrderID);
            var deviceItems = RemontinkaServer.Instance.DataStore.GetDeviceItems(repairOrder.RepairOrderID);
#endif

            var stringBuilder = new StringBuilder();

            foreach (var workItem in workItems)
            {
                total += (decimal)workItem.Price;
                stringBuilder.AppendFormat(DeviceAndWorkItemsListItemFormat, workItem.Title, "-",
                                           DecimalToString(workItem.Price), DecimalToString(workItem.Price));
            } //foreach

            foreach (var deviceItem in deviceItems)
            {
                total += (decimal)deviceItem.Price * (decimal)deviceItem.Count;
                stringBuilder.AppendFormat(DeviceAndWorkItemsListItemFormat, deviceItem.Title, DecimalToString(deviceItem.Count),
                                           DecimalToString(deviceItem.Price), DecimalToString(deviceItem.Price*deviceItem.Count));
            } //foreach

            return string.Format(DeviceAndWorkItemsListTableFormat, stringBuilder, DecimalToString(total),
                                 DecimalToString(prePayment), DecimalToString(total - prePayment));
        }

        private const string ProductListTableFormat = @"
    <table class='infoTable'>
    <tr><th>№</th><th>Наименование товара</th><th>Кол-во</th><th>Цена, руб</th><th>Сумма, руб</th></tr>
    {0}
    <tr><td colspan='4' class='bottomtext'>Итого:</td><td>{1}</td></tr>    
</table>
";

        private const string ProductListItemFormat = @"
    <tr><td>{0}</td><td>{1}</td><td>{2}</td><td>{3}</td><td>{4}</td></tr>
";

        /// <summary>
        /// Создает таблицу для товарного чека .
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="repairOrder">Заказ.</param>
        /// <param name="branch">Филиал.</param>
        /// <returns>Созданная таблица.</returns>
        private static string CreateProductList(SecurityToken token, RepairOrderDTO repairOrder, Branch branch)
        {
            var total = decimal.Zero;
#if CLIENT
            var workItems = ClientCore.Instance.DataStore.GetWorkItems(repairOrder.RepairOrderIDGuid);
            var deviceItems = ClientCore.Instance.DataStore.GetDeviceItems(repairOrder.RepairOrderIDGuid);

#else
            var workItems = RemontinkaServer.Instance.DataStore.GetWorkItems(repairOrder.RepairOrderID);
            var deviceItems = RemontinkaServer.Instance.DataStore.GetDeviceItems(repairOrder.RepairOrderID);
#endif
            var stringBuilder = new StringBuilder();

            int index = 1;

            foreach (var workItem in workItems)
            {
                total += (decimal)workItem.Price;
                stringBuilder.AppendFormat(ProductListItemFormat, IntToString(index), workItem.Title, "-",
                                           DecimalToString(workItem.Price), DecimalToString(workItem.Price));
                index++;
            } //foreach

            foreach (var deviceItem in deviceItems)
            {
                total += (decimal)deviceItem.Price * (decimal)deviceItem.Count;
                stringBuilder.AppendFormat(ProductListItemFormat, IntToString(index), deviceItem.Title, DecimalToString(deviceItem.Count),
                                           DecimalToString(deviceItem.Price), DecimalToString(deviceItem.Price * deviceItem.Count));
                index++;
            } //foreach

            return string.Format(ProductListTableFormat, stringBuilder, DecimalToString(total));
        }

        #endregion Context Tables

        /// <summary>
        /// Переводит число в строковое представление.
        /// </summary>
        /// <param name="value">Число.</param>
        /// <returns>Строковое представление.</returns>
        public static string IntToString(int? value)
        {
            
#if CLIENT
            if (value == null)
            {
                return string.Empty;
            } //if
            return value.Value.ToString(CultureInfo.InvariantCulture);
#else
            return Utils.IntToString(value);
#endif
        }

        /// <summary>
        /// Переводит дату в строковое представление.
        /// </summary>
        /// <param name="dateTime">Дата.</param>
        /// <returns>Строковое представление.</returns>
        private static string DateTimeToString(object dateTime)
        {
#if CLIENT
            if (dateTime is string)
            {
                var dt = FormatUtils.StringToDateTime((string)dateTime);
                dateTime = dt;
            }
            return ((DateTime)dateTime).ToString("dd.MM.yyyy");
#else
            return Utils.DateTimeToString((DateTime) dateTime);
#endif
        }

        /// <summary>
        /// Переводит число в строковое представление.
        /// </summary>
        /// <param name="value">Число.</param>
        /// <returns>Строковое представление.</returns>
        private static string DecimalToString(object value)
        {
#if CLIENT
            if (value is decimal)
            {
                return ((decimal)value).ToString("0.00");
            } //if  
            if (value is double)
            {
                return ((double)value).ToString("0.00");
            } //if

            if (value is int)
            {
                return ((int)value).ToString("0.00");
            } //if

            throw new Exception("Нет типа" + value.GetType());
#else
            return Utils.DecimalToString((decimal) value);
#endif
        }
    }
}