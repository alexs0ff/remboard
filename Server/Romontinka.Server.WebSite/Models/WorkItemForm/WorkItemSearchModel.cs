using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.WorkItemForm
{
    /// <summary>
    /// Модель поиска для пунктов выполненных работ.
    /// </summary>
    public class WorkItemSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает строку поика по имени.
        /// </summary>
        public string WorkItemName { get; set; }

        /// <summary>
        /// Задает или получает код связанного заказа.
        /// </summary>
        public Guid? WorkItemRepairOrderID { get; set; }
    }
}