using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.WarehouseItemForm
{
    /// <summary>
    /// Модель пункта грида с остатками на складе.
    /// </summary>
    public class WarehouseItemGridItemModel : JGridItemModel<Guid>
    {
        /// <summary>
        /// Задает или получает название связанной номенклатуры.
        /// </summary>
        public string GoodsItemTitle { get; set; }

        /// <summary>
        /// Задает или получает количество.
        /// </summary>
        public string Total { get; set; }

        /// <summary>
        /// Задает или получает нулевую цену.
        /// </summary>
        public string StartPrice { get; set; }

        /// <summary>
        /// Задает или получает ремонтную цену.
        /// </summary>
        public string RepairPrice { get; set; }

        /// <summary>
        /// Задает или получает цену продажи.
        /// </summary>
        public string SalePrice { get; set; }
    }
}