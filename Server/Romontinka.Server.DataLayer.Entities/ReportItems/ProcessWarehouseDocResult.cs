using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities.ReportItems
{
    /// <summary>
    /// Контейнер результатов по обработке складских документов.
    /// </summary>
    public class ProcessWarehouseDocResult
    {
        /// <summary>
        /// Задает или получает описание ошибки.
        /// </summary>
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Задает или получает результат обработки ошибки.
        /// </summary>
        public bool ProcessResult { get; set; }
    }
}
