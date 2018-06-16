using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// DTO объект для остатка на складе.
    /// </summary>
    public class WarehouseItemDTO
    {
        /// <summary>
        /// Задает или получает код элемента остатка на складе.
        /// </summary>
        public Guid? WarehouseItemID { get; set; }

        /// <summary>
        /// Задает или получает код склада.
        /// </summary>
        public Guid? WarehouseID { get; set; }

        /// <summary>
        /// Задает или получает код номенклатуры.
        /// </summary>
        public Guid? GoodsItemID { get; set; }

        /// <summary>
        /// Задает или получает количество.
        /// </summary>
        public decimal Total { get; set; }

        /// <summary>
        /// Задает или получает нулевую цену.
        /// </summary>
        public decimal StartPrice { get; set; }

        /// <summary>
        /// Задает или получает ремонтную цену.
        /// </summary>
        public decimal RepairPrice { get; set; }

        /// <summary>
        /// Задает или получает цену продажи.
        /// </summary>
        public decimal SalePrice { get; set; }
    }
}
