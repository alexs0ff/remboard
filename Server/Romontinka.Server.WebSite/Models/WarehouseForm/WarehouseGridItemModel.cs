using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.WarehouseForm
{
    /// <summary>
    /// Модель пункта грида с складами.
    /// </summary>
    public class WarehouseGridItemModel : JGridItemModel<Guid>
    {
        /// <summary>
        /// Задает или получает название склада.
        /// </summary>
        public string Title { get; set; }
    }
}