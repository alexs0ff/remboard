using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.WorkItemForm
{
    /// <summary>
    /// Представляет модель пункта для грида.
    /// </summary>
    public class WorkItemGridItemModel : JGridItemModel<Guid>
    {
        /// <summary>
        /// Задает или получает ФИО инженера.
        /// </summary>
        public string WorkItemEngineerFullName { get; set; }

        /// <summary>
        /// Задает или получает описание работы.
        /// </summary>
        public string WorkItemTitle { get; set; }

        /// <summary>
        /// Задает или получает дату выполнения работы.
        /// </summary>
        public string WorkItemEventDate { get; set; }

        /// <summary>
        /// Задает или получает значение цены работы.
        /// </summary>
        public string WorkItemPrice { get; set; }
    }
}