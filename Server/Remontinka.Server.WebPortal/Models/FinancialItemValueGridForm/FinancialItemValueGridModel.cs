using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.FinancialItemValueGridForm
{
    /// <summary>
    /// Модель грида для текущих расходов и доходов.
    /// </summary>
    public class FinancialItemValueGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "FinancialItemValueID"; } }

        /// <summary>
        /// Задает или получает финансовые статьи.
        /// </summary>
        public IQueryable<FinancialItem> FinancialItems { get; set; }

        /// <summary>
        /// Задает или получает финансовую группу.
        /// </summary>
        public IQueryable<FinancialGroupItem> FinancialGroups { get; set; }

    }
}