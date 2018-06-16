using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.FinancialItemForm
{
    /// <summary>
    /// Пункт грида статей бюджета.
    /// </summary>
    public class FinancialItemGridItemModel : JGridItemModel<Guid>
    {
        /// <summary>
        /// Задает или получает название статьи.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает название типа дохода или расхода.
        /// </summary>
        public string TransactionKindTitle { get; set; }
    }
}