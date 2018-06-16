using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.OrderStatus
{
    /// <summary>
    /// Модель строки для грида
    /// </summary>
    public class OrderStatusGridItemModel : JGridItemModel<Guid>
    {
        /// <summary>
        /// Задает или получает название.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает название типа статуса.
        /// </summary>
        public string KindTitle { get; set; }
    }
}