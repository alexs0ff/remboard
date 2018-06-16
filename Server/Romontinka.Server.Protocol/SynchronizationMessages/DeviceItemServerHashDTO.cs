using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// DTO объект для хэша серверных данных по установленным запчастям.
    /// </summary>
    public class DeviceItemServerHashDTO
    {
        /// <summary>
        /// Задает или получает код установленной запчасти.
        /// </summary>
        public Guid? DeviceItemID { get; set; }

        /// <summary>
        /// Задает или получает хэш данных.
        /// </summary>
        public string DataHash { get; set; }
    }
}
