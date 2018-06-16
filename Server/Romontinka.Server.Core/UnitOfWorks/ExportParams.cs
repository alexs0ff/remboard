using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Romontinka.Server.Core.ServiceEntities;

namespace Romontinka.Server.Core.UnitOfWorks
{
    /// <summary>
    /// Параметры экспорта.
    /// </summary>
    public class ExportParams : UnitBase
    {
        /// <summary>
        /// Задает или получает дату начала периода.
        /// </summary>
        public DateTime BeginDate { get; set; }

        /// <summary>
        /// Задает или получает дату окончания периода.
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Задает или получает тип экспорта.
        /// </summary>
        public ExportKind Kind { get; set; }
    }
}
