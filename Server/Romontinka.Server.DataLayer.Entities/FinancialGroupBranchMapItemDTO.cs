using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// DTO Объект для соответствия финансовой группы и филиалов.
    /// </summary>
    public class FinancialGroupBranchMapItemDTO : FinancialGroupBranchMapItem
    {
        /// <summary>
        /// Задает или получает название филиалов.
        /// </summary>
        public string BranchTitle { get; set; }

        /// <summary>
        /// Задает или получает название финансовой группы.
        /// </summary>
        public string FinancialGroupTitle { get; set; }

    }
}
