using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Models.DataGrid;

namespace Romontinka.Server.WebSite.Models.FinancialItemValueForm
{
    /// <summary>
    /// Модель для отображения значений финансовых статей.
    /// </summary>
    public class FinancialItemValueViewModel
    {
        /// <summary>
        /// Задает или получает описатель грида.
        /// </summary>
        public DataGridDescriptor FinancialItemValuesGrid { get; set; }
    }
}