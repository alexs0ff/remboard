using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities.HashItems
{
    public class DeviceItemHash
    {
        /// <summary>
        /// Задает или получает код выполненной работы.
        /// </summary>
        public Guid? DeviceItemID { get; set; }

        /// <summary>
        /// Задает или получает хэш данных.
        /// </summary>
        public string DataHash { get; set; }
    }
}
