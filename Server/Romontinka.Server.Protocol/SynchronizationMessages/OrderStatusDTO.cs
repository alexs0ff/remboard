using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// DTO объект для статусов заказа.
    /// </summary>
    public class OrderStatusDTO
    {
        /// <summary>
        /// Задает или получает код заказа.
        /// </summary>
        public Guid? OrderStatusID { get; set; }

        /// <summary>
        /// Задает или получает название.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает код типа статуса.
        /// </summary>
        public byte? StatusKindID { get; set; }

    }
}
