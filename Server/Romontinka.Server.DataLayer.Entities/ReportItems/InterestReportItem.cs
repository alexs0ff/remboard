using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Romontinka.Server.DataLayer.Entities.ReportItems
{
    /// <summary>
    /// Пункт отчета по вознаграждениям пользователей.
    /// </summary>
    public class InterestReportItem
    {
        /// <summary>
        /// Получает ФИО пользователя.
        /// </summary>
        public string FullName
        {
            get { return string.Concat(LastName, " ", FirstName, " ", MiddleName); }
        }

        /// <summary>
        /// Задает или получает имя пользователя.
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Задает и ли получает фамилию пользователя.
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        /// Задает или получает отчество пользователя.
        /// </summary>
        public string MiddleName { get; set; }

        /// <summary>
        /// Задает или получает значение вознаграждения по запчастям.
        /// </summary>
        public decimal DeviceInterest { get; set; }

        /// <summary>
        /// Задает или получает значение вознаграждения по работе.
        /// </summary>
        public decimal WorkInterest { get; set; }
    }
}
