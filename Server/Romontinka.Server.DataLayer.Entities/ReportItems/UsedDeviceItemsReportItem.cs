using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities.ReportItems
{
    /// <summary>
    /// Пункт отчета по использованным запчастям системы.
    /// </summary>
    public class UsedDeviceItemsReportItem
    {
        /// <summary>
        /// Задает или получает дату когда был установлена запчасть.
        /// </summary>
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Задает или получает имя пользователя кто установил запчасть.
        /// </summary>
        public string UserFirstName { get; set; }

        /// <summary>
        /// Задает или получает фамилию пользователя кто установил запчасть.
        /// </summary>
        public string UserLastName { get; set; }

        /// <summary>
        /// Задает или получает отчетство пользователя кто установил запчасть.
        /// </summary>
        public string UserMiddleName { get; set; }

        /// <summary>
        /// Задает или получает название запчасти.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает количество запчастей.
        /// </summary>
        public decimal Count { get; set; }

        /// <summary>
        /// Задает или получает сумму запчасти.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Задает или получает номер заказа.
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// Задает или получает юр название филиала.
        /// </summary>
        public string BranchLegalName { get; set; }

        /// <summary>
        /// Задает или получает название филиала.
        /// </summary>
        public string BranchTitle { get; set; }
    }
}
