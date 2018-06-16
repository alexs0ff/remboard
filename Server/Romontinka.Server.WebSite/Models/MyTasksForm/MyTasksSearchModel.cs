using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.MyTasksForm
{
    /// <summary>
    /// Модель поиска для заказов.
    /// </summary>
    public class MyTasksSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает строку поика по имени.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Задает или получает код заказа с которого нужно сделать копию.
        /// </summary>
        public Guid? CopyFromRepairOrderID { get; set; }
    }
}