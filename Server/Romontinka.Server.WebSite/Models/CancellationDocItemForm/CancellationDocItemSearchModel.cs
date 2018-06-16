using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.CancellationDocItemForm
{
    /// <summary>
    /// Модель для поиска в гриде с пунктами документов о списании.
    /// </summary>
    public class CancellationDocItemSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает название для поиска в элементах документа.
        /// </summary>
        public string CancellationDocItemName { get; set; }

        /// <summary>
        /// Задает или получает код связанного документа.
        /// </summary>
        public Guid? CancellationDocItemDocID { get; set; }
    }
}