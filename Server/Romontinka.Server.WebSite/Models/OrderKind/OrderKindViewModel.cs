using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Models.DataGrid;

namespace Romontinka.Server.WebSite.Models.OrderKind
{
    /// <summary>
    /// Представляет модель типов заказов.
    /// </summary>
    public class OrderKindViewModel
    {
        /// <summary>
        /// Задает или получает описатель грида.
        /// </summary>
        public DataGridDescriptor OrderKindGrid { get; set; }
    }
}