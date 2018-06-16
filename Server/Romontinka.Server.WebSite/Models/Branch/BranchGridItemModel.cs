using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.Branch
{
    /// <summary>
    /// модель строки для грида.
    /// </summary>
    public class BranchGridItemModel : JGridItemModel<Guid>
    {
        /// <summary>
        /// Задает или получает название филиала.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Задает или получает адрес филиала.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Задает или получает юр название филиала.
        /// </summary>
        public string LegalName { get; set; }
    }
}