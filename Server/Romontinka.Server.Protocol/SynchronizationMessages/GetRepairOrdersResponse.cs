using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// Ответ на запрос о получении заказов.
    /// </summary>
    public class GetRepairOrdersResponse:MessageBase
    {
        public GetRepairOrdersResponse()
        {
            Kind = MessageKind.GetRepairOrdersResponse;
            RepairOrders = new List<RepairOrderDTO>();
        }

        /// <summary>
        /// Получает список заказов.
        /// </summary>
        public List<RepairOrderDTO> RepairOrders { get;private set; }
    }
}
