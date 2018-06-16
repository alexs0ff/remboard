using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// Ответ на запрос о статусах заказов.
    /// </summary>
    public class GetOrderStatusesResponse:MessageBase
    {
        public GetOrderStatusesResponse()
        {
            Kind = MessageKind.GetOrderStatusesResponse;
            OrderKinds= new List<OrderKindDTO>();
            OrderStatuses = new List<OrderStatusDTO>();
        }

        /// <summary>
        /// Получает список типов заказов.
        /// </summary>
        public IList<OrderKindDTO> OrderKinds { get; private set; }

        /// <summary>
        /// Получает список статусов заказов.
        /// </summary>
        public IList<OrderStatusDTO> OrderStatuses { get; private set; }
    }
}
