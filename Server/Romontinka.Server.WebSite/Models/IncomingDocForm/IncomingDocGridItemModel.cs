using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.IncomingDocForm
{
    /// <summary>
    /// Модель пункта грида приходной накладной.
    /// </summary>
    public class IncomingDocGridItemModel : JGridItemModel<Guid>
    {
        /// <summary>
        /// Задает или получает ФИО создателя.
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// Задает или получает название склада.
        /// </summary>
        public string WarehouseTitle { get; set; }

        /// <summary>
        /// Задает или получает название контрагента.
        /// </summary>
        public string ContractorLegalName { get; set; }

        /// <summary>
        /// Задает или получает дату накладной.
        /// </summary>
        public string DocDate { get; set; }

        /// <summary>
        /// Задает или получает номер накладной.
        /// </summary>
        public string DocNumber { get; set; }

        /// <summary>
        /// Задает или получает описание документа.
        /// </summary>
        public string DocDescription { get; set; }
    }
}