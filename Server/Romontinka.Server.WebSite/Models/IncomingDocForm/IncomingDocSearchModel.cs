using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.IncomingDocForm
{
    /// <summary>
    /// Модель поиска приходных накладных в гриде.
    /// </summary>
    public class IncomingDocSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает название для поиска в накладных.
        /// </summary>
        public string IncomingDocName { get; set; }

        /// <summary>
        /// Задает или получает начало периода поиска.
        /// </summary>
        public DateTime IncomingDocBeginDate { get; set; }

        /// <summary>
        /// Задает или получает окончание периода поиска.
        /// </summary>
        public DateTime IncomingDocEndDate { get; set; }

        /// <summary>
        /// Задает или получает код склада для поиска.
        /// </summary>
        public Guid? IncomingDocWarehouseID { get; set; }
    }
}