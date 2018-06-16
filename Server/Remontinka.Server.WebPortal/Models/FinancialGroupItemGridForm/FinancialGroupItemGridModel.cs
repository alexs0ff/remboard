using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.FinancialGroupItemGridForm
{
    /// <summary>
    /// Модель грида для финансовых групп.
    /// </summary>
    public class FinancialGroupItemGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "FinancialGroupID"; } }

        /// <summary>
        /// Задает или получает филиалы.
        /// </summary>
        public IQueryable<Branch> Branches { get; set; }

        /// <summary>
        /// Задает или получает склады.
        /// </summary>
        public IQueryable<Warehouse> Warehouses { get; set; }
    }
}