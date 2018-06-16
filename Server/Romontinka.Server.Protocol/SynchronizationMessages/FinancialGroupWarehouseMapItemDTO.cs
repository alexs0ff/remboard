using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// DTO объект для пункта соответвия финансовой группы и склада.
    /// </summary>
    public class FinancialGroupWarehouseMapItemDTO
    {
        /// <summary>
        /// Код пункта соответствия финансовой группы и склада.
        /// </summary>
        public Guid? FinancialGroupWarehouseMapID { get; set; }

        /// <summary>
        /// Задает или получает код связанного склада.
        /// </summary>
        public Guid? WarehouseID { get; set; }

        /// <summary>
        /// Задает или получает код связанной группы.
        /// </summary>
        public Guid? FinancialGroupID { get; set; }
    }
}
