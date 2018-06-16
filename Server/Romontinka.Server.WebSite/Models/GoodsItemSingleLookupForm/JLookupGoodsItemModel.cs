using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.GoodsItemSingleLookupForm
{
    /// <summary>
    /// Модель для пункта отображения в лукапе номенклатуры.
    /// </summary>
    public class JLookupGoodsItemModel : JLookupItemBaseModel
    {
        /// <summary>
        /// Задает или получает код товара.
        /// </summary>
        public string UserCode { get; set; }

        /// <summary>
        /// Задает или получает Артикул.
        /// </summary>
        public string Particular { get; set; }

        /// <summary>
        /// Задает или получает название.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает название категории.
        /// </summary>
        public string ItemCategoryTitle { get; set; }
    }
}