using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// DTO объект для хэша серверных данных по выполненным работам.
    /// </summary>
    public class WorkItemServerHashDTO
    {
        /// <summary>
        /// Задает или получает код выполненной работы.
        /// </summary>
        public Guid? WorkItemID { get; set; }

        /// <summary>
        /// Задает или получает хэш данных.
        /// </summary>
        public string DataHash { get; set; }
    }
}
