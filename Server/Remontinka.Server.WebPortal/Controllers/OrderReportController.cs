using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using log4net;
using Remontinka.Server.WebPortal.Metadata;
using Remontinka.Server.WebPortal.Models.CustomReport;
using Romontinka.Server.Core;

namespace Remontinka.Server.WebPortal.Controllers
{
    /// <summary>
    /// Контроллер для генерации отчетов по заказам.
    /// </summary>
    [ExtendedAuthorize]
    public class OrderReportController : BaseController
    {
        /// <summary>
        /// Содержит имя контроллера.
        /// </summary>
        public const string ControllerName = "OrderReport";

        /// <summary>
        ///   Текущий логер.
        /// </summary>
        private static readonly ILog _logger = LogManager.GetLogger(typeof(OrderReportController));

        /// <summary>
        /// Создает представление отчета из определенных параметров.
        /// </summary>
        /// <param name="report">Код отчета.</param>
        /// <param name="order">Код заказа.</param>
        /// <returns>Результат.</returns>
        public ActionResult Index(string report, string order)
        {
            _logger.InfoFormat("Страт генерации отчета {0} для заказа {1}", report, order);
            var model = new OrderReportModel();
            try
            {
                var token = GetToken();

                Guid reportID;
                if (Guid.TryParse(report, out reportID))
                {
                    Guid orderId;
                    if (Guid.TryParse(order, out orderId))
                    {
                        model.Report = RemontinkaServer.Instance.EntitiesFacade.CreateRepairOrderReport(token,
                                                                                                        reportID,
                                                                                                        orderId);
                        var reportTitle = string.Empty;
                        var orderNumber = RemontinkaServer.Instance.DataStore.GetRepairOrderNumber(orderId);

                        var reportObj = RemontinkaServer.Instance.EntitiesFacade.GetCustomReportItem(token, reportID);

                        if (reportObj != null)
                        {
                            reportTitle = reportObj.Title;
                        } //if

                        ViewBag.Title = string.Format("{0}, номер заказа \"{1}\"", reportTitle, orderNumber);
                    } //if
                    else
                    {
                        model.Error = "Неверный формат кода заказа";
                    } //else
                } //if
                else
                {
                    model.Error = "Неверный формат кода отчета";
                } //else
            }
            catch (Exception ex)
            {

                var innerException = string.Empty;
                if (ex.InnerException != null)
                {
                    innerException = ex.InnerException.Message;
                } //if

                _logger.ErrorFormat("Во время генерации отчета {0} для заказа {1} произошла ошибка {2} {3} {4} {5}",
                                    report, order, ex.Message, ex.GetType(), innerException, ex.StackTrace);
            } //try

            return View(model);
        }
    }
}