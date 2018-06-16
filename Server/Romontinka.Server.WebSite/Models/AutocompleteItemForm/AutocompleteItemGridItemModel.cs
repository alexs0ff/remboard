using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.AutocompleteItemForm
{
    /// <summary>
    /// Модель элемента грида для представления пункта автодополнения.
    /// </summary>
    public class AutocompleteItemGridItemModel : JGridItemModel<Guid>
    {
        /// <summary>
        /// Задает или получает название типа пункта автозаполнения.
        /// </summary>
        public string AutocompleteKindTitle { get; set; }

        /// <summary>
        /// Задает или получает название пункта.
        /// </summary>
        public string Title { get; set; }
    }
}