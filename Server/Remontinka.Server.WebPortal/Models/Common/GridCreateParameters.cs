using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Remontinka.Server.WebPortal.Models.Common
{
    /// <summary>
    /// Параметры для передачи в адаптер при создании грида.
    /// </summary>
    public class GridCreateParameters
    {
        /// <summary>
        /// Задает или получает код связанной сущности.
        /// </summary>
        public string ParentId { get; set; }
    }
}