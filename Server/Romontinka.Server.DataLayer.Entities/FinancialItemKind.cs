using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Тип статьи дохода или расхода.
    /// </summary>
    public class FinancialItemKind:EntityBase<int>
    {
        /// <summary>
        /// Задает или получает тип статьи бюджета.
        /// </summary>
        public int? FinancialItemKindID { get; set; }

        /// <summary>
        /// Задает или получает название типа статьи.
        /// </summary>
        public string Title { get; set; }
    }
}
