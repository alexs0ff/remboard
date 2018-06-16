using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.TransferDocForm
{
    /// <summary>
    /// Модель поиска в гриде документов перемещения между складами.
    /// </summary>
    public class TransferDocSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает название для поиска в накладных.
        /// </summary>
        public string TransferDocName { get; set; }

        /// <summary>
        /// Задает или получает начало периода поиска.
        /// </summary>
        public DateTime TransferDocBeginDate { get; set; }

        /// <summary>
        /// Задает или получает окончание периода поиска.
        /// </summary>
        public DateTime TransferDocEndDate { get; set; }

        /// <summary>
        /// Задает или получает код отправляющего склада для поиска.
        /// </summary>
        public Guid? TransferDocSenderWarehouseID { get; set; }

        /// <summary>
        /// Задает или получает код получающего склада для поиска.
        /// </summary>
        public Guid? TransferDocRecipientWarehouseID { get; set; }
    }
}