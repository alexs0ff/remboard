using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities.HashItems
{
    /// <summary>
    /// Хэш заказа.
    /// </summary>
    public class RepairOrderHash
    {
        /// <summary>
        /// Задает или получает код отчета.
        /// </summary>
        public Guid? RepairOrderID { get; set; }

        /// <summary>
        /// Задает или получает хэш данных.
        /// </summary>
        public string DataHash { get; set; }

        /// <summary>
        /// Задает или получает количество пунктов графика.
        /// </summary>
        public int OrderTimelinesCount { get; set; }

        /// <summary>
        /// Задает или получает хэши установленных запчастей.
        /// </summary>
        public IList<DeviceItemHash> DeviceItemHashes { get; set; }

        /// <summary>
        /// Задает или получает хэши выполненных работ.
        /// </summary>
        public IList<WorkItemHash> WorkItemHashes { get; set; }
    }
}
