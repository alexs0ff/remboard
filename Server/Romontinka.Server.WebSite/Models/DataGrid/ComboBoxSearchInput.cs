using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Romontinka.Server.WebSite.Helpers;
using Romontinka.Server.WebSite.Models.Controls;

namespace Romontinka.Server.WebSite.Models.DataGrid
{
    /// <summary>
    /// Представляет собой комбобокс области поиска грида.
    /// </summary>
    public class ComboBoxSearchInput : SearchInputBase
    {
        /// <summary>
        /// Задает или получает модель комбобокса.
        /// </summary>
        public AjaxComboBoxModel ComboBoxModel { get; set; }
    }
}