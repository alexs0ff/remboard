using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.FinancialItemValueForm
{
    /// <summary>
    /// Модель поиска для значений бюджетных статей.
    /// </summary>
    public class FinancialItemValueSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает наименование статьи бюджета.
        /// </summary>
        public string FinancialItemSearchName { get; set; }

        /// <summary>
        /// Задает или получает дату начала периода.
        /// </summary>
        public DateTime FinancialItemValueSearchBeginDate { get; set; }

        /// <summary>
        /// Задает или получает дату окончания периода.
        /// </summary>
        public DateTime FinancialItemValueSearchEndDate { get; set; }

        /// <summary>
        /// Задает или получает код финансовой группы.
        /// </summary>
        public Guid? FinancialItemValueSearchFinancialGroupID { get; set; }
    }
}