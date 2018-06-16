using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.FinancialGroupForm
{
    /// <summary>
    /// Модель пункта грида по финансовой группе филиалов.
    /// </summary>
    public class FinancialGroupGridItemModel : JGridItemModel<Guid>
    {
        /// <summary>
        /// Задает или получает название финансовой группы.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает юр название фирмы.
        /// </summary>
        public string LegalName { get; set; }
    }
}