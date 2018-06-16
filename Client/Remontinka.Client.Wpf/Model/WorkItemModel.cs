using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Remontinka.Client.DataLayer.Entities;

namespace Remontinka.Client.Wpf.Model
{
    /// <summary>
    /// Модель пункта выполненных работ.
    /// </summary>
    public class WorkItemModel
    {
        /// <summary>
        /// Задает или получает код работы.
        /// </summary>
        public Guid? Id { get; set; }

        public WorkItemModel(WorkItemDTO workItem)
        {
            Id = workItem.WorkItemIDGuid;
            WorkItemEngineerFullName = workItem.EngineerFullName;
            WorkItemTitle = workItem.Title;
            WorkItemEventDate = WpfUtils.DateTimeToString(workItem.EventDateDateTime);
            WorkItemPrice = WpfUtils.DecimalToString((decimal?) workItem.Price);
        }

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
