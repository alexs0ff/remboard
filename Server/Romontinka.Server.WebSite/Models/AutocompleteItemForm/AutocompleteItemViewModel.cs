using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Models.DataGrid;

namespace Romontinka.Server.WebSite.Models.AutocompleteItemForm
{
    /// <summary>
    /// Модель отображения списка пунктов автозаполнения.
    /// </summary>
    public class AutocompleteItemViewModel
    {
        /// <summary>
        /// Задает или получает описатель грида.
        /// </summary>
        public DataGridDescriptor AutocompleteItemsGrid { get; set; }
    }
}