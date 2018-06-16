using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.DataLayer.Entities;

namespace Remontinka.Server.WebPortal.Models.WarehouseItemGridForm
{
    /// <summary>
    /// Модель грида складских остатков.
    /// </summary>
    public class WarehouseItemGridModel : GridModelBase
    {
        /// <summary>
        /// Получает наименование ключевого поля.
        /// </summary>
        public override string KeyFieldName { get { return "WarehouseItemID"; } }

        /// <summary>
        /// Задает или получает склады.
        /// </summary>
        public IQueryable<Warehouse> Warehouses { get; set; }
    }
}