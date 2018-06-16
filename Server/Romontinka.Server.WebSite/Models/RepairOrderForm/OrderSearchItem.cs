using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Models.RepairOrderForm
{
    /// <summary>
    /// Пункт фильтров поиска по заказам.
    /// </summary>
    public class OrderSearchItem
    {
        /// <summary>
        /// Задает или получает название пункта поиска.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает ключ пунктов фильтра.
        /// </summary>
        public int? Key { get; set; }
    }
}