using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    ///  DTO объект для хэша серверных данных по заказу.
    /// </summary>
    public class RepairOrderServerHashDTO
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object"/> class.
        /// </summary>
        public RepairOrderServerHashDTO()
        {
            WorkItems = new List<WorkItemServerHashDTO>();
            DeviceItems = new List<DeviceItemServerHashDTO>();
        }

        /// <summary>
        /// Получает список хэшей для проделанной работы.
        /// </summary>
        public IList<WorkItemServerHashDTO> WorkItems { get; private set; }

        /// <summary>
        /// Получает список хэшей для запчастей.
        /// </summary>
        public IList<DeviceItemServerHashDTO> DeviceItems { get; private set; }

        /// <summary>
        /// Задает или получает код заказа.
        /// </summary>
        public Guid? RepairOrderID { get; set; }

        /// <summary>
        /// Задает или получает хэш данных.
        /// </summary>
        public string DataHash { get; set; }

        /// <summary>
        /// Задает или получает количество пунктов графика.
        /// </summary>
        public long OrderTimelinesCount { get; set; }
    }
}
