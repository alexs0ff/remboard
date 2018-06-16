using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.RepairOrderGridForm
{
    /// <summary>
    /// Модель детализации заказов.
    /// </summary>
    public class RepairOrderGridDetailModel
    {
        /// <summary>
        /// Задает или получает код связанного заказа.
        /// </summary>
        public Guid? RepairOrderId { get; set; }

        /// <summary>
        /// Задает или получает код клиента.
        /// </summary>
        public string Number { get; set; }

        /// <summary>
        /// Задает или получает полное наименование клиента.
        /// </summary>
        public string ClientFullName { get; set; }

        public IEnumerable<RepairOrderDocumentModel> Documents { get; set; }
    }
}