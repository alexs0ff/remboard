using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities.ReportItems
{
    /// <summary>
    /// Пункт отчета по доходам и расходам за определенный период.
    /// </summary>
    public class RevenueAndExpenditureReportItem
    {
        /// <summary>
        /// Задает или получает название статьи.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает дату внесения записи.
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Задает или получает доходы по статье.
        /// </summary>
        public decimal RevenueAmount { get; set; }

        /// <summary>
        /// Задает или получает расходы по статье.
        /// </summary>
        public decimal ExpenditureAmount { get; set; }

        /// <summary>
        /// Задает или получает юр название фингруппы.
        /// </summary>
        public string FinancialGroupLegalName { get; set; }

        /// <summary>
        /// Задает или получает название фингруппы.
        /// </summary>
        public string FinancialGroupTitle { get; set; }
    }
}
