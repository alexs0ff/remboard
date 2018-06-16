using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.TransferDocItemForm
{
    /// <summary>
    /// Модель пункта документов грида.
    /// </summary>
    public class TransferDocItemGridItemModel : JGridItemModel<Guid>
    {
        /// <summary>
        /// Задает или получает количество элементов.
        /// </summary>
        public string Total { get; set; }

        /// <summary>
        /// Задает или получает название связанной номенклатуры.
        /// </summary>
        public string GoodsItemTitle { get; set; }

        /// <summary>
        /// Задает или получает описание.
        /// </summary>
        public string Description { get; set; }
    }
}