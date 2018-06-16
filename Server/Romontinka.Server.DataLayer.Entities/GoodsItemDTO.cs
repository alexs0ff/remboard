using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// DTO объект для номенклатуры.
    /// </summary>
    public class GoodsItemDTO : GoodsItem
    {
        /// <summary>
        /// Задает или получает название категории.
        /// </summary>
        public string ItemCategoryTitle { get; set; }
    }
}
