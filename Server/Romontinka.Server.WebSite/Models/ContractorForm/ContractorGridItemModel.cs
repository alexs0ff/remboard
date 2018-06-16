using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.ContractorForm
{
    /// <summary>
    /// Модель строки грида.
    /// </summary>
    public class ContractorGridItemModel : JGridItemModel<Guid>
    {
        /// <summary>
        /// Задает или получает юрназвание.
        /// </summary>
        public string LegalName { get; set; }

        /// <summary>
        /// Задает или получает адрес.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Задает или получает дату заведения.
        /// </summary>
        public string Phone { get; set; }
    }
}