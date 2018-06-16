using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// Ответ на запрос о получении информации по складам и их связям с фингруппами.
    /// </summary>
    public class GetWarehousesResponse : MessageBase
    {
        public GetWarehousesResponse()
        {
            Kind = MessageKind.GetWarehousesResponse;

            Warehouses= new List<WarehouseDTO>();
            MapItems = new List<FinancialGroupWarehouseMapItemDTO>();
        }

        /// <summary>
        /// Получает список существующих складов.
        /// </summary>
        public IList<WarehouseDTO> Warehouses { get; private set; }

        /// <summary>
        /// Получает список связей между складами и фингруппами.
        /// </summary>
        public IList<FinancialGroupWarehouseMapItemDTO> MapItems { get; private set; }
    }
}
