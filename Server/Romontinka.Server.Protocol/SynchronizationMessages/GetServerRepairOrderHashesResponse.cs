using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// Ответ на запрос о получении хэшей серверных заказов.
    /// </summary>
    public class GetServerRepairOrderHashesResponse:MessageBase
    {
        public GetServerRepairOrderHashesResponse()
        {
            Kind = MessageKind.GetServerRepairOrderHashesResponse;
            RepairOrderServerHashes = new List<RepairOrderServerHashDTO>();
        }

        /// <summary>
        /// Получает список серверных хэшей заказов.
        /// </summary>
        public IList<RepairOrderServerHashDTO> RepairOrderServerHashes { get; private set; }

        /// <summary>
        /// Задает или получает общее количество.
        /// </summary>
        public int TotalCount { get; set; }
    }
}
