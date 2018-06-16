using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// DTO объект для элемента документа списания.
    /// </summary>
    public class CancellationDocItemDTO : CancellationDocItem
    {
        /// <summary>
        /// Задает или получает название связанной номенклатуры.
        /// </summary>
        public string GoodsItemTitle { get; set; }

        /// <summary>
        /// Задает или получает код измерения.
        /// </summary>
        public byte? DimensionKindID { get; set; }
    }
}
