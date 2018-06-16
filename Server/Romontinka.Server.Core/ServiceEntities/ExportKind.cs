using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Core.ServiceEntities
{
    /// <summary>
    /// Виды экспортируемых данных.
    /// </summary>
    public class ExportKind
    {
        /// <summary>
        /// Задает или получает тип экспорта.
        /// </summary>
        public int KindID { get; set; }

        /// <summary>
        /// Задает или получает название типа экспорта.
        /// </summary>
        public string Title { get; set; }
    }
}
