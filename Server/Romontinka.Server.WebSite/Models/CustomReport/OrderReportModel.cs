using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Models.CustomReport
{
    /// <summary>
    /// Модель для отчета по заказу.
    /// </summary>
    public class OrderReportModel
    {

        /// <summary>
        /// Задает или получает данные в HTML формате отчета.
        /// </summary>
        public string Report { get; set; }

        /// <summary>
        /// Задает или получает описание ошибки.
        /// </summary>
        public string Error { get; set; }
    }
}