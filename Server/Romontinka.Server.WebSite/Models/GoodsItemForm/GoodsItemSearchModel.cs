using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.GoodsItemForm
{
    /// <summary>
    /// Модель поиска в гриде номенклатуры.
    /// </summary>
    public class GoodsItemSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает строку поика по имени номенклатуры.
        /// </summary>
        public string GoodsItemName { get; set; }
    }
}