using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.ContractorForm
{
    /// <summary>
    /// Модель для строки поиска контрагента.
    /// </summary>
    public class ContractorSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает строку поика по имени.
        /// </summary>
        public string ContractorName { get; set; }
    }
}