using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// Ответ с данными по остаткам на складах.
    /// </summary>
    public class GetWarehouseItemsResponse : MessageBase
    {
        public GetWarehouseItemsResponse()
        {
            Kind = MessageKind.GetWarehouseItemsResponse;
            WarehouseItems = new List<WarehouseItemDTO>();
        }

        /// <summary>
        /// Получает список остатков на складах.
        /// </summary>
        public IList<WarehouseItemDTO> WarehouseItems { get; private set; }
    }
}
