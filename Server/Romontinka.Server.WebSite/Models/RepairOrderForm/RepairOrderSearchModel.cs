using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.RepairOrderForm
{
    /// <summary>
    /// Модель поиска для заказов.
    /// </summary>
    public class RepairOrderSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает строку поика по имени.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Заадет или получает код поиска по пользователю.
        /// </summary>
        public Guid? UserID { get; set; }

        /// <summary>
        /// Задает или получает код примененного фильтра.
        /// </summary>
        public int? FilterID { get; set; }

        /// <summary>
        /// Задает или получает статус заказа.
        /// </summary>
        public Guid? OrderStatusID { get; set; }

        /// <summary>
        /// Задает или получает код заказа с которого нужно сделать копию.
        /// </summary>
        public Guid? CopyFromRepairOrderID { get; set; }
    }
}