using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.FinancialItemSingleLookupForm
{
    /// <summary>
    /// Модель пункта отображения
    /// </summary>
    public class JLookupFinancialItemModel : JLookupItemBaseModel
    {
        /// <summary>
        /// Задает или получает полное имя статьи.
        /// </summary>
        public string FinancialItemTitle { get; set; }

        /// <summary>
        /// Задает или получает тип операции доход/расход.
        /// </summary>
        public string TransactionKindTitle { get; set; }
    }
}