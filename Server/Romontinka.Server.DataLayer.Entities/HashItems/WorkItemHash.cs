using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities.HashItems
{
    public class WorkItemHash
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
