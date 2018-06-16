using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.CancellationDocForm
{
    /// <summary>
    /// Модель строки для грида.
    /// </summary>
    public class CancellationDocGridItemModel : JGridItemModel<Guid>
    {
        /// <summary>
        /// Задает или получает отчетство создателя.
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// Задает или получает название склада.
        /// </summary>
        public string WarehouseTitle { get; set; }

        /// <summary>
        /// Задает или получает номер документа
        /// </summary>
        public string DocNumber { get; set; }

        /// <summary>
        /// Задает или получает дату документа.
        /// </summary>
        public string DocDate { get; set; }

        /// <summary>
        /// Задает или получает описание.
        /// </summary>
        public string DocDescription { get; set; }
    }
}