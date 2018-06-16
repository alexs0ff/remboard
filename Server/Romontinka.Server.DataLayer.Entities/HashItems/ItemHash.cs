using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities.HashItems
{
    internal class ItemHash
    {
        /// <summary>
        /// Задает или получает код пункта данных.
        /// </summary>
        public Guid? ItemID { get; set; }

        /// <summary>
        /// Задает или получает хэш данных.
        /// </summary>
        public string DataHash { get; set; }

        /// <summary>
        /// Задает или получает тип данных.
        /// </summary>
        public int Kind { get; set; }
    }
}
