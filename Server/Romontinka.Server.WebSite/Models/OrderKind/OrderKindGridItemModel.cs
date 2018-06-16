using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.OrderKind
{
    /// <summary>
    /// Модель строки для грида.
    /// </summary>
    public class OrderKindGridItemModel : JGridItemModel<Guid>
    {
        /// <summary>
        /// Задает или получает название.
        /// </summary>
        public string Title { get; set; }
    }
}