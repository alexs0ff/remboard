using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities.ReportItems
{
    /// <summary>
    /// Пункт отчета по итогам конкретных приходных накладных.
    /// </summary>
    public class WarehouseDocTotalItem
    {
        /// <summary>
        /// Задает или получает дату накладной.
        /// </summary>
        public DateTime DocDate { get; set; }

        /// <summary>
        /// Задает или получает номер накладной.
        /// </summary>
        public string DocNumber { get; set; }

        /// <summary>
        /// Задает или получает сумму всех цен закупки умноженных на количество.
        /// </summary>
        public decimal SumInitPriceTotal { get; set; }
    }
}
