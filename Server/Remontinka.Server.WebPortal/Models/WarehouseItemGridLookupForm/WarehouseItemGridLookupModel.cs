using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Remontinka.Server.WebPortal.Models.Common;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.WarehouseItemGridLookupForm
{
    /// <summary>
    /// Модель для лоокпа грида по остаткам на складе.
    /// </summary>
    public class WarehouseItemGridLookupModel: GridLookupModelBase
    {
        /// <summary>
        /// Задает или получает список всех складов.
        /// </summary>
        public IQueryable<Warehouse> Warehouses { get; set; }
    }
}