using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// DTO объект для выполненных работ заказов.
    /// </summary>
    public class WorkItemDTO
    {
        /// <summary>
        /// Задает или получает пункт выполненной работы.
        /// </summary>
        public Guid? WorkItemID { get; set; }

        /// <summary>
        /// Задает или получает код инженера.
        /// </summary>
        public Guid? UserID { get; set; }

        /// <summary>
        /// Задает или получает наименование работы.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает дату выполненной работы.
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Задает или получает стоимость работ.
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// Задает или получает код связанного заказа.
        /// </summary>
        public Guid? RepairOrderID { get; set; }
    }
}
