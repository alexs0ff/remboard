using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.IncomingDocItemForm
{
    /// <summary>
    /// Модель поиска для грида с элементами прикладной накладной.
    /// </summary>
    public class IncomingDocItemSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает название для поиска в элементах накладных.
        /// </summary>
        public string IncomingDocItemName { get; set; }

        /// <summary>
        /// Задает или получает код связанного документа.
        /// </summary>
        public Guid? IncomingDocItemDocID { get; set; }
    }
}