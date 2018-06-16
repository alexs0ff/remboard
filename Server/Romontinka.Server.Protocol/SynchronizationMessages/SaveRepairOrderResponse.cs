using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// Ответ на запрос о сохранении заказа.
    /// </summary>
    public class SaveRepairOrderResponse:MessageBase
    {
        public SaveRepairOrderResponse()
        {
            Kind = MessageKind.SaveRepairOrderResponse;
        }

        /// <summary>
        /// Задает или получает признак успешного сохранения.
        /// </summary>
        public bool Success { get; set; }
    }
}
