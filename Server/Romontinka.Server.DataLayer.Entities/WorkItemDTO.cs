using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities
{
    /// <summary>
    /// DTO объект для пункта выполненных работ.
    /// </summary>
    public class WorkItemDTO : WorkItem
    {
        /// <summary>
        /// Задает или получает ФИО инженера.
        /// </summary>
        public string EngineerFullName { get; set; }
    }
}
