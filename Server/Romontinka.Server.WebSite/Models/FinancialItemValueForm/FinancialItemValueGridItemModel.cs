using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.FinancialItemValueForm
{
    /// <summary>
    /// Модель пункта для грида значений финансовых статей бюджета.
    /// </summary>
    public class FinancialItemValueGridItemModel : JGridItemModel<Guid>
    {
        /// <summary>
        /// Задает или получает название статьи бюджета.
        /// </summary>
        public string FinancialItemTitle { get; set; }

        /// <summary>
        /// Задает или получает дату добавления значения.
        /// </summary>
        public string EventDate { get; set; }

        /// <summary>
        /// Задает или получает само значение.
        /// </summary>
        public string Amount { get; set; }

        /// <summary>
        /// Задает или получает себестоимость.
        /// </summary>
        public string CostAmount { get; set; }

        /// <summary>
        /// Задает или получает описание.
        /// </summary>
        public string Description { get; set; }
    }
}