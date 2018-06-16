using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.WarehouseItemForm
{
    /// <summary>
    /// Модель поиска для грида по остаткам на складе.
    /// </summary>
    public class WarehouseItemSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает строку поика по имени.
        /// </summary>
        public string WarehouseItemName { get; set; }

        /// <summary>
        /// Задает или получает код склада по которому производится поиск.
        /// </summary>
        public Guid? WarehouseItemWarehouseID { get; set; }
    }
}