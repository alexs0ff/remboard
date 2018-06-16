using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.DataLayer.Entities;

namespace Remontinka.Client.Core
{
    /// <summary>
    /// Интерфейс создания html отчетов.
    /// </summary>
    public interface IHTMLReportService
    {
        /// <summary>
        /// Создает html представление отчетов связанных с заказами.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="customReportID">Код необходимого отчета. </param>
        /// <param name="repairOrderID">Код связанного заказа.</param>
        /// <returns>Сгенерированный результат.</returns>
        string CreateRepairOrderReport(SecurityToken token, Guid? customReportID, Guid? repairOrderID);

        /// <summary>
        /// Создает html представление отчетов связанных с заказами.
        /// </summary>
        /// <param name="token">Токен безопасности.</param>
        /// <param name="customReportID">Код необходимого отчета. </param>
        /// <param name="repairOrder">Связанный заказ.</param>
        /// <returns>Сгенерированный результат.</returns>
        string CreateRepairOrderReport(SecurityToken token, Guid? customReportID, RepairOrderDTO repairOrder);
    }
}
