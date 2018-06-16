using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities.ReportItems
{
    /// <summary>
    /// Пункт отчета по исполнителям.
    /// </summary>
    public class EngineerWorkReportItem
    {
        /// <summary>
        /// Задает или получает код пользователя.
        /// </summary>
        public Guid? UserID { get; set; }

        /// <summary>
        /// Получает ФИО.
        /// </summary>
        public string FullName { get { return string.Concat(LastName, " ", FirstName, " ", MiddleName); } }

        /// <summary>
        /// Задает или получает имя пользователя.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Задает или получает Фамилию пользователя.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Задает или получает отчетство пользователя.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Задает или получает номер заказа.
        /// </summary>
        public string OrderNumber { get; set; }

        /// <summary>
        /// Задает или получает наименование работы.
        /// </summary>
        public string WorkTitle { get; set; }

        /// <summary>
        /// Задает или получает дату выполненной работы.
        /// </summary>
        public DateTime WorkEventDate { get; set; }

        /// <summary>
        /// Задает или получает стоимость работ.
        /// </summary>
        public decimal WorkPrice { get; set; }
    }
}
