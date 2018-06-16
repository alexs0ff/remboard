using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.OrderStatus
{
    /// <summary>
    /// Модель для поиска статусов заказа.
    /// </summary>
    public class OrderStatusSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает строку поика по имени.
        /// </summary>
        public string Name { get; set; }
    }
}