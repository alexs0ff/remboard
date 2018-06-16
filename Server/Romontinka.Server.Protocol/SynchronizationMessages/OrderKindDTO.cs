using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// DTO объект для типа заказа.
    /// </summary>
    public class OrderKindDTO
    {
        /// <summary>
        /// Задает или получает тип заказа.
        /// </summary>
        public Guid? OrderKindID { get; set; }

        /// <summary>
        /// Задает или получает название.
        /// </summary>
        public string Title { get; set; }
    }
}
