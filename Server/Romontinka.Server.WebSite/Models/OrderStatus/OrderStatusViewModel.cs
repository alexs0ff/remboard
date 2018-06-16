using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Models.DataGrid;

namespace Romontinka.Server.WebSite.Models.OrderStatus
{
    /// <summary>
    /// Модель отображения для списка статусов заказа.
    /// </summary>
    public class OrderStatusViewModel
    {
        /// <summary>
        /// Задает или получает описатель грида.
        /// </summary>
        public DataGridDescriptor OrderStatusesGrid { get; set; }
    }
}