using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;
using Romontinka.Server.WebSite.Metadata;

namespace Romontinka.Server.WebSite.Models.FinancialItemSingleLookupForm
{
    /// <summary>
    /// Модель поиска статей бюджета для лукапа.
    /// </summary>
    public class JLookupFinancialItemSearchModel : JLookupSearchBaseModel
    {
        /// <summary>
        /// Задает или получает строку поиска в гриде.
        /// </summary>
        [DisplayName("Наименование")]
        public string JLookupFinancialItemName { get; set; }
    }
}