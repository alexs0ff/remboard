using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Models.RepairOrderForm
{
    /// <summary>
    /// Модель документа для отчета.
    /// </summary>
    public class RepairOrderDocumentModel
    {
        /// <summary>
        /// Задает или получает название документа.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает код отчета.
        /// </summary>
        public Guid? CustomReportID { get; set; }
    }
}