using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Romontinka.Server.WebSite.Models.RepairOrderForm
{
    /// <summary>
    /// Модель для пункта истории заказа.
    /// </summary>
    public class RepairOrderTimelineItemModel
    {
        /// <summary>
        /// Задает или получает название класса типа пункта истории.
        /// </summary>
        public string TimelineKindClass { get; set; }

        /// <summary>
        /// Задает или получает дату изменения.
        /// </summary>
        public string EventDate { get; set; }

        /// <summary>
        /// Задает или получает описание события.
        /// </summary>
        public string Title { get; set; }
    }
}