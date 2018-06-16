using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// DTO Объект для пользовательских отчетов.
    /// </summary>
    public class CustomReportItemDTO
    {
        /// <summary>
        /// Задает или получает код отчета.
        /// </summary>
        public Guid? CustomReportID { get; set; }

        /// <summary>
        /// Задает или получает название документа.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает тип настраеваемого документа.
        /// </summary>
        public byte? DocumentKindID { get; set; }

        /// <summary>
        /// Задает или получает контент документа.
        /// </summary>
        public string HtmlContent { get; set; }

    }
}
