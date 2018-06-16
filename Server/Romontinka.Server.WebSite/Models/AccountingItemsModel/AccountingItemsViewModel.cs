using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Models.DataGrid;

namespace Romontinka.Server.WebSite.Models.AccountingItemsModel
{
    /// <summary>
    /// Модель управления бухгалтерскими сущностями.
    /// </summary>
    public class AccountingItemsViewModel
    {
        /// <summary>
        /// Задает или получает описатель грида с финансовыми группами филиалов.
        /// </summary>
        public DataGridDescriptor FinancialGroupGrid { get; set; }

        /// <summary>
        /// Задает или получает описатель грида со статьями бюджета.
        /// </summary>
        public DataGridDescriptor FinancialItemsGrid { get; set; }
    }
}