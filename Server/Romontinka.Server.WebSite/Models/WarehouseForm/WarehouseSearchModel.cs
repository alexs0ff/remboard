using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.WarehouseForm
{
    /// <summary>
    /// Модель поиска для складов.
    /// </summary>
    public class WarehouseSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает строку поика по имени.
        /// </summary>
        public string WarehouseName { get; set; }
    }
}