using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.IncomingDocItemForm
{
    /// <summary>
    /// Модель пункта грида элемента прикладной накладной.
    /// </summary>
    public class IncomingDocItemGridItemModel : JGridItemModel<Guid>
    {
        /// <summary>
        /// Задает или получает количество элементов.
        /// </summary>
        public string Total { get; set; }

        /// <summary>
        /// Задает или получает цену закупки.
        /// </summary>
        public string InitPrice { get; set; }

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