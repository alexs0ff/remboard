using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.GoodsItemSingleLookupForm
{
    /// <summary>
    /// Модедь поиска в гриде номенклатуры.
    /// </summary>
    public class JLookupGoodsItemSearchModel : JLookupSearchBaseModel
    {
        /// <summary>
        /// Задает или получает строку поиска в гриде.
        /// </summary>
        [DisplayName("Наименование")]
        public string JLookupGoodsItemName { get; set; }
    }
}