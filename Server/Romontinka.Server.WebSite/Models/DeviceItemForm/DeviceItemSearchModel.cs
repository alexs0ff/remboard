using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.DeviceItemForm
{
    /// <summary>
    /// Модель поиска для списка запчастей.
    /// </summary>
    public class DeviceItemSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает строку поика по имени.
        /// </summary>
        public string DeviceItemName { get; set; }

        /// <summary>
        /// Задает или получает код связанного заказа.
        /// </summary>
        public Guid? DeviceItemRepairOrderID { get; set; }
    }
}