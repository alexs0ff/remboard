using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Common;

namespace Romontinka.Server.WebSite.Models.AutocompleteItemForm
{
    /// <summary>
    /// Модель поиска пунктов автозаполнения.
    /// </summary>
    public class AutocompleteItemSearchModel : JGridSearchBaseModel
    {
        /// <summary>
        /// Задает или получает название для поиска.
        /// </summary>
        public string AutocompleteItemSearchTitle { get; set; }

        /// <summary>
        /// Задает или получает тип для поиска.
        /// </summary>
        public byte? AutocompleteItemSearchKindID { get; set; }
    }
}