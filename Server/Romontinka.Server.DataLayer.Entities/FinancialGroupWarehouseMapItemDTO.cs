using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// DTO объект для соотвествия между финансовой группой и складом.
    /// </summary>
    public class FinancialGroupWarehouseMapItemDTO : FinancialGroupWarehouseMapItem
    {
        /// <summary>
        /// Задает или получает название склада.
        /// </summary>
        public string WarehouseTitle { get; set; }

        /// <summary>
        /// Задает или получает название финансовой группы.
        /// </summary>
        public string FinancialGroupTitle { get; set; }
    }
}
