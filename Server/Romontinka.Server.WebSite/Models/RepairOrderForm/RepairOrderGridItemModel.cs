using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.RepairOrderForm
{
    /// <summary>
    /// Модель для заказа.
    /// </summary>
    public class RepairOrderGridItemModel : JGridItemModel<Guid>
    {
        /// <summary>
        /// Задает или получает номер заказа.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Задает или получает статус заказа.
        /// </summary>
        public string StatusTitle { get; set; }

        /// <summary>
        /// Задает или получает назначеного инженера.
        /// </summary>
        public string EngineerFullName { get; set; }

        /// <summary>
        /// Задает или получает назначеного менеджера.
        /// </summary>
        public string ManagerFullName { get; set; }

        /// <summary>
        /// Задает или получает дату заказа.
        /// </summary>
        public string EventDate { get; set; }

        /// <summary>
        /// Задает или получает дату готовности.
        /// </summary>
        public string EventDateOfBeReady { get; set; }

        /// <summary>
        /// Задает или получает ФИО клиента.
        /// </summary>
        public string ClientFullName { get; set; }

        /// <summary>
        /// Задает или получает название девайса.
        /// </summary>
        public string DeviceTitle { get; set; }

        /// <summary>
        /// Задает или получает список общих сумм.
        /// </summary>
        public string Totals { get; set; }

        /// <summary>
        /// Задает или получает неисправности.
        /// </summary>
        public string Defect { get; set; }
       
    }
}