using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.WarehouseItemSingleLookupForm
{
    /// <summary>
    /// Модель пункта отображения остатков на складе.
    /// </summary>
    public class JLookupWarehouseItemModel : JLookupItemBaseModel
    {
        /// <summary>
        /// Задает или получает название номенклатуры на складе.
        /// </summary>
        public string GoodsItemTitle { get; set; }

        /// <summary>
        /// Задает или получает количество элементов.
        /// </summary>
        public string ItemCount { get; set; }

        /// <summary>
        /// Задает или получает название измерения.
        /// </summary>
        public string DimensionKindTitle { get; set; }

        /// <summary>
        /// Задает или получает закупочную цену.
        /// </summary>
        public string ItemPrice { get; set; }
    }
}