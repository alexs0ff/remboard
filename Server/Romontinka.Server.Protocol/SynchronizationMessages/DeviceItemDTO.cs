using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// DTO объект для установленной запчасти.
    /// </summary>
    public class DeviceItemDTO
    {
        /// <summary>
        /// Задает или получает код установленной запчасти.
        /// </summary>
        public Guid? DeviceItemID { get; set; }

        /// <summary>
        /// Наименование запчасти.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает количество запчастей.
        /// </summary>
        public decimal Count { get; set; }

        /// <summary>
        /// Задает или получает себестоимость.
        /// </summary>
        public decimal CostPrice { get; set; }

        /// <summary>
        /// Задает или получает окончательную цену.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Задает или получает код связанного заказа.
        /// </summary>
        public Guid? RepairOrderID { get; set; }

        /// <summary>
        /// Дата установки.
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Задает или получает код установившего пользователя.
        /// </summary>
        public Guid? UserID { get; set; }

        /// <summary>
        /// Задает или получает код установленной запчасти со склада.
        /// </summary>
        public Guid? WarehouseItemID { get; set; }
    }
}
