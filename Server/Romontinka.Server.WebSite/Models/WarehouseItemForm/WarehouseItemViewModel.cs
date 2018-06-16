using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Models.DataGrid;

namespace Romontinka.Server.WebSite.Models.WarehouseItemForm
{
    /// <summary>
    /// Модель для отображения складских остатков
    /// </summary>
    public class WarehouseItemViewModel
    {
        /// <summary>
        /// Задает или получает описатель грида с остатками на складах.
        /// </summary>
        public DataGridDescriptor WarehouseItemsGrid { get; set; }
    }
}