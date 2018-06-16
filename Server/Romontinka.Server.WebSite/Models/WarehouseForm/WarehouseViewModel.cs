using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Models.DataGrid;

namespace Romontinka.Server.WebSite.Models.WarehouseForm
{
    /// <summary>
    /// Модель для отображения складов и сопутствующих сущностей.
    /// </summary>
    public class WarehouseViewModel
    {
        /// <summary>
        /// Задает или получает описатель грида со складами.
        /// </summary>
        public DataGridDescriptor WarehousesGrid { get; set; }

        /// <summary>
        /// Задает или получает описатель грида с категориями товара.
        /// </summary>
        public DataGridDescriptor ItemCategoryGrid { get; set; }

        /// <summary>
        /// Задает или получает описатель грида с контрагентами.
        /// </summary>
        public DataGridDescriptor ContractorGrid { get; set; }

        /// <summary>
        /// Задает или получает грид номенклатуры.
        /// </summary>
        public DataGridDescriptor GoodsItemGrid { get; set; }
    }
}