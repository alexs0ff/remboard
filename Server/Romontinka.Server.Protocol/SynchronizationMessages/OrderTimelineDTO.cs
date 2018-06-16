using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// DTO объект для пункта графика заказа.
    /// </summary>
    public class OrderTimelineDTO
    {
        /// <summary>
        /// Задает или получает код графика.
        /// </summary>
        public Guid? OrderTimelineID { get; set; }

        /// <summary>
        /// Задает или получает тип графика.
        /// </summary>
        public byte? TimelineKindID { get; set; }

        /// <summary>
        /// Задает или получает код связанного запроса.
        /// </summary>
        public Guid? RepairOrderID { get; set; }

        /// <summary>
        /// Задает или получает дату и время события.
        /// </summary>
        public DateTime EventDateTime { get; set; }

        /// <summary>
        /// Задает или получает описание события.
        /// </summary>
        public string Title { get; set; }
    }
}
