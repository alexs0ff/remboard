using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.FinancialGroupForm
{
    /// <summary>
    /// Модель поиска для финансовых групп филиалов.
    /// </summary>
    public class FinancialGroupSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает строку поика по имени фингруппы.
        /// </summary>
        public string FinancialGroupName { get; set; }
    }
}