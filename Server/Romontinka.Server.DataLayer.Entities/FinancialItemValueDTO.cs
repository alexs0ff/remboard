using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// DTO объект для значения статей доходов и расходов.
    /// </summary>
    public class FinancialItemValueDTO : FinancialItemValue
    {
        /// <summary>
        /// Задает или получает название статьи дохода или расхода.
        /// </summary>
        public string FinancialItemTitle { get; set; }

        /// <summary>
        /// Задает или получает название финансовой группы.
        /// </summary>
        public string FinancialGroupTitle { get; set; }

        /// <summary>
        /// Задает или получает тип статьи (доход/расход).
        /// </summary>
        public byte? TransactionKindID { get; set; }

        /// <summary>
        /// Задает или получает тип статьи.
        /// </summary>
        public int? FinancialItemKindID { get; set; }
    }
}
