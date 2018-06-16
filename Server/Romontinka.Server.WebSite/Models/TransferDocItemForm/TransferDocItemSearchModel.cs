using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.TransferDocItemForm
{
    /// <summary>
    /// Модель поиска в гриде по пунктам документа о перемещениях между складами.
    /// </summary>
    public class TransferDocItemSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает название для поиска в элементах документа.
        /// </summary>
        public string TransferDocItemName { get; set; }

        /// <summary>
        /// Задает или получает код связанного документа.
        /// </summary>
        public Guid? TransferDocItemDocID { get; set; }
    }
}