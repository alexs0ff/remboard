using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.CancellationDocForm
{
    /// <summary>
    /// Модель поиска документов списания.
    /// </summary>
    public class CancellationDocSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает название для поиска в накладных.
        /// </summary>
        public string CancellationDocName { get; set; }

        /// <summary>
        /// Задает или получает начало периода поиска.
        /// </summary>
        public DateTime CancellationDocBeginDate { get; set; }

        /// <summary>
        /// Задает или получает окончание периода поиска.
        /// </summary>
        public DateTime CancellationDocEndDate { get; set; }

        /// <summary>
        /// Задает или получает код склада для поиска.
        /// </summary>
        public Guid? CancellationDocWarehouseID { get; set; }
    }
}