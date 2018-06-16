using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// Задает или получает DTO объект элемента приходной накладной.
    /// </summary>
    public class IncomingDocItemDTO : IncomingDocItem
    {
        /// <summary>
        /// Задает или получает название связанной номенклатуры.
        /// </summary>
        public string GoodsItemTitle { get; set; }

        /// <summary>
        /// Задает или получает код измерения.
        /// </summary>
        public byte? DimensionKindID { get; set; }

        /// <summary>
        /// Задает или получает название связанной категории.
        /// </summary>
        public string GoodsItemCategoryTitle { get; set; }

        /// <summary>
        /// Задает или получает название измерения.
        /// </summary>
        public string DimensionKindTitle { get; set; }

    }
}
