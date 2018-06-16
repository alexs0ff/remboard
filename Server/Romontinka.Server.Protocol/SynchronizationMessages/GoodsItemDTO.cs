using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.Protocol.SynchronizationMessages
{
    /// <summary>
    /// DTO объект для номенклатуры товара.
    /// </summary>
    public class GoodsItemDTO
    {
        /// <summary>
        /// Задает или получает код номенклатуры.
        /// </summary>
        public Guid? GoodsItemID { get; set; }

        /// <summary>
        /// Задает или получает код измерения.
        /// </summary>
        public byte? DimensionKindID { get; set; }

        /// <summary>
        /// Задает или получает код категории.
        /// </summary>
        public Guid? ItemCategoryID { get; set; }

        /// <summary>
        /// Задает или получает название.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает описание.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Задает или получает код товара.
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// Задает или получает Артикул.
        /// </summary>
        public string Particular { get; set; }

        /// <summary>
        /// Задает или получает штрих код.
        /// </summary>
        public string BarCode { get; set; }
    }
}
