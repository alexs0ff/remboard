using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.FinancialItemForm
{
    /// <summary>
    /// Модель поиска для статей бюджета.
    /// </summary>
    public class FinancialItemSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает строку поика по имени статьи бюджета.
        /// </summary>
        public string FinancialItemName { get; set; }
    }
}